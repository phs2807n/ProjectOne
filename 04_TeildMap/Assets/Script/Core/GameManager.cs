using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    Player player;
    public Player Player
    {
        get
        {
            if(player == null)
                player = FindObjectOfType<Player>();
            return player;
        }
    }

    WorldManager worldManager;

    public WorldManager World => worldManager;

    /// <summary>
    /// �������� �̵� ��θ� ���ӿ��� ���̰� ���� ������ �����ϴ� ����(true�� ���̰�, false�� �Ⱥ��δ�.)
    /// </summary>
    public bool showSlimePath = false;

    private void OnValidate()
    {
        Slime[] slimes = FindObjectsOfType<Slime>();
        foreach(Slime slime in slimes)
        {
            slime.ShowPath(showSlimePath);
        }
    }

    protected override void OnPreInitialize()
    { 
        base.OnPreInitialize();
        worldManager = GetComponent<WorldManager>();
        worldManager.PreInitialize();
    }

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();
        worldManager.Initialize();
    }
}
