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
    /// 장비여부 표시용 텍스트
    /// </summary>
    TextMeshProUGUI equipText;

    public Action<uint> onDrageBegin;

    public Action<uint, bool> onDragEnd;

    public Action<uint> onClick;

    /// <summary>
    /// 마우스 커서가 슬롯 위로 올라왔다.(uint: 클릭이 된 슬롯의 인덱스
    /// </summary>
    public Action<uint> onPointEnter;

    /// <summary>
    /// 마우스 커서가 슬롯에서 나갔다.(uint: 나간 슬롯의 인덱스)
    /// </summary>
    public Action onPointExit;

    /// <summary>
    /// 마우스 커것가 슬롯 위에서 
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
            equipText.color = Color.red;    // 장비했을때는 빨간색
        }
        else
        {
            equipText.color = Color.clear;  // 장비하지 않았을 때는 투명
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"드래그 시작 : [{Index}]번 슬롯");
        onDrageBegin?.Invoke(Index);
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;    // UI 대산 레이케이슽
        if(obj != null)
        {
            InventorySlotUI endSlot = obj.GetComponent<InventorySlotUI>();
            if(endSlot != null)
            {
                // 슬롯이다.
                Debug.Log($"드래그 종료 : [{endSlot.Index}]번 슬롯");
                onDragEnd?.Invoke(endSlot.Index, true);
            }
            else
            {
                // 슬롯이 아니다.
                Debug.Log($"{endSlot.Index}");
                onDragEnd?.Invoke(Index, false);
            }
        }
        else
        {
            // 마우스 위치에 어떤 UI도 없다.
            Debug.Log("어떤 UI도 없다.");
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
