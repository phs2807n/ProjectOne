using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sickle_Skill : Skill_Base
{
    protected override void OnAction3(InputAction.CallbackContext context)
    {
        if (isUse)
        {
            if (player.curMp > UseMp[Level])
            {
                player.curMp -= UseMp[Level];
                onSickle();
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

    void onSickle()
    {
        Vector3 dir = GameManager.Instance.GetMousePostion() - GameManager.Instance.player_moving.transform.position;
        dir = dir.normalized;

        Sickle bullet = Factory.Instance.GetSickle(fireTransform.position);

        Debug.Log("발사");
        bullet.transform.localScale = new Vector3(count[Level], count[Level], count[Level]);
        bullet.Init(-1,
            GameManager.Instance.player_state.Atk * damage[Level],
            GameManager.Instance.player_state.Atk_speed,
            dir);
    }
}
