using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimePool : ObjectPool<Slime>
{
    protected override void GenerateObject(Slime comp)
    {
        comp.Pool = comp.transform.parent;  // pool ����
        comp.ShowPath(GameManager.Instance.showSlimePath);
    }
}
