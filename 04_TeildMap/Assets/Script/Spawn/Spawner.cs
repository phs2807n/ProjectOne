using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class Spawner : MonoBehaviour
{
    /// <summary>
    /// ���� ����
    /// </summary>
    public float interval = 1.0f;

    /// <summary>
    /// ������ �������� ���� �ð�
    /// </summary>
    float elapsedTime = 0.0f;

    /// <summary>
    /// ���� ������ ũ��(transform.position���� ������(x), ����(y)��ŭ�� ũ��
    /// </summary>
    public Vector2 size;

    /// <summary>
    /// �����ʿ��� ���ÿ� �ִ�� ���������� �������� ��
    /// </summary>
    public int capacity = 3;

    /// <summary>
    /// ���� ������ ������ ��
    /// </summary>
    int count = 0;

    /// <summary>
    /// ������ �� �� �ִ� ����� ����Ʈ
    /// </summary>
    List<Node> spawnAreaList;

    /// <summary>
    /// �׸����, Ÿ�ϸ�, �����ʸ� �����ϴ� Ŭ����
    /// </summary>
    MapArea map;

    Player player;

    private void Start()
    {
        map = GetComponentInParent<MapArea>();
        spawnAreaList = map.CalcSpawnArea(this);
    }

    private void Update()
    {
        if(count < capacity)
        {
            elapsedTime += Time.deltaTime;  
            if(elapsedTime > interval)      // ���͹� Ȯ��
            {
                Spawn();                    // �� �� ����� �Ǹ� ����
                elapsedTime = 0.0f;
            }
        }
    }

    /// <summary>
    /// �������� �� ���� �����ϴ� �Լ�
    /// </summary>
    void Spawn()
    {
        if(IsSpawnAvailable(out Vector3 spawnablePosition))
        {
            Slime slime = Factory.Instance.GetSlime(); // ��ġ�� �ʱ�ȭ�Լ����� ����
            slime.Initialize(map.GridMap, spawnablePosition);           // �ʱ�ȭ

            slime.onDie += () =>        // �׾��� �� ����� �Լ� ���
            {
                count--;        // count ����
                GameManager.Instance.Player.MonsterKill(slime.lifeTimeBonus);
            };
            slime.transform.SetParent(transform);
            count++;
        }
    }

    /// <summary>
    /// ������ �������� Ȯ���ϰ� �����ϸ� ���������� ��ġ�� �����ִ� �Լ�
    /// </summary>
    /// <param name="spawnablePosition">���� ������ ��ġ</param>
    /// <returns>true�� ���� ����, false�� ���� �Ұ�</returns>
    bool IsSpawnAvailable(out Vector3 spawnablePosition)
    {
        bool result = false;
        List<Node> positions = new List<Node>();

        foreach(Node node in spawnAreaList)
        {
            if(node.nodeType == Node.NodeType.Plain)
            {
                positions.Add(node);
            }
        }

        if(positions.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, positions.Count);
            Node target = positions[index];
            spawnablePosition = map.GridToWorld(target.X, target.Y);
            result = true;
        }
        else
        {
            spawnablePosition = Vector3.zero;
        }

        return result;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector3 p0 = new(Mathf.Floor(transform.position.x), Mathf.Floor(transform.position.y));
        Vector3 p1 = p0 + Vector3.right * size.x;
        Vector3 p2 = p0 + (Vector3)size;
        Vector3 p3 = p0 + Vector3.up * size.y;

        Handles.color = Color.red;
        Handles.DrawLine(p0, p1, 5);
        Handles.DrawLine(p1, p2, 5);
        Handles.DrawLine(p2, p3, 5);
        Handles.DrawLine(p3, p0, 5);
    }
#endif
}
