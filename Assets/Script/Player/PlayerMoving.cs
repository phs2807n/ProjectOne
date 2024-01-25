using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoving : MonoBehaviour
{
    /// <summary>
    /// 방향 벡터
    /// </summary>
    public Vector3 inputedVector;

    /// <summary>
    /// 도착 벡터
    /// </summary>
    Vector3 endVector;

    // 애니메이션
    Animator anim;
    SpriteRenderer sprite;
    Rigidbody2D rigid2d;
    PlayerState state;
    Transform hand;

    readonly int InputMove_String = Animator.StringToHash("Move");

    /// <summary>
    /// 좌우 변환 bool값
    /// </summary>
    public bool filp;

    /// <summary>
    /// 무적 시간
    /// </summary>
    public float invincibleTime = 2.0f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rigid2d = GetComponent<Rigidbody2D>();
        hand = transform.GetChild(0);
    }

    private void OnEnable()
    {
        state = FindObjectOfType<PlayerState>();
        state.onDie += OnDie;
    }

    private void OnDisable()
    {
        state.onDie -= OnDie;
    }


    public void OnMove()
    {
        endVector = GameManager.Instance.GetMousePostion();
        inputedVector = endVector - transform.position;
        inputedVector = inputedVector.normalized;
        anim.SetBool(InputMove_String, true);
    }

    public void OnStop()
    {
        inputedVector = Vector3.zero;
        rigid2d.velocity = Vector2.zero;
        anim.SetBool(InputMove_String, false);
    }

    public void OnHit()
    {
        StartCoroutine(InvinvibleMode());
    }

    IEnumerator InvinvibleMode()
    {
        gameObject.layer = LayerMask.NameToLayer("Invincible");

        float timeElapsed = 0.0f;
        while(timeElapsed < invincibleTime)
        {
            timeElapsed += Time.deltaTime;

            float alpha = (Mathf.Cos(timeElapsed * 30.0f) + 1.0f) * 0.5f;
            sprite.color = new Color(1, 1, 1, alpha);

            yield return null;
        }

        gameObject.layer = LayerMask.NameToLayer("Player");
        sprite.color = Color.white;
    }

    /// <summary>
    /// 플레이어가 죽을 시 함수
    /// </summary>
    private void OnDie()
    {
        gameObject.layer = LayerMask.NameToLayer("Invincible");
        anim.SetBool("Dead", true);
        hand.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if(transform.position.x > GameManager.Instance.GetMousePostion().x)
        {
            filp = true;
        } 
        else { filp = false; }

        sprite.flipX = filp;

        float distance = Vector3.Magnitude(endVector - transform.position);
        //Debug.Log(distance);

        if(distance > 0.5f)
        {
            transform.Translate(Time.deltaTime * GameManager.Instance.player_state.Agi * inputedVector);
            //rigid2d.velocity = GameManager.instance.player_state.Agi * inputedVector;
        }
        else
        {
            OnStop();
        }
    }
}
