using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTree : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject applePrefab;
    public float speed = 1f;
    public float leftAndRightEdge = 10f; //左右边缘
    public float chanceToChangeDirections = 1.0f;
    public float secondsBetweenAppleDrops = 1f;

    void Start()
    {
        Invoke("DropApple", 2f);//在调用DropApple之前等待两秒
    }

    void DropApple()
    {
        GameObject apple = Instantiate<GameObject>(applePrefab);
        apple.transform.position = transform.position;
        Invoke("DropApple", secondsBetweenAppleDrops);//Invoke以秒为单位反复调用DropApple
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.x += speed * Time.deltaTime;
        transform.position = pos;

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
        if (Random.value < chanceToChangeDirections)
        {
            speed *= -1; //改变方向
        }
    }
}
