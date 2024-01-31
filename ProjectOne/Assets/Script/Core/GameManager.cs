using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{
    [Header("# Game Control")]
    PlayerInputActions inputActions;

    [Header("# Game Object")]
    public PlayerMoving player_moving;
    public PlayerState player_state;
    public Weapon weapon;
    public Enemy enemy;

    [Header("# Player Info")]
    public int Weapon_no = 0;

    public float gameTime;
    public float evasion = 3.0f;
    public float calltime = 1.0f;

    public bool isMove;

    public Action<int> changeWeaon;

    bool isEvasion = true;

    protected override void Awake()
    {
        base.Awake();

        player_moving = FindObjectOfType<PlayerMoving>(true);

        inputActions = new PlayerInputActions();
        Move();
    }

    private void Start()
    {
        player_state = GetComponent<PlayerState>();
        player_state.onDie += onDie;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        inputActions.Player.Enable();
        inputActions.Player.RightAction.performed += onRightAction;
        inputActions.Player.RightAction.canceled += onRightAction;
        inputActions.Player.LeftAction.performed += onLeftAction;
        inputActions.Player.LeftAction.canceled += onLeftAction;
        inputActions.Player.Run.performed += OnRun;
        inputActions.Player.Run.canceled += OnRun;
        inputActions.Player.Evasion.performed += OnEvasion;
        inputActions.Player.change.performed += OnChange;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        inputActions.Player.change.performed -= OnChange;
        inputActions.Player.Evasion.performed -= OnEvasion;
        inputActions.Player.Run.canceled -= OnRun;
        inputActions.Player.Run.performed -= OnRun;
        inputActions.Player.LeftAction.canceled -= onLeftAction;
        inputActions.Player.LeftAction.performed -= onLeftAction;
        inputActions.Player.RightAction.canceled -= onRightAction;
        inputActions.Player.RightAction.performed -= onRightAction;
        inputActions.Player.Disable();
    }

    private void OnChange(InputAction.CallbackContext context)
    {
        Weapon_no++;
        if(Weapon_no > 2)
        {
            Weapon_no = 0;
        }
        changeWeaon?.Invoke(Weapon_no);
    }
   
    /// <summary>
    /// 좌클릭 메서드
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void onLeftAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
           // Debug.Log(GetMousePostion());
            if (isMove)
            {
                player_moving.OnMove();
            }
            else
            {
                weapon.Behaviour();
            }
        }
        if(context.canceled)
        {
            weapon.StopBehaviour();
        }
    }

    /// <summary>
    /// 우클릭 이벤트
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void onRightAction(InputAction.CallbackContext context)
    {
    }

    private void OnRun(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            NotMove();
        }
        if (context.canceled)
        {
            Move();
        }
    }

    private void OnEvasion(InputAction.CallbackContext context)
    {
        if (isEvasion)
        {
            Evasion();
            StartCoroutine(OnEvasionCoroutine());
        }
        
    }

    IEnumerator OnEvasionCoroutine()
    {
        float curTime = calltime;
        while(curTime > 0.0f)
        {
            curTime -= Time.deltaTime;
            

            yield return null;
        }

        isEvasion = true;
    }

    void Evasion()
    {
        isEvasion = false;

        Vector3 dir = GetMousePostion() - player_moving.transform.position;
        dir = dir.normalized;
        player_moving.transform.position += dir * evasion;
    }

    /// <summary>
    /// 마우스 좌표 구하기 메서드
    /// </summary>
    /// <returns>마우스 좌표값</returns>
    public Vector3 GetMousePostion()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, -Camera.main.transform.position.z));
    }

    void onDie()
    {
        enemy = FindAnyObjectByType<Enemy>();

        NotMove();
    }

    public void NotMove()
    {
        player_moving.OnStop();
        isMove = false;
        switch (Weapon_no)
        {
            case 0:
                Slow(true);
                break;
        }
    }

    void Slow(bool isSlow)
    {
        EnemyPool enemyPool = FindObjectOfType<EnemyPool>();
        enemyPool.Set();
        for (int i = 0; i < enemyPool.enemys.Length; i++)
        {
            if (isSlow)
            {
                enemyPool.enemys[i].Agi = enemyPool.enemys[i].Agi_Slow;
            }
            else
            {
                enemyPool.enemys[i].Agi = enemyPool.enemys[i].Agi_Base;
            }
        }
    }

    public void Move()
    {
        isMove = true;
        switch (Weapon_no)
        {
            case 0:
                Slow(false);
                break;
        }
    }


    private void Update()
    {
        gameTime += Time.deltaTime;
    }

}
