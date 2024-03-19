using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// �κ��丮 ����(�޸� �󿡼��� ����, UI ����)
public class Inventory
{
    /// <summary>
    /// �κ��丮�� �⺻ ���� ����(6ĭ)
    /// </summary>
    const int Default_Inventory_Size = 6;

    /// <summary>
    /// �κ��丮�� ���Ե�
    /// </summary>
    InventorySlot[] slots;

    /// <summary>
    /// �κ��丮 ���Կ� �����ϱ� ���� �ε���
    /// </summary>
    /// <param name="index">������ �ε���</param>
    /// <returns>����</returns>
    public InventorySlot this[uint index] => slots[index];
    //public InventorySlot this[uint index] => (index != tempSlotIndex) ? slots[index] : tempSlot;

    /// <summary>
    /// �κ��丮 ������ ����
    /// </summary>
    int SlotCount => slots.Length;

    /// <summary>
    /// �ӽ� ������ ����
    /// </summary>
    InvenTempSlot tempSlot;

    /// <summary>
    /// �ӽ� ���Կ� �ε���
    /// </summary>
    uint tempSlotIndex = 999999;

    /// <summary>
    /// �ӽ� ���� Ȯ�ο� ������Ƽ
    /// </summary>
    public InvenTempSlot TempSlot => tempSlot;

    /// <summary>
    /// ������ ������ �Ŵ���(�������� ������ ������ ��� ������ �ִ�.
    /// </summary>
    ItemDataManager itemDataManager;

    /// <summary>
    /// �κ��丮�� ������ �ִ� ������
    /// </summary>
    Player owner;

    /// <summary>
    /// ������ Ȯ�ο� ������Ƽ
    /// </summary>
    public Player Owner => owner;

    public Inventory(Player owner, uint size = Default_Inventory_Size)
    {
        slots = new InventorySlot[size];
        for (uint i = 0; i < size; i++)
        {
            slots[i] = new InventorySlot(i);
        }
        tempSlot = new InvenTempSlot(tempSlotIndex);
        itemDataManager = GameManager.Instance.ItemData;    // Ÿ�̹� ����
        this.owner = owner;
    }

    // �κ��丮�� �ؾ��� ����?

    /// <summary>
    /// �κ��丮�� Ư�� �������� 1�� �߰��ϴ� �Լ�
    /// </summary>
    /// <param name="code">�߰��� �������� �ڵ�</param>
    /// <returns>true�� �߰� ����, false�� �߰� ����</returns>
    public bool AddItem(ItemCode code)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (AddItem(code, (uint)i))
                return true;
        }
        return false;
    }

    /// <summary>
    /// �κ��丮�� Ư�� ���Կ� Ư�� �������� 1�� �߰��ϴ� �Լ�
    /// </summary>
    /// <param name="code">�߰��� �������� �ڵ�</param>
    /// <param name="slotIndex">�������� �߰��� ������ �ε���</param>
    /// <returns>true�� �߰� ����, false�� �߰� ����</returns>
    public bool AddItem(ItemCode code, uint slotIndex)
    {
        bool result = false;

        if (IsValidIndex(slotIndex))    // �ε����� �������� Ȯ��
        {
            // ������ �ε�����
            ItemData data = itemDataManager[code];  // ������ ��������
            InventorySlot slot = slots[slotIndex];  // ���� ��������
            if (slot.IsEmpty)
            {
                // ������ �������
                slot.AssignSlotItem(data);          // �״�� ������ ����
                result = true;
            }
            else
            {
                // ������ �Ⱥ���ִ�.
                if (slot.ItemData == data)
                {
                    // ���� ������ �������̸� 
                    result = slot.IncreaseSlotItem(out _);  // ������ ���� �õ�
                }
                else
                {
                    // �ٸ� ������ ������
                    Debug.Log($"������ �߰� ���� : [{slotIndex}]�� ���Կ��� �ٸ� �������� ����ֽ��ϴ�.");
                }
            }
        }
        else
        {
            // �ε����� �߸� ������ ����
            Debug.Log($"������ �߰� ���� : [{slotIndex}]�� �߸��� �ε��� �Դϴ�.");
        }

        return result;
    }

    /// <summary>
    /// �κ��丮�� Ư�� ���Կ��� �������� ���� ������ŭ �����ϴ� �Լ�
    /// </summary>
    /// <param name="slotIndex">�������� ���ҽ�ų ������ �ε���</param>
    /// <param name="decreaseCount">���ҽ�ų ����</param>
    public void RemoveItem(uint slotIndex, uint decreaseCount = 1)
    {
        if (IsValidIndex(slotIndex))
        {
            InventorySlot slot = slots[slotIndex];
            slot.DecreaseSlotItem(decreaseCount);
        }
        else
        {
            Debug.Log($"���� ������ ���� ���� : [{slotIndex}]�� ���� �ε����Դϴ�.");
        }
    }

    /// <summary>
    /// �κ��丮�� Ư�� ������ ���� �Լ�
    /// </summary>
    /// <param name="slotIndex">�������� ��� ����</param>
    public void ClearSlot(uint slotIndex)
    {
        if (IsValidIndex(slotIndex))
        {
            InventorySlot slot = slots[slotIndex];
            slot.ClearSlotItem();
        }
        else
        {
            Debug.Log($"���� ������ ���� ���� : [{slotIndex}]�� ���� �ε����Դϴ�.");
        }
    }

    /// <summary>
    /// �κ��丮�� ������ ���� ���� �Լ�
    /// </summary>
    public void ClearInventory()
    {
        foreach (var slot in slots)
            slot.ClearSlotItem();
    }

    /// <summary>
    /// �κ��丮�� from���Կ� �ִ� �������� to ��ġ�� �ű�� �Լ�
    /// </summary>
    /// <param name="from">��ġ ���� ���� �ε���</param>
    /// <param name="to">��ġ ���� ���� �ε���</param>
    public void MoveItem(uint from, uint to)
    {
        // from������ to������ ���� �ٸ� ��ġ�̰� ��� valid�� �ε����̾�� �Ѵ�.
        if ((from != to) && IsValidIndex(from) && IsValidIndex(to))
        {
            bool fromIsTemp = (from == tempSlotIndex);
            InventorySlot fromSlot = fromIsTemp ? TempSlot : slots[from];

            if (!fromSlot.IsEmpty)
            {
                // from�� �������� �ִ�.
                InventorySlot toSlot = null;
                if(to == tempSlotIndex)
                {
                    toSlot = tempSlot;
                    TempSlot.SetFromIndex(fromSlot.Index);
                }
                else
                {
                    toSlot = slots[to];
                }

                if (fromSlot.ItemData == toSlot.ItemData)
                {
                    // ���� ������ ������ => to�� ä�� �� �ִµ����� ä���. to�� �Ѿ ��ŭ from�� ���ҽ�Ų��.
                    toSlot.IncreaseSlotItem(out uint overCount, fromSlot.ItemCount);    // from�� ���� ������ŭ to �߰�
                    fromSlot.DecreaseSlotItem(fromSlot.ItemCount - overCount);          // from���� to�� �Ѿ ������ŭ�� ����

                    //Debug.Log($"[{from}]�� ���Կ��� [{to}]�� �������� ������ ��ġ��");
                }
                else
                {
                    // �ٸ� ������ ������(or ����ִ�.) => ���� ����
                    //Debug.Log($"[{from}]�� ���԰� [{to}]�� ������ ���� ������ ��ü");

                    // �巡�װ� ������ �� �ӽ� -> to, ���� to -> ���� from
                    if (fromIsTemp)
                    {
                        // temp�� ����� ���� FromIndex ���Կ� toSlot ������ ������ �ֱ�

                        // temp ���Կ��� to �������� �ű�� ���
                        //      returnSlot(temp�� ����� ���� FromIndex ����)�� toSlot������ ������ �ְ�
                        //      toSlot�� TempSlot�� ������ �ֱ�


                        InventorySlot returnSlot = slots[TempSlot.FromIndex];   // temp�� ����� ���� FromIndex

                        if (returnSlot.IsEmpty)
                        {
                            // returnSlot�� ����ִ� ���(�� �ּ� �״�� ó��)
                            returnSlot.AssignSlotItem(toSlot.ItemData, toSlot.ItemCount, false);
                            toSlot.AssignSlotItem(tempSlot.ItemData, TempSlot.ItemCount, false);
                            TempSlot.ClearSlotItem();
                        }
                        else
                        {
                            // returnSlot�� �������� �ִ�.
                            if(returnSlot.ItemData == tempSlot.ItemData)
                            {
                                // ���� ������ �������̴�. => toSlot�� ������ tempSlot�� ��ġ�� �õ�
                                returnSlot.IncreaseSlotItem(out uint overCount, toSlot.ItemCount);
                                toSlot.DecreaseSlotItem(toSlot.ItemCount - overCount);
                                // �������� ���Ƶ� �Ʒ��ʿ��� ó��
                            }
                            // tempSlot�� toSlot�� ����
                            SwapSlot(tempSlot, toSlot);
                        }
                    }
                    else
                    {
                        SwapSlot(fromSlot, toSlot);
                    }
                }
            }
        }
    }

    /// <summary>
    /// ���� ���ҿ� �Լ�
    /// </summary>
    /// <param name="slotA"></param>
    /// <param name="slotB"></param>
    private void SwapSlot(InventorySlot slotA, InventorySlot slotB)
    {
        ItemData tempData = slotA.ItemData;
        uint tempCount = slotA.ItemCount;
        slotA.AssignSlotItem(slotB.ItemData, slotB.ItemCount, false);
        slotB.AssignSlotItem(tempData, tempCount, false);
    }

    /// <summary>
    /// �κ��丮�� Ư�� ���Կ��� �������� ������ ����� �ӽ� �������� ������ �Լ�
    /// </summary>
    /// <param name="slotIndex">�������� ��� ����</param>
    /// <param name="count">��� ����</param>
    public void SplitItem(uint slotIndex, uint count)
    {
        if (IsValidIndex(slotIndex))
        {
            InventorySlot slot = slots[slotIndex];
            count = System.Math.Min(count, slot.ItemCount); // count�� ���Կ� ����ִ� �������� ũ�� ���Կ� ����ִ� ����������

            TempSlot.AssignSlotItem(slot.ItemData, count);  // �ӽ� ���Կ� �켱 �ְ�
            slot.DecreaseSlotItem(count);                   // �����ϴ� ���Կ��� ����
        }
    }

    /// <summary>
    /// �κ��丮�� �����ϴ� �Լ�
    /// </summary>
    /// <param name="sortBy">���� ����</param>
    /// <param name="isAcending">true�� ��������, false�� ��������</param>
    public void SlotSorting(ItemSortBy sortBy, bool isAcending = true)
    {
        // ������ ���� �ӽ� ����Ʈ
        List<InventorySlot> temp = new List<InventorySlot>(slots);  // slots�� ������� ����Ʈ ����

        // ���� ����� ���� �ӽ� ����Ʈ ����
        switch (sortBy)
        {
            case ItemSortBy.Code:
                temp.Sort((current, other) =>    // current, other�� temp����Ʈ�� ����ִ� ��� �� 2��
                {
                    if (current.ItemData == null)        // ����ִ� ������ �������� ������
                        return 1;
                    if (other.ItemData == null)
                        return -1;
                    if (isAcending)
                    {
                        return current.ItemData.code.CompareTo(other.ItemData.code);
                    }
                    else
                    {
                        return current.ItemData.code.CompareTo(current.ItemData.code);
                    }
                });
                break;
            case ItemSortBy.Name:
                temp.Sort((current, other) =>
                {
                    if (current.ItemData == null)        // ����ִ� ������ �������� ������
                        return 1;
                    if (other.ItemData == null)
                        return -1;
                    if (isAcending)
                    {
                        return current.ItemData.itemName.CompareTo(other.ItemData.itemName);
                    }
                    else
                    {
                        return other.ItemData.itemName.CompareTo(current.ItemData.itemName);
                    }
                });
                break;
            case ItemSortBy.Price:
                temp.Sort((current, other) =>
                {
                    if (current.ItemData == null)        // ����ִ� ������ �������� ������
                        return 1;
                    if (other.ItemData == null)
                        return -1;
                    if (isAcending)
                    {
                        return current.ItemData.price.CompareTo(other.ItemData.price);
                    }
                    else
                    {
                        return other.ItemData.price.CompareTo(current.ItemData.price);
                    }
                });
                break;
        }

        // �ӽ� ����Ʈ�� ������ ���Կ� ����
        List<(ItemData, uint, bool)> sortedData = new List<(ItemData, uint, bool)>(SlotCount);
        foreach (var slot in temp)
        {
            sortedData.Add((slot.ItemData, slot.ItemCount, slot.IsEquipped));   // �ʿ� �����͸� �����ؼ� ��������
        }

        int index = 0;
        foreach (var data in sortedData)
        {
            slots[index].AssignSlotItem(data.Item1, data.Item2, data.Item3);    // �ӽ� ����Ʈ�� ������ ����
            index++;
        }
    }

    /// <summary>
    /// ���� �ε����� ������ �ε������� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <param name="index">Ȯ���� �ε���</param>
    /// <returns>true�� ������ �ε���, false�� �߸��� �ε���</returns>
    private bool IsValidIndex(uint index)
    {
        return (index < SlotCount) || (index == tempSlotIndex);
    }

    /// <summary>
    /// ã�� ���и� ��Ÿ���� ���
    /// </summary>
    const uint FailFind = uint.MaxValue;

    /// <summary>
    /// ����ִ� ������ ã�� �ε����� ����ִ� �Լ�
    /// </summary>
    /// <param name="index">����ִ� ������ ã������ �� ������ �ε���</param>
    /// <returns>ã������ true, ��ã������ false</returns>
    public bool FindEmptySlot(out uint index)
    {
        bool result = false;
        index = FailFind;

        foreach(var slot in slots)
        {
            if(slot.IsEmpty)
            {
                index = slot.Index;
                result = true;
                break;
            }
        }

        return result;
    }

#if UNITY_EDITOR
    public void Test_InventoryPrint()
    {
        // ��� ����
        // [ ���(1/10), �����̾�(2/3), (��ĭ), ���޶���(3/5), (��ĭ), (��ĭ) ]

        string printText = "[ ";

        for (int i = 0; i < SlotCount - 1; i++)
        {
            if (slots[i].IsEmpty)
            {
                printText += "(��ĭ)";
            }
            else
            {
                printText += $"{slots[i].ItemData.itemName}({slots[i].ItemCount}/{slots[i].ItemData.maxStackCount}";
            }
            printText += ", ";
        }
        InventorySlot last = slots[SlotCount - 1];
        if (last.IsEmpty)
        {
            printText += "(��ĭ)";
        }
        else
        {
            printText += $"{last.ItemData.itemName}({last.ItemCount}/{last.ItemData.maxStackCount}";
        }
        printText += " ]";
        Debug.Log(printText);
    }
#endif
}
