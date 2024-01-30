using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

/// <summary>
/// ������Ʈ Ǯ�� ����ϴ� ������Ʈ�� ����
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
    // ������Ʈ Ǯ��
    EnemyPool enemy;
    BulletPool bullet;
    ShovelPool shovel;
    SicklePool sickle;
    TridentPool trident;

    /// <summary>
    /// ���� �ε� �Ϸ�� ������ ����Ǵ� �ʱ�ȭ �Լ�
    /// </summary>
    protected override void OnInitialize()
    {
        base.OnInitialize();

        // Ǯ ������Ʈ ã��, ã���� �ʱ�ȭ�ϱ�
        enemy = GetComponentInChildren<EnemyPool>();
        if (enemy != null)
            enemy.Initialized();

        bullet = GetComponentInChildren<BulletPool>();  // ���� �� �ڽ� ������Ʈ���� ������Ʈ ã��
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
    /// Ǯ�� �ִ� ���� ������Ʈ �ϳ� ��������
    /// </summary>
    /// <param name="type">������ ������Ʈ�� ����</param>
    /// <returns>Ȱ��ȭ�� ������Ʈ</returns>
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
    /// Ǯ�� �ִ� ���� ������Ʈ �ϳ� ��������
    /// </summary>
    /// <param name="type">������ ������Ʈ�� ����</param>
    /// <param name="position">������Ʈ�� ��ġ�� ��ġ</param>
    /// <returns>Ȱ��ȭ�� ������Ʈ</returns>
    public GameObject GetObject(PoolObjectType type, Vector3 position)
    {
        GameObject obj = GetObject(type);       // �����ͼ�
        obj.transform.position = position;      // ��ġ ����

        //switch (type)       // ���������� �߰� ó���� �ʿ��� ������Ʈ
        //{
        //    case PoolObjectType.Enemy:
        //        Enemy enemy = obj.GetComponent<Enemy>();
        //        enemy.SetStartPosition(position);   // ���� spawnY ����
        //        break;  
        //}

        return obj;
    }

    /// <summary>
    /// �Ѿ� �ϳ� �������� �Լ�
    /// </summary>
    /// <returns></returns>
    public Bullet GetBullet()
    {
        return bullet.GetObject();
    }

    /// <summary>
    /// �Ѿ� �ϳ� �����ͼ� Ư�� ��ġ�� ��ġ�Ǵ� �Լ�
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
