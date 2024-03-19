using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    /// <summary>
    /// �Էµ� �̵� ����
    /// </summary>
    Vector3 inputDirection = Vector3.zero;  // y�� ������ �ٴ� ����

    /// <summary>
    /// ĳ������ ��ǥ�������� ȸ����Ű�� ȸ��
    /// </summary>
    Quaternion targetRotation = Quaternion.identity;

    Transform weaponParent;
    Transform shiledParent;

    // ������Ʈ
    PlayerInputController controller;
    Animator animator;
    CharacterController characterController;

    // �ִϸ����Ϳ� �ؽð� �� ���
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
    /// ���� �̵� ���
    /// </summary>
    MoveMode currentMoveMode = MoveMode.Run;

    /// <summary>
    /// ���� �̵� ��� Ȯ�� �� ������ ������Ƽ
    /// </summary>
    MoveMode MoveSpeedMode
    {
        get => currentMoveMode;
        set
        {
            currentMoveMode = value;    // ���� ����
            if (currentSpeed > 0.0f)     // �̵� ������ �ƴ��� Ȯ��
            {
                // �̵� ���̸� ��忡 �°� �ӵ��� �ִϸ��̼� ����
                MoveModeChange(currentMoveMode);
            }
        }
    }

    /// <summary>
    /// ���� ����Ʈ �Ѱ� ���� ��ȣ�� ������ ��������Ʈ
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
        characterController.Move(Time.deltaTime * currentSpeed * inputDirection);       // �� �� ����
                                                                                        // characterController.SimpleMove(currentSpeed * inputDirection);                  // �� �� �ڵ�

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
        // �Է� ���� ����
        inputDirection.x = input.x;
        inputDirection.y = 0;
        inputDirection.z = input.y;


        if (isPress)
        {
            // ������ ��Ȳ

            // �Է� ���� ȸ�� ��Ű��
            Quaternion camY = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0); // ī�޶��� yȸ���� ���� ����
            inputDirection = camY * inputDirection;     // �Է� ������ ī�޶��� yȸ���� ���� ������ ȸ�� ��Ű��
            targetRotation = Quaternion.LookRotation(inputDirection);

            // �̵� ��� ����
            MoveModeChange(currentMoveMode);
        }
        else
        {
            // �Է��� ���� ��Ȳ
            currentSpeed = 0.0f;    // ���� ��Ű��
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
    /// ���� �Է¿� ���� ��������Ʈ�� ����Ǵ� �Լ�
    /// </summary>
    private void OnAttackInput()
    {
        // ��Ÿ���� �� �Ǿ���, ������ ���ְų� �ȴ� ������ ���� ���� ����
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
    /// ��忡 ���� �̵� �ӵ��� �����ϴ� �Լ�
    /// </summary>
    /// <param name="mode">������ ���</param>
    void MoveModeChange(MoveMode mode)
    {
        switch (mode)   // �̵� ��忡 ���� �ӵ��� �ִϸ��̼� ����
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
    /// ������ ����Ʈ�� Ű�ų� ����� �Լ�
    /// </summary>
    /// <param name="isShow"></param>
    public void ShowWeaponEffect(bool isShow = true)
    {
        onWeaponEffectEnable?.Invoke(isShow);
    }
}
