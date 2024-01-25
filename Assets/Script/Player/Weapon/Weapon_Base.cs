using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponType
{
    public int id;
    public int UseMp;
    public float[] damage;
    public int[] count;
    public float[] calltime;
    public Sprite sprite;
    public bool isShotGun;
}

public class Weapon_Base : MonoBehaviour
{
    public WeaponType[] weaponTypes;
    public int id;
    public int UseMp;
    public int Level;
    public float damage;
    public int count;
    public float calltime;
    public bool isShotGun;

    float distance;

    public SpriteRenderer sprite;

    protected Vector3 rightPos = new Vector3(0.35f, -0.15f, 0);
    protected Vector3 rightPosReverse = new Vector3(-0.15f, -0.15f, 0);
    protected Quaternion leftRot = Quaternion.Euler(0, 0, -35);
    protected Quaternion leftRotReverse = Quaternion.Euler(0, 0, -135);


    /// <summary>
    /// ÁÂ¿ì ¹«±â °ª
    /// </summary>
    public bool isRight;

    protected virtual void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Setting(WeaponType data)
    {
        id = data.id;
        UseMp = data.UseMp;
        damage = data.damage[Level];
        count = data.count[Level];
        calltime = data.calltime[Level];
        sprite.sprite = data.sprite;
        isShotGun = data.isShotGun;
    }

    public void ChangeWeapon(int _no)
    {
        Setting(weaponTypes[_no]);
    }

    protected virtual void LateUpdate()
    {
        bool isReverse = GameManager.Instance.player_moving.filp;

        if (isReverse)
        {
            Debug.Log("ÁÂ¿ì ¹ÝÀü");
        }

        if (isRight)
        {
            sprite.flipY = isReverse;
            sprite.sortingOrder = isReverse ? 6 : 4;
        }
    }
}
