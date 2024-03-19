using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 인벤토리에서 몇번째 슬롯인지를 나타내는 변수
public class InventorySlot
{
    /// <summary>
    /// 인벤토리에서 몇 번째 슬롯인지를 나타내는 변수
    /// </summary>
    uint slotIndex;

    /// <summary>
    /// 슬롯 인덱스를 확인하기 위한 프로퍼티
    /// </summary>
    public uint Index => slotIndex;

    /// <summary>
    /// 이 슬롯에 들어있는 아이템 종류(null이면 슬롯이 비어있는 것)
    /// </summary>
    ItemData slotItemData = null;

    public ItemData ItemData
    {
        get => slotItemData;
        private set // 내부에서만 설정 가능
        {
            slotItemData = value;
            onSlotItemChange?.Invoke(); // 변경이 일어나면 델리게이트로 알린다.
        }
    }

    /// <summary>
    /// 슬롯에 들어있는 아이템의 종류, 개수, 장비여부의 변경을 알리는 델리게이트
    /// </summary>
    public Action onSlotItemChange;

    /// <summary>
    /// 슬롯에 아이템이 있는지 없는지 확인하기 위한 프로퍼티
    /// </summary>
    public bool IsEmpty => slotItemData == null;

    /// <summary>
    /// 이 슬롯에 들어있는 아이템 갯수
    /// </summary>
    uint itemCount = 0;

    /// <summary>
    /// 아이템 갯수를 확인하기 위한 프로퍼티 (쓰기는 private)
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
    /// 이 슬롯의 아이템이 장비되었는지 여부
    /// </summary>
    bool isEquipped = false;

    /// <summary>
    /// 이 슬롯의 장비 여부를 확인하기 위한 프로퍼티(set는 private
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
    /// 생성자
    /// </summary>
    /// <param name="index">슬롯의 인덱스</param>
    public InventorySlot(uint index)
    {
        slotIndex = index;  // 생성할 때만 설정하고 절대 변하지 않아야한다.
        ItemData = null;
        itemCount = 0;
        IsEquipped = false;
    }

    /// <summary>
    /// 이 슬롯에 아이템을 설정(set)하는 함수
    /// </summary>
    /// <param name="data">설정할 아이템의 종류</param>
    /// <param name="count">설정할 아이템의 갯수</param>
    /// <param name="isEquipped">장비 상태</param>
    public virtual void AssignSlotItem(ItemData data, uint count = 1, bool isEquipped = false, uint? from = null)
    {
        if(data != null)
        {
            ItemData = data;
            ItemCount = count;
            IsEquipped = isEquipped;
            Debug.Log($"인벤토리 [{slotIndex}]번 슬롯에 [{ItemData.itemName}]아이템이 [{ItemCount}]개 설정");
        }
        else
        {
            ClearSlotItem();
        }
    }

    /// <summary>
    /// 이 슬롯을 비우는 함수
    /// </summary>
    public virtual void ClearSlotItem()
    {
        ItemData = null;
        itemCount = 0;
        isEquipped = false;
        Debug.Log($"인벤토리 [{slotIndex}]번 슬롯을 비옵니다.");
    }

    // 아이템 갯수 변화

    /// <summary>
    /// 이 슬롯에 아이템 갯수를 증가시키는 함수
    /// </summary>
    /// <param name="overCount">return이 fasle일 때 못먹고 남은 갯수</param>
    /// <param name="increaseCount">증가시킬 개수</param>
    /// <returns>슬롯에 increaseCount만큼 증가시켰으면 true, 다 증가시키지 못했으면 false</returns>
    public bool IncreaseSlotItem(out uint overCount, uint increaseCount = 1)
    {
        bool result;

        uint newCount = ItemCount + increaseCount;
        int over = (int)newCount - (int)ItemData.maxStackCount;

        Debug.Log($"인벤토리 [{slotIndex}]번 슬롯에 아이템이 증가, 현재 [{ItemCount}]개");

        if( over > 0 )
        {
            // 넘쳤다.
            ItemCount = ItemData.maxStackCount;
            overCount = (uint)over;
            result = false;
            Debug.Log($"아이템이 최대치까지 증가. [{over}]개 넘침");
        }
        else
        {
            // 정상
            ItemCount = newCount;
            overCount = 0;
            result = true;
            Debug.Log($"[{increaseCount}]개 증가");
        }

        return result;
    }

    public void DecreaseSlotItem(uint decreaseCount = 1)
    {
        int newCount = (int)ItemCount - (int)decreaseCount;
        if( newCount > 0 )
        {
            // 아직 아이템이 남아있음
            ItemCount = (uint)newCount;
            Debug.Log($"인벤토리 [{slotIndex}]번 슬롯에 [{ItemData.itemName}]이 [{decreaseCount}]개 감소, 현재 [{ItemCount}]개");
        }
        else
        {
            // 다 없어짐
            ClearSlotItem();
        }
    }

    /// <summary>
    /// 이 슬롯의 아이템을 사용하는 함수
    /// </summary>
    /// <param name="target">아이템의 효과를 받을 대상</param>
    public void UseItem(GameObject target)
    {

    }

    /// <summary>
    /// 이 슬롯의 아이템을 장비하는 함수
    /// </summary>
    /// <param name="target"></param>
    public void EquipItem(GameObject target)
    {

    }
}
