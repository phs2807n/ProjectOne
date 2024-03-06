using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    /// <summary>
    /// ������Ʈ
    /// </summary>
    PlayerInputActions inputAction;
    Animator animator;
    Rigidbody2D rigid;

    [Header("# �÷��̾� ����")]

    /// <summary>
    /// �̵� �ӵ�
    /// </summary>
    public float speed = 3.0f;

    /// <summary>
    /// ���� �̵� �ӵ�
    /// </summary>
    float currentSpeed = 3.0f;

    /// <summary>
    /// ���� �Էµ� �̵� ����
    /// </summary>
    Vector2 inputDirection = Vector2.zero;

    /// <summary>
    /// ���� �̵��̰� �ִ��� Ȯ���ϴ� 
    /// </summary>
    bool isMove = false;

    /// <summary>
    /// ���� ��Ÿ��
    /// </summary>
    public float Attack_CoolTime = 1.0f;

    /// <summary>
    /// ���� �����ִ� ���� ��Ÿ��
    /// </summary>
    float currentAttack_CoolTime = 0.0f;
    
    /// <summary>
    /// ���� ��Ÿ���� �� �Ǿ����� Ȯ���ϱ� ���� ������Ƽ
    /// </summary>
    bool isAttackReady => currentAttack_CoolTime < 0.0f;

    /// <summary>
    /// AttackSensor�� ȸ�� ��
    /// </summary>
    Transform attackSensorAxix;

    /// <summary>
    /// ���� �� ���� ���� �ȿ� ����ִ� ���� ���
    /// </summary>
    public List<Slime> attackTargetList;

    /// <summary>
    /// ���� ������ ��ȿ�� �������� Ȯ���ϴ� ����
    /// </summary>
    public bool isAttackValid = false;


    readonly int inputX_Hash = Animator.StringToHash("InputX");
    readonly int inputY_Hash = Animator.StringToHash("InputY");
    readonly int IsMove_Hash = Animator.StringToHash("IsMove");
    readonly int IsAttack_Hash = Animator.StringToHash("IsAttack");

    /// <summary>
    /// �÷��̾ ���� ��ġ�ϰ� �ִ� ���� �׸���
    /// </summary>
    Vector2Int currentMap;

    /// <summary>
    /// CurrentMap�� ���� ������ �� ������ �־����� ��������Ʈ�� �����ؼ� �˸��� ������Ƽ
    /// </summary>
    Vector2Int CurrentMap
    {
        get => currentMap;
        set
        {
            if(value != currentMap) 
            {
                currentMap = value;
                Debug.Log(currentMap);
                onMapChange?.Invoke(currentMap);
            }
        }
    }

    /// <summary>
    /// �÷��̾ �ִ� ���� ����Ǹ� ����Ǵ� ��������Ʈ
    /// </summary>
    public Action<Vector2Int> onMapChange;

    /// <summary>
    /// ���� �Ŵ���
    /// </summary>
    WorldManager world;

    /// <summary>
    /// �÷��̾��� �ִ� ����
    /// </summary>
    public float maxLifeTime = 10.0f;

    /// <summary>
    /// �÷��̾��� ���� ����
    /// </summary>
    float lifeTime;

    float LifeTime
    {
        get => lifeTime;
        set
        {
            // ������ ����� �� ��������Ʈ�� ����ȴ�.
            lifeTime = value;
            if(lifeTime < 0.0f && isAlive)
            {
                Die();
            }
            else
            {
                lifeTime = Mathf.Clamp(lifeTime, 0.0f, maxLifeTime);
                onLifeTimeChange?.Invoke(lifeTime / maxLifeTime);
            }
        }
    }

    /// <summary>
    /// ��ü �÷��̽ð�
    /// </summary>
    float totalPlayTime;

    /// <summary>
    /// �÷��̾��� ������ ����Ǿ��� �� ����� ��������Ʈ(float:������ ����)
    /// </summary>
    public Action<float> onLifeTimeChange;

    /// <summary>
    /// �÷��̾��� �׾����� �˸��� ��������Ʈ(float:��ü �÷��̽ð�, int:ųī��Ʈ)
    /// </summary>
    public Action<float, int> onDie;

    bool isAlive = true;

    /// <summary>
    /// ���� ������ ��
    /// </summary>
    int killCount = -1;

    int KillCount
    {
        get => killCount;
        set
        {
            if(killCount != value)
            {
                killCount = value;
                onKillCountChange?.Invoke(killCount);
            }
        }
    }

    public Action<int> onKillCountChange;

    private void Awake()
    {
        inputAction = new PlayerInputActions();

        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();

        currentSpeed = speed;
        
        attackSensorAxix = transform.GetChild(0);

        attackTargetList = new List<Slime>(4);
        AttackSensor sensor = attackSensorAxix.GetComponentInChildren<AttackSensor>();
        sensor.onEnemyEnter += (slime) =>       // ���� ���� �ȿ� ������ 
        {
            if (isAttackValid)
            {
                // ������ ��ȿ�� ��Ȳ�̸�
                slime.Die();                    // ��� ���̱�
            }
            else
            {
                // ������ ��ȿ���� ������
                attackTargetList.Add(slime);    // ����Ʈ�� �߰�
            }
            slime.ShowOutline();                // �ƿ������� �׸���.
        };
        sensor.onEnemyExit += (slime) =>
        {
            attackTargetList.Remove(slime);
            slime.ShowOutline(false);                // �ƿ������� ����.
        };
    }

    private void OnEnable()
    {
        inputAction.Player.Enable();
        inputAction.Player.Move.performed += OnMove;
        inputAction.Player.Move.canceled += OnStop;
        inputAction.Player.Attack.performed += OnAttack;
    }

    private void OnDisable()
    {
        inputAction.Player.Attack.performed -= OnAttack;
        inputAction.Player.Move.canceled -= OnStop;
        inputAction.Player.Move.performed -= OnMove;
        inputAction.Player.Disable();
    }

    private void Start()
    {
        world = GameManager.Instance.World;
        lifeTime = maxLifeTime;
        killCount = 0;
    }

    private void Update()
    {
        currentAttack_CoolTime -= Time.deltaTime;
        LifeTime -= Time.deltaTime;
        totalPlayTime += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        // ���� �����Ӹ��� inputDirection�������� �ʴ� speed��ŭ �̵�
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * currentSpeed * inputDirection);

        CurrentMap = world.WorldToGrid(rigid.position); // �÷��̾ �ִ� �� ����
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        // �Է°� �޾ƿͼ�
        inputDirection = context.ReadValue<Vector2>();

        // �ִϸ��̼� ����
        animator.SetFloat(inputX_Hash, inputDirection.x);
        animator.SetFloat(inputY_Hash, inputDirection.y);
        isMove = true;
        animator.SetBool(IsMove_Hash, isMove);

        // ���� ���� ȸ����Ű��
        AttackSensorRotate(inputDirection);
    }

    private void OnStop(InputAction.CallbackContext _)
    {
        // �̵� �������� 0���� �����
        inputDirection = Vector2.zero;

        // InputX�� InputY�� �������� �ʴ� ����
        // Idle �ִϸ��̼��� ������ �̵� �������� ����ϱ� ����

        isMove = false;     // ����
        animator.SetBool(IsMove_Hash, isMove);
    }

    private void OnAttack(InputAction.CallbackContext _)
    {
        if(isAttackReady)
        {
            animator.SetTrigger(IsAttack_Hash);         // �ִϸ��̼� ���
            currentAttack_CoolTime = Attack_CoolTime;   // ��Ÿ�� �ʱ�ȭ
            currentSpeed = 0.0f;                        // �̵� ����
            isAttackValid = false;                      // ������ ����� �ʱ�ȭ
        }
    }

    /// <summary>
    /// �̵� �ӵ��� ������� �ǵ����� �Լ�
    /// </summary>
    public void RestoreSpeed()
    {
        currentSpeed = speed;
    }

    /// <summary>
    /// �Է� ���⿡ ���� AttackSensor�� ȸ����Ű�� �Լ�
    /// </summary>
    /// <param name="direction"></param>
    void AttackSensorRotate(Vector2 direction)
    {
        if(direction.y < 0)
        {
            attackSensorAxix.rotation = Quaternion.identity;            // �Ʒ�
        }
        else if(direction.y > 0)
        {
            attackSensorAxix.rotation = Quaternion.Euler(0, 0, 180);    // ��
        }
        else if(direction.x < 0)
        {
            attackSensorAxix.rotation = Quaternion.Euler(0, 0, -90);    // ����
        }
        else if(direction.x > 0)
        {
            attackSensorAxix.rotation = Quaternion.Euler(0, 0, 90);     // ������
        }
        else
        {
            attackSensorAxix.rotation = Quaternion.identity;            // �Է��� ����(0,0)
        }
    }

    /// <summary>
    /// ���� �ִϸ��̼� ���� �߿� ������ ��ȿ������ �ִϸ��̼� �̺�Ʈ�� ����
    /// </summary>
    void AttackValid()
    {
        isAttackValid = true;
        foreach(var slime in attackTargetList)
        {
            slime.Die();
        }
        attackTargetList.Clear();
    }

    /// <summary>
    /// ���� �ִϸ��̼� ���� �߿� ������ ��ȿ���� �ʰ� �Ǹ� �ִϸ��̼� �̺�Ʈ�� ����
    /// </summary>
    void AttackNotValid()
    {
        isAttackValid = false;
    }

    /// <summary>
    /// ���͸� ����� �� ������ �Լ�
    /// </summary>
    /// <param name="bonus">���� ó�� ���ʽ�(�����߰�)</param>
    public void MonsterKill(float bonus)
    {
        LifeTime += bonus;
        KillCount++;
    }

    /// <summary>
    /// �÷��̰� ����� �����ϴ� �Լ�
    /// </summary>
    void Die()
    {
        isAlive = false;                            // �׾��ٰ� ǥ��
        lifeTime = 0.0f;                            // ������ 0���� ����
        onLifeTimeChange?.Invoke(0);                // ����ȭ�� �˸���
        inputAction.Player.Disable();               // �÷��̾� �Է� ����
        onDie?.Invoke(totalPlayTime, killCount);    // 
    }

#if UNITY_EDITOR
    public void TestDie()
    {
        Die();
    }
#endif
}
