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
    /// ���İ��� ���ϴ� �ӵ�
    /// </summary>
    public float alphaChangeSpeed = 10.0f;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();      // ������Ʈ ã��
        canvasGroup.alpha = 0.0f;

        Transform child = transform;
        itemIcon = transform.GetChild(0).GetComponent<Image>();
        itemName = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        itemPrice = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        itemDescription = transform.GetChild(4).GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// �� ����â ���� �Լ�
    /// </summary>
    /// <param name="itemData">ǥ���� ������ ������</param>
    public void Open(ItemData itemData)
    {
        // ������Ʈ�� ä���
        if(itemData != null)
        {
            itemIcon.sprite = itemData.itemIcon;
            itemName.text = itemData.itemName;
            itemPrice.text = itemData.price.ToString();
            itemDescription.text = itemData.itemDescription;

            MovePosition(Mouse.current.position.ReadValue());

            // ���� ���� ����
            StopAllCoroutines();
            StartCoroutine(FadeIn());
        }
    }

    /// <summary>
    /// �� ����â�� ���� �� ����Ǵ� �Լ�
    /// </summary>
    public void Close()
    {
        // ���� ���� ����(1 -> 0)
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    /// <summary>
    /// �� ����â�� �����̴� �Լ�
    /// </summary>
    /// <param name="screenPos">��ũ�� ��ǥ</param>
    public void MovePosition(Vector2 screenPos)
    {
        // Screen.width;   // ȭ���� ���� �ػ�

        // ������ ����â�� screenPos�� �̵���Ų��.
        // �� ������ ����â�� ȭ�� ������ ��� ���� â ��ü�� ������ �Ѵ�.
        if(canvasGroup.alpha > 0.0f)
        {
            RectTransform rect = (RectTransform)transform;
            int over = (int)(screenPos.x + rect.sizeDelta.x) - Screen.width;
            screenPos.x -= Mathf.Max(0, over);  // over�� ����θ� ���(�����϶��� ���� ó�� �ʿ����)
            rect.position = screenPos;
        }
    }

    /// <summary>
    /// ���ĸ� 0 -> 1�� ����� �ڷ�ƾ
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
    /// ���ĸ� 1 -> 0���� ����� �ڷ�ƾ
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
