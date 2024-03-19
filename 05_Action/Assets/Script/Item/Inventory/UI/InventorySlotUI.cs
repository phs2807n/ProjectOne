using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : SlotUI_Base, IDragHandler, IBeginDragHandler, IEndDragHandler, 
    IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
    
{
    /// <summary>
    /// ��񿩺� ǥ�ÿ� �ؽ�Ʈ
    /// </summary>
    TextMeshProUGUI equipText;

    public Action<uint> onDrageBegin;

    public Action<uint, bool> onDragEnd;

    public Action<uint> onClick;

    /// <summary>
    /// ���콺 Ŀ���� ���� ���� �ö�Դ�.(uint: Ŭ���� �� ������ �ε���
    /// </summary>
    public Action<uint> onPointEnter;

    /// <summary>
    /// ���콺 Ŀ���� ���Կ��� ������.(uint: ���� ������ �ε���)
    /// </summary>
    public Action onPointExit;

    /// <summary>
    /// ���콺 Ŀ�Ͱ� ���� ������ 
    /// </summary>
    public Action<Vector2> onPointerMove;

    protected override void Awake()
    {
        base.Awake();
        Transform child = transform.GetChild(2);
        equipText = child.GetComponent<TextMeshProUGUI>();
    }

    protected override void OnRefresh()
    {
        if(InvenSlot.IsEquipped)
        {
            equipText.color = Color.red;    // ����������� ������
        }
        else
        {
            equipText.color = Color.clear;  // ������� �ʾ��� ���� ����
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"�巡�� ���� : [{Index}]�� ����");
        onDrageBegin?.Invoke(Index);
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;    // UI ��� �������̚�
        if(obj != null)
        {
            InventorySlotUI endSlot = obj.GetComponent<InventorySlotUI>();
            if(endSlot != null)
            {
                // �����̴�.
                Debug.Log($"�巡�� ���� : [{endSlot.Index}]�� ����");
                onDragEnd?.Invoke(endSlot.Index, true);
            }
            else
            {
                // ������ �ƴϴ�.
                Debug.Log($"{endSlot.Index}");
                onDragEnd?.Invoke(Index, false);
            }
        }
        else
        {
            // ���콺 ��ġ�� � UI�� ����.
            Debug.Log("� UI�� ����.");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke(Index);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointEnter?.Invoke(Index);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onPointExit?.Invoke();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        onPointerMove?.Invoke(eventData.position);
    }
}
