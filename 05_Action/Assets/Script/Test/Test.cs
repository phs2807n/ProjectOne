using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
        GameObject objName = GameObject.Find("�̸�");                           // �̸����� ã��
        GameObject objTag1 = GameObject.FindWithTag("��ũ");                    // �±׷� ã�� - �Լ� �̸��� �ٿ� ���� ��
        GameObject objTag2 = GameObject.FindGameObjectWithTag("��ũ");          // �±׷� ã��
        GameObject[] objTag3 = GameObject.FindGameObjectsWithTag("�±�");       // ���� �±� ��� ã��
        GameObject objType1 = GameObject.FindAnyObjectByType<GameObject>();     // Ư�� Ÿ������ ã��(�ƹ��ų� 1����, ���� ������ ����Ұ���, FindFirst
        GameObject objType2 = GameObject.FindFirstObjectByType<GameObject>();   // Ư�� Ÿ������ ã��(Ÿ�� �� ù ��°)

        // Ư��Ÿ�� ��� ã��(�Ķ���ͷ� ��Ȱ��ȭ�� ������Ʈ �����ҽ� ��������)
        GameObject[] objType3 = GameObject.FindObjectsByType<GameObject>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        // FindObjectOfType : �����

        // ������Ʈ ã��
        Transform t = GetComponent<Transform>();        // = this.transform;

        // ������Ʈ �߰��ϱ�
        this.gameObject.AddComponent<Test>();
    }
}
