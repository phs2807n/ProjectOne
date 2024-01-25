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
    public MaginWeapon magin;
    public Enemy enemy;

    [Header("# Player Info")]
    public int Weapon_no = 0;

    public float gameTime;
    public float evasion = 3.0f;
    public float calltime = 1.0f;

    public bool isMove;


    protected override void Awake()
    {
        base.Awake();

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
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        inputActions.Player.Evasion.performed -= OnEvasion;
        inputActions.Player.Run.canceled -= OnRun;
        inputActions.Player.Run.performed -= OnRun;
        inputActions.Player.LeftAction.canceled -= onLeftAction;
        inputActions.Player.LeftAction.performed -= onLeftAction;
        inputActions.Player.RightAction.canceled -= onRightAction;
        inputActions.Player.RightAction.performed -= onRightAction;
        inputActions.Player.Disable();
    }

    //private void OnAction1(InputAction.CallbackContext context)
    //{
    //    Weapon_no = 0;
    //    weapon.ChangeWeapon(Weapon_no);
    //    magin.ChangeWeapon(Weapon_no);
    //}

    //private void OnAction2(InputAction.CallbackContext context)
    //{
    //    Weapon_no = 1;
    //    weapon.ChangeWeapon(Weapon_no);
    //    magin.ChangeWeapon(Weapon_no);
    //}

    //private void OnAction3(InputAction.CallbackContext context)
    //{
    //    Weapon_no = 2;
    //    weapon.ChangeWeapon(Weapon_no);
    //    magin.ChangeWeapon(Weapon_no);
    //}

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
        if (context.canceled)
        {
            magin.Magin();
        }
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
        StartCoroutine(OnEvasionCoroutine());
    }

    IEnumerator OnEvasionCoroutine()
    {
        while(true)
        {
            Vector3 dir = GetMousePostion() - player_moving.transform.position;
            dir = dir.normalized;
            player_moving.transform.position += dir * evasion;
            yield return new WaitForSeconds(calltime);
        }
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
    }

    public void Move()
    {
        isMove = true;
    }


    private void Update()
    {
        gameTime += Time.deltaTime;
    }
}
