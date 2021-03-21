using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_3 : Enemy
{
    [Header("Set in Inspector : Enemy_3")]
    public float lifeTime = 5;

    [Header("Set Dynamically : Enemy_3")]
    public Vector3[] points;
    public float birthTime;

    // Start可以运行是因为Enemy的父类没有使用它
    void Start()
    {
        //初始化节点数组
        points = new Vector3[3];

        //初始位置在Main.SpawnEnemy()中进行设置
        points[0] = pos;

        //最大值和最小值与Main.SpawnEnemy()中相同
        float xMin = -bndCheck.camWidth + bndCheck.radius;
        float xMax = bndCheck.camWidth - bndCheck.radius;

        Vector3 v;
        //在屏幕下方随机取一个点作为中间节点
        v = Vector3.zero;
        v.x = Random.Range(xMin, xMax);
        v.y = -bndCheck.camHeight * Random.Range(2.75f, 2);
        points[1] = v;

        //在屏幕顶部随机选一个点作为终点
        v = Vector3.zero;
        v.y = pos.y;
        v.x = Random.Range(xMin, xMax);
        points[2] = v;

        birthTime = Time.time;
    }

    public override void Move()
    {
        float u = (Time.time - birthTime) / lifeTime;

        if (u > 1)
        {
            Destroy(this.gameObject);
            return;
        }

        //在三点贝济埃曲线上插值
        Vector3 p01, p12;
        u = u - 0.2f * Mathf.Sin(u * Mathf.PI * 2);
        p01 = (1 - u) * points[0] + u * points[1];
        p12 = (1 - u) * points[1] + u * points[2];
        pos = (1 - u) * p01 + u * p12;
    }
}
