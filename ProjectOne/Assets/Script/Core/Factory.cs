using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

/// <summary>
/// 오브젝트 풀을 사용하는 오브젝트의 종류
/// </summary>
public enum PoolObjectType
{
    Enemy = 0,
    PlayBullet,
    Shovel,
    Sickle,
    Trident
}

public class Factory : Singleton<Factory>
{
    // 오브젝트 풀들
    EnemyPool enemy;
    BulletPool bullet;
    ShovelPool shovel;
    SicklePool sickle;
    TridentPool trident;

    /// <summary>
    /// 씬이 로딩 완료될 때마다 실행되는 초기화 함수
    /// </summary>
    protected override void OnInitialize()
    {
        base.OnInitialize();

        // 풀 컴포넌트 찾고, 찾으면 초기화하기
        enemy = GetComponentInChildren<EnemyPool>();
        if (enemy != null)
            enemy.Initialized();

        bullet = GetComponentInChildren<BulletPool>();  // 나와 내 자식 오브젝트에서 컴포넌트 찾음
        if (bullet != null)
            bullet.Initialized();
        
        shovel = GetComponentInChildren<ShovelPool>();
        if(shovel != null)
            shovel.Initialized();

        sickle = GetComponentInChildren<SicklePool>();
        if(sickle != null)
            sickle.Initialized();

        trident = GetComponentInChildren<TridentPool>();
        if(trident != null)
            trident.Initialized();
    }

    /// <summary>
    /// 풀에 있는 게임 오브젝트 하나 가져오기
    /// </summary>
    /// <param name="type">가져올 오브젝트의 종류</param>
    /// <returns>활성화된 오브젝트</returns>
    public GameObject GetObject(PoolObjectType type, Vector3? position = null, Vector3? eular = null)
    {
        GameObject result = null;

        switch (type)
        {
            case PoolObjectType.PlayBullet:
                result = bullet.GetObject(position, eular).gameObject;
                break;
            case PoolObjectType.Enemy:
                result = enemy.GetObject().gameObject;
                break;
            case PoolObjectType.Shovel:
                result = shovel.GetObject().gameObject; 
                break;
            case PoolObjectType.Sickle:
                result = sickle.GetObject().gameObject;
                break;
            case PoolObjectType.Trident:
                result = trident.GetObject().gameObject;
                break;
        }

        return result;
    }

    /// <summary>
    /// 풀에 있는 게임 오브젝트 하나 가져오기
    /// </summary>
    /// <param name="type">가져울 오브젝트의 종류</param>
    /// <param name="position">오브젝트가 배치될 위치</param>
    /// <returns>활성화된 오브젝트</returns>
    public GameObject GetObject(PoolObjectType type, Vector3 position)
    {
        GameObject obj = GetObject(type);       // 가져와서
        obj.transform.position = position;      // 위치 적용

        //switch (type)       // 개별적으로 추가 처리가 필요한 오브젝트
        //{
        //    case PoolObjectType.Enemy:
        //        Enemy enemy = obj.GetComponent<Enemy>();
        //        enemy.SetStartPosition(position);   // 적의 spawnY 지정
        //        break;  
        //}

        return obj;
    }

    /// <summary>
    /// 총알 하나 가져오는 함수
    /// </summary>
    /// <returns></returns>
    public Bullet GetBullet()
    {
        return bullet.GetObject();
    }

    /// <summary>
    /// 총알 하나 가져와서 특정 위치에 배치되는 함수
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Bullet GetBullet(Vector3 position, float angle = 0.0f)
    {
        return bullet.GetObject(position, angle * Vector3.forward);
    }

    public Enemy GetEnemy()
    {
        return enemy.GetObject();
    }

    public Enemy GetEnemy(Vector3 position, float angle = 0.0f)
    {
        return enemy.GetObject(position, angle * Vector3.forward);
    }

    public Bullet GetShovel()
    {
        return shovel.GetObject();
    }

    public Bullet GetShovel(Vector3 position, float angle = 0.0f)
    {
        return shovel.GetObject(position, angle * Vector3.forward);
    }

    public Sickle GetSickle()
    {
        return sickle.GetObject();
    }

    public Sickle GetSickle(Vector3 position, float angle = 0.0f)
    {
        return sickle.GetObject(position, angle * Vector3.forward);
    }

    public Trident GetTrident()
    {
        return trident.GetObject();
    }

    public Trident GetTrident(Vector3 position, float angle = 0.0f)
    {
        return trident.GetObject(position, angle * Vector3.forward);
    }
}
