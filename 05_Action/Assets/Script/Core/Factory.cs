using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectType
{
}

public class Factory : Singleton<Factory>
{

    protected override void OnInitialize()
    {
        base.OnInitialize();
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

        return result;
    }

}
