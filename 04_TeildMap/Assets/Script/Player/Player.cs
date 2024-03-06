using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 컴포넘트
    /// </summary>
    PlayerInputActions inputAction;
    Animator animator;
    Rigidbody2D rigid;

    [Header("# 플레이어 정보")]

    /// <summary>
    /// 이동 속도
    /// </summary>
    public float speed = 3.0f;

    /// <summary>
    /// 현재 이동 속도
    /// </summary>
    float currentSpeed = 3.0f;

    /// <summary>
    /// 현재 입력된 이동 방향
    /// </summary>
    Vector2 inputDirection = Vector2.zero;

    /// <summary>
    /// 지금 이동이고 있는지 확인하는 
    /// </summary>
    bool isMove = false;

    /// <summary>
    /// 공격 쿨타임
    /// </summary>
    public float Attack_CoolTime = 1.0f;

    /// <summary>
    /// 현재 남아있는 공격 쿨타임
    /// </summary>
    float currentAttack_CoolTime = 0.0f;
    
    /// <summary>
    /// 공격 쿨타임이 다 되었는지 확인하기 위한 프로퍼티
    /// </summary>
    bool isAttackReady => currentAttack_CoolTime < 0.0f;

    /// <summary>
    /// AttackSensor의 회전 축
    /// </summary>
    Transform attackSensorAxix;

    /// <summary>
    /// 현재 내 공격 범위 안에 들어있는 적의 목록
    /// </summary>
    public List<Slime> attackTargetList;

    /// <summary>
    /// 지금 공격이 유효한 상태인지 확인하는 변수
    /// </summary>
    public bool isAttackValid = false;


    readonly int inputX_Hash = Animator.StringToHash("InputX");
    readonly int inputY_Hash = Animator.StringToHash("InputY");
    readonly int IsMove_Hash = Animator.StringToHash("IsMove");
    readonly int IsAttack_Hash = Animator.StringToHash("IsAttack");

    /// <summary>
    /// 플레이어가 현재 위치하고 있는 맵의 그리드
    /// </summary>
    Vector2Int currentMap;

    /// <summary>
    /// CurrentMap에 값을 설정할 때 변경이 있었으면 델리게이트를 실행해서 알리는 프로퍼티
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
    /// 플레이어가 있는 맵이 변경되면 실행되는 델리게이트
    /// </summary>
    public Action<Vector2Int> onMapChange;

    /// <summary>
    /// 월드 매니저
    /// </summary>
    WorldManager world;

    /// <summary>
    /// 플레이어의 최대 수명
    /// </summary>
    public float maxLifeTime = 10.0f;

    /// <summary>
    /// 플레이어의 현재 수명
    /// </summary>
    float lifeTime;

    float LifeTime
    {
        get => lifeTime;
        set
        {
            // 수명이 변경될 때 델리게이트가 실행된다.
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
    /// 전체 플레이시간
    /// </summary>
    float totalPlayTime;

    /// <summary>
    /// 플레이어의 수명이 변경되었을 때 실행될 델리게이트(float:수명의 비율)
    /// </summary>
    public Action<float> onLifeTimeChange;

    /// <summary>
    /// 플레이어의 죽었음을 알리는 델리게이트(float:전체 플레이시간, int:킬카운트)
    /// </summary>
    public Action<float, int> onDie;

    bool isAlive = true;

    /// <summary>
    /// 잡은 슬라임 수
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
        sensor.onEnemyEnter += (slime) =>       // 적이 센서 안에 들어오면 
        {
            if (isAttackValid)
            {
                // 공격이 유효한 상황이면
                slime.Die();                    // 즉시 죽이기
            }
            else
            {
                // 공격이 유효하지 않으면
                attackTargetList.Add(slime);    // 리스트에 추가
            }
            slime.ShowOutline();                // 아웃라인을 그린다.
        };
        sensor.onEnemyExit += (slime) =>
        {
            attackTargetList.Remove(slime);
            slime.ShowOutline(false);                // 아웃라인을 끈다.
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
        // 물리 프레임마다 inputDirection방향으로 초당 speed만큼 이동
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * currentSpeed * inputDirection);

        CurrentMap = world.WorldToGrid(rigid.position); // 플레이어가 있는 맵 설정
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        // 입력값 받아와서
        inputDirection = context.ReadValue<Vector2>();

        // 애니메이션 조정
        animator.SetFloat(inputX_Hash, inputDirection.x);
        animator.SetFloat(inputY_Hash, inputDirection.y);
        isMove = true;
        animator.SetBool(IsMove_Hash, isMove);

        // 공격 범위 회전시키기
        AttackSensorRotate(inputDirection);
    }

    private void OnStop(InputAction.CallbackContext _)
    {
        // 이동 방향으로 0으로 만들고
        inputDirection = Vector2.zero;

        // InputX와 InputY를 변경하지 않는 이유
        // Idle 애니메이션을 마지막 이동 방향으로 재생하기 위해

        isMove = false;     // 정지
        animator.SetBool(IsMove_Hash, isMove);
    }

    private void OnAttack(InputAction.CallbackContext _)
    {
        if(isAttackReady)
        {
            animator.SetTrigger(IsAttack_Hash);         // 애니메이션 재생
            currentAttack_CoolTime = Attack_CoolTime;   // 쿨타임 초기화
            currentSpeed = 0.0f;                        // 이동 정지
            isAttackValid = false;                      // 만약을 대비한 초기화
        }
    }

    /// <summary>
    /// 이동 속도를 원래대로 되돌리는 함수
    /// </summary>
    public void RestoreSpeed()
    {
        currentSpeed = speed;
    }

    /// <summary>
    /// 입력 방향에 따라 AttackSensor를 회전시키는 함수
    /// </summary>
    /// <param name="direction"></param>
    void AttackSensorRotate(Vector2 direction)
    {
        if(direction.y < 0)
        {
            attackSensorAxix.rotation = Quaternion.identity;            // 아래
        }
        else if(direction.y > 0)
        {
            attackSensorAxix.rotation = Quaternion.Euler(0, 0, 180);    // 위
        }
        else if(direction.x < 0)
        {
            attackSensorAxix.rotation = Quaternion.Euler(0, 0, -90);    // 왼쪽
        }
        else if(direction.x > 0)
        {
            attackSensorAxix.rotation = Quaternion.Euler(0, 0, 90);     // 오른쪽
        }
        else
        {
            attackSensorAxix.rotation = Quaternion.identity;            // 입력이 없음(0,0)
        }
    }

    /// <summary>
    /// 공격 애니메이션 진행 중에 공격이 유효해지면 애니메이션 이벤트로 실행
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
    /// 공격 애니메이션 진행 중에 공격이 유효하지 않게 되면 애니메이션 이벤트로 실행
    /// </summary>
    void AttackNotValid()
    {
        isAttackValid = false;
    }

    /// <summary>
    /// 몬스터를 잡았을 때 실행할 함수
    /// </summary>
    /// <param name="bonus">몬스터 처리 보너스(수명추가)</param>
    public void MonsterKill(float bonus)
    {
        LifeTime += bonus;
        KillCount++;
    }

    /// <summary>
    /// 플레이가 사망시 동작하는 함수
    /// </summary>
    void Die()
    {
        isAlive = false;                            // 죽었다고 표시
        lifeTime = 0.0f;                            // 수명을 0으로 정리
        onLifeTimeChange?.Invoke(0);                // 수명변화를 알리기
        inputAction.Player.Disable();               // 플레이어 입력 정지
        onDie?.Invoke(totalPlayTime, killCount);    // 
    }

#if UNITY_EDITOR
    public void TestDie()
    {
        Die();
    }
#endif
}
