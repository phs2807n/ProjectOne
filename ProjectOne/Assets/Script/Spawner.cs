using System.Collections;
using Unity.VisualScripting;
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

    private void Start()
    {
        foreach(var data in spawnDatas)
        {
            StartCoroutine(SpawnCoroutine(data));
        }
    }

    //void Update()
    //{
    //    timer += Time.deltaTime;
    //    level = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.gameTime / 10f), spawnDatas.Length -1);

    //    if (timer > spawnDatas[level].interval)
    //    {
    //        timer = 0;
    //        Spawn();
    //    }
    //}

    private IEnumerator SpawnCoroutine(SpawnData data)
    {
        while (true)
        {
            yield return new WaitForSeconds(data.interval);

            Enemy enemy = Factory.Instance.GetEnemy(spawnPoint[Random.Range(1, spawnPoint.Length)].position);
            //enemy.GetComponent<Enemy>().Init(number);
            enemy.Init(data.EnemyID);
        }
    }

    //void Spawn()
    //{
    //    int number = Random.Range(0, GameManager.Instance.enemy.EnemyList.Count);
    //    //GameObject enemy = GameManager.Instance.pool.Get(0);
    //    Enemy enemy = Factory.Instance.GetEnemy(spawnPoint[Random.Range(1, spawnPoint.Length)].position);
    //    //enemy.GetComponent<Enemy>().Init(number);
    //    enemy.Init(number);
    //}
}

[System.Serializable]
public struct SpawnData
{
    public SpawnData(int id, float interval)
    {
        this.EnemyID = id;
        this.interval = interval;
    }

    public int EnemyID;
    public float interval;
}