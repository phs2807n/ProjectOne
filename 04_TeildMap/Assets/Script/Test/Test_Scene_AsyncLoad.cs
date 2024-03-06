using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Test_Scene_AsyncLoad : TestBase
{
    /// <summary>
    /// 다음에 로딩씬이 끝나고 나서 불려질 씬의 이름
    /// </summary>
    public string nextScencName = "LoadingSampleScenes";

    /// <summary>
    /// 유니티에서 비동기 명령 처리를 위해 필요한 클래스
    /// </summary>
    AsyncOperation async;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        async = SceneManager.LoadSceneAsync(nextScencName);
        async.allowSceneActivation = false; // 비동기 씬 로딩이 완료되어도 자동으로 씬 전환을 하지 않는다.
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        async.allowSceneActivation = true;  // 비동기 씬 로딩이 완료되면 자동으로 씬 전환을 한다.
    }

    IEnumerator LoadSceneCoroutine()
    {
        async = SceneManager.LoadSceneAsync(nextScencName);
        async.allowSceneActivation = false;

        while(async.progress < 0.9f)    // allowSceneActivation가 false면 progress는 0.9가 최대(로딩완료 = 0.9)
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
