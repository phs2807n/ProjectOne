using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGridMap : GridMap
{
    /// <summary>
    /// ���� ����
    /// </summary>
    Vector2Int origin;

    /// <summary>
    /// ��� Ÿ�ϸ�(ũ�� Ȯ�� �� ��ǥ����)
    /// </summary>
    Tilemap background;

    Vector2Int[] moveblepositions;

    public TileGridMap(Tilemap background, Tilemap obstacle)
    {
        this.background = background;
        width = background.size.x;
        height = background.size.y;

        origin = (Vector2Int)background.origin; // ���� ����

        nodes = new Node[width * height];       // ��� �迭 ����

        Vector2Int min = (Vector2Int)background.cellBounds.min; // for�� ���� �ּ�/�ִ밪 ���ϱ�
        Vector2Int max = (Vector2Int)background.cellBounds.max;

        List<Vector2Int> movable = new List<Vector2Int>(width * height);

        for (int y = min.y; y < max.y; y++)
        {
            for(int x = min.x; x < max.x; x++)
            {
                if(GridToIndex(x, y, out int? index))               // �ε����� ���ϱ�
                {
                    Node.NodeType nodeType = Node.NodeType.Plain; 
                    TileBase tile = obstacle.GetTile(new(x, y));
                    if(tile != null)
                    {
                        nodeType = Node.NodeType.Wall;              // ��ֹ� Ÿ���� �ִ� ���̸� ������ ����
                    }
                    else
                    {
                        movable.Add(new(x, y));
                    }

                    nodes[index.Value] = new Node(x, y, nodeType);    // ��� ����
                }
            }
        }

        moveblepositions = movable.ToArray();   // �ӽ� ����Ʈ�� �迭�� ����
    }

    /// <summary>
    /// Ÿ�ϸ��� ������� ���� �ε��� ����
    /// </summary>
    /// <param name="x">x��ǥ</param>
    /// <param name="y">y��ǥ</param>
    /// <returns>���� �ε��� ��</returns>
    protected override int CalcIndex(int x, int y)
    {
        // ���� ���� : x + y * width;
        // ������ (0,0)�� �ƴ� ���� ���� ó�� : (x - origin.x) + (y - origin.y) + width;
        // y�� ������ ���� ���� ó�� : (x - origin.x) + ((height - 1) - (y - origin.y)) * width;

        return (x - origin.x) + ((height - 1) - (y - origin.y)) * width;
    }

    /// <summary>
    /// Ư�� ��ġ�� Ÿ�ϸ� ������ 
    /// </summary>
    /// <param name="x">Ȯ���� x��ǥ</param>
    /// <param name="y">Ȯ���� y��ǥ</param>
    /// <returns></returns>
    public override bool IsValidPostion(int x, int y)
    {
        // ���� �� : x < width && y < height && x >= 0 && y >= 0;
        //return x < (width + origin.x) && y < (height + origin.y) && x >= origin.x && y >= origin.y;
        return x < (width + origin.x) && y < (height + origin.y) && x >= origin.x && y >= origin.y;
    }

    /// <summary>
    /// ���� ��ǥ�� �׸��� ��ǥ�� �����ϴ� �Լ�
    /// </summary>
    /// <param name="worldPosition">������ǥ</param>
    /// <returns>������ �׸��� ��ǥ</returns>
    public Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        return (Vector2Int)background.WorldToCell(worldPosition);
    }

    /// <summary>
    /// �׸��� ��ǥ�� ������ǥ�� �����ϴ� �Լ�
    /// </summary>
    /// <param name="gridPosition">�׸��� ��ǥ</param>
    /// <returns>�׸����� ��� ������ �ش��ϴ� ���� ��ǥ</returns>
    public Vector2 GridToWorld(Vector2Int gridPosition)
    {
        return background.CellToWorld((Vector3Int) gridPosition) + new Vector3(0.5f, 0.5f);
    }

    /// <summary>
    /// �̵� ������ ��ġ �� �������� �����ؼ� �����ϴ� �Լ�
    /// </summary>
    /// <returns>�̵������� ��ġ</returns>
    public Vector2Int GetRandomMoveablePosition()
    {
        int index = UnityEngine.Random.Range(0, moveblepositions.Length);
        return moveblepositions[index];
    }

    public Node GetNode(Vector3 worldPosition)
    {
        return GetNode(WorldToGrid(worldPosition));
    }
}
