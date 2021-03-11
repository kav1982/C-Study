using System.Collections;               //用于数组和其它组合
using System.Collections.Generic;       //用于Listhe字典
using UnityEngine;                      //用于游戏引擎
using UnityEngine.SceneManagement;      //用于加载和重载场景

public class Main : MonoBehaviour
{
    static public Main S;

    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies;          //Enemy预设数组
    public float enemySpawnPerSecond = 0.5f;    //每秒产生的敌机数量
    public float enemySpawnPadding = 1.5f;      //位置填充

    private BoundsCheck bndCheck;

    void Awake()
    {
        S = this;
        //将bndCheck设置为当前游戏对象BoundsCheck组件的引用
        bndCheck = GetComponent<BoundsCheck>();
        // 1/0.5=2秒, 调用一次SpewnEnemy()
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond); ;
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
    
}
