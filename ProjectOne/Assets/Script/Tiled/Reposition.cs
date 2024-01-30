using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D coll;
    PlayerMoving player;

    void Awake()
    {
        coll = GetComponent<Collider2D>();
        player = GameManager.Instance.player_moving;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
        {
            return;
        }

        Vector3 playerPos = player.transform.position;
        
        Vector3 myPos = transform.position;

        // 코딩 변경
        float dirX = playerPos.x - myPos.x;
        float dirY = playerPos.y - myPos.y;

        float diffX = Mathf.Abs(dirX);
        float diffY = Mathf.Abs(dirY);

        dirX = dirX > 0 ? 1 : -1;
        dirY = dirY > 0 ? 1 : -1;

        Vector3 playerDir = GameManager.Instance.player_moving.inputedVector;

        switch(transform.tag)
        {
            case "Ground":
                if (diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 80);
                }
                else if (diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 80);
                }
                break;
            case "Enemy":
                if(coll.enabled)
                {
                    transform.Translate(playerDir * 20 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f));
                }
                break;
        }
    }
}
