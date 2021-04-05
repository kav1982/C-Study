using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S;

    [Header("Set in Inspector")]
    [SerializeField]
    
    //控制飞船的运动
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    public float gameRestartDelay = 2f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 40;
    public Weapon[] weapons;

    [Header("Set Dynamically")]
    //public float shieldLevel = 1;
    private float _shieldLevel = 4;//全局变量改为属性伪装成字段
    public GameObject lastTriggerGo = null;

    public delegate void WeaponFireDelegate();
    public WeaponFireDelegate fireDelegate;

    void Start()
    {
        if (S == null)
        {
            S = this; //设置单例对象
        }
        else
        {
            Debug.LogError("Hero.Awake()-Attempted to assign second Hero.S!");
        }
        //fireDelegate += TempFire;
        //重置武器,从第一个高爆武器开始
        ClearWeapons();
        weapons[0].SetType(WeaponType.blaster);
    }

    void Update()
    {
        //从用户输入中获取信息
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        //让飞船旋转一个角度,更有动感
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);

        //我方飞船开火
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    TempFire();
        //}

        if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
        {
            fireDelegate();
        }
    }

    //void TempFire()
    //{
    //    GameObject projGO = Instantiate<GameObject>(projectilePrefab);
    //    projGO.transform.position = transform.position;
    //    Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
    //    //rigidB.velocity = Vector3.up * projectileSpeed;

    //    Projectile proj = projGO.GetComponent<Projectile>();
    //    proj.type = WeaponType.blaster;
    //    float tSpeed = Main.GetWeaponDefinition(proj.type).velocity;
    //    rigidB.velocity = Vector3.up * tSpeed;
    //}

    void OnTriggerEnter(Collider other)
    {
        print("触发器事件:" + other.gameObject.name);
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        print("Triggered: " + go.name);
        //确保此次触发的对象与上一次不同
        if (go == lastTriggerGo)
        {
            return;
        }
        //更新后供下一次OnTriggerEnter使用
        lastTriggerGo = go;

        if (go.tag == "Enemy")
        {
            shieldLevel--;
            Destroy(go);
        }
        else if(go.tag == "PowerUp")
        {
            AbsorbPowerUp(go);
        }
        else
        {
            print("触发碰撞事件: " + go.name);
        }
    }

    public void AbsorbPowerUp(GameObject go)
    {
        PowerUp pu= go.GetComponent<PowerUp>();
        switch (pu.type)
        {
            //如果升级道具具有护盾类型,它可以为护盾增加一个等级
            case WeaponType.shield:
                shieldLevel++;
                break;
            //其它升级道具的类型都是武器,所以这是默认状态
            default:
                //如果是任何一种武器升级道具
                if(pu.type == weapons[0].type)
                {
                    Weapon w = GetEmptyWeaponSlot();
                    if(w != null)
                    {
                        //将其赋值给pu.type
                        w.SetType(pu.type);
                    }
                }
                else//如果武器类型不一致
                {
                    //清空所有武器槽
                    ClearWeapons();
                    //将新获得的武器设置为Weapon_0武器类型
                    weapons[0].SetType(pu.type);
                }
                break;
        }
        pu.AbsorbedBy(this.gameObject);
    }

    public float shieldLevel
    {
        get
        {
            return (_shieldLevel);
        }
        set
        {
            //确保shieldLevel的值永远不大于4
            _shieldLevel = Mathf.Min(value, 4);
            //如果传入的护盾值小于0,销毁_Hero
            if (value < 0)
            {
                Destroy(this.gameObject);
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }
    }

     Weapon GetEmptyWeaponSlot()
    {
        for (int i=0; i<weapons.Length; i++)
        {
            if(weapons[i].type == WeaponType.none)
            {
                return (weapons[i]);
            }
        }
        return (null);
    }

    void ClearWeapons()
    {
        foreach(Weapon w in weapons)
        {
            w.SetType(WeaponType.none);
        }
    }
}
