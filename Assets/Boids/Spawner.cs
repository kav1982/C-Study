using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    static public Spawner S;
    static public List<Boid> boids;

    [Header("Set in Inspector: Spawing")]
    public GameObject boidPrefab;
    public Transform boidAnchor;
    public int numBoids = 100;              //鸟群数量
    public float spawnRadius = 100f;        //刷新半径
    public float spawnDelay = 0.1f;         //刷新延迟

    //调整全体Boids的行为
    [Header("Set in Inspector : Boids")]
    public float velocity = 30f;            //速度
    public float neighborDist = 30f;        //相邻距离
    public float collDist = 4f;             //外径
    public float velMatching = 0.25f;       //容差
    public float flockCentering = 0.2f;     //群集中心
    public float collAvoid = 2f;            //内径
    public float attractPull = 2f;          //吸引力拉
    public float attractPush = 2f;          //吸引力推
    public float attractPushDist = 5f;      //推力外径

    void Awake()
    {
        //设置单例变量S为BoidSpwner的当前实例
        S = this;
        //初始化Boids
        boids = new List<Boid>();
        InstantiateBoid();
    }

    public void InstantiateBoid()
    {
        GameObject go = Instantiate(boidPrefab);
        Boid b = go.GetComponent<Boid>();
        b.transform.SetParent(boidAnchor);
        boids.Add(b);
        if (boids.Count < numBoids)
        {
            Invoke("InstantiateBoid", spawnDelay);
        }
        
    }

}
