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
        // Ÿ�ϸ��� �׸��� ��ǥ ���ϱ�
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
        // Ŭ���� ��ġ�� Ÿ���� �ִ��� ������ Ȯ��
        Vector2 screenPosition = Mouse.current.position.ReadValue();
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        Vector2Int gridPosition = (Vector2Int)background.WorldToCell(worldPosition);

        //if(IsWall(gridPosition))
        //{
        //    Debug.Log("Ÿ�� ����");
        //}
        //else
        //{
        //    Debug.Log("Ÿ�� ����");
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
    /// ������ ��ġ�� Ÿ���� ������ ��, �ƴϸ� ���
    /// </summary>
    /// <param name="gridPosition">Ȯ���� ��ġ</param>
    /// <returns>true�� ��, false�� ���</returns>
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
        // background.size.x;   // background�� ���ο� ����ִ� ���� ����(���α���)

        Debug.Log($"background : {background.size.x}, {background.size.y}");
        Debug.Log($"obstacle : {obstacle.size.x}, {obstacle.size.y}");
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        // background.origin : background�� �ִ� �� �߿� ���� �Ʒ��� ����
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
