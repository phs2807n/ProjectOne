using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �κ��丮���� ���° ���������� ��Ÿ���� ����
public class InventorySlot
{
    /// <summary>
    /// �κ��丮���� �� ��° ���������� ��Ÿ���� ����
    /// </summary>
    uint slotIndex;

    /// <summary>
    /// ���� �ε����� Ȯ���ϱ� ���� ������Ƽ
    /// </summary>
    public uint Index => slotIndex;

    /// <summary>
    /// �� ���Կ� ����ִ� ������ ����(null�̸� ������ ����ִ� ��)
    /// </summary>
    ItemData slotItemData = null;

    public ItemData ItemData
    {
        get => slotItemData;
        private set // ���ο����� ���� ����
        {
            slotItemData = value;
            onSlotItemChange?.Invoke(); // ������ �Ͼ�� ��������Ʈ�� �˸���.
        }
    }

    /// <summary>
    /// ���Կ� ����ִ� �������� ����, ����, ��񿩺��� ������ �˸��� ��������Ʈ
    /// </summary>
    public Action onSlotItemChange;

    /// <summary>
    /// ���Կ� �������� �ִ��� ������ Ȯ���ϱ� ���� ������Ƽ
    /// </summary>
    public bool IsEmpty => slotItemData == null;

    /// <summary>
    /// �� ���Կ� ����ִ� ������ ����
    /// </summary>
    uint itemCount = 0;

    /// <summary>
    /// ������ ������ Ȯ���ϱ� ���� ������Ƽ (����� private)
    /// </summary>
    public uint ItemCount
    {
        get => itemCount;
        set
        {
            if(ItemCount != 0)
            {
                ItemCount = value;
                onSlotItemChange.Invoke();
            }
        }
    }

    /// <summary>
    /// �� ������ �������� ���Ǿ����� ����
    /// </summary>
    bool isEquipped = false;

    /// <summary>
    /// �� ������ ��� ���θ� Ȯ���ϱ� ���� ������Ƽ(set�� private
    /// </summary>
    public bool IsEquipped
    {
        get => isEquipped;
        set
        {
            isEquipped = value;
            onSlotItemChange?.Invoke();
        }
    }

    /// <summary>
    /// ������
    /// </summary>
    /// <param name="index">������ �ε���</param>
    public InventorySlot(uint index)
    {
        slotIndex = index;  // ������ ���� �����ϰ� ���� ������ �ʾƾ��Ѵ�.
        ItemData = null;
        itemCount = 0;
        IsEquipped = false;
    }

    /// <summary>
    /// �� ���Կ� �������� ����(set)�ϴ� �Լ�
    /// </summary>
    /// <param name="data">������ �������� ����</param>
    /// <param name="count">������ �������� ����</param>
    /// <param name="isEquipped">��� ����</param>
    public virtual void AssignSlotItem(ItemData data, uint count = 1, bool isEquipped = false, uint? from = null)
    {
        if(data != null)
        {
            ItemData = data;
            ItemCount = count;
            IsEquipped = isEquipped;
            Debug.Log($"�κ��丮 [{slotIndex}]�� ���Կ� [{ItemData.itemName}]�������� [{ItemCount}]�� ����");
        }
        else
        {
            ClearSlotItem();
        }
    }

    /// <summary>
    /// �� ������ ���� �Լ�
    /// </summary>
    public virtual void ClearSlotItem()
    {
        ItemData = null;
        itemCount = 0;
        isEquipped = false;
        Debug.Log($"�κ��丮 [{slotIndex}]�� ������ ��ɴϴ�.");
    }

    // ������ ���� ��ȭ

    /// <summary>
    /// �� ���Կ� ������ ������ ������Ű�� �Լ�
    /// </summary>
    /// <param name="overCount">return�� fasle�� �� ���԰� ���� ����</param>
    /// <param name="increaseCount">������ų ����</param>
    /// <returns>���Կ� increaseCount��ŭ ������������ true, �� ������Ű�� �������� false</returns>
    public bool IncreaseSlotItem(out uint overCount, uint increaseCount = 1)
    {
        bool result;

        uint newCount = ItemCount + increaseCount;
        int over = (int)newCount - (int)ItemData.maxStackCount;

        Debug.Log($"�κ��丮 [{slotIndex}]�� ���Կ� �������� ����, ���� [{ItemCount}]��");

        if( over > 0 )
        {
            // ���ƴ�.
            ItemCount = ItemData.maxStackCount;
            overCount = (uint)over;
            result = false;
            Debug.Log($"�������� �ִ�ġ���� ����. [{over}]�� ��ħ");
        }
        else
        {
            // ����
            ItemCount = newCount;
            overCount = 0;
            result = true;
            Debug.Log($"[{increaseCount}]�� ����");
        }

        return result;
    }

    public void DecreaseSlotItem(uint decreaseCount = 1)
    {
        int newCount = (int)ItemCount - (int)decreaseCount;
        if( newCount > 0 )
        {
            // ���� �������� ��������
            ItemCount = (uint)newCount;
            Debug.Log($"�κ��丮 [{slotIndex}]�� ���Կ� [{ItemData.itemName}]�� [{decreaseCount}]�� ����, ���� [{ItemCount}]��");
        }
        else
        {
            // �� ������
            ClearSlotItem();
        }
    }

    /// <summary>
    /// �� ������ �������� ����ϴ� �Լ�
    /// </summary>
    /// <param name="target">�������� ȿ���� ���� ���</param>
    public void UseItem(GameObject target)
    {

    }

    /// <summary>
    /// �� ������ �������� ����ϴ� �Լ�
    /// </summary>
    /// <param name="target"></param>
    public void EquipItem(GameObject target)
    {

    }
}
