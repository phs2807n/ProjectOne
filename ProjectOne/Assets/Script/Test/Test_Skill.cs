using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Skill : Skill_Base
{
    protected override void OnAction1(InputAction.CallbackContext context)
    {
        base.OnAction1(context);
    }

    protected override void OnAction2(InputAction.CallbackContext context)
    {
        Debug.Log(isUse);
    }

    protected override void OnAction3(InputAction.CallbackContext context)
    {
        Debug.Log(cooltime);
    }
}
