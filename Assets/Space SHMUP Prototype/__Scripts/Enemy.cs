using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector: Enemy")]
    public float speed = 10f;       //运动速度,米/秒
    public float fireRate = 0.3f;   //发射频率
    public float health = 10;
    public int score = 100;         //击毁敌机得到的分数
    private BoundsCheck bndCheck;   //是否飞出屏幕

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
}
