using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Trident_Skill : Skill_Base
{
    protected override void OnAction2(InputAction.CallbackContext context)
    {
        if (isUse)
        {
            if (player.curMp > UseMp[Level])
            {
                player.curMp -= UseMp[Level];
                for (int i = 0; i < count[Level]; i++)
                    onTrident();
                CoolTimeStart();
            }
            else
            {
                Debug.Log("마나가 부족합니다.");
            }
        }
        else
        {
            Debug.Log("쿨타임 중입니다.");
        }
    }

    void onTrident()
    {
        Trident bullet = Factory.Instance.GetTrident(fireTransform.position);

        Debug.Log("발사");

        bullet.Init(-1,
            GameManager.Instance.player_state.Atk * damage[Level],
            GameManager.Instance.player_state.Atk_speed * Random.Range(0,2.0f),
            Vector3.zero);
    }
}
