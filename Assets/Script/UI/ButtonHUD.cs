using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHUD : MonoBehaviour
{
    public StatusType type;
    public bool IsDown;

    PlayerState player;

    private void Start()
    {
        player = GameManager.Instance.player_state;
    }

    public void Click()
    {
        if (player == null)
        {
            player = GameManager.Instance.player_state;
        }

        if (IsDown)
        {
            player.State_Down(type);
        }
        else
        {
            player.State_Up(type);
        }
    }
}
