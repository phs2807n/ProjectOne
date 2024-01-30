using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;
using UnityEngine.InputSystem;

public class MpPotion : Skill_Base
{ 
    protected override void OnMpPotion(InputAction.CallbackContext context)
    {
        if (isUse)
        {
            OnReMind();
            CoolTimeStart();
        }
        else
        {
            Debug.Log("쿨타임 중입니다.");
        }
    }

    void OnReMind()
    {
        player.curMp += count[Level];
        if (player.curMp > player.maxMp)
        {
            player.curMp = player.maxMp;
        }
    }
}
