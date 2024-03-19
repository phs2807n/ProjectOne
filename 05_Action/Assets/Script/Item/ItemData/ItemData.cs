using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ �� ������ ������ �����ϴ� ��ũ���ͺ� ������Ʈ
[CreateAssetMenu(fileName = "New Item Data", menuName = "Scriptable Object/Item Data", order = 0)]
public class ItemData : ScriptableObject
{
    [Header("������ �⺻ ����")]
    public ItemCode code;
    public string itemName = "������";
    public string itemDescription = "����";
    public Sprite itemIcon;
    public uint price = 0;
    public uint maxStackCount = 1;
    public GameObject modelPrefab;
}
