using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { Exp, Level, Kill, Time, Health, MP }
    public InfoType type;

    Text myText;
    Slider mySlider;

    void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:
                float curExp = GameManager.Instance.player_state.curExp;
                float maxExp = GameManager.Instance.player_state.nextExp[GameManager.Instance.player_state.Level];
                mySlider.value = curExp / maxExp;
                break;
            case InfoType.Level:
                myText.text = string.Format("Lv.{0:F0}", GameManager.Instance.player_state.Level + 1);
                break;
            case InfoType.Kill:
                //myText.text = string.Format("{0:F0}", GameManager.Instance.kill);
                break;
            case InfoType.Time:
                float remainTime = GameManager.Instance.gameTime;
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;
            case InfoType.Health:
                float curHealth = GameManager.Instance.player_state.curHp;
                float maxHealth = GameManager.Instance.player_state.maxHp;
                mySlider.value = curHealth / maxHealth;
                break;
            case InfoType.MP:
                float curMp = GameManager.Instance.player_state.curMp;
                float maxMp = GameManager.Instance.player_state.maxMp;
                mySlider.value = curMp / maxMp;
                break;
        }
    }
}
