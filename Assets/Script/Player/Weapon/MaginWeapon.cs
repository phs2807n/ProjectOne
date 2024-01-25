using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MaginWeapon : Weapon_Base
{
    Weapon weapon;

    public Transform fireTransform;

    Transform shovelTransform;

    PlayerState player;

    IEnumerator[] calltimer = new IEnumerator[3];

    protected override void Awake()
    {
        base.Awake();

        shovelTransform = transform.GetChild(0);

        player = FindObjectOfType<PlayerState>();

        for(int i = 0; i < calltimer.Length; i++)
        {
            calltimer[i] = CallTimeCoroutine(calltime);
        }
    }

    private void Update()
    {
         shovelTransform.Rotate(Vector3.back * 150 * Time.deltaTime);
    }

    public void Magin()
    {
        StartCoroutine(calltimer[id]);
    }

    void StartMagin()
    {
        Debug.Log("¸¶¹ý");
        if(UseMp < player.curMp)
        {
            player.curMp -= UseMp;
            switch (id)
            {
                case 0:
                    // »ð
                    Batch();
                    break;
                case 1:
                    //²¿Ã¬ÀÌ
                    onTrident();
                    break;
                case 2:
                    // ³´
                    onSickle();
                    break;
            }
        }
        else
        {
            Debug.Log("¸¶·ÂÀÌ ºÎÁ·ÇÕ´Ï´Ù.");
        }
    }

    IEnumerator CallTimeCoroutine(float value)
    {
        while(true)
        {
            StartMagin();
            yield return new WaitForSeconds(value);
        }
    }

    void Batch()
    {
        Debug.Log(count);
        for (int index = 0; index < count; index++)
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

            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(-1, damage, 150, Vector3.zero); // -1 is Infinity per.
        }
    }

    void onSickle()
    {
        Vector3 dir = GameManager.Instance.GetMousePostion() - GameManager.Instance.player_moving.transform.position;
        dir = dir.normalized;

        Sickle bullet = Factory.Instance.GetSickle(fireTransform.position);

        Debug.Log("¹ß»ç");
        bullet.transform.localScale = new Vector3(count, count, count);
        bullet.Init(-1,
            GameManager.Instance.player_state.Atk * damage,
            GameManager.Instance.player_state.Atk_speed,
            dir);
    }

    void onTrident()
    {
        for(int i = 0; i < count; i++)
        {
            Trident bullet = Factory.Instance.GetTrident(fireTransform.position);

            Debug.Log("¹ß»ç");
            bullet.transform.localScale = new Vector3(count, count, count);
            bullet.Init(-1,
                GameManager.Instance.player_state.Atk * damage,
                GameManager.Instance.player_state.Atk_speed,
                Vector3.zero);
        }
    }
}
