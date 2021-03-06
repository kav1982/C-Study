﻿using System.Collections;               //用于数组和其它组合
using System.Collections.Generic;       //用于Listhe字典
using UnityEngine;                      //用于游戏引擎
using UnityEngine.SceneManagement;      //用于加载和重载场景

public class Main : MonoBehaviour
{
    static public Main S;
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;

    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies;              //Enemy预设数组
    public float enemySpawnPerSecond = 0.5f;        //每秒产生的敌机数量
    public float enemySpawnPadding = 1.5f;          //位置填充
    public WeaponDefinition[] WeaponDefinitions;    //武器的定义

    public GameObject prefabPowerUp;                //管理所有设备升级的预设

    //通过数组中的的数量决定创建升级道具的频率
    public WeaponType[] powerUpFrequency = new WeaponType[]
    {
        WeaponType.blaster, WeaponType.blaster,
        WeaponType.spread,WeaponType.shield
    };
                        
    private BoundsCheck bndCheck;                   //边界检查

    public void ShipDestroyed(Enemy e)
    {
        //掉落升级道具的概率
        if(Random.value <= e.powerUpDropChance)
        {
            int ndx = Random.Range(0, powerUpFrequency.Length);
            WeaponType puType = powerUpFrequency[ndx];

            //生成升级道具
            GameObject go = Instantiate(prefabPowerUp) as GameObject;
            PowerUp pu = go.GetComponent<PowerUp>();
            //将其设置为正确的武器类型
            pu.SetType(puType);

            //将其摆放在敌机被消灭时的位置
            pu.transform.position = e.transform.position;
        }
    }

    void Awake()
    {
        S = this;
        //将bndCheck设置为当前游戏对象BoundsCheck组件的引用
        bndCheck = GetComponent<BoundsCheck>();
        // 1/0.5=2秒, 调用一次SpewnEnemy()
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
        //WeaponType作为Key
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        //循环遍历WeaponDefinitions数组的每一个元素,创建和它匹配的WEAP_DICT字典入口
        foreach(WeaponDefinition def in WeaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }
    }

    public void SpawnEnemy()
    {
        //随机选取一架敌机预设并实例化
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);

        //使用随机生成的坐标,将敌机放置在屏幕上方
        //float enemyPadding = enemyDefaultPadding;
        float enemyPadding = enemySpawnPadding;
        if (go.GetComponent<BoundsCheck>() != null)
        {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        //设置spawned Enemy的初始位置
        Vector3 pos = Vector3.zero;
        //使用当前_MainCamera对象的BoundsCheck组件获取相机窗口的长宽值
        float xMin = -bndCheck.camWidth + enemyPadding;
        float xMax = bndCheck.camWidth - enemyPadding;
        pos.x = Random.Range(xMin, xMax);
        //当X坐标确认后,选定Y坐标
        pos.y = bndCheck.camHeight + enemyPadding;
        go.transform.position = pos;

        //再次调用 SpawnEnemy()
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }

    public void DelayedRestart(float delay)
    {
        Invoke("Restart", delay);
    }
    public void Restart()
    {
        SceneManager.LoadScene("_Scene_0");
    }

    static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        //确认Dictionary中有关键字
        if (WEAP_DICT.ContainsKey(wt))
        {
            return (WEAP_DICT)[wt];
        }
       //返回新的WeaponDefinition,表示未能找到正确的返回新的WeaponDefinition
        return (new WeaponDefinition());
    }   

}
