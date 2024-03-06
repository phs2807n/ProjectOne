using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeTimeGauge : MonoBehaviour
{
    Slider slider;
    Image fill;

    public Color startColor = Color.white;
    public Color endColor = Color.red;
    public Gradient color;



    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.value = 1;

        Transform child = transform.GetChild(1);
        child = child.GetChild(0);
        fill = child.GetComponent<Image>();
    }

    private void Start()
    {
        GameManager.Instance.Player.onLifeTimeChange += OnLifeTimeChange;
    }

    private void OnLifeTimeChange(float radio)
    {
        slider.value = radio;

        //fill.color = Color.Lerp(startColor, endColor, radio);
        //Debug.Log(Color.Lerp(startColor, endColor, radio));

        fill.color = color.Evaluate(radio);
    }
}
