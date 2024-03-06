using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncLoadingScene : MonoBehaviour
{
    /// <summary>
    /// ������ �ε����� ������ ���� �ҷ��� ���� �̸�
    /// </summary>
    public string nextScencName = "LoadingSampleScenes";

    /// <summary>
    /// ����Ƽ���� �񵿱� ��� ó���� ���� �ʿ��� Ŭ����
    /// </summary>
    AsyncOperation async;

    /// <summary>
    /// slider�� value�� ������ �� ��
    /// </summary>
    float loadRatio = 0.0f;

    /// <summary>
    /// slider�� value�� �����ϴ� �ӵ�(�ʴ�)
    /// </summary>
    public float loadingBarSpeed = 1.0f;

    public float loadingTextSpeed = 0.2f;

    /// <summary>
    /// ���� ����� �ڷ�ƾ
    /// </summary>
    IEnumerator loadingTextCoroutine;

    /// <summary>
    /// �ε� �Ϸ� ǥ��(true�� �Ϸ�, false �̿�)
    /// </summary>
    bool loadingDone = false;


    // UI
    Slider loadingSlider;
    TextMeshProUGUI loadingText;

    // �Է� ó����
    PlayerInputActions inputActions;


    private void Awake()
    {
        inputActions  = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.UI.Click.performed += Press;
        inputActions.UI.AnyKey.performed += Press;
    }

    private void OnDisable()
    {
        inputActions.UI.AnyKey.performed -= Press;
        inputActions.UI.Click.performed -= Press;
        inputActions.UI.Disable();
    }

    private void Start()
    {
        loadingSlider = FindAnyObjectByType<Slider>();
        loadingText = FindAnyObjectByType<TextMeshProUGUI>();

        loadingTextCoroutine = LoadingTextProgress();

        StartCoroutine(loadingTextCoroutine);
        StartCoroutine(AsyncLoadScene());
    }

    private void Update()
    {
        // �����̴��� value�� loadRatio�� �� ������ ��� ����
        if(loadingSlider.value < loadRatio)
        {
            loadingSlider.value += Time.deltaTime * loadingBarSpeed;
        }
    }

    /// <summary>
    /// ���콺�� Ű�� �������� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="context"></param>
    private void Press(InputAction.CallbackContext context)
    {
        async.allowSceneActivation = loadingDone;   // loadingDone�� true�� allowSceneActivation�� true�� �����
    }

    /// <summary>
    /// ������ ����� ��� �����ϴ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadingTextProgress()
    {
        WaitForSeconds wait = new WaitForSeconds(loadingTextSpeed);

        string[] texts =
        {
            "Loading",
            "Loading .",
            "Loading . .",
            "Loading . . .",
            "Loading . . . .",
            "Loading . . . . ."
        };

        int index = 0;
        while (true)
        {
            loadingText.text = texts[index];
            index++;
            index = index % texts.Length;
            yield return wait;
        }
    }

    /// <summary>
    /// �񵿱�� ���� �ε��ϴ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator AsyncLoadScene()
    {
        loadRatio = 0.0f;
        loadingSlider.value = loadRatio;

        async = SceneManager.LoadSceneAsync(nextScencName); // �񵿱� �ε� ����    
        async.allowSceneActivation = false;                 // �ڵ����� ����ȯ���� �ʵ��� �ϱ�
    
        while(loadRatio < 1.0f)
        {
            loadRatio += async.progress + 0.1f;     // �ε� �������� ���� loadRatio ����
            yield return null;
        }

        // �����ִ� �����̴��� �� �� ������ ��ٸ���
        yield return new WaitForSeconds((1 - loadingSlider.value) / loadingBarSpeed);

        StopCoroutine(loadingTextCoroutine);    // ���� ���� �ȵǰ� �����    
        loadingText.text = "Loading\nComplete"; // �Ϸ��
        loadingDone = true;                     // �ε�
    }
}
