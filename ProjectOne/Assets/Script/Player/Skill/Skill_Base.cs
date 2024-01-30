using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum MaginType
{
    Shovel,     // ��
    Trident,    // ����â
    Sickle      // ��
}

public class Skill_Base : MonoBehaviour
{
    PlayerInputActions inputActions;

    [Header("# ��ų ���̽�")]
    /// <summary>
    /// ���� Ÿ��
    /// </summary>
    public MaginType maginType;

    public int Level;
    public int[] UseMp = new int[5];
    public float[] damage = new float[5];
    public int[] count = new int[5];
    public float[] cooltimeList = new float[5];

    public PlayerState player;

    /// <summary>
    /// ��ų�� ��Ÿ��
    /// </summary>
    public float cooltime_max;

    /// <summary>
    /// ���� ��Ÿ��
    /// </summary>
    protected float cooltime;

    /// <summary>
    /// ��ų ��� ���� ����
    /// </summary>
    protected bool isUse = true;

    /// <summary>
    /// ����ü �߻� ��ġ
    /// </summary>
    protected Transform fireTransform;


    Transform timerbase;
    Text timer_text;     // ���� �ð��� ǥ���� �ؽ�Ʈ
    Image disable;  // ���� �ð��� ǥ���� �̹���

    protected IEnumerator cooltimeCoroutine;

    protected virtual void Awake()
    {
        inputActions = new PlayerInputActions();

        disable = transform.GetChild(1).GetComponent<Image>();
        timerbase = transform.GetChild(2);
        timer_text = timerbase.GetChild(0).GetComponent<Text>();

        player = FindObjectOfType<PlayerState>();

        Transform weapon = GameManager.Instance.weapon.transform;
        fireTransform = weapon.GetChild(0);
    }

    protected virtual void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Action1.performed += OnAction1;
        inputActions.Player.Action2.performed += OnAction2;
        inputActions.Player.Action3.performed += OnAction3;
    }

    protected virtual void OnDisable()
    {
        inputActions.Player.Action3.performed -= OnAction3;
        inputActions.Player.Action2.performed -= OnAction2;
        inputActions.Player.Action1.performed -= OnAction1;
        inputActions.Player.Disable();
    }
    protected virtual void OnAction1(InputAction.CallbackContext context)
    {
    }

    protected virtual void OnAction2(InputAction.CallbackContext context)
    {
    }

    protected virtual void OnAction3(InputAction.CallbackContext context)
    {
    }

    protected IEnumerator CoolTimeCoroutine()
    {
        while (cooltime > 0.0f)
        {
            cooltime -= Time.deltaTime;

            disable.fillAmount = cooltime / cooltime_max;

            string timer = TimeSpan.FromSeconds(cooltime).ToString("s\\:ff");
            string[] tokens = timer.Split(':');

            timer_text.text = string.Format("{0}:{1}", tokens[0], tokens[1]);

            yield return new WaitForFixedUpdate();
        }

        CoolTimeEnd();
    }

    public void CoolTimeStart()
    {
        cooltimeCoroutine = CoolTimeCoroutine();

        // ��Ÿ�� ����
        cooltime_max = cooltimeList[Level];
        cooltime = cooltime_max;
        Debug.Log("��Ÿ�� ����");

        isUse = false;

        timerbase.gameObject.SetActive(true);

        StartCoroutine(cooltimeCoroutine);
    }

    void CoolTimeEnd()
    {
        Debug.Log("��Ÿ�� ��");

        isUse = true;
        timerbase.gameObject.SetActive(false);

        StopCoroutine(cooltimeCoroutine);
    }
}
