using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Weapon : Weapon_Base
{
    [Header("��")]
    /// <summary>
    /// ���縦 ������ �ڷ�ƾ
    /// </summary>
    IEnumerator fireCoroutine;

    public Transform[] fireTransforms;

    /// <summary>
    /// ��ų�� ��Ÿ��
    /// </summary>
    public float cooltime_max;

    /// <summary>
    /// ���� ��Ÿ��
    /// </summary>
    protected float cooltime;

    /// <summary>
    /// �ݹ� ���� ����
    /// </summary>
    public bool isShot = true;

    /// <summary>
    /// �ݹ� ���� ����� �ڸ�ƾ
    /// </summary>
    public IEnumerator cooltimeCoroutine;

    protected override void Awake()
    {
        base.Awake();

        fireCoroutine = FireCoroutine();

        Transform fireRoot = transform.GetChild(0);
        fireTransforms = new Transform[fireRoot.childCount];
        for(int i = 0; i < fireTransforms.Length; i++)
        {
            fireTransforms[i] = fireRoot.GetChild(i);
        }
    }

    public void Behaviour()
    {
        Debug.Log("�ൿ");
        switch (id)
        {
            case 0:
                if(isShot)
                    onFire();
                CoolTimeStart();
                break;
            case 1:
                //onFire();
                Debug.Log("�ڸ�ƾ ����");
                StartCoroutine(fireCoroutine);
                break;
            case 2:
                StartCoroutine(fireCoroutine);
                break;
        }
    }

    public void StopBehaviour()
    {
        StopCoroutine(fireCoroutine);
    }

    float Angle()
    {
        Vector2 dir = GameManager.Instance.GetMousePostion() - GameManager.Instance.player_moving.transform.position;
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

    IEnumerator FireCoroutine()
    {
        while (true)
        {
            if(isShot)
            {
                onFire();
            }
            yield return new WaitForSeconds(calltime);
            isShot = true;
        }
    }

    protected IEnumerator CoolTimeCoroutine()
    {
        while (cooltime > 0.0f)
        {
            cooltime -= Time.deltaTime;

            yield return new WaitForFixedUpdate();
        }

        CoolTimeEnd();
    }

    public void CoolTimeStart()
    {
        cooltimeCoroutine = CoolTimeCoroutine();

        // ��Ÿ�� ����
        cooltime_max = calltime;
        cooltime = cooltime_max;
        Debug.Log("��Ÿ�� ����");

        isShot = false;

        StartCoroutine(cooltimeCoroutine);
    }

    void CoolTimeEnd()
    {
        Debug.Log("��Ÿ�� ��");

        isShot = true;

        StopCoroutine(cooltimeCoroutine);
    }

    void onFire()
    {
        isShot = false;

        Vector3 dir = GameManager.Instance.GetMousePostion() - transform.position;
        dir = dir.normalized;

        RefreshFirePositions();

        if (!isShotGun)
        {
            fireTransforms[0].rotation = Quaternion.Euler(0, 0, 0);
            fireTransforms[0].gameObject.SetActive(false);
            Bullet bullet = Factory.Instance.GetBullet(fireTransforms[0].position, Angle() - 90.0f);

            Debug.Log("�߻�");
            bullet.Init(count,
                GameManager.Instance.player_state.Atk * damage,
                GameManager.Instance.player_state.Atk_speed,
                dir);
        }
        else
        {
            for(int i = 0; i < count; i++)
            {
                Bullet bullet = Factory.Instance.GetBullet(fireTransforms[i].position, Angle() - 90.0f);

                Debug.Log("�߻�");
                bullet.Init(1,
                    GameManager.Instance.player_state.Atk * damage,
                    GameManager.Instance.player_state.Atk_speed,
                    (dir + AngleToDir(fireTransforms[i].eulerAngles.z)).normalized);
                Debug.Log(dir + AngleToDir(fireTransforms[i].eulerAngles.z));
            }
        }

        AudioManager.Instance.PlaySfx(Sfx.Range);
    }

    Vector3 AngleToDir(float angle)
    {
        return new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0.0f);
    }

    void RefreshFirePositions()
    {
        for(int i = 0; i < count;i++)
        {
            if (isShotGun)
            {
                float angleDelta = 3 * Random.Range(-3.0f, 3.0f);
                fireTransforms[i].rotation = Quaternion.Euler(0, 0, angleDelta);

                fireTransforms[i].localPosition = Vector3.zero;

                fireTransforms[i].gameObject.SetActive(true);
            }
            else
            {
                fireTransforms[i].gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    protected override void LateUpdate()
    {
        base.LateUpdate();
        //if (GameManager.instance.player_moving.transform.position.x > GameManager.instance.GetMousePostion().x)
        //{
        //    transform.localPosition = rightPos;

        //    filp = true;
        //}
        //else
        //{
        //    transform.localPosition = rightPosReverse;

        //    filp = false;
        //}
        //sprite.flipY = filp;
        transform.rotation = Quaternion.Euler(0, 0, Angle());
    }

}
