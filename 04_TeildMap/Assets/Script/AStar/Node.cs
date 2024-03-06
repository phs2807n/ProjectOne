using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IComparable<Node>
{
    /// <summary>
    /// �׸��� �ʿ����� x��ǥ
    /// </summary>
    int x;
    public int X => x;

    /// <summary>
    /// �׸��� �ʿ����� y��ǥ
    /// </summary>
    int y;
    public int Y => y;

    /// <summary>
    /// A* �˰����� G��(��������� �� ������ ���µ� �ɸ� ���� �Ÿ�)
    /// </summary>
    public float G;

    /// <summary>
    /// A* �˰����� H��(�� ��忡�� ������������ ���� �Ÿ�)
    /// </summary>
    public float H;

    /// <summary>
    /// G�� H�� ��(��������� �� ��带 �����ؼ� ���������� �̵��� �� ����Ǵ� �Ÿ�)
    /// </summary>
    public float F => G + H;

    /// <summary>
    /// ��尡 ���� �� �ִ� ������
    /// </summary>
    public enum NodeType
    {
        Plain,  // ����(������ �� ����)
        Wall,   // ��(������ �� ����)
        Slime   // ������(������ �� ����)
    }

    /// <summary>
    /// �� ����� ����
    /// </summary>
    public NodeType nodeType = NodeType.Plain;

    /// <summary>
    /// ��λ� �տ� �ִ� ���
    /// </summary>
    public Node parent;

    /// <summary>
    /// Node�� ������
    /// </summary>
    /// <param name="x">�׸��� �ʿ����� x��ǥ</param>
    /// <param name="y">�׸��� �ʿ����� y��ǥ</param>
    /// <param name="nodeType">����� ����(�⺻������ ����)</param>
    public Node(int x, int y, NodeType nodeType = NodeType.Plain)
    {
        this.x = x;
        this.y = y;
        this.nodeType = nodeType;
        ClearData();
    }

    /// <summary>
    /// ��ã�⸦ �� ������ �ʱ�ȭ
    /// </summary>
    public void ClearData()
    {
        G = float.MaxValue;     // g���� ���ؼ� ���ŵǴ� ������ �����Ƿ� �⺻���� �ſ� Ŀ�� �Ѵ�.
        H = float.MaxValue;     // 
        parent = null;
    }

    public int CompareTo(Node other)
    {
        // ���� �� �ִ� ������ ����� ��
        // 0���� �۴�(-1)   : ���� �۴�(this < other)
        // 0�� ����         : ���� ����(this == other)
        // 0���� ũ��(+1)   : ���� ũ��(this > other)

        if(other == null)               // other�� nulll�̸� ���� ũ��.
            return 1;

        return F.CompareTo(other.F);    // F ���� �������� ������ ���ض�
    }

    /// <summary>
    /// == ��ɾ� �����ε�, �� ��尡 ������ Ȯ��(x, y�� ������ ����.)
    /// </summary>
    /// <param name="left">== ���ʿ� �ִ� ���</param>
    /// <param name="right">== �����ʿ� �ִ� ���</param>
    /// <returns></returns>
    //public static bool operator ==(Node left, Node right)
    //{
    //    return left.x == right.x && left.y == right.y;  
    //}
    
    public static bool operator ==(Node left, Vector2Int right)
    {
        return left.x == right.x && left.y == right.y;  
    }

    /// <summary>
    /// != ��ɾ� �����ε�, �� ��尡 �ٸ��� Ȯ��(x,y �� �ϳ��� �ٸ��� �ٸ���.)
    /// </summary>
    /// <param name="left">== ���ʿ� �ִ� ���</param>
    /// <param name="right">== �����ʿ� �ִ� ���</param>
    /// <returns>������ false, �ٸ��� true</returns>
    //public static bool operator !=(Node left, Node right)
    //{
    //    return left.x != right.x || left.y != right.y;
    //}

    public static bool operator !=(Node left, Vector2Int right)
    {
        return left.x != right.x || left.y != right.y;
    }

    public override bool Equals(object obj)
    {
        // obj�� Node Ŭ������ this�� obj�� x, y�� ����.
        return obj is Node other && this.x == other.x && this.y == other.y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y);  // ��ġ�� 2���� �ؽ��ڵ� �����
    }
}
