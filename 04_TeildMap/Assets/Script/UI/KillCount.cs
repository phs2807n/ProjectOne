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
        target = count; // 새 킬카운트를 target으로 지정
    }

    private void Update()
    {
        current += Time.deltaTime * countingSpeed;      // current는 target까지 지속적으로 증가
        if(current > target)
        {
            current = target;   // 넘치는 것을 방지
        }
        imageNumber.Number = Mathf.FloorToInt(current);
    }
}
