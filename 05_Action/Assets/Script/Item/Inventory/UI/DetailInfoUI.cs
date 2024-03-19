using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DetailInfoUI : MonoBehaviour
{
    Image itemIcon;
    TextMeshProUGUI itemName;
    TextMeshProUGUI itemPrice;
    TextMeshProUGUI itemDescription;

    CanvasGroup canvasGroup;

    /// <summary>
    /// 알파값이 변하는 속도
    /// </summary>
    public float alphaChangeSpeed = 10.0f;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();      // 컴포넌트 찾기
        canvasGroup.alpha = 0.0f;

        Transform child = transform;
        itemIcon = transform.GetChild(0).GetComponent<Image>();
        itemName = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        itemPrice = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        itemDescription = transform.GetChild(4).GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// 상세 정보창 여는 함수
    /// </summary>
    /// <param name="itemData">표시할 아이템 데이터</param>
    public void Open(ItemData itemData)
    {
        // 컴포넌트들 채우기
        if(itemData != null)
        {
            itemIcon.sprite = itemData.itemIcon;
            itemName.text = itemData.itemName;
            itemPrice.text = itemData.price.ToString();
            itemDescription.text = itemData.itemDescription;

            MovePosition(Mouse.current.position.ReadValue());

            // 알파 변경 시작
            StopAllCoroutines();
            StartCoroutine(FadeIn());
        }
    }

    /// <summary>
    /// 상세 정보창이 닫힐 때 실행되는 함수
    /// </summary>
    public void Close()
    {
        // 알파 변경 시작(1 -> 0)
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    /// <summary>
    /// 상세 정보창을 움직이는 함수
    /// </summary>
    /// <param name="screenPos">스크린 좌표</param>
    public void MovePosition(Vector2 screenPos)
    {
        // Screen.width;   // 화면의 가로 해상도

        // 디테일 인포창을 screenPos로 이동시킨다.
        // 단 디테일 인포창이 화면 밖으로 벗어날 경우라도 창 전체가 보여야 한다.
        if(canvasGroup.alpha > 0.0f)
        {
            RectTransform rect = (RectTransform)transform;
            int over = (int)(screenPos.x + rect.sizeDelta.x) - Screen.width;
            screenPos.x -= Mathf.Max(0, over);  // over를 양수로만 사용(음수일때는 별도 처리 필요없음)
            rect.position = screenPos;
        }
    }

    /// <summary>
    /// 알파를 0 -> 1로 만드는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeIn()
    {
        while(canvasGroup.alpha < 1.0f)
        {
            canvasGroup.alpha += Time.deltaTime * alphaChangeSpeed;
            yield return null;
        }
        canvasGroup.alpha = 1.0f;
    }

    /// <summary>
    /// 알파를 1 -> 0으로 만드는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeOut()
    {
        while (canvasGroup.alpha > 0.0f)
        {
            canvasGroup.alpha -= Time.deltaTime * alphaChangeSpeed;
            yield return null;
        }
        canvasGroup.alpha = 0.0f;
    }
}
