using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Trident : Bullet
{
    public Scanner scanner;

    PlayerMoving player;

    /// <summary>
    /// Å¸°Ù À§Ä¡
    /// </summary>
    Transform targer;

    bool IsTracking = true;

    private void Start()
    {
        player = GameManager.Instance.player_moving;   
    }

    protected override void Update()
    {
        Tracking();

        Dir = (targer.position - transform.position).normalized;

        transform.rotation = Quaternion.FromToRotation(Vector3.up, Dir);

        base.Update();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        StartCoroutine(NextCoroutine());
    }

    IEnumerator NextCoroutine()
    {
        IsTracking = false;

        yield return new WaitForSeconds(0.5f);

        IsTracking = true;
    }

    void Tracking()
    {
        if (IsTracking)
        {
            targer = scanner.nearestTarger;

            if (targer == null)
            {
                targer = player.transform;
            }
        }
        else
        {
            targer = transform;
        }
    }
}
