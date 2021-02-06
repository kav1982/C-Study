using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    [Header("Set Dynamically")]
    public Rigidbody rigid;
    private Neighborhood neighborhood;

    //初始化
    void Awake()
    {
        neighborhood = GetComponent<Neighborhood>();
        //缓存刚体组件的引用,优化性能
        rigid = GetComponent<Rigidbody>();
        //设置一个初始位置
        pos = Random.insideUnitSphere * Spawner.S.spawnRadius;
        //设置一个初始速度
        Vector3 vel = Random.onUnitSphere * Spawner.S.velocity;
        rigid.velocity = vel;
        //设置方向
        LookAhead();

        //随机颜色
        Color randColor = Color.black;
        while (randColor.r + randColor.g + randColor.b < 1.0f)
        {
            randColor = new Color(Random.value, Random.value, Random.value);
        }
        Renderer[] rends = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rends)
        {
            r.material.color = randColor;
        }
        TrailRenderer tRend = GetComponent<TrailRenderer>();
        tRend.material.SetColor("_TintColor", randColor);
    }

    void LookAhead()
    {
        //让Boid朝向飞行的方向
        transform.LookAt(pos + rigid.velocity);
    }

    public Vector3 pos
    {
        get { return transform.position; }
        set { transform.position = value; } 
    }
    
    //FixedUpdate 每次物理更新时调用
    void FixedUpdate()
    {
        Vector3 vel = rigid.velocity;
        Spawner spn = Spawner.S;

        //避免Boid之间太近产生碰撞
        Vector3 velAvoid = Vector3.zero;
        Vector3 tooClosePos = neighborhood.avgClosePos;
        //如果返回Vector.zero, 不执行任何操作
        if (tooClosePos != Vector3.zero)
        {
            velAvoid = pos - tooClosePos;
            velAvoid.Normalize();
            velAvoid *= spn.velocity;     
        }

        //速度匹配-与周围邻居的速度保持一致
        Vector3 velAlign = neighborhood.avgVel;
        //只在veiAlign不为0时起效
        if(velAlign != Vector3.zero)
        {
            velAlign.Normalize();
            velAlign *= spn.velocity;
        }

        //中心聚集
        Vector3 velCenter = neighborhood.avgPos;
        if(velCenter != Vector3.zero)
        {
            velCenter -= transform.position;
            velCenter.Normalize();
            velCenter *= spn.velocity;
        }

        //朝向Attractor移动
        Vector3 delta = Attractor.POS - pos;
        //检查是朝向还是背向Attractor移动
        bool attracted = (delta.magnitude > spn.attractPushDist);
        Vector3 velAttract = delta.normalized * spn.velocity;

        //应用所有的速度
        float fdt = Time.fixedDeltaTime;
        if (velAvoid != Vector3.zero)
        {
            vel = Vector3.Lerp(vel, velAvoid, spn.collAvoid * fdt);
        }
        else
        {
            if (velAlign != Vector3.zero)
            {
                vel = Vector3.Lerp(vel, velAlign, spn.flockCentering * fdt);
            }
            if (velCenter != Vector3.zero)
            {
                vel = Vector3.Lerp(vel, velAlign, spn.flockCentering * fdt);
            }
            if(velAttract != Vector3.zero)
            {
                if (attracted)
                {
                    vel = Vector3.Lerp(velAttract, velAttract, spn.attractPull * fdt);
                }
                else
                {
                    vel = Vector3.Lerp(vel, -velAttract, spn.attractPush * fdt);
                }
            }
        }
        

        //设置vel为Spawner单例中设置的速度
        vel = vel.normalized * spn.velocity;
        //将其赋予刚体
        rigid.velocity = vel;
        //检查新速度的方向
        LookAhead();
    }
}
