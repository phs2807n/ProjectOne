using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character : RecycleObject
{
    [Header("# ���� ����")]
    /// <summary>
    /// �ִ� ü��
    /// </summary>
    public float maxHp;
    /// <summary>
    /// ����ü��
    /// </summary>
    public float curHp;

    /// <summary>
    /// �ִ� ����
    /// </summary>
    public int maxMp;

    /// <summary>
    /// ���� ����
    /// </summary>
    public int curMp;

    /// <summary>
    /// ���ݷ�
    /// </summary>
    public float Atk;
    /// <summary>
    /// ����
    /// </summary>
    public float Def;
    /// <summary>
    /// �ӵ�
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
