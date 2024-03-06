using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AStar
{
    const float sideDistance = 1.0f;
    const float diagonalDistance = 1.4f;

    /// <summary>
    /// �ʰ� �������� �������� �޾� ��θ� ����ϴ� �Լ�
    /// </summary>
    /// <param name="map">���� ã�� ��</param>
    /// <param name="start">������</param>
    /// <param name="end">������</param>
    /// <returns>���������� ������������ ���</returns>
    public static List<Vector2Int> PathFind(GridMap map, Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> path = null;

        //Debug.Log($"{map.IsValidPostion(start)}, {map.IsValidPostion(end)}, {map.IsPlain(start)}, {map.IsPlain(end)}");

        if (map.IsValidPostion(start) && map.IsValidPostion(end) &&  !map.IsWall(start) && !map.IsWall(end))   // ������ �ƴ��� Ȯ���ϸ� �� ������ �ƴ����� Ȯ�� ����
        {
            // start�� end�� �� ���̰� ���� �ƴϴ�.
            map.ClearMapData(); // �� ������ ��ü �ʱ�ȭ

            List<Node> open = new List<Node>();     // open list : ������ Ž���� ����� ����Ʈ
            List<Node> close = new List<Node>();    // close list : Ž���� �Ϸ�� ����� ����Ʈ

            // A* �˰��� �����ϱ�
            Node current = map.GetNode(start);
            current.G = 0.0f;
            current.H = GetHeuristic(current, end);
            open.Add(current);

            // A* ���� ����(�ٽ� ��ƾ)
            while(open.Count > 0)   // open ����Ʈ�� ��尡 ������ ��� �ݺ�
            {
                open.Sort();        // f���� �������� ����
                current = open[0];  // ���� �տ� �ִ� ���(=f���� ���� ���� ���)�� current�� ����
                open.RemoveAt(0);   // open����Ʈ���� ���� �տ� �ִ� ��带 ����

                if(current != end)
                {
                    // �������� �ƴϴ�.
                    close.Add(current); // close����Ʈ�� current�� �߰��ؼ� Ž���� �Ϸ�Ǿ����� ǥ��

                    // current�� �ֺ� 8������ open����Ʈ�� �߰�
                    for(int y = -1; y < 2; y++)
                    {
                        for(int x = -1; x < 2; x++)
                        {
                            Node node = map.GetNode(current.X + x, current.Y + y);  // �ֺ� ���

                            // ��ŵ�� ������� Ȯ��
                            if(node == null) continue;                          // �� ��
                            if (node == current) continue;                      // �ڱ� �ڽ�
                            if (node.nodeType == Node.NodeType.Wall) continue;  // ��
                            if (close.Exists((x) => x == node)) continue;       // close ����Ʈ�� ����
                                                                                // (close�� �ִ� ��� ���(x)�� node�� ��
                            // �밢������ ���µ� ���� ���� �ִ� ���
                            bool isDiagonal = (x * y) != 0;     // �밢������ Ȯ��(true�� �밢��)
                            if (isDiagonal &&
                                (map.IsWall(current.X + x, current.Y ) || map.IsWall(current.X, current.Y + y)))
                                continue;

                            // current���� (x, y)�� ���µ� �ɸ��� �Ÿ��� Ȯ��(�밢���� 1.4, ���� 1.0)
                            float distance = isDiagonal ? diagonalDistance : sideDistance;

                            // G���� �������� ����
                            if(node.G > current.G + distance)
                            {
                                // ���� ������ ��κ��� current�� ���ļ� �̵��ϴ� ���� �� ������.(= ���� �ʿ�)
                                if(node.parent == null)
                                {
                                    // �θ� ������ ���� open����Ʈ�� �ȵ�� ����
                                    node.H = GetHeuristic(node, end);       // �޸���ƽ ���
                                    open.Add(node);                         // open����Ʈ�� �߰�
                                }

                                node.G = current.G + distance;              // G�� ����
                                node.parent = current;                      // �θ� ����
                            }
                        }
                    }
                }
                else
                {
                    // �������� �����ߴ�.   
                    break;  // �������� ���������� while ����
                }
            }

            // ������ �۾�(�������� �����ߴ� or ���� ��ã�Ҵ�.
            if(current == end)
            {
                // �������� �����ߴ�. => ��� �����
                path = new List<Vector2Int>();
                // ��� �����
                Node result = current;
                while(result != null)   // result�� start�� �� ������ �ݺ�
                {
                    path.Add(new Vector2Int(result.X, result.Y));   // �� current�� ��ġ�� �߰�
                    result = result.parent;
                }
                path.Reverse(); // ������ -> ���������� �Ǿ� �ִ� ��θ� ������
            }
        }

        return path;
    }

    /// <summary>
    /// �޸���ƽ ���� ����ϴ� �Լ�(���� ��ġ���� ������������ ���� �Ÿ�)
    /// </summary>
    /// <param name="current">���� ���</param>
    /// <param name="end">��������</param>
    /// <returns>����Ÿ�</returns>
    private static float GetHeuristic(Node current, Vector2Int end)
    {
        return Mathf.Abs(current.X - end.x) + Mathf.Abs(current.Y - end.y);
    }
}
