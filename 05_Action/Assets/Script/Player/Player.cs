using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 입력된 이동 방향
    /// </summary>
    Vector3 inputDirection = Vector3.zero;  // y는 무조건 바닥 높이

    /// <summary>
    /// 캐릭터의 목표방향으로 회전시키는 회전
    /// </summary>
    Quaternion targetRotation = Quaternion.identity;

    Transform weaponParent;
    Transform shiledParent;

    // 컴포넌트
    PlayerInputController controller;
    Animator animator;
    CharacterController characterController;

    // 애니메이터용 해시값 및 상수
    readonly int Speed_Hash = Animator.StringToHash("Speed");
    const float AnimatorStopSpeed = 0.0f;
    const float AnimatorWalkSpeed = 0.3f;
    const float AnimatorRunSpeed = 1.0f;
    readonly int Attack_Hash = Animator.StringToHash("Attack");



    public float walkSpeed = 3.0f;
    public float runSpeed = 5.0f;
    float currentSpeed = 0.0f;

    public float turnSpeed = 10.0f;

    public float attackCooltime = 0.5f;
    public float currentCooltime;

    public bool IsAttack => attackCooltime < currentCooltime;

    enum MoveMode
    {
        Walk = 0,
        Run
    }

    /// <summary>
    /// 현재 이동 모드
    /// </summary>
    MoveMode currentMoveMode = MoveMode.Run;

    /// <summary>
    /// 현재 이동 모드 확인 및 설정용 프로퍼티
    /// </summary>
    MoveMode MoveSpeedMode
    {
        get => currentMoveMode;
        set
        {
            currentMoveMode = value;    // 상태 변경
            if (currentSpeed > 0.0f)     // 이동 중인지 아닌지 확인
            {
                // 이동 중이면 모드에 맞게 속도와 애니메이션 변경
                MoveModeChange(currentMoveMode);
            }
        }
    }

    /// <summary>
    /// 무기 이펙트 켜고 끄는 신호를 보내는 델리게이트
    /// </summary>
    public Action<bool> onWeaponEffectEnable;

    private void Awake()
    {
        Transform child = transform.GetChild(2);
        child = child.GetChild(0);
        child = child.GetChild(0);
        child = child.GetChild(0);
        Transform spine3 = child.GetChild(0);
        weaponParent = spine3.GetChild(1);
        weaponParent = spine3.GetChild(0);
        weaponParent = spine3.GetChild(0);
        weaponParent = spine3.GetChild(2);

        shiledParent = spine3.GetChild(1);
        shiledParent = shiledParent.GetChild(1);
        shiledParent = shiledParent.GetChild(0);
        shiledParent = shiledParent.GetChild(0);
        shiledParent = shiledParent.GetChild(2);

        controller = GetComponent<PlayerInputController>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        controller.onMove += OnMoveInput;
        controller.onMoveModeChange += OnMoveModeChangeInput;
        controller.onAttack += OnAttackInput;
    }

    private void Start()
    {
        Weapon weapon = weaponParent.GetComponentInChildren<Weapon>();
        onWeaponEffectEnable = weapon.EffectEnable;
    }

    private void Update()
    {
        characterController.Move(Time.deltaTime * currentSpeed * inputDirection);       // 좀 더 수동
                                                                                        // characterController.SimpleMove(currentSpeed * inputDirection);                  // 좀 더 자동

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
        currentCooltime += Time.deltaTime;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <param name="isPress"></param>
    private void OnMoveInput(Vector2 input, bool isPress)
    {
        // 입력 방향 저장
        inputDirection.x = input.x;
        inputDirection.y = 0;
        inputDirection.z = input.y;


        if (isPress)
        {
            // 눌려진 상황

            // 입력 방향 회전 시키기
            Quaternion camY = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0); // 카메라의 y회전만 따로 추출
            inputDirection = camY * inputDirection;     // 입력 방향을 카메라의 y회전과 같은 정도로 회전 시키기
            targetRotation = Quaternion.LookRotation(inputDirection);

            // 이동 모든 변경
            MoveModeChange(currentMoveMode);
        }
        else
        {
            // 입력을 끝낸 상황
            currentSpeed = 0.0f;    // 정지 시키기
            animator.SetFloat(Speed_Hash, AnimatorStopSpeed);
        }
    }

    private void OnMoveModeChangeInput()
    {
        if (MoveSpeedMode == MoveMode.Walk)
        {
            currentMoveMode = MoveMode.Run;
        }
        else if (MoveSpeedMode == MoveMode.Run)
        {
            currentMoveMode = MoveMode.Walk;
        }
    }

    /// <summary>
    /// 공격 입력에 대한 델리게이트로 실행되는 함수
    /// </summary>
    private void OnAttackInput()
    {
        // 쿨타임이 다 되었고, 가만히 서있거나 걷는 상태일 때만 공격 가능
        if (IsAttack && ((currentSpeed < 0.001f) || (currentMoveMode == MoveMode.Walk)))
        {
            animator.SetTrigger(Attack_Hash);
            currentCooltime = 0.0f;
        }
    }

    public void ShowWeaponAndShield(bool isShow = true)
    {
        weaponParent.gameObject.SetActive(isShow);
        shiledParent.gameObject.SetActive(isShow);
    }

    /// <summary>
    /// 모드에 따라 이동 속도를 변경하는 함수
    /// </summary>
    /// <param name="mode">설정된 모드</param>
    void MoveModeChange(MoveMode mode)
    {
        switch (mode)   // 이동 모드에 따라 속도와 애니메이션 변경
        {
            case MoveMode.Walk:
                currentSpeed = walkSpeed;
                animator.SetFloat(Speed_Hash, AnimatorWalkSpeed);
                break;
            case MoveMode.Run:
                currentSpeed = runSpeed;
                animator.SetFloat(Speed_Hash, AnimatorRunSpeed);
                break;
        }
    }

    /// <summary>
    /// 무기의 이펙트를 키거나 끄라는 함수
    /// </summary>
    /// <param name="isShow"></param>
    public void ShowWeaponEffect(bool isShow = true)
    {
        onWeaponEffectEnable?.Invoke(isShow);
    }
}
