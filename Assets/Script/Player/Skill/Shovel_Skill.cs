using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shovel_Skill : Skill_Base
{
    Transform shovelTransform;

    protected override void Awake()
    {
        base.Awake();

        Transform playerTrnasform = GameManager.Instance.player_moving.transform;
        shovelTransform = playerTrnasform.GetChild(1);
    }

    protected override void OnAction1(InputAction.CallbackContext context)
    {
        if (isUse)
        {
            if(player.curMp > UseMp[Level])
            {
                player.curMp -= UseMp[Level];
                Batch();
                CoolTimeStart();
            }
            else
            {
                Debug.Log("마나가 부족합니다.");
            }
        }
        else
        {
            Debug.Log("쿨타임 중입니다.");
        }
    }

    void LevelUp()
    {
        Level++;
    }

    void Batch()
    {
        Debug.Log(count);
        for (int index = 0; index < count[Level]; index++)
        {
            Transform bullet;

            if (index < shovelTransform.childCount)
            {
                bullet = shovelTransform.GetChild(index);
                bullet.gameObject.SetActive(true);
            }
            else
            {
                bullet = Factory.Instance.GetShovel().transform;
                bullet.parent = shovelTransform;
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count[Level];
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(-1, damage[Level], 150, Vector3.zero); // -1 is Infinity per.
        }
    }
    private void Update()
    {
        shovelTransform.Rotate(Vector3.back * 150 * Time.deltaTime);
    }
}
