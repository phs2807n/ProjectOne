using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Weapon : Weapon_Base
{
    [Header("총")]
    /// <summary>
    /// 연사를 실행할 코루틴
    /// </summary>
    IEnumerator fireCoroutine;

    public Transform[] fireTransforms;

    /// <summary>
    /// 스킬의 쿨타임
    /// </summary>
    public float cooltime_max;

    /// <summary>
    /// 현재 쿨타임
    /// </summary>
    protected float cooltime;

    /// <summary>
    /// 격발 가능 여부
    /// </summary>
    public bool isShot = true;

    /// <summary>
    /// 격발 여부 변경용 코르틴
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
        Debug.Log("행동");
        switch (id)
        {
            case 0:
                if(isShot)
                    onFire();
                CoolTimeStart();
                break;
            case 1:
                //onFire();
                Debug.Log("코르틴 시작");
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

        // 콜타임 설정
        cooltime_max = calltime;
        cooltime = cooltime_max;
        Debug.Log("쿨타임 시작");

        isShot = false;

        StartCoroutine(cooltimeCoroutine);
    }

    void CoolTimeEnd()
    {
        Debug.Log("쿨타임 끝");

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

            Debug.Log("발사");
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

                Debug.Log("발사");
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
