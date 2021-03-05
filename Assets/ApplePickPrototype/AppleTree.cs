using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTree : MonoBehaviour
{
    [Header("Set in Inspector")]
    //初始化预设
    public GameObject applePrefab;
    //苹果移动的速度
    public float speed = 1f;
    //苹果树的活动区域,到达边界改变方向
    public float leftAndRightEdge = 10f; //左右边缘
    //改变方向的概率
    public float chanceToChangeDirections = 1.0f;
    //苹果出现的时间间隔
    public float secondsBetweenAppleDrops = 1f;

    void Start()
    {
        Invoke("DropApple", 2f);//在调用DropApple之前等待两秒 Invokes the method methodName in time seconds.      
    }

    //DropApple 在苹果树所在位置实例化Apple对象
    void DropApple()
    {
        GameObject apple = Instantiate<GameObject>(applePrefab);
        //transform.position : The world space position of the Transform.
        //将苹果树的位置赋值给调用出现的苹果
        apple.transform.position = transform.position;
        //Invoke以秒为单位(固定间隔反复调用DropApple
        Invoke("DropApple", secondsBetweenAppleDrops);
    }

    // Update is called once per frame
    void Update()
    {
        //随时间的基本运动
        //transform.position 属性伪装成字段,无法写入,无法直接赋值.
        Vector3 pos = transform.position;
        //增量时间 deltaTime : The completion time in seconds since the last frame(Read Only).
        pos.x += speed * Time.deltaTime;
        transform.position = pos;

        //改变方向
        if(pos.x < -leftAndRightEdge)
        {
            speed = Mathf.Abs(speed);//正数向右移动
        }
        else if (pos.x > leftAndRightEdge) //负数向左移动
        {
            speed = -Mathf.Abs(speed);
        }
        
    }

    void FixedUpdate()
    {
        //随机值有10%概率,random.value 返回一个0-1之间的浮点数
        if (Random.value < chanceToChangeDirections)
        {
            speed *= -1; //改变方向(speed的反方向)
        }
    }
}
