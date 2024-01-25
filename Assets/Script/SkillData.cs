using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData : ScriptableObject
{
    public enum Type
    {
        Weapon
    }

    [Header("# Main Info")]
    public Type skillType;
    public int skillId;
    public string skillName;
    public string skillDescription;
    public Sprite skillIcon;

    [Header("# Level Datat")]
    public float baseDamge;
    public int baseCount;
    public float calltime;
    public float[] LevelDamge;
    public int[] LevelCount;
}
