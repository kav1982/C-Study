using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    none,       //默认没有武器
    blaster,    //简单爆破
    spread,     //同时射击2发子弹
    phaser,     //[IN] 波浪形射击
    missile,    //[IN] 自控导弹
    laser,      //[IN] 持续摧毁
    shield      //提升护盾
}

[System.Serializable]
public class WeaponDefinition
{
    public WeaponType type = WeaponType.none;
    public string letter;                           //升级道具中显示的字母
    public Color color = Color.white;               //Collar & 升级道具的颜色
    public GameObject projectilePrefab;             //弹丸的预设
    public Color projectileColor = Color.white;     //弹丸颜色
    public float damageOnHit = 0;                   //造成的伤害点数
    public float continuousDamage = 0;              //每秒伤害数(laser)
    public float delayBetweenShots = 0;             //发射间隔
    public float velocity = 20;                     //弹丸的速度
}

public class Weapon : MonoBehaviour
{
    static public Transform PROJECTILE_ANCHOR;

    [Header("Set Dynamically")]
    [SerializeField]
    private WeaponType _type = WeaponType.blaster;
    public WeaponDefinition def;
    public GameObject collar;
    public float lastShot;
    private Renderer collarRend;

    void Start()
    {
        collar = transform.Find("Collar").gameObject;
        collarRend = collar.GetComponent<Renderer>();
        //调用Set Type(),正确设置默认武器类型_type
        SetType(_type);
        //动态为所有的游戏对象创建anchor
        if (PROJECTILE_ANCHOR == null)
        {
            GameObject go = new GameObject("_Projectile_Anchor");
            PROJECTILE_ANCHOR = go.transform;
        }

        //查找父对象的fireDelegate
        //查找武器子对象的根游戏对象
        GameObject rootGO = transform.root.gameObject;
        if (rootGO.GetComponent<Hero>() != null)
        {
            rootGO.GetComponent<Hero>().fireDelegate += Fire;
        }
    }

    public WeaponType type
    {
        get { return (_type); }
        set { SetType(value); }
    }

    //确保武器消失
    //设置武器类型激活子弹发射功能
    public void SetType(WeaponType wt)
    {
        _type = wt;
        if(type == WeaponType.none)
        {
            this.gameObject.SetActive(false);
            return;
        }
        else
        {
            this.gameObject.SetActive(true);
        }
        def = Main.GetWeaponDefinition(_type);
        collarRend.material.color = def.color;
        lastShot = 0; //_type设置后立即发射
    }

    public void Fire()
    {
        //如果this.gameObject处于未激活状态
        if (!gameObject.activeInHierarchy) return;
        //如果距离上次发射的时间不足最小时间间隔
        if(Time.time - lastShot < def.delayBetweenShots)
        {
            return;
        }

        Projectile p;
        Vector3 vel = Vector3.up * def.velocity;
        if(transform.up.y < 0)
        {
            vel.y = -vel.y;
        }

        switch (type)
        {
            case WeaponType.blaster:    //单点模式
                p = MakeProjectile();
                p.rigid.velocity = vel;
                break;

            case WeaponType.spread:     //创建左右10度的三叉戟模式
                p = MakeProjectile();
                p.rigid.velocity = vel;
                p = MakeProjectile();

                p.transform.rotation = Quaternion.AngleAxis(10, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                p = MakeProjectile();

                p.transform.rotation = Quaternion.AngleAxis(-10, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                break;
        }

    }

    //制造子弹
    public Projectile MakeProjectile()
    {
        GameObject go = Instantiate<GameObject>(def.projectilePrefab);
        if (transform.parent.gameObject.tag == "Hero")
        {
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        else
        {
            go.tag = "ProjectileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }
        go.transform.position = collar.transform.position;
        go.transform.SetParent(PROJECTILE_ANCHOR, true);
        Projectile p = go.GetComponent<Projectile>();
        p.type = type;
        lastShot = Time.time;
        return (p);
    }
}


