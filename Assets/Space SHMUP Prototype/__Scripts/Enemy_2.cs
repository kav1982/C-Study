using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : Enemy
{
    [Header("Set in Inspector : Enemy_2")]

    //确定正弦波对运动的影响程度
    public float sinEccentricity = 0.6f;
    public float lifeTime = 10;

    [Header("Set Dynamically : Enemy_2")]
    //使用正弦波修正两点插值
    public Vector3 p0;
    public Vector3 p1;
    public float birthTime;
    void Start()
    {
        p0 = Vector3.zero;
        //-bndCheck.camWidt屏幕左边界,bndCheck.radius使Enemy_2完全飞出屏幕
        p0.x = -bndCheck.camWidth - bndCheck.radius;
        //在屏幕底部和顶部的范围内
        p0.y = Random.Range(-bndCheck.camHeight, bndCheck.camHeight);

        //有一半可能会换边
        if (Random.value > 0.5f)
        {
            p0.x *= -1;
            p1.x *= -1;
        }
        //设置出生时间dirthTime为当前时间
        //dirthTime在Move()函数中用于线性插值
        birthTime = Time.time;
    }

    public override void Move()
    {
        //贝济埃曲线形成一个基于0-1之间的u值
        float u = (Time.time - birthTime) / lifeTime;
        //如果从dirthTime开始计时,已经超过了lifeTime的大小,u将大于1
        if(u > 1)
        {
            Destroy(this.gameObject);
            return;
        }

        //通过叠加一个基于正弦曲线的平滑曲线调整u值
        u = u + sinEccentricity * (Mathf.Sin(u * Mathf.PI * 2));
        //在两点之间进行插值
        pos = (1 - u) * p0 + u * p1;
    }
}
