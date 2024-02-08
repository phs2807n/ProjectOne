using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    public Sprite[] WeaponIcon = new Sprite[3];

    Image icon;

    private void Awake()
    {
        icon = GetComponent<Image>();
    }

    private void OnEnable()
    {
        GameManager.Instance.changeWeaon += onChangeIcon;
    }

    //private void OnDisable()
    //{
    //    GameManager.Instance.changeWeaon -= onChangeIcon;
    //}

    private void onChangeIcon(int obj)
    {
        icon.sprite = WeaponIcon[obj];
    }
}
