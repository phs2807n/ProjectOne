using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    public MaginType type;

    public Sprite[] icons = new Sprite[3];


    PlayerState state;
    Shovel_Skill Shovel;
    Trident_Skill Trident;
    Sickle_Skill Sickle;

    Text SkillName;
    public Image Icon;

    private void Awake()
    {
        state = FindObjectOfType<PlayerState>();
        Shovel = FindObjectOfType<Shovel_Skill>();
        Trident = FindObjectOfType<Trident_Skill>();
        Sickle = FindObjectOfType<Sickle_Skill>();

        SkillName = GetComponentInChildren<Text>();
    }

    private void Start()
    {
        Set();
    }

    void Set()
    {
        Icon.sprite = icons[(int)type];

        switch (type)
        {
            case MaginType.Shovel:
                SkillName.text = $"�� Lv.{Shovel.Level + 1}";
                break;
            case MaginType.Trident:
                SkillName.text = $"â Lv.{Trident.Level + 1}";
                break;
            case MaginType.Sickle:
                SkillName.text = $"�� Lv.{Sickle.Level + 1}";
                break;
        }
    }

    /// <summary>
    /// ��ų ��
    /// </summary>
    public void SkillUp()
    {
        AudioManager.Instance.PlaySfx(Sfx.Select);

        state.Skill_point--;
        if(state.Skill_point < 1)
        {
            Debug.Log("����Ʈ�� �����ϴ�.");
            state.Skill_point = 0;
            return;
        }

        switch (type)
        {
            case MaginType.Shovel:
                Shovel.Level++;
                if(Shovel.Level > 4)
                {
                    Shovel.Level = 4;
                    state.Skill_point++;
                }
                break;
            case MaginType.Trident:
                Trident.Level++;
                if (Trident.Level > 4)
                {
                    Trident.Level = 4;
                    state.Skill_point++;
                }
                break;
            case MaginType.Sickle:
                Sickle.Level++;
                if (Sickle.Level > 4)
                {
                    Sickle.Level = 4;
                    state.Skill_point++;
                }
                break;
        }

        Set();
    }

    /// <summary>
    /// ��ų �ٿ�
    /// </summary>
    public void SkillDown()
    {
        AudioManager.Instance.PlaySfx(Sfx.Select);

        switch (type)
        {
            case MaginType.Shovel:
                Shovel.Level--;
                if(Shovel.Level < 1)
                {
                    Shovel.Level = 0;
                    Debug.Log("����Ʈ�� �����ϴ�.");
                    return;
                }
                state.Skill_point++;
                break;
            case MaginType.Trident:
                Trident.Level--;
                if (Trident.Level < 1)
                {
                    Trident.Level = 0;
                    Debug.Log("����Ʈ�� �����ϴ�.");
                    return;
                }
                state.Skill_point++;
                break;
            case MaginType.Sickle:
                Sickle.Level--;
                if (Sickle.Level < 1)
                {
                    Sickle.Level = 0;
                    Debug.Log("����Ʈ�� �����ϴ�.");
                    return;
                }
                state.Skill_point++;
                break;
        }

        Set();
    }
}