using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public RuntimeAnimatorController animCon;

    /// <summary>
    /// 현재체력
    /// </summary>
    public float Hp;
    /// <summary>
    /// 공격력
    /// </summary>
    public float Atk;
    /// <summary>
    /// 방어력
    /// </summary>
    public float Def;
    /// <summary>
    /// 속도
    /// </summary>
    public float Agi;
    /// <summary>
    /// 공격범위
    /// </summary>
    public float Atk_dir;
}

public class Enemy : Character
{
    public List<EnemyData> EnemyList;
    public Rigidbody2D target;
    public float Atk_dir;

    Rigidbody2D rigid;
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;

    PlayerMoving player;
    PlayerState playerState;
    Scanner scanner;

    public bool isMove = true;

    public IEnumerator atkCouritine;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        scanner = GetComponent<Scanner>();
        wait = new WaitForFixedUpdate();
        player = GameManager.Instance.player_moving;
        atkCouritine = AtkCouritine();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        curHp = maxHp;
    }

    private void Start()
    {
        playerState = GameManager.Instance.player_state;
    }

    void FixedUpdate()
    {
        if(!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit")) 
        { return; }

        Transform targer = scanner.nearestTarger;
        Vector2 dirVec;

        if (targer != null)
        {
            dirVec = (Vector2)targer.position - rigid.position;
        }
        else
        {
            dirVec = new Vector2(Random.value, Random.value);
        }

        //Vector2 dirVec = target.position - rigid.position;
        //플레이어의 키입력 값을 더한 좌표
        Vector2 nextVec = dirVec.normalized * Agi * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    void LateUpdate()
    {
        if(!isLive) { return; }

        spriter.flipX = target.position.x < rigid.position.x;   
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        target = GameManager.Instance.player_moving.GetComponent<Rigidbody2D>();
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
        gameObject.layer = 7;
    }

    /// <summary>
    /// 몬스터 생성
    /// </summary>
    /// <param name="randomnumber">몬스터 고유 아이디</param>
    public void Init(int randomnumber)
    {
        anim.runtimeAnimatorController = EnemyList[randomnumber].animCon;
        maxHp = EnemyList[randomnumber].Hp;
        curHp = maxHp;
        Atk = EnemyList[randomnumber].Atk;
        Def = EnemyList[randomnumber].Def;
        Agi = EnemyList[randomnumber].Agi;
        Atk_dir = EnemyList[randomnumber].Atk_dir;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log($"{collision.name}");

        if (isLive)
        {
            if (collision.CompareTag("Bullet"))
            {
                curHp -= collision.GetComponent<Bullet>().damage;
                StartCoroutine(KnockBack());

                if (curHp > 0)
                {
                    // .. Live, Hit Action
                    Debug.Log("curHp");
                    anim.SetTrigger("Hit");
                }
                else
                {
                    Dead();
                }
            }
            else if (collision.CompareTag("Player"))
            {
                StartCoroutine(atkCouritine);
            }
            StopCoroutine(atkCouritine);
        }
    }

    void Attark()
    {
        Debug.Log($"공격 / {Atk_speed}");
        if(playerState == null)
        {
            playerState = GameManager.Instance.player_state;
        }
        playerState.OnDamge(Atk - playerState.Def);
        player.OnHit();
    }

    public IEnumerator AtkCouritine()
    {
        Attark();
        yield return new WaitForSeconds(Atk_speed);
    }

    IEnumerator KnockBack()
    {
        yield return wait; // 하나의 물리 프레임 딜레이
        Vector3 playerPos = GameManager.Instance.player_moving.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }
    
    void Dead()
    {
        coll.enabled = false;
        rigid.simulated = false;
        spriter.sortingOrder = 1;
        //GameManager.instance.kill++;
        GameManager.Instance.player_state.GetExp();
        gameObject.layer = 0;
        anim.SetBool("Dead", true);
        isMove = false;
        StartCoroutine(LifeOver(1.0f));
    }
}
