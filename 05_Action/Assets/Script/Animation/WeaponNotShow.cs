using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponNotShow : StateMachineBehaviour
{
    Player player = null;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(player == null)
        {
            player = GameManager.Instance.Player;
        }
        player.ShowWeaponAndShield(false);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null)
        {
            player = GameManager.Instance.Player;
        }
        player.ShowWeaponAndShield(true);
    }
}
