using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Test_AStar_Tilemap : TestBase
{
    public Tilemap background;
    public Tilemap obstacle;

    public Vector2Int start;
    public Vector2Int end;

    public TileGridMap gridMap;

    public PathLine pathLine;

    private void Start()
    {
        gridMap = new TileGridMap(background, obstacle);

        //foreach(Node node in gridMap.nodes)
        //{
        //    Debug.Log(node);
        //}

        pathLine.ClearPath();
    }

    protected override void OnTestLClick(InputAction.CallbackContext context)
    {
        // 타일맵의 그리드 좌표 구하기
        Vector2 screenPosition = Mouse.current.position.ReadValue();
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        Vector2Int gridPosition = (Vector2Int) background.WorldToCell(worldPosition);

        if (!IsWall(gridPosition))
        {
            start = gridPosition;
        }
    }

    protected override void OnTestRClick(InputAction.CallbackContext context)
    {
        // 클릭한 위치에 타일이 있는지 없는지 확인
        Vector2 screenPosition = Mouse.current.position.ReadValue();
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        Vector2Int gridPosition = (Vector2Int)background.WorldToCell(worldPosition);

        //if(IsWall(gridPosition))
        //{
        //    Debug.Log("타일 있음");
        //}
        //else
        //{
        //    Debug.Log("타일 없음");
        //}

        if(!IsWall(gridPosition))
        {
            end = gridPosition;
            Debug.Log($"{start}, {end}");
            List<Vector2Int> path = AStar.PathFind(gridMap, start, end);
            PrintList(path);
            pathLine.DrawPath(gridMap, path);
        }
    }

    /// <summary>
    /// 지정된 위치에 타일이 있으면 벽, 아니면 빈곳
    /// </summary>
    /// <param name="gridPosition">확인할 위치</param>
    /// <returns>true면 벽, false면 빈곳</returns>
    bool IsWall(Vector2Int gridPosition)
    {
        TileBase tile = obstacle.GetTile((Vector3Int)gridPosition);
        return tile != null;
    }

    bool IsPlain(Vector2Int gridPosition)
    {
        TileBase tile = background.GetTile((Vector3Int)gridPosition);
        return tile != null;
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        // background.size.x;   // background의 가로에 들어있는 섹의 개수(가로길이)

        Debug.Log($"background : {background.size.x}, {background.size.y}");
        Debug.Log($"obstacle : {obstacle.size.x}, {obstacle.size.y}");
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        // background.origin : background에 있는 셀 중에 왼쪽 아래가 원점
        Debug.Log($"background origin : {background.origin}");
        Debug.Log($"obstacle origin : {obstacle.origin}");
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        // cellBounds
        Debug.Log($"background : {background.cellBounds.min}, {background.cellBounds.max}");
        Debug.Log($"obstacle : {obstacle.cellBounds.min}, {obstacle.cellBounds.max}");
    }

    void PrintList(List<Vector2Int> list)
    {
        string str = "";
        Debug.Log(list);
        foreach (Vector2Int v in list)
        {
            str += $"{v} -> ";
        }
        Debug.Log(str + "End");
    }
}
