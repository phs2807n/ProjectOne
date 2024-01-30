using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Trident : Bullet
{
    public Scanner scanner;

    PlayerMoving player;

    private void Start()
    {
        player = GameManager.Instance.player_moving;   
    }

    protected override void Update()
    {
        Transform targer = scanner.nearestTarger;

        if(targer == null)
        {
            targer = player.transform;
        }

        Dir = (targer.position - transform.position).normalized;

        transform.rotation = Quaternion.FromToRotation(Vector3.up, Dir);

        base.Update();
    }
}
