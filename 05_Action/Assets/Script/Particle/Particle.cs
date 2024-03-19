using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Particle : MonoBehaviour
{
    PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Effect.Enable();
        inputActions.Effect.PointerMove.performed += OnPointerMove;
    }

    private void OnDisable()
    {
        inputActions.Effect.PointerMove.performed -= OnPointerMove; 
        inputActions.Effect.Disable();
    }

    private void OnPointerMove(InputAction.CallbackContext context)
    {
        Vector3 mousePos = context.ReadValue<Vector2>();            // ���콺�� ��ũ�� ��ǥ �޾ƿ���

        Debug.Log(mousePos);
        mousePos.z = 30.0f;
        Vector3 target = Camera.main.ScreenToWorldPoint(mousePos);  // ��ũ�� ��ǥ�� ���� ��ǥ�� �ٲٱ�
        transform.position = target;
    }
}
