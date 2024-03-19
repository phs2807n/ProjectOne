using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomIdleSelector : StateMachineBehaviour
{
    readonly int IdleSelect_Hash = Animator.StringToHash("IdleSelect");

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger(IdleSelect_Hash, RandomSelect());
    }

    int RandomSelect()
    {
        int select = 0;
        float num = Random.value;

        if(num < 0.05f)
        {
            select = 5;
        }
        else if(num < 0.10f)
        {
            select = 4;
        }
        else if (num < 0.15f)
        {
            select = 3;
        }
        else if (num < 0.20f)
        {
            select = 2;
        }
        else
        {
            select = 1;
        }

        return select;
    }
}
