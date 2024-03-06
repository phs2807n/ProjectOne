using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Timeline;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class RoadTile : Tile
{
    [Flags]                     // �� enum�� bit flag�� ����Ѵٰ� ǥ���ϴ� ��Ʈ����Ʈ
    enum AdjTilePosition : byte // �� enum�� ũ��� 1byte
    {
        None = 0,       // 0000 0000
        North = 1,      // 0000 0001
        East = 2,       // 0000 0010
        South = 4,      // 0000 0100
        West = 8,       // 0000 1000
        All = North | East | South | West   // 0000 1111
    }

    /// <summary>
    /// ���� ������ ��������Ʈ��
    /// </summary>
    public Sprite[] sprites;

    /// <summary>
    /// Ÿ���� �׷��� �� �ڵ����� ȣ���� �Ǵ� �Լ�
    /// </summary>
    /// <param name="position">Ÿ���� ��ġ(�׸��� ��ǥ)</param>
    /// <param name="tilemap">�� Ÿ���� �׷����� </param>
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        // �ֺ��� �ִ� ���� ������ Ÿ�� �����ϱ�
        for(int y = -1; y < 2; y++)
        {
            for(int x = -1; x < 2; x++)
            {
                Vector3Int location = new Vector3Int(position.x + x, position.y + y, position.z);
                if(HasThisTile(tilemap, location))
                {
                    tilemap.RefreshTile(location);
                }
            }
        }
    }

    /// <summary>
    /// Ÿ�ϸ��� RefreshTile �Լ��� ȣ��� �� ȣ��, � ��������Ʈ�� �׸��� �����ϴ� �Լ�
    /// </summary>
    /// <param name="position">Ÿ�� �����͸� ������ Ÿ���� ��ġ</param>
    /// <param name="tilemap">Ÿ�� �����͸� ������ Ÿ�ϸ�</param>
    /// <param name="tileData">������ Ÿ�� �������� ����(�б� ���� �� �� ����)</param>
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        // 8���� Ȯ��, Ȯ���� ������ ����
        AdjTilePosition mask = AdjTilePosition.None;

        mask |= HasThisTile(tilemap, position + new Vector3Int(0, 1, 0)) ? AdjTilePosition.North : 0;
        //if(HasThisTile(tilemap, position + new Vector3Int(0,1, 0)))   // ���ٰ� �Ȱ��� �ڵ�
        //{
        //    mask = mask | AdjTilePosition.North;
        //}

        mask |= HasThisTile(tilemap, position + new Vector3Int(1, 0, 0)) ? AdjTilePosition.East : 0;
        mask |= HasThisTile(tilemap, position + new Vector3Int(0, -1, 0)) ? AdjTilePosition.South : 0;
        mask |= HasThisTile(tilemap, position + new Vector3Int(-1, 0, 0)) ? AdjTilePosition.West : 0;

        // �̹��� �����ϱ�
        int index = GetIndex(mask);
        if (index > -1 && index < sprites.Length)
        {
            tileData.sprite = sprites[index];
            Matrix4x4 matrix = tileData.transform;
            matrix.SetTRS(Vector3.zero, GetRotation(mask), Vector3.one);
            tileData.transform = matrix;
            tileData.flags = TileFlags.LockTransform;
        }
        else
        {
            Debug.LogError($"�߸��� �ε��� : {index}, mask = {mask}");
        }
    }

    /// <summary>
    /// Ư�� Ÿ�ϸ��� Ư�� ��ġ�� �� Ÿ�ϰ� ���� ������ Ÿ���� �ִ��� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <param name="tilemap">Ȯ���� Ÿ�ϸ�</param>
    /// <param name="position">Ȯ���� ��ġ</param>
    /// <returns>true�� ���� ������ Ÿ��, false�� �ٸ� ������ Ÿ��</returns>
    bool HasThisTile(ITilemap tilemap, Vector3Int position)
    {
        return tilemap.GetTile(position) == this;
    }

    /// <summary>
    /// ����ũ ���� ���� �׸� ��������Ʈ�� �ε����� �����ϴ� �Լ�
    /// </summary>
    /// <param name="mask">�ֺ� Ÿ���� ��Ȳ�� ǥ���� ��Ʈ�÷��� ��</param>
    /// <returns>�׷����� ��������Ʈ�� �ε���</returns>
    int GetIndex(AdjTilePosition mask)
    {
        int index = -1;

        switch (mask)
        {
            case AdjTilePosition.None:  // ����
            case AdjTilePosition.North: // ��
            case AdjTilePosition.East:  // ��
            case AdjTilePosition.South: // ��
            case AdjTilePosition.West:  // ��
            case AdjTilePosition.North | AdjTilePosition.South: // �ϳ�
            case AdjTilePosition.East | AdjTilePosition.West:   // ����
                index = 0;// 1�� ����� ��������Ʈ
                break;
            case AdjTilePosition.North | AdjTilePosition.East:
            case AdjTilePosition.North | AdjTilePosition.West:
            case AdjTilePosition.South | AdjTilePosition.East:
            case AdjTilePosition.South | AdjTilePosition.West:
                index = 1;// ���� ����� ��������Ʈ
                break;
            case AdjTilePosition.All & ~AdjTilePosition.North:      // ~0001 = 1110 ���ʸ� ����
            case AdjTilePosition.All & ~AdjTilePosition.East:       // ~0010 = 1101 ���ʸ� ����
            case AdjTilePosition.All & ~AdjTilePosition.South:      // ~0100 = 1011 ���ʸ� ����
            case AdjTilePosition.All & ~AdjTilePosition.West:       // ~1000 = 0111 ���ʸ� ����
                index = 2;// ���� ����� ��������Ʈ
                break;
            case AdjTilePosition.All:
                index = 3;// +�� ����� ��������Ʈ
                break;
        }

        return index;
    }

    Quaternion GetRotation(AdjTilePosition mask)
    {
        Quaternion rotate = Quaternion.identity;
        switch (mask)
        {
            case AdjTilePosition.East:
            case AdjTilePosition.West:
            case AdjTilePosition.East | AdjTilePosition.West:   // I��
            case AdjTilePosition.North | AdjTilePosition.West:  // ����
            case AdjTilePosition.All & ~AdjTilePosition.West:   // ����
                rotate = Quaternion.Euler(0, 0, -90);
                // -90�� ������
                break;
            case AdjTilePosition.North | AdjTilePosition.East:  //����
            case AdjTilePosition.All & ~AdjTilePosition.North:   // ����
                rotate = Quaternion.Euler(0, 0, -180);
                // -180�� ������
                break;
            case AdjTilePosition.South | AdjTilePosition.East:  // ����
            case AdjTilePosition.All & ~AdjTilePosition.East:   // ����
                rotate = Quaternion.Euler(0, 0, -270);
                // -270�� ������
                break;
        }
        return rotate;
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/2D/Tiles/Custom/RoadTile")]

    public static void CreateRoadTile()
    {
        string path = EditorUtility.SaveFilePanelInProject( // ���� ����â�� ���� �Է°���� �����ִ� �Լ�
            "Sace Road Tile",   // ����
            "New Road Tile",    // ����Ʈ ���ϸ�
            "Asset",            // ������ ����Ʈ Ȯ����
            "Save Road Tile",   // ��� �޼���
            "Assets/Tiles");    // ���� �⺻ ����
        if(path != null)
        {
            AssetDatabase.CreateAsset(CreateInstance<RoadTile>(), path);    // RoadTile�� ���Ϸ� ����
        }
    }
#endif
}

/*
    21, 34, 25, 32
 */