using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectType
{
    Slime = 0,
}

public class Factory : Singleton<Factory>
{
    SlimePool slimePool;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        slimePool = GetComponentInChildren<SlimePool>();
        if (slimePool != null) slimePool.Initialized();
    }

    /// <summary>
    /// Ǯ�� �ִ� ���� ������Ʈ �ϳ� ��������
    /// </summary>
    /// <param name="type">������ ������Ʈ�� ����</param>
    /// <param name="position">������Ʈ�� ��ġ�� ��ġ</param>
    /// <param name="euler">������Ʈ�� �ʱ� ����</param>
    /// <returns>Ȱ��ȭ�� ������Ʈ</returns>
    public GameObject GetObject(PoolObjectType type, Vector3? position = null, Vector3? euler = null)
    {
        GameObject result = null;
        switch(type)
        {
            case PoolObjectType.Slime:
                result = slimePool.GetObject(position, euler).gameObject; 
                break;
        }

        return result;
    }

    /// <summary>
    /// ������ �ϳ� �������� �Լ�
    /// </summary>
    /// <returns>��ġ�� ������ �ϳ�</returns>
    public Slime GetSlime()
    {
        return slimePool.GetObject();
    }

    /// <summary>
    /// ������ �ϳ��� Ư�� ��ġ��, Ư�� ������ ��ġ
    /// </summary>
    /// <param name="position">��ġ�� ��ġ</param>
    /// <param name="angle">��ġ �� ���� ����</param>
    /// <returns>��ġ�� ������ �ϳ�</returns>
    public Slime GetSlime(Vector3 position, float angle = 0.0f)
    {
        return slimePool.GetObject(position, angle * Vector3.forward);
    }
}
