using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCount : MonoBehaviour
{
    public float countingSpeed = 1.0f;

    float target = 0.0f;
    float current = 0.0f;

    ImageNumber imageNumber;

    private void Awake()
    {
        imageNumber = GetComponent<ImageNumber>();       
    }

    private void Start()
    {
        Player player = GameManager.Instance.Player;
        player.onKillCountChange += OnKillCountChange;
    }

    private void OnKillCountChange(int count)
    {
        target = count; // �� ųī��Ʈ�� target���� ����
    }

    private void Update()
    {
        current += Time.deltaTime * countingSpeed;      // current�� target���� ���������� ����
        if(current > target)
        {
            current = target;   // ��ġ�� ���� ����
        }
        imageNumber.Number = Mathf.FloorToInt(current);
    }
}
