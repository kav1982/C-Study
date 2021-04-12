using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Part
{
    public string name;             //组件的名称
    public float health;            //组件的生命值
    public string[] protectedBy;    //保护该组件的其它组件

    [HideInInspector]
    public GameObject go;           //组件的游戏对象引用
    [HideInInspector]
    public Material mat;            //显示伤害的材质
}

public class Enemy_4 : Enemy
{
    [Header("Set in Inspector: Enemy_4")]
    public Part[] parts;            //储存敌机各组件的数组

    public Vector3 p0, p1;          //插值的p0和p1
    public float timeStart;         //出生时间
    public float duration = 4;      //每段的运动时长

    void Start()
    {
        p0 = p1 = pos;
        InitMovement();

        //分别缓存每个子块的游戏对象和材质
        Transform t;
        foreach (Part prt in parts)
        {
            t = transform.Find(prt.name);
            if (t != null)
            {
                prt.go = t.gameObject;
                prt.mat = prt.go.GetComponent<Renderer>().material;
            }
                
        }
    }

    void InitMovement()
    {
        p0 = p1;
        //为P1分配新的屏幕位置
        float widMinRad = bndCheck.camWidth - bndCheck.radius;
        float hgtMinRad = bndCheck.camHeight - bndCheck.radius;
        p1.x = Random.Range(-widMinRad, widMinRad);
        p1.y = Random.Range(-hgtMinRad, hgtMinRad);
        //重置时间
        timeStart = Time.time;
    }

    public override void Move()
    {
        //使用线性插值彻底重写Enemy.Move()
        float u = (Time.time - timeStart) / duration;
        if(u>=1)
        {
            InitMovement();
            u = 0;
        }

        u = 1 - Mathf.Pow(1 - u, 2);
        pos = (1 - u) * p0 + u * p1;
     
    }

    Part FindPart(string n)
    {
        foreach(Part prt in parts)
        {
            if (prt.name == n)
                return (prt);
        }
        return (null);
    }

    Part FindPart(GameObject go)
    {
        foreach(Part prt in parts)
        {
            if(prt.go == go)
            {
                return (prt);
            }
        }
        return (null);
    }

    bool Destroyed(GameObject go)
    {
        return (Destroyed(FindPart(go)));
    }

    bool Destroyed(string n)
    {
        return (Destroyed(FindPart(n)));
    }

    bool Destroyed(Part prt)
    {
        if(prt == null)
        {
            return (true);
        }
        return (prt.health <= 0);
    }

    //改变组件的颜色
    void ShowLocalizedDamage(Material m)
    {
        m.color = Color.red;

        damageDoneTime = Time.time + showDamageDuration;
        showingDamage = true;
    }

    //重写Enemy.OnCollisionEnter
    //public override void OnCollisionEnter(Collision coll)
    void OnCollisionEnter(Collision coll)
    {
        GameObject other = coll.gameObject;
        switch (other.tag)
        {
            case "ProjectileHero":
                Projectile p = other.GetComponent<Projectile>();
                if(!bndCheck.isOnScreen)
                {
                    Destroy(other);
                    break;
                }

                //给敌机造成伤害
                GameObject goHit = coll.contacts[0].thisCollider.gameObject;
                Part prtHit = FindPart(goHit);
                if(prtHit == null) //如果未找到被击中的组件prtHit
                {
                    goHit = coll.contacts[0].otherCollider.gameObject;
                    prtHit = FindPart(goHit);
                }

                //检查组件是否受到保护
                if(prtHit.protectedBy != null)
                {
                    foreach(string s in prtHit.protectedBy)
                    {
                        if(!Destroyed(s))   //如果保护他的组件还未被摧毁
                        {
                            //暂时不对该组件造成伤害
                            Destroy(other); //销毁弹丸ProjectileHero
                            return;         //在造成伤害之前返回
                        }
                    }
                }

                //根据弹丸类型Projectile.type和字典Main.W_DEFS得到伤害值
                prtHit.health -= Main.GetWeaponDefinition(p.type).damageOnHit;
                //在该组件上显示伤害效果
                ShowLocalizedDamage(prtHit.mat);
                if(prtHit.health <= 0)
                {
                    //禁用被伤害的组件
                    prtHit.go.SetActive(false);
                }

                bool allDestroyed = true;

                foreach(Part prt in parts)
                {
                    if(!Destroyed(prt))
                    {
                        allDestroyed = false;
                        break;
                    }
                }
                if(allDestroyed)
                {
                    Main.S.ShipDestroyed(this);
                    Destroy(this.gameObject);
                }
                Destroy(other);
                break;
        }
    }
}

