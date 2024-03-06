using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSensor : MonoBehaviour
{
    /// <summary>
    /// �������� �� Ʈ���� �ȿ� ���Դٰ� �˸��� ��������Ʈ
    /// </summary>
    public Action<Slime> onEnemyEnter;

    /// <summary>
    /// �������� �� Ʈ���� ������ �����ٰ� �˸��� ��������Ʈ
    /// </summary>
    public Action<Slime> onEnemyExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Slime slime = collision.GetComponent<Slime>();
        if(slime != null )
        {
            Debug.Log($"{slime.gameObject.name}�� ã�Ҵ�.");
            onEnemyEnter?.Invoke(slime);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Slime slime = collision.GetComponent<Slime>();
        if (slime != null)
        {
            onEnemyExit?.Invoke(slime);
        }
    }
}
