using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StatusMenu : MonoBehaviour
{
    PlayerInputActions inputActions;

    Transform status;

    bool isOpen;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        status = transform.GetChild(0);
        isOpen = false;
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Status.performed += onStatusOpen;
    }

    private void OnDisable()
    {
        inputActions.Player.Status.performed -= onStatusOpen;
        inputActions.Player.Disable();
    }

    private void onStatusOpen(InputAction.CallbackContext context)
    {
        isOpen = !isOpen;
        status.gameObject.SetActive(isOpen);
    }
}
