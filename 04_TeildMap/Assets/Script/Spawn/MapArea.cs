using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapArea : MonoBehaviour
{
    /// <summary>
    /// 맵의 배경(전체 크기)용 타일맵
    /// </summary>
    Tilemap background;

    /// <summary>
    /// 맵의 벽 확인용 타일맵
    /// </summary>
    Tilemap obstacle;

    /// <summary>
    /// 타일맵으로 생성한 그리드맵(A*맵)
    /// </summary>
    TileGridMap gridMap;
    public TileGridMap GridMap => gridMap;

    /// <summary>
    /// 이 맵영역에 있는 모든 스포너
    /// </summary>
    Spawner[] spawners;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        background = child.GetComponent<Tilemap>();         // 배경 찾기

        child = transform.GetChild(1);
        obstacle = child.GetComponent<Tilemap>();           // 벽 찾기

        gridMap = new TileGridMap(background, obstacle);    // 그리드맵 생성

        child = transform.GetChild(2);
        spawners = child.GetComponentsInChildren<Spawner>(); // 스포너 모두 찾기
    }

    public List<Node> CalcSpawnArea(Spawner spawner)
    {
        List<Node> result = new List<Node>();

        Vector2Int min = gridMap.WorldToGrid(spawner.transform.position);
        Vector2Int max = gridMap.WorldToGrid(spawner.transform.position + (Vector3)spawner.size);
        
        for(int y = min.y; y < max.y; y++)
        {
            for (int x = min.x; x < max.x; x++)
            {
                if (!gridMap.IsWall(x, y))
                {
                    result.Add(gridMap.GetNode(x, y));
                }
            }
        }

        return result;
    }

    /// <summary>
    /// 맵의 그리드 좌표를 월드 좌표로 변경해주는 함수
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Vector2 GridToWorld(int x, int y)
    {
        return GridMap.GridToWorld(new(x, y));
    }
}
