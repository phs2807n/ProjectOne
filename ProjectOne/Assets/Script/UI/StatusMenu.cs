using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StatusMenu : MonoBehaviour
{
    PlayerInputActions inputActions;

    Transform status;
    Transform skill;

    bool isStatusOpen = false;
    bool isSkillOpen = false;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        status = transform.GetChild(0);
        skill = transform.GetChild(1);
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Status.performed += onStatusOpen;
        inputActions.Player.Skill.performed += onSkillOpen;
    }

    private void OnDisable()
    {
        inputActions.Player.Skill.performed -= onSkillOpen;
        inputActions.Player.Status.performed -= onStatusOpen;
        inputActions.Player.Disable();
    }

    private void onSkillOpen(InputAction.CallbackContext context)
    {
        isSkillOpen = !isSkillOpen;
        skill.gameObject.SetActive(isSkillOpen);

    }

    private void onStatusOpen(InputAction.CallbackContext context)
    {
        isStatusOpen = !isStatusOpen;
        status.gameObject.SetActive(isStatusOpen);
    }
}
