using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnDatas;

    int level;
    float timer;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.gameTime / 10f), spawnDatas.Length -1);

        if (timer > spawnDatas[level].spawnTime)
        {
            timer = 0;
            Spawn();
        }
    }

    void Spawn()
    {
        int number = Random.Range(0, GameManager.Instance.enemy.EnemyList.Count);
        //GameObject enemy = GameManager.Instance.pool.Get(0);
        Enemy enemy = Factory.Instance.GetEnemy(spawnPoint[Random.Range(1, spawnPoint.Length)].position);
        //enemy.GetComponent<Enemy>().Init(number);
        enemy.Init(number);
    }
}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}