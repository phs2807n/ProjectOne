using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : RecycleObject
{
    /// <summary>
    /// 관통력
    /// </summary>
    public int per;

    /// <summary>
    /// 공격력
    /// </summary>
    public float damage;

    public float speed;

    public float lifetime = 10.0f;

    public Vector3 Dir;

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(LifeOver(lifetime));
    }

    public void Init(int _per, float _damage, float _speed, Vector3 dir, float angle = 0.0f)
    {
        Debug.Log("생성");

        per = _per;
        damage = _damage;
        speed = _speed;
        Dir = dir;

        //transform.rotation = Quaternion.FromToRotation(Vector3.up, angle * Vector3.forward);
    }

    protected virtual void Update()
    {
        transform.Translate(Time.deltaTime * speed * Dir, Space.World);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
            return;

        per--;

        if(per == -1)
        {
            gameObject.SetActive(false);
        }
    }
}
