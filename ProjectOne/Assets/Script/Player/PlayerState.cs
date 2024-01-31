using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusType
{
    Hp, Mp, Atk, Def, Agi, StatusPoint, SkillPoint
}

public class PlayerState : Character
{
    [Header("# 포인트 스택")]
    public int point_Hp;
    public int point_Mp;
    public int point_Atk;
    public int point_Def;
    public int point_Agi;

    public Character basicState;

    public int Level;
    public float curExp;
    public float[] nextExp = { 10, 30, 60, 100, 150, 210, 280, 360, 450, 600 };
    public int Status_point;
    public int Skill_point;

    public Action onDie;

    private float HP
    {
        get => curHp;
        set
        {
            if(curHp != value)
            {
                curHp = value;
                Debug.Log(isLive);
                if (!isLive)
                {
                    onDie();
                }

                curHp = Mathf.Clamp(curHp, 0, maxHp);
            }
        }
    }


    private void Start()
    {
        basicState = GetComponent<Character>();
        Set();
        onSetting();
    }

    public void OnDamge(float damge)
    {
        Debug.Log(damge);
        if(damge < 0)
        {
            Debug.Log("너무 약합니다.");
            damge = 0;
        }
        curHp -= damge;

        if(curHp < 1)
        {
            OnDie();
        }
    }

    /// <summary>
    /// 경험치 얻는 메서드
    /// </summary>
    public void GetExp()
    {
        curExp++;

        if (curExp == nextExp[Level])
        {
            AudioManager.Instance.PlaySfx(Sfx.LevelUp);

            Level++;
            curExp = 0;
            Status_point += 3;
            Skill_point += 3;

            onSetting();
        }
    }

    /// <summary>
    /// 스택 증가 메서드
    /// </summary>
    /// <param name="type"></param>
    public void State_Up(StatusType type)
    {
        AudioManager.Instance.PlaySfx(Sfx.Select);
        Status_point--;
        if (Status_point < 0)
        {
            Debug.Log("포인트가 없습니다.");
            Status_point = 0;
            return;
        }

        switch(type)
        {
            case StatusType.Hp:
                point_Hp++;
                break;
            case StatusType.Mp:
                point_Mp++;
                break;
            case StatusType.Atk:
                point_Atk++;
                break;
            case StatusType.Def:
                point_Def++;
                break;
            case StatusType.Agi:
                point_Agi++;
                break;
        }

        Set();
    }

    public void State_Down(StatusType type)
    {
        AudioManager.Instance.PlaySfx(Sfx.Select);
        Status_point++;
        switch (type)
        {
            case StatusType.Hp:
                point_Hp--;
                if(point_Hp < 0)
                {
                    Debug.Log("포인터가 없습니다.");
                    point_Hp = 0;
                    Status_point--;
                }
                break;
            case StatusType.Mp:
                point_Mp--;
                if (point_Mp < 0)
                {
                    Debug.Log("포인터가 없습니다.");
                    point_Mp = 0;
                    Status_point--;
                }
                break;
            case StatusType.Atk:
                point_Atk--;
                if (point_Atk < 0)
                {
                    Debug.Log("포인터가 없습니다.");
                    point_Atk = 0;
                    Status_point--;
                }
                break;
            case StatusType.Def:
                point_Def--;
                if (point_Def < 0)
                {
                    Debug.Log("포인터가 없습니다.");
                    point_Def = 0;
                    Status_point--;
                }
                break;
            case StatusType.Agi:
                point_Agi--;
                if (point_Agi < 0)
                {
                    Debug.Log("포인터가 없습니다.");
                    point_Agi = 0;
                    Status_point--;
                }
                break;
        }

        Set();
    }

    public void Set()
    {
        this.maxHp = basicState.maxHp + point_Hp * 5 + Level * 10;
        this.maxMp = basicState.maxMp + point_Mp * 3 + Level * 5;
        this.Atk = basicState.Atk + point_Atk + Level;
        this.Def = basicState.Def + point_Def + Level;
        this.Agi = basicState.Agi + point_Agi + Level;
    }

    public void onSetting()
    {
        this.curHp = maxHp;
        this.curMp = maxMp;
    }

    void OnDie()
    {
        AudioManager.Instance.PlaySfx(Sfx.Lose);
        onDie?.Invoke();
    }
}
