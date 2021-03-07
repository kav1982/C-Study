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

    void Start()
    {
        int currLevel = Mathf.FloorToInt(Hero.S.shieldLevel);
        //如果当前护盾等级与显示的等级不符
        if (levelShown != currLevel)
        {
            levelShown = currLevel;
        }
    }

}
