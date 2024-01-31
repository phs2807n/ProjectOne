using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusHUD : MonoBehaviour
{
    public StatusType type;
    public bool IsPoint;

    Text myText;


    private void Awake()
    {
        myText = GetComponent<Text>();
    }

    private void LateUpdate()
    {
        if (!IsPoint)
        {
            switch (type)
            {
                case StatusType.Hp:
                    myText.text = $"{GameManager.Instance.player_state.maxHp}";
                    break;
                case StatusType.Mp:
                    myText.text = $"{GameManager.Instance.player_state.maxMp}";
                    break;
                case StatusType.Atk:
                    myText.text = $"{GameManager.Instance.player_state.Atk}";
                    break;
                case StatusType.Def:
                    myText.text = $"{GameManager.Instance.player_state.Def}";
                    break;
                case StatusType.Agi:
                    myText.text = $"{GameManager.Instance.player_state.Agi}";
                    break;
                case StatusType.StatusPoint:
                    myText.text = $"현재 포인트 : {GameManager.Instance.player_state.Status_point}";
                    break;
                case StatusType.SkillPoint:
                    myText.text = $"현재 포인트 : {GameManager.Instance.player_state.Skill_point}";
                    break;
            }
        }
        else
        {
            switch (type)
            {
                case StatusType.Hp:
                    myText.text = $"{GameManager.Instance.player_state.point_Hp}";
                    break;
                case StatusType.Mp:
                    myText.text = $"{GameManager.Instance.player_state.point_Mp}";
                    break;
                case StatusType.Atk:
                    myText.text = $"{GameManager.Instance.player_state.point_Atk}";
                    break;
                case StatusType.Def:
                    myText.text = $"{GameManager.Instance.player_state.point_Def}";
                    break;
                case StatusType.Agi:
                    myText.text = $"{GameManager.Instance.player_state.point_Agi}";
                    break;
                case StatusType.SkillPoint:
                    myText.text = $"현재 포인트 : {GameManager.Instance.player_state.Status_point}";
                    break;
            }
        }
    }
}
