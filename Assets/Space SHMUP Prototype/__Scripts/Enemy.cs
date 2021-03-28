using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector: Enemy")]
    public float speed = 10f;           //运动速度,米/秒
    public float fireRate = 0.3f;       //发射频率
    public float health = 10;           //血量
    public int score = 100;             //击毁敌机得到的分数
    protected BoundsCheck bndCheck;     //是否飞出屏幕

    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
    }

    //pos属性,保护字段
    public Vector3 pos
    {
        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }

    void Update()
    {
        Move();
        //if (bndCheck != null && !bndCheck.isOnScreen)
        //检查以确保对象从屏幕底部消失
        if (bndCheck != null && bndCheck.offDown)
        {
            //if (pos.y<bndCheck.camHeight - bndCheck.radius)
            
                Destroy(gameObject);
            
        }
    }

    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }

    //void OnCollisionEnter(Collision coll)
    //{
    //    //获取被击中的Collider实例
    //    GameObject otherGO = coll.gameObject;
    //    //如果tag是ProjectileHero,销毁敌机实例
    //    if (otherGO.tag == "ProjectileHero") 
    //    {
    //        //消除子弹
    //        Destroy(otherGO);
    //        //消除敌人
    //        Destroy(gameObject);
    //    }
    //    else
    //    {
    //        //将标签不是ProjectileHero的物体打印到控制台
    //        print("Enemy hit by non-ProjectileHero:" + otherGO.name);
    //    }
    //}

    void OnCollisionEnter(Collision coll)
    {       
        GameObject other = coll.gameObject;
        switch(other.tag)
        {
            case "ProjectileHero":
                Projectile p = other.GetComponent<Projectile>();
                //进入屏幕前敌机不会被被伤害
                if(!bndCheck.isOnScreen)
                {
                    Destroy(other);
                    break;
                }
                //扣血
                health -= Main.GetWeaponDefinition(p.type).damageOnHit;

                if(health <= 0)
                {
                    Destroy(this.gameObject);
                }
                Destroy(other);
                break;

            default:
                print("Enemy hit by non-ProjectileHero:" + other.name);
                break;
                
        }
    }
}
