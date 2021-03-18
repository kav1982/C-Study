using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy  //不会继承私有变量bndCheck
{
    [Header("Set in Inspector : Enemy_1")]
    //完成一个正玄曲线周期所需要的时间
    public float waveFrequency = 2;
    //正玄曲线的宽度(米)
    public float waveWidth = 4;
    public float waveRotY = 45;

    private float x0; //初始位置的X坐标
    private float birthTime;

    //使用Start是因为父类没有使用它,不会被Override
    void Start()
    {
        x0 = pos.x;
        birthTime = Time.time;
    }
    
    public override void Move() //覆盖父类的虚方法
    {
        Vector3 tempPos = pos;

        //基于时间调整theta值
        float age = Time.time - birthTime;
        float theta = Mathf.PI * 2 * age / waveFrequency;
        float sin = Mathf.Sin(theta);
        tempPos.x = x0 + waveWidth * sin;
        pos = tempPos;

        //让对象绕着Y轴旋转
        Vector3 rot = new Vector3(0, sin * waveRotY, 0);
        this.transform.rotation = Quaternion.Euler(rot);

        //对象在Y方向的运动仍然由base.Move()处理
        //这个Move是父类的Move,主要负责向下移动
        base.Move();
    }
    
}
