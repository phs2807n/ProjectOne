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
    /// 풀에 있는 게임 오브젝트 하나 가져오기
    /// </summary>
    /// <param name="type">가져올 오브젝트의 종류</param>
    /// <param name="position">오브젝트가 배치될 위치</param>
    /// <param name="euler">오브젝트의 초기 각도</param>
    /// <returns>활성화된 오브젝트</returns>
    public GameObject GetObject(PoolObjectType type, Vector3? position = null, Vector3? euler = null)
    {
        GameObject result = null;

        return result;
    }

}
