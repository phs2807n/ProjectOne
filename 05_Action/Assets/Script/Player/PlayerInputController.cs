using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerInputController : MonoBehaviour
{
    // 입력용 인풋 액션
    PlayerInputActions inputAction;

    public Action<Vector2, bool> onMove;
    public Action onMoveModeChange;
    public Action onAttack;

    private void Awake()
    {
        inputAction = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputAction.Player.Enable();
        inputAction.Player.Move.performed += OnMove;
        inputAction.Player.Move.canceled += OnMove;
        inputAction.Player.MoveModeChange.performed += OnMoveModeChange;
        inputAction.Player.Attack.performed += OnAttack;
    }

    private void OnDisable()
    {
        inputAction.Player.Attack.performed -= OnAttack;
        inputAction.Player.MoveModeChange.performed -= OnMoveModeChange;
        inputAction.Player.Move.canceled -= OnMove;
        inputAction.Player.Move.performed -= OnMove;
        inputAction.Player.Disable();
    }



    /// <summary>
    /// 이동 입력 처리용 함수
    /// </summary>
    /// <param name="context"></param>
    private void OnMove(InputAction.CallbackContext context)
    {
        Vector3 input = context.ReadValue<Vector2>();
        onMove?.Invoke(input, !context.canceled);
    }


    /// <summary>
    /// 이동 모드 변경용 함수
    /// </summary>
    /// <param name="context"></param>
    private void OnMoveModeChange(InputAction.CallbackContext context)
    {
        onMoveModeChange?.Invoke();
    }


    private void OnAttack(InputAction.CallbackContext _)
    {
        onAttack?.Invoke();
    } 
}
