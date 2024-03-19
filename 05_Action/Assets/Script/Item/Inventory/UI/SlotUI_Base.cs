using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI_Base : MonoBehaviour
{
    /// <summary>
    /// �� UI�� ������ ����
    /// </summary>
    InventorySlot invenSlot;

    /// <summary>
    /// ���� Ȯ�ο� ������Ƽ
    /// </summary>
    public InventorySlot InvenSlot => invenSlot;

    /// <summary>
    /// �������� �������� ǥ���� UI
    /// </summary>
    Image itemIcon;

    /// <summary>
    /// �������� ������ Ȯ���ϴ� UI
    /// </summary>
    TextMeshProUGUI itemCount;

    /// <summary>
    /// ������ �ε���
    /// </summary>
    public uint Index => invenSlot.Index;

    protected virtual void Awake()
    {
        Transform child = transform.GetChild(0);
        itemIcon = child.GetComponent<Image>();
        child = transform.GetChild(1);
        itemCount = child.GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// ������ �ʱ�ȭ �ϴ� �Լ�(=InventorySlot�� InvenSlotUI�� ����)
    /// </summary>
    /// <param name="slot"></param>
    public virtual void InitializeSlot(InventorySlot slot)
    {
        invenSlot = slot;
        invenSlot.onSlotItemChange = Refresh;   // ������ �����ۿ� ������ ���� �� Refresh�Լ� ����(������ �Լ��� ��� ����)
        Refresh();  // ù ȭ�� ����
    }

    /// <summary>
    /// ���� UI�� ȭ�� ����
    /// </summary>
    private void Refresh()
    {
        if (invenSlot.IsEmpty)
        {
            itemIcon.color = Color.clear;   // ������ �����ϰ�
            itemIcon.sprite = null;         // ��������Ʈ ����
            itemCount.text = string.Empty;  // ���ڵ� ����
        }
        else
        {
            itemIcon.sprite = InvenSlot.ItemData.itemIcon;  // ��������Ʈ �̹��� ����
            itemIcon.color = Color.white;                   // �̹��� ���̰� �����
            itemCount.text = InvenSlot.ItemCount.ToString();// ������ ���� ����
        }
        OnRefresh();
    }

    /// <summary>
    /// ȭ�� ������ �� �ڽ� Ŭ�������� ���������� ����� �ڵ�
    /// </summary>
    protected virtual void OnRefresh()
    {
        // ��� ���� ǥ�� 
    }
}
