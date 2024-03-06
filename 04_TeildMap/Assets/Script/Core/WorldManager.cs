using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour
{
    /// <summary>
    /// ���� ���� ����
    /// </summary>
    const int HeightCount = 3;

    /// <summary>
    /// ���� ���� ����
    /// </summary>
    const int WidthCount = 3;

    /// <summary>
    /// �� �ϳ��� ���� ����
    /// </summary>
    const float mapHeightSize = 20.0f;

    /// <summary>
    /// �� �ϳ��� ���� ����
    /// </summary>
    const float mapWidthSize = 20.0f;

    /// <summary>
    /// ������ ����(��� ���� ������ �� ���� �Ʒ� ������ ����)
    /// </summary>
    readonly Vector2 worldOrigin = new Vector2(-mapWidthSize * WidthCount * 0.5f, -mapHeightSize * HeightCount * 0.5f);

    /// <summary>
    /// �� �̸� ���տ� �⺻ �̸�
    /// </summary>
    const string SceneNameBase = "Seemless";

    /// <summary>
    /// ��� ���� �̸��� ������ �迭
    /// </summary>
    string[] sceneNames;

    /// <summary>
    /// ���� �ε� ���¸� ��Ÿ�� enum
    /// </summary>
    enum SceneLoadState : byte
    {
        Unload = 0,     // �ε��� �ȵǾ��ִ� ����(��ü�Ǿ� �ִ� ����)
        PendingUnload,  // �ε� ��ü �������� ����
        PendingLoad,    // �ε� �������� ����
        Loaded          // �ε� �Ϸ�� ����
    }

    /// <summary>
    /// ��� ���� �ε� ����
    /// </summary>
    SceneLoadState[] sceneLoadStates;

    /// <summary>
    /// ��� ���� ���ε� �Ǿ����� Ȯ���ϱ� ���� ������Ƽ(��� ���� Unloade���¸� true, �ƴϸ� false) 
    /// </summary>
    public bool IsUnloadAll
    {
        get
        {
            bool result = true;
            foreach(SceneLoadState state in sceneLoadStates)
            {
                if(state != SceneLoadState.Unload)  // �ϳ��� unload�� �ƴϸ� false
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
    }

    /// <summary>
    /// �ε� ��û�� ���� ���� ���
    /// </summary>
    List<int> loadWork = new List<int>();

    /// <summary>
    /// �ε��� �Ϸ�� ���� ���
    /// </summary>
    List<int> loadWorkComplete = new List<int>();

    /// <summary>
    /// �ε� ��ü ��û�� ���� ���� ���
    /// </summary>
    List<int> unloadWork = new List<int>();

    /// <summary>
    /// �ε� ��ü�� �Ϸ�� ���� ���
    /// </summary>
    List<int> unloadWorkComplete = new List<int>();

    /// <summary>
    /// ó�� ��������� �� �� ���� ����Ǵ� �Լ�
    /// </summary>
    public void PreInitialize()
    {
        int mapCount = HeightCount * WidthCount;
        sceneNames = new string[mapCount];
        sceneLoadStates = new SceneLoadState[mapCount];

        for(int y = 0; y < HeightCount; y++)
        {
            for(int x = 0; x < WidthCount; x++)
            {
                int index = GetIndex(x, y);
                sceneNames[index] = $"{SceneNameBase}_{x}_{y}";
                sceneLoadStates[index] = SceneLoadState.Unload;
            }
        }
    }

    /// <summary>
    /// ���� single�� �ε��� ������ ȣ��� �ʱ�ȭ�� �Լ�
    /// </summary>
    public void Initialize()
    {
        // �� �ε� ���� �ʱ�ȭ
        for(int i = 0; i < sceneLoadStates.Length; i++)
        {
            sceneLoadStates[i] = SceneLoadState.Unload;
        }

        Player player = GameManager.Instance.Player;
        if(player != null)
        {
            player.onMapChange += (currentGrid) =>
            {
                RefreshScenes(currentGrid.x, currentGrid.y);
            };

            player.onDie += (_, _) =>
            {
                // �÷��̾ ������ ��� ���� �ε���ü ��û�ϱ�
                for(int y = 0; y < HeightCount; y++)
                {
                    for(int x = 0; x < WidthCount; x++)
                    {
                        RequestAsyncSceneUnload(x, y);
                    }
                }
            };

            Vector2Int grid = WorldToGrid(player.transform.position);   // �÷��̾ �ִ� ���� �׸��� �� ��������
            Debug.Log($"{grid}");
            RequestAsyncSceneLoad(grid.x, grid.y);  // �÷��̾ �ִ� ���� �ֿ켱���� �ε� ��û
            RefreshScenes(grid.x, grid.y);          // �ֺ��� �ε� ��û
        }
    }

    /// <summary>
    /// ���� �׸��� ��ġ�� �ε����� �������ִ� �Լ�
    /// </summary>
    /// <param name="x">���� x��ġ</param>
    /// <param name="y">���� y��ġ</param>
    /// <returns>�迭�� �ε��� ��</returns>
    int GetIndex(int x, int y)
    {
        return x + y * WidthCount;
    }

    /// <summary>
    /// �񵿱� �ε� ��û �Լ�
    /// </summary>
    /// <param name="x">�ε��� ���� x��ġ</param>
    /// <param name="y">�ε��� ���� y��ġ</param>
    void RequestAsyncSceneLoad(int x, int y)
    {
        int index = GetIndex(x, y);     // �ε��� ���   
        if (sceneLoadStates[index] == SceneLoadState.Unload)
        {
            loadWork.Add(index);      // Unload�� ���� �ε� ����Ʈ�� �߰�
        }
    }

    /// <summary>
    /// ���� �񵿱�� �ε��ϴ� �Լ�(Additive �ε�)
    /// </summary>
    /// <param name="index">�ε��� ���� �ε���</param>
    void AsyncSceneLoad(int index)
    {
        if (sceneLoadStates[index] == SceneLoadState.Unload)    // Unload ������ �ʸ� ó��
        {
            sceneLoadStates[index] = SceneLoadState.PendingLoad;    // panding ���·� ���� ���� ���̶�� ǥ��

            //Debug.Log(sceneNames[index]);
            AsyncOperation async = SceneManager.LoadSceneAsync(sceneNames[index], LoadSceneMode.Additive);  // �񵿱� �ε� ����
            async.completed += (_) =>       // �񵿱� �۾��� ������ �� ����Ǵ� ��������Ʈ�� �����Լ� �߰�
            {
                sceneLoadStates[index] = SceneLoadState.Loaded; // Loaded ���·� ����
                loadWorkComplete.Add(index);                    // �Ϸ� ��Ͽ� �߰�
            };
        }
    }

    /// <summary>
    /// �񵿱� �ε� ��ü ��û �Լ�
    /// </summary>
    /// <param name="x">�ε���ü�� ���� x��ġ</param>
    /// <param name="y">�ε���ü�� ���� y��ġ</param>
    void RequestAsyncSceneUnload(int x, int y)
    {
        int index = GetIndex(x, y);     // �ε��� ���
        if (sceneLoadStates[index] == SceneLoadState.Loaded)
        {
            unloadWork.Add(index);      // �ε� �Ϸ�Ǿ��� 
        }
    }

    /// <summary>
    /// �񵿱� �ε� ��ü�� ó���ϴ� �Լ�
    /// </summary>
    /// <param name="index">�ε� ��ü�� ���� �ε���</param>
    void AsyncSceneUnload(int index)
    {
        if (sceneLoadStates[index] == SceneLoadState.Loaded)            // �ε��� �Ϸ�� �ʸ� ó��
        {
            sceneLoadStates[index] = SceneLoadState.PendingUnload;      // �ε� ��ü���̶�� ǥ��

            // �ʿ� �ִ� �������� Ǯ�� �ǵ�����(�� ��ε�� �����Ǵ� �� ����
            Scene scene = SceneManager.GetSceneByName(sceneNames[index]);   // �� ã��
            if(scene.isLoaded)
            {
                GameObject[] sceneRoots = scene.GetRootGameObjects();       // ��Ʈ ������Ʈ ��� ã��
                if(sceneRoots != null && sceneRoots.Length > 0)             // ��Ʈ ������Ʈ 1�� �̻� ������
                {
                    Slime[] slimes = sceneRoots[0].GetComponentsInChildren<Slime>();    // ������ ��� ã�Ƽ�
                
                    foreach(Slime slime in slimes)
                    {
                        slime.ReturnToPool();
                    }
                }
            }

            AsyncOperation async = SceneManager.UnloadSceneAsync(sceneNames[index]);
            async.completed += (_) =>
            {
                sceneLoadStates[index] = SceneLoadState.Unload;
                unloadWorkComplete.Add(index);
            };
        }
    }



    private void Update()
    {
        // �Ϸ�� �۾��� ����Ʈ���� ����
        foreach(var index in loadWorkComplete)
        {
            loadWork.RemoveAll((x) => x == index);  // loadWork����Ʈ���� index�� ���� �������� ��� ����
        }
        loadWorkComplete.Clear();

        // ���� ��û ó��
        foreach(var index in loadWork)
        {
            AsyncSceneLoad(index);
        }

        // �Ϸ�� �۾��� ����Ʈ���� ����
        foreach(var index in unloadWorkComplete)
        {
            unloadWork.RemoveAll((x) => x == index);  // loadWork����Ʈ���� index�� ���� �������� ��� ����
        }
        unloadWorkComplete.Clear();

        // ���� ��û ó��
        foreach(var index in unloadWork)
        {
            AsyncSceneUnload(index);        // �񵿱� �ε� ��ü ����(������ �ǵ�����)
        }
    }

    /// <summary>
    /// ������ ��ġ �ֺ� ���� �ε� ��û�ϰ� �� �ܴ� �ε� ��ü�� ��û�ϴ� �Լ�
    /// </summary>
    /// <param name="mapX">������ ���� x ��ġ</param>
    /// <param name="mapY">������ ���� y ��ġ</param>
    void RefreshScenes(int mapX, int mapY)
    {
        int startX = Mathf.Max(0, mapX - 1);    // �������� (mapX,mapY)���� 1�۰ų� (0,0)
        int startY = Mathf.Max(0, mapY - 1);
        int endX = Mathf.Min(WidthCount, mapX + 2); // �ִ����� (mapX, mapY)���� 1ũ�ų� (WidthCount, HeightCount)
        int endY = Mathf.Min(HeightCount, mapY + 2);

        // (mapX,mapY) �ֺ��� RequestAsyncSceneLoad ����
        List<Vector2Int> open = new List<Vector2Int>(9);
        for(int y = startY; y < endY; y++)
        {
            for(int x = startX; x < endX; x++)
            {
                RequestAsyncSceneLoad(x, y);        // �ش� �ϴ� �͵� �ε� ��ó
                open.Add(new Vector2Int(x, y));     // �ε� ��û�� �͵� ���
            }
        }

        // ������ �κ��� ��� RequestAsyncSceneUnload ����
        for(int y = 0; y < HeightCount; y++)
        {
            for(int x = 0; x < WidthCount; x++)
            {
                // Contains : �ܼ� �ִ� ���� Ȯ�ο�
                // Exits : Ư�� ������ �����ϴ� ���� ������ Ȯ�ο�
                if(!open.Contains(new Vector2Int(x, y)))
                {
                    RequestAsyncSceneUnload(x, y);
                }
            }
        }
    }

    /// <summary>
    /// ���� ��ǥ�� � �ʿ� ���ϴ��� ����ϴ� �Լ�
    /// </summary>
    /// <param name="worldPosition">Ȯ���� ���� ��ǥ</param>
    /// <returns>���� ��ǥ( (0,0) ~ (2,2) )</returns>
    public Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        Vector2 offset = (Vector2)worldPosition - worldOrigin;

        return new Vector2Int((int)(offset.x/mapWidthSize), (int)(offset.y/mapHeightSize));
    }

#if UNITY_EDITOR
    public void TestLoadScene(int x, int y)
    {
        RequestAsyncSceneLoad(x, y);
    }

    public void TestUnloadScene(int x, int y)
    {
        RequestAsyncSceneUnload(x, y);
    }

    public void TestRefreshscenes(int x, int y)
    {
        RefreshScenes(x, y);
    }

    public void TestUnloadAllScene()
    {
        for(int y = 0; y < HeightCount; y++)
        {
            for(int x = 0;x < WidthCount; x++)
            {
                RequestAsyncSceneUnload(x,y);
            }
        }
    }
#endif
}
