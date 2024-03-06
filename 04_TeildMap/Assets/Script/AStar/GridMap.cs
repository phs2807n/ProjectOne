using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
/// <summary>
/// �� Ŭ����
/// </summary>
public class GridMap
{
    /// <summary>
    /// �� �ʿ� �ִ� ��� ���
    /// </summary>
    public Node[] nodes;

    /// <summary>
    /// ���� ���� ����
    /// </summary>
    protected int width;

    /// <summary>
    /// ���� ���� ����
    /// </summary>
    protected int height;

    protected GridMap() { }

    /// <summary>
    /// ������
    /// </summary>
    /// <param name="width">���α���</param>
    /// <param name="height">���α���</param>
    public GridMap(int width, int height)
    {
        this.width = width;
        this.height = height;

        nodes = new Node[width * height];                       // ��� �迭 ����

        for(int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if(GridToIndex(x, y, out int? index))
                    nodes[index.Value] = new Node(x, y);      // ��� ����
            }
        }
    }

    /// <summary>
    /// ��� ����� A* ���� ������ �ʱ�ȯ
    /// </summary>
    public void ClearMapData()
    {
        foreach(Node node in nodes)
        {
            node.ClearData();
        }
    }

    /// <summary>
    /// Ư�� ��ġ�� �ִ� ��带 �����ϴ� �Լ�
    /// </summary>
    /// <param name="x">�ʿ����� x��ǥ</param>
    /// <param name="y">�ʿ����� y��ǥ</param>
    /// <returns>ã�� ���</returns>
    public Node GetNode(int x, int y)
    {
        Node node = null;
        if(GridToIndex(x, y, out int? index))
        {
            node = nodes[index.Value];
        }
        return node;
    }

    /// <summary>
    /// Ư�� ��ġ�� �ִ� ��带 �����ϴ� �Լ�
    /// </summary>
    /// <param name="gridPosition">�ʿ����� �׸��� ��ǥ</param>
    /// <returns>ã�� ���</returns>
    public Node GetNode(Vector2Int gridPosition)
    {
        return GetNode(gridPosition.x, gridPosition.y);
    }

    //public Vector3 GetPositionToNode(Node node)
    //{
    //    return new Vector3(node.X, node.Y, 0);
    //}

    /// <summary>
    /// Ư�� ��ġ�� �������� �ƴ��� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <param name="x">�ʿ����� x��ǥ</param>
    /// <param name="y">�ʿ����� y��ǥ</param>
    /// <returns>true�� ����, fasle�� ���� �ƴ�</returns>
    public bool IsPlain(int x, int y)
    {
        Node node = GetNode(x, y);
        //Debug.Log($"����� �� ���� : {node != null}");
        //Debug.Log($"{node.nodeType == Node.NodeType.Plain}");
        return node != null && node.nodeType == Node.NodeType.Plain;
    }

    public bool IsPlain(Vector2Int gridPositio)
    {
        return IsPlain(gridPositio.x, gridPositio.y);
    }

    /// <summary>
    /// Ư�� ��ġ�� ������ �ƴ��� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <param name="x">�ʿ����� x��ǥ</param>
    /// <param name="y">�ʿ����� y��ǥ</param>
    /// <returns>true�� ��, false�� �� �ƴ�</returns>
    public bool IsWall(int x, int y)
    {
        Node node = GetNode(x, y);
        return node != null && node.nodeType == Node.NodeType.Wall;
    }

    public bool IsWall(Vector2Int gridPositio)
    {
        return IsWall(gridPositio.x, gridPositio.y);
    }

    /// <summary>
    /// Ư�� ��ġ�� ���������� �ƴ��� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <param name="x">�ʿ����� x��ǥ</param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool IsSlime(int x, int y)
    {
        Node node = GetNode(x, y);
        return node != null && node.nodeType == Node.NodeType.Slime;
    }

    /// <summary>
    /// Ư�� ��ġ�� ���������� �ƴ���
    /// </summary>
    /// <param name="gridPosition">�ʿ����� �׸��� ��ǥ</param>
    /// <returns>true�� ������, false�� ������ �ƴ�</returns>
    public bool IsSlime(Vector2Int gridPosition)
    {
        return IsSlime(gridPosition.x, gridPosition.y);
    }

    /// <summary>
    /// �׸��� ��ǥ�� �ε��� ������ �������ִ� �Լ�
    /// </summary>
    /// <param name="x">�ʿ����� x��ǥ</param>
    /// <param name="y">�ʿ����� y��ǥ</param>
    /// <param name="index">(��¿�)����� �ε���</param>
    /// <returns>��ǥ�� �����ϸ� true, �� ���̸� false</returns>
    protected bool GridToIndex(int x, int y, out int? index)
    {
        bool result = false;
        index = null;

        if (IsValidPostion(x, y))
        {
            index = CalcIndex(x, y);
            result = true;
        }

        return result;
    }

    /// <summary>
    /// �ε���
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    protected virtual int CalcIndex(int x, int y)
    {
        return x + y * width;
    }

    /// <summary>
    /// �ε��� ���� �׸��� ��ǥ�� �������ִ� �Լ�
    /// </summary>
    /// <param name="index">������ index��</param>
    /// <returns>����� �׸��� ��ǥ</returns>
    public Vector2Int IndexToGrid(int index)
    {
        return new Vector2Int(index % width, index / width);
    }

    /// <summary>
    /// Ư�� ��ġ�� �� ������ �ƴ��� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <param name="x">Ȯ���� x��ǥ</param>
    /// <param name="y">Ȯ���� y��ǥ</param>
    /// <returns>true�� �� ��, false�� �� ��</returns>
    public virtual bool IsValidPostion(int x, int y)
    {
        return x < width && y < height &&  x >= 0 && y >= 0;
    }

    /// <summary>
    /// Ư�� ��ġ�� �� ������ �ƴ��� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <param name="gridPosition">Ȯ���� �׸��� ��ǥ</param>
    /// <returns>true�� �� ��, false�� �� ��</returns>
    public bool IsValidPostion(Vector2Int gridPosition)
    {
        return IsValidPostion(gridPosition.x, gridPosition.y);
    }
}
