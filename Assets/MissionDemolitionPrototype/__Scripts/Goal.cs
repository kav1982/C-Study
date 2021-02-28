using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    //可以任意位置访问的静态公共字段
    static public bool goalMet = false;

    void OnTriggerEnter(Collider other)
    {
        //检查是否是弹丸
        if(other.gameObject.tag == "Projectile")
        {
            Goal.goalMet = true;
            //如果是,指定材质和颜色
            Material mat = GetComponent<Renderer>().material;
            Color c = mat.color;
            c.a = 1;
            mat.color = c;
        }
    }
}
