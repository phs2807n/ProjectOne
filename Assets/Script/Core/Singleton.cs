using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : Component
{
    /// <summary>
    /// �� �̱����� �ʱ�ȭ�Ǿ����� Ȯ���ϱ� ���� ����
    /// </summary>
    bool isInitialized = false;

    /// <summary>
    /// ����ó���� ������ Ȯ���ϱ� ���� ����
    /// </summary>
    private static bool isShutdown = false;
    
    /// <summary>
    /// �� �̱����� ��ü(�ν��Ͻ�)
    /// </summary>
    private static T instance = null;

    /// <summary>
    /// �� �̱���
    /// </summary>
    public static T Instance
    {
        get
        {
            if(isShutdown)  // ����ó���� ������
            {
                Debug.LogWarning("�̱����� �׾����ϴ�.");    // �������ϰ� 
                return null;                                 // null ����
            }

            if (instance == null)        // ��ü�� ������
            {
                T singletion = FindAnyObjectByType<T>();            // �ٸ����� ������Ʈ�� �ش� �̱����� ������
                if(singletion == null)                              // �ٸ� ���� ������Ʈ���� �� �̱����� ������
                {
                    GameObject obj = new GameObject();              // �� ���� ������Ʈ �����
                    obj.name = "Singleton";                         // �̸� ������ ����
                    singletion = obj.AddComponent<T>();             // ��Ŭ�� ������Ʈ ���� �߰�
                }
                instance = singletion;                              // �ٸ� ���ӿ�����Ʈ�� �ִ� �̱����̳� ���θ��� �̱����� ����
                DontDestroyOnLoad(instance.gameObject);             // ���� ����� �� ���ӿ�����Ʈ�� �������� �ʵ��� ����
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if(instance == null)        // ���� �̹� ��ġ�� �ٸ� �̱����� ����.
        {  
            instance = this as T;        // ù��°�� ����
            DontDestroyOnLoad(instance.gameObject); // ���� ����� �� ���ӿ�����Ʈ�� �������� �ʵ��� ����
        }
        else                // 
        {
            if(instance != this)
            {
                Destroy(this.gameObject);   // �� �ڽ��� ����
            }
        }
    }

    protected virtual void OnEnable()
    {
        // SceneManager.sceneLoaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    protected virtual void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// ���� �ε�Ǿ��� �� ȣ��� �Լ�
    /// </summary>
    /// <param name="scene">������</param>
    /// <param name="mode">�ε����</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(!isInitialized)
        {
            OnPreInitialize();
        }
        if(mode != LoadSceneMode.Additive)
        {
            OnInitialize();
        }
    }

    /// <summary>
    /// ��Ŭ���� ������� �� �� �ѹ��� ȣ��Ǵ� �Լ�
    /// </summary>
    protected virtual void OnPreInitialize()
    {
        isInitialized = true;
    }

    /// <summary>
    /// �̱����� ��������� ���� Single�� �ε�� ������ ȣ��� �Լ�(additive�� �ȵ�)
    /// </summary>
    protected virtual void OnInitialize()
    {

    }

    private void OnApplicationQuit()
    {
        isShutdown = true;
    }
}