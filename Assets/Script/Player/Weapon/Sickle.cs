using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sickle : Bullet
{
    /// <summary>
    /// ȸ�� �ӵ�
    /// </summary>
    float rotateSpeed = -560.0f;

    protected override void Update()
    {
        base.Update();

        transform.Rotate(0, 0, Time.deltaTime * rotateSpeed);
    }
}
