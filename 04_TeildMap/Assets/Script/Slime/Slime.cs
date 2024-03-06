using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UIElements;

public class Slime : RecycleObject
{
    /// <summary>
    /// ������ ���� �ð�
    /// </summary>
    public float phaseDuration = 0.5f;

    /// <summary>
    /// ������ ���� �ð�
    /// </summary>
    public float dissolveDuration = 1.0f;

    /// <summary>
    /// �ƿ������� ���� ���� �β�
    /// </summary>
    const float VisibleOutlinethickness = 0.004f;

    /// <summary>
    /// ����� ���� ���� �β�
    /// </summary>
    const float VisiblePhaseThickness = 0.1f;

    /// <summary>
    /// �������� ��Ƽ����
    /// </summary>
    Material mainMaterial;

    // ���̴� ������Ƽ ���̵��
    readonly int OutlineThicknessID = Shader.PropertyToID("_OutlineThickness");
    readonly int PhaseSplitID = Shader.PropertyToID("_PhaseSplit");
    readonly int PhaseThicknessID = Shader.PropertyToID("_PhaseThinknes");
    readonly int DissolveFadeID = Shader.PropertyToID("_DissolveFade");

    /// <summary>
    /// ����� ������ �˸��� ��������Ʈ
    /// </summary>
    Action onPhaseEnd;

    /// <summary>
    /// �����갡 ������ �˸��� ��������Ʈ
    /// </summary>
    Action onDissolveEnd;

    /// <summary>
    /// �������� ���� ������ ���
    /// </summary>
    List<Vector2Int> path;

    /// <summary>
    /// �������� �̵��� ��θ� �׷��ִ� Ŭ����
    /// </summary>
    public PathLine pathLine;

    /// <summary>
    /// �������� ������ �׸��� ��
    /// </summary>
    TileGridMap map;

    /// <summary>
    /// �������� ��ġ�� �׸��� ��ǥ�� �˷��ִ� ������Ƽ
    /// </summary>
    Vector2Int GridPosition => map.WorldToGrid(transform.position);

    /// <summary>
    /// �� �������� ��ġ�ϰ� �ִ� ���
    /// </summary>
    Node current = null;

    Node Current
    {
        get => current;
        set
        {
            if (current != value)
            {
                if (current != null) // ���� ��尡 null�̸� ��ŵ
                {
                    current.nodeType = Node.NodeType.Plain; // ���� ��带 Plain���� �ǵ�����         
                }
                current = value;
                if (current != null)
                {
                    current.nodeType = Node.NodeType.Slime; // ���� �̵��� ���� Slime���� �����ѱ�
                }
            }
        }
    }

    /// <summary>
    /// ������ �̵��ӵ�
    /// </summary>
    public float moveSpeed = 2.0f;

    /// <summary>
    /// �������� �̵� Ȱ��ȭ ǥ�ÿ� ����(true�� ������, false�� �� ������
    /// </summary>
    bool isMoveActivate = false;

    /// <summary>
    /// �������� �׾����� �˸��� ��������Ʈ
    /// </summary>
    public Action onDie;

    /// <summary>
    /// �������� ������ Ǯ�� Ʈ������
    /// </summary>
    Transform pool;

    /// <summary>
    /// pool�� �� �� ���� ���� �����ϴ� ������Ƽ
    /// </summary>
    public Transform Pool
    {
        set
        {
            if(pool == null)
            {
                pool = value;
            }
        }
    }

    /// <summary>
    /// ��Ʈ����Ʈ ������(Order In Layer ������)
    /// </summary>
    SpriteRenderer spriteRenderer;

    /// <summary>
    /// �ٸ� �����ӿ� ���� ��ΰ� ������ �� ��ٸ� �ð�
    /// </summary>
    float pathWaitTime = 0.0f;

    /// <summary>
    /// ��ΰ� ������ �� �ִ�� ��ٸ��� �ð�
    /// </summary>
    const float MaxPathWaitTime = 1.0f;

    /// <summary>
    /// ��� �������� ���� �����ϴ� ����
    /// </summary>
    bool isShowPath = false;

    /// <summary>
    /// �� �������� ���� �� �÷��̾�� �ִ� ���� ���ʽ�
    /// </summary>
    public float lifeTimeBonus = 2.0f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainMaterial = spriteRenderer.material;

        onPhaseEnd += () =>
        {
            isMoveActivate = true;
        };

        onDissolveEnd += ReturnToPool;

        path = new List<Vector2Int>();
        pathLine = GetComponentInChildren<PathLine>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        ResetShaderProperty();
        StartCoroutine(StartPhase());

        isMoveActivate = false;
    }

    protected override void OnDisable()
    {
        path.Clear();
        pathLine.ClearPath();

        base.OnDisable();
    }


    private void Update()
    {
        MoveUpdate();   // �̵�ó��
    }

    /// <summary>
    /// ������ �ʱ�ȭ�� �Լ�(�������Ŀ� ����)
    /// </summary>
    /// <param name="gridMap">�������� ���� Ÿ�ϸ�</param>
    /// <param name="world">�������� ������ġ(���� ��ǥ)</param>
    public void Initialize(TileGridMap gridMap, Vector3 world)
    {
        map = gridMap;  // �� ����
        transform.position = map.GridToWorld(map.WorldToGrid(world));   // ���� ��� ��ġ�� ��ġ
        Current = map.GetNode(world);
    }

    /// <summary>
    /// �������� ���� �� ����Ǵ� �Լ�
    /// </summary>
    public void Die()
    {
        isMoveActivate = false;
        onDie?.Invoke();
        onDie = null;
        StartCoroutine(StartDissolve());    // �����길 ����(������ �ڷ�ƾ�ȿ��� ��Ȱ��ȭ���� ó��)
    }

    /// <summary>
    /// ��Ȱ�� ��Ű�鼭 Ǯ�� �ǵ����� �Լ�
    /// </summary>
    public void ReturnToPool()
    {
        Current = null;
        transform.SetParent(pool);      // Ǯ�� �ٽ� �θ� ����
        gameObject.SetActive(false);    // ��Ȱ��ȭ
    }

    /// <summary>
    /// ���̴��� ������Ƽ ���� �ʱ�ȭ
    /// </summary>
    void ResetShaderProperty()
    {
        ShowOutline(false);                         // �ƿ����� ����
        mainMaterial.SetFloat(PhaseThicknessID, 0); // ������ �� �Ⱥ��̰� �ϱ�
        mainMaterial.SetFloat(PhaseSplitID, 1);     // ���� ���̰� �ϱ�
        mainMaterial.SetFloat(DissolveFadeID, 1);   // ������ �Ⱥ��̰� �ϱ�
    }

    /// <summary>
    /// �ƿ����� �Ѱ� ���� �Լ�
    /// </summary>
    /// <param name="isShow">true�� ���̰� false�� ������ �ʴ´�.</param>
    public void ShowOutline(bool isShow = true)
    {
        //  - Outline on/off
        if (isShow)
        {
            mainMaterial.SetFloat(OutlineThicknessID, VisibleOutlinethickness); // ���̴� ���� �β��� �����ϴ� ������ ���̰� ����
        }
        else
        {
            mainMaterial.SetFloat(OutlineThicknessID, 0);   // �Ⱥ��̴� ���� �β��� 0���� ���� �� ���̰� ����
        }
    }

    /// <summary>
    /// ������ �����ϴ� �ڷ�ƾ(�Ⱥ��� -> ���̱�
    /// </summary>
    /// <returns></returns>
    IEnumerator StartPhase()
    {
        //  - PhaseReverse�� �Ⱥ��̴� ���¿��� ���̰� ����� (1->0)

        float phaseNormalize = 1.0f / phaseDuration;    // ������ ����� ���̱� ���� �̸� ���

        float timeElapsed = 0.0f;   // �ð� ������

        mainMaterial.SetFloat(PhaseThicknessID, VisiblePhaseThickness); // ������ ���� ���̰� �����

        while (timeElapsed < phaseDuration)
        {
            timeElapsed += Time.deltaTime;

            //mainMaterial.SetFloat(PhaseSplitID, 1 - (timeElapsed / dissolveDuration));
            mainMaterial.SetFloat(PhaseSplitID, 1 - (timeElapsed * phaseNormalize));

            yield return null;
        }

        mainMaterial.SetFloat(PhaseThicknessID, 0); // ������ �� �Ⱥ��̰� �����
        mainMaterial.SetFloat(PhaseSplitID, 0);     // ���ڸ� ����ϰ� �����ϱ� ���� ��

        onPhaseEnd?.Invoke();
    }

    /// <summary>
    /// ������ �����ϴ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator StartDissolve()
    {
        //  - Dissolve �����Ű��(1->0)
        float dissolveNormalize = 1.0f / dissolveDuration;

        float timeElapsed = 0.0f;

        while (timeElapsed < dissolveDuration)
        {
            timeElapsed += Time.deltaTime;

            //mainMaterial.SetFloat(PhaseSplitID, 1 - (timeElapsed / dissolveDuration));
            mainMaterial.SetFloat(DissolveFadeID, 1 - (timeElapsed * dissolveNormalize));

            yield return null;
        }

        mainMaterial.SetFloat(DissolveFadeID, 0);

        onDissolveEnd?.Invoke();
    }

    /// <summary>
    /// �������� ������
    /// </summary>
    /// <param name="destination">�������� �׸��� ��ǥ</param>
    public void SetDestination(Vector2Int destination)
    {
        // ������������ ��� ����
        path = AStar.PathFind(map, GridPosition, destination);

        if (isShowPath)
        {
            pathLine.DrawPath(map, path);
        }
    }

    /// <summary>
    /// �������� �������� �� ����Ǵ� �Լ�
    /// </summary>
    void OnDestinationArrive()
    {
        SetDestination(map.GetRandomMoveablePosition());
    }

    /// <summary>
    /// Update�Լ����� �̵� ó���ϴ� �Լ�
    /// </summary>
    void MoveUpdate()
    {
        if (isMoveActivate)
        {
            // ��ΰ� �ְ� ���� ��ΰ� �ְ�, ���� ��ٸ��� �ʾ��� ���� ó��
            if (path != null && path.Count > 0 && pathWaitTime < MaxPathWaitTime)
            {
                // path�� ù��° ��ġ�� ��� �̵�
                Vector2Int destGrid = path[0];

                // �ٸ� �������� �ִ� ĭ���� �̵����� �ʴ´�.
                //  -> ���������� ǥ�õ� ��尡 �ƴϰų�, ���� �ִ� ����� ���� �����̱�
                if (!map.IsSlime(destGrid) || map.GetNode(destGrid) == Current)
                {
                    // ������ �̵��ϴ� ó��
                    Vector3 destPosition = map.GridToWorld(destGrid);       // ������ ������ǥ ���ϱ�
                    Vector3 direction = destPosition - transform.position;  // ���� ���

                    if (direction.sqrMagnitude < 0.001f)    // ���⺤���� ���̸� Ȯ���ؼ� �����ߴ��� Ȯ��
                    {
                        // ù��° ��ġ�� ����
                        transform.position = destPosition;  // ��������
                        path.RemoveAt(0);                   // path�� ù��° ��ġ�� ����
                    }
                    else
                    {
                        // ������������ direction �������� �̵�
                        transform.Translate(Time.deltaTime * moveSpeed * direction.normalized);
                        Current = map.GetNode(transform.position);  // Current ���� �õ� �� ó��
                    }
                    spriteRenderer.sortingOrder = -Mathf.FloorToInt(transform.position.y * 100);    // �Ʒ��ʿ� �ִ� �������� ���� �׷����� �����

                    pathWaitTime = 0.0f;    // ��ٸ��� �ð� �ʱ�ȭ
                }
                else
                {
                    // �ٸ� �������� �ִ� ���� ��ٸ��� 
                    pathWaitTime += Time.deltaTime;
                }
            }
            else
            {
                // �������� ���� or ���� ��ٷ���
                pathWaitTime = 0.0f;    
                OnDestinationArrive();  // ���� ������ �ڵ����� ����
            }
        }
    }

    /// <summary>
    /// ��θ� �������� ���� �����ϴ� �Լ�
    /// </summary>
    /// <param name="isShow">true�� �����ְ�, false�� �������� �ʴ´�.</param>
    public void ShowPath(bool isShow = true)
    {
        pathLine.gameObject.SetActive(isShow);
        if (isShow)
        {
            pathLine.DrawPath(map, path);
        }
        else
        {
            pathLine.ClearPath();
        }
    }

#if UNITY_EDITOR
    public void TestShader(int index)
    {
        switch(index)
        {
            case 1:
                ResetShaderProperty();
                break;
            case 2:
                ShowOutline(true);
                break;
            case 3:
                ShowOutline(false);
                break;
            case 4:
                StartCoroutine(StartPhase());
                break;
            case 5:
                StartCoroutine(StartDissolve());
                break;
        }
    }

    public void TestDie()
    {
        Die();
    }
#endif
}
