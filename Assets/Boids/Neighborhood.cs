using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neighborhood : MonoBehaviour
{
    [Header("Set Dynamically")]
    public List<Boid> neighbors;
    private SphereCollider coll;

    // Start is called before the first frame update
    void Start()
    {
        //初始化neighbors列表
        neighbors = new List<Boid>();
        //引用这个对象的球体碰撞
        coll = GetComponent<SphereCollider>();
        //设定球体碰撞的半径为Spawner单例的neighborDist的一半
        coll.radius = Spawner.S.neighborDist / 2;
    }

    //检查neighborDist是否变化,如果是它会改变球体碰撞的半径
    //球体碰撞的半径会导致大量PhysX运算,慎用
    void FixedUpdate()
    {
        if (coll.radius != Spawner.S.neighborDist/2)
        {
            coll.radius = Spawner.S.neighborDist/2;
        }
    }

    
    //当其它物体进入触发器
    void OnTriggerEnter(Collider other)
    {
        Boid b = other.GetComponent<Boid>();
        if (b != null)
        {
            if(neighbors.IndexOf(b) == -1)
            {
                neighbors.Add(b);
            }
        }
    }
    //当另一个Boid不再接触Biod触发器
    void OnTriggerExit(Collider other)
    {
        Boid b = other.GetComponent<Boid>();
        if (b != null)
        {
            if(neighbors.IndexOf(b) != -1)
            {
                neighbors.Remove(b);
            }
        }
    }

    //avgPos是个只读属性,记录所有在collisionDist之内的邻居,并且取平均位置
    public Vector3 avgPos
    {
        get 
        {
            Vector3 avg = Vector3.zero;
            if (neighbors.Count == 0) return avg;
            for (int i=0; i<neighbors.Count; i++)
            {
                avg += neighbors[i].pos;
            }
            avg /= neighbors.Count;

            return avg;
        }
    }
    
    //只读,记录附近Boid的平均速度
    public Vector3 avgVel
    {
        get
        {
            Vector3 avg = Vector3.zero;
            if (neighbors.Count == 0) return avg;
            for(int i=0; i<neighbors.Count; i++)
            {
                avg += neighbors[i].rigid.velocity;
            }
            avg /= neighbors.Count;

            return avg;
        }
    }

    //只读,记录所有在collisionDist之内的邻居,并且取平均位置.
    public Vector3 avgClosePos
    {
        get
        {
            Vector3 avg = Vector3.zero;
            Vector3 delta;
            int nearCount = 0;
            for (int i=0; i< neighbors.Count; i++)
            {
                delta = neighbors[i].pos - transform.position;
                if(delta.magnitude <= Spawner.S.collDist)
                {
                    avg += neighbors[i].pos;
                    nearCount++;
                }
            }
            //如果附件什么也么有,返回Vector为0
            if (nearCount == 0) return avg;
            //否则,取它的平均位置.
            avg /= nearCount;
            return avg;
        }
    }
}
