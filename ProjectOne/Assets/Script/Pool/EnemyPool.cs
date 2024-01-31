using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : ObjectPool<Enemy>
{
    public Enemy[] enemys;

    public void Set()
    {
        enemys = new Enemy[transform.childCount];
        for (int i = 0; i < enemys.Length; i++)
        {
            enemys[i] = transform.GetChild(i).GetComponent<Enemy>();
        }
    }
}
