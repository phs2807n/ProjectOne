using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    /// <summary>
    /// �� UI�� ������ �κ��丮
    /// </summary>
    Inventory inven;

    /// <summary>
    /// �κ��丮�� �ִ� slot UI��
    /// </summary>
    InventorySlotUI[] slotUIs;

    /// <summary>
    /// �ӽ� ����
    /// </summary>
    TempSlotUI tempSlotUI;

    /// <summary>
    /// �� ����â
    /// </summary>
    DetailInfoUI detail;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        slotUIs = child.GetComponentsInChildren<InventorySlotUI>();
        tempSlotUI = GetComponentInChildren<TempSlotUI>();
        detail = GetComponentInChildren<DetailInfoUI>();
    }

    /// <summary>
    /// �κ��丮 �ʱ�ȭ�� �Լ�
    /// </summary>
    /// <param name="playerInventory">�� UI�� ǥ���� �κ��丮</param>
    public void InitializeInventory(Inventory playerInventory)
    {
        inven = playerInventory;                    // ����

        for(uint i = 0; i < slotUIs.Length; i++)    
        {       
            slotUIs[i].InitializeSlot(inven[i]);    // ��� �κ��丮 ���� �ʱ�ȭ
            slotUIs[i].onDrageBegin += OnItemMoveBegin; // ����
            slotUIs[i].onDragEnd += OnItemMoveEnd;
            slotUIs[i].onClick += OnSlotClick;
            slotUIs[i].onPointEnter += OnItemDetailOn;
            slotUIs[i].onPointExit += OnItemDetailOff;
            slotUIs[i].onPointerMove += OnSlotPointerMove;
        }

        tempSlotUI.InitializeSlot(inven.TempSlot);
    }

    /// <summary>
    /// �巡�� �������� �� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="index">�巡�� ������ ��ġ�� �ִ� ������ �ε���</param>
    private void OnItemMoveBegin(uint index)
    {
        inven.MoveItem(index, tempSlotUI.Index);    // ���� -> �ӽ� ������ �ű��
        tempSlotUI.Open();                          // �ӽ� ���� ����
    }

    /// <summary>
    /// �巡�װ� ������ �� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="index">�巡�װ� ���� ������ �ε���, �ƴϸ� �巡�� ������ ������ �ε���</param>
    /// <param name="isSuccess">���Կ��� �巡�װ� �������� true, �ƴϸ� false</param>
    private void OnItemMoveEnd(uint index, bool isSuccess)
    {
        //uint finalIndex = index;
        //if(!isSuccess)
        //{
        //    // ����� ã�Ƽ� ���� �ش�.
        //    if(inven.FindEmptySlot(out uint emptySlotIndex))
        //    {
        //        finalIndex = emptySlotIndex;
        //    }
        //    else
        //    {
        //        // �ٴڿ� ���
        //        Debug.LogWarning("�ٴڿ� �������� ����ؾ� �Ѵ�.");
        //        return;
        //    }
        //}

        //inven.MoveItem(tempSlotUI.Index, finalIndex);
        inven.MoveItem(tempSlotUI.Index, index);     // �ӽ� -> �������� ������ �����

        if (tempSlotUI.InvenSlot.IsEmpty)
        {
            tempSlotUI.Close();
        }
    }

    /// <summary>
    /// ������ Ŭ������ �� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="index">Ŭ���� ������ �ε���</param>
    private void OnSlotClick(uint index)
    {
        if(!tempSlotUI.InvenSlot.IsEmpty)   // �ӽ� ������ ������� ������
        {
            OnItemMoveEnd(index, true);     // Ŭ���� ���Կ� ������ �ֱ�
        }
    }

    /// <summary>
    /// ������ �� ����â�� ���� �Լ�
    /// </summary>
    /// <param name="index">�� ����â���� ǥ�õ� �������� ����ִ� ������ �ε���</param>
    private void OnItemDetailOn(uint index)
    {
        detail.Open(slotUIs[index].InvenSlot.ItemData); // ����
    }

    /// <summary>
    /// ������ ���� ����â�� �ݴ� �Լ�
    /// </summary>
    private void OnItemDetailOff()
    {
        detail.Close();     // �ݱ�
    }

    /// <summary>
    /// ���� �ȿ��� ���콺 Ŀ���� �������� �� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="screen">���콺 Ŀ���� ��ũ�� ��ǥ</param>
    private void OnSlotPointerMove(Vector2 screen)
    {
        detail.MovePosition(screen);
    }
}
