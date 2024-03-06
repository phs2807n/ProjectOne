using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Test_Scene_AsyncLoad : TestBase
{
    /// <summary>
    /// ������ �ε����� ������ ���� �ҷ��� ���� �̸�
    /// </summary>
    public string nextScencName = "LoadingSampleScenes";

    /// <summary>
    /// ����Ƽ���� �񵿱� ��� ó���� ���� �ʿ��� Ŭ����
    /// </summary>
    AsyncOperation async;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        async = SceneManager.LoadSceneAsync(nextScencName);
        async.allowSceneActivation = false; // �񵿱� �� �ε��� �Ϸ�Ǿ �ڵ����� �� ��ȯ�� ���� �ʴ´�.
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        async.allowSceneActivation = true;  // �񵿱� �� �ε��� �Ϸ�Ǹ� �ڵ����� �� ��ȯ�� �Ѵ�.
    }

    IEnumerator LoadSceneCoroutine()
    {
        async = SceneManager.LoadSceneAsync(nextScencName);
        async.allowSceneActivation = false;

        while(async.progress < 0.9f)    // allowSceneActivation�� false�� progress�� 0.9�� �ִ�(�ε��Ϸ� = 0.9)
        {
            Debug.Log($"Progress : {async.progress}");
            yield return null;
        }

        Debug.Log("Loading Complete");
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        StartCoroutine(LoadSceneCoroutine());
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(nextScencName);
    }
}
