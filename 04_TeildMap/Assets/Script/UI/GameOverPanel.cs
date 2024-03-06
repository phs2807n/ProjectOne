using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    public float alphaChangeSpeed = 1.0f;
    CanvasGroup canvasGroup;
    TextMeshProUGUI playTime;
    TextMeshProUGUI killCount;
    Button restart;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        Transform child = transform.GetChild(1);
        playTime = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(2);
        killCount = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(3);
        restart = child.GetComponent<Button>();

        restart.onClick.AddListener(() =>
        {
            StartCoroutine(WaitUnloadAll());
        });      // restart��ư�� �������� AddListener�� ����� �Լ��� ����ȴ�.
    }

    private void Start()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        Player player = GameManager.Instance.Player;
        player.onDie += OnPlayerDie;
    }

    private void OnPlayerDie(float totalPlayTime, int totalKillCount)
    {
        playTime.text = $"Total Play Time\n\r< {totalPlayTime:f1} Sec >";
        killCount.text = $"Total Kill Count\n\r< {totalKillCount} Kill >";
        StartCoroutine(StartAlphaChange());
    }

    IEnumerator StartAlphaChange()
    {
        while(canvasGroup.alpha < 1.0f)
        {
            canvasGroup.alpha += Time.deltaTime * alphaChangeSpeed;
            yield return null;
        }
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    IEnumerator WaitUnloadAll()
    {
        WorldManager world = GameManager.Instance.World;
        while (!world.IsUnloadAll)
        {
            yield return null;
        }
        SceneManager.LoadScene("AsyncLoadScene");
    }
}
