using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class ImageNumber : MonoBehaviour
{
    public Sprite[] numberImages;

    public Image[] digits;

    /// <summary>
    /// ��ǥ��
    /// </summary>
    int number = -1;

    public int Number
    {
        get => number;
        set
        {
            if(number != value)
            {
                number = Mathf.Min(value, 99999);    // �ִ� 5�ڸ��� ���� ����

                int temp = number;

                for(int i = 0; i < digits.Length; i++)  // �ӽ� ������ number����
                {
                    if(temp != 0 || i == 0)                     // temp�� 0�� �ƴϸ� ó��
                    {
                        int digit = temp % 10;                // 1�ڸ� ���� �����ϱ�
                        digits[i].sprite = numberImages[digit]; // ������ ���ڿ� �°� �̹��� ����
                        digits[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        digits[i].gameObject.SetActive(false);  // temp�� 0�� �ƴϸ� �� �ڸ����� �Ⱥ��̰� �����(1�ڸ� ����)
                    }
                    temp /= 10;                                 // 1�ڸ� �� �����ϱ�    
                }
            }
        }
    }

    private void Awake()
    {
        digits = GetComponentsInChildren<Image>();  
    }
}


