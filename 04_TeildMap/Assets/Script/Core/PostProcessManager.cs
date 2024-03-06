using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessManager : MonoBehaviour
{
    /// <summary>
    /// ����Ʈ���μ����� ����Ǵ� �ҷ�
    /// </summary>
    Volume postProcessVolume;

    /// <summary>
    /// �ҷ��ȿ� �ִ� ���Ʈ�� ��ü
    /// </summary>
    Vignette vignette;

    public AnimationCurve curve;

    private void Awake()
    {
        postProcessVolume = GetComponent<Volume>();
        postProcessVolume.profile.TryGet<Vignette>(out vignette);   // �ҷ����� ���Ʈ�� ���������� �õ�
    }

    private void Start()
    {
        Player player = GameManager.Instance.Player;
        player.onLifeTimeChange += OnLifeTimeChange;
    }

    private void OnLifeTimeChange(float radio)
    {
        vignette.intensity.value = curve.Evaluate(radio);
    }
}
