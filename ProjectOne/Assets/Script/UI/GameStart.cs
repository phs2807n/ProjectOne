using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    Transform[] childTransform;
    Transform UI;
    PlayerMoving player;


    private void Awake()
    {
        UI = transform.GetChild(0);
        childTransform = new Transform[transform.childCount - 1];
        for(int i = 0; i < childTransform.Length; i++)
        {
            childTransform[i] = transform.GetChild(i + 1);
        }
        player = FindObjectOfType<PlayerMoving>(true);
    }

    public void OnGameStart()
    {   
        GameManager.Instance.isStart = true;
        
        for(int i = 0;i < childTransform.Length;i++)
        {
            childTransform[i].gameObject.SetActive(false);
        }

        AudioManager.Instance.PlayBgm(true);
        AudioManager.Instance.PlaySfx(Sfx.Select);

        UI.gameObject.SetActive(true);
        player.gameObject.SetActive(true);
    }
}
