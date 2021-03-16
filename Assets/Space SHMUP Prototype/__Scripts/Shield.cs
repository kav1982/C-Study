using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float rotationsPerSecond = 0.1f;
    [Header("Set Dynamically")]
    public int levelShown = 0;

    //非共有变量不会出现在检视器中
    Material mat;

    private void Start()
    {
        //把mat定义游戏对象渲染的材料
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        //向下取整,确保水平偏移量为纹理宽度的倍数.避免偏移在两个纹理之间
        int currLevel = Mathf.FloorToInt(Hero.S.shieldLevel);
        //如果当前护盾等级与显示的等级不符
        if (levelShown != currLevel)
        {
            levelShown = currLevel;
            //调整纹理偏移量,显示正确的护盾画面
            mat.mainTextureOffset = new Vector2(0.2f*levelShown, 0);
        }

        float rz = -(rotationsPerSecond * Time.time * 360) % 360f;
        //使盾牌环绕Z轴,每秒旋转一定的角度
        transform.rotation = Quaternion.Euler(0, 0, rz);
    }

}
