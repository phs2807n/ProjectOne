using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HpPotion : Skill_Base
{
    protected override void OnHpPotion(InputAction.CallbackContext context)
    {
        if (isUse)
        {
            OnHeal();
            CoolTimeStart();
        }
        else
        {
            Debug.Log("쿨타임 중입니다.");
        }
    }

    void OnHeal()
    {
        Debug.Log($"{count[Level]}");
        player.curHp += (float)count[Level];
        if(player.curHp > player.maxHp)
        {
            player.curHp = player.maxHp;
        }
    }
}
