using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character : RecycleObject
{
    [Header("# 최종 스택")]
    /// <summary>
    /// 최대 체력
    /// </summary>
    public float maxHp;
    /// <summary>
    /// 현재체력
    /// </summary>
    public float curHp;

    /// <summary>
    /// 최대 마력
    /// </summary>
    public int maxMp;

    /// <summary>
    /// 현재 마력
    /// </summary>
    public int curMp;

    /// <summary>
    /// 공격력
    /// </summary>
    public float Atk;
    /// <summary>
    /// 방어력
    /// </summary>
    public float Def;
    /// <summary>
    /// 속도
    /// </summary>
    public float Agi;


    public float Atk_speed
    {
        get => Agi * 1.5f;
    }

    public bool isLive => curHp > 0;

    protected override void OnEnable()
    {
        base.OnEnable();

        curHp = maxHp;
    }
}
