using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S;

    [Header("Set in Inspector")]
    [SerializeField]
    
    //控制飞船的运动
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    public float gameRestartDelay = 2f;

    [Header("Set Dynamically")]
    //public float shieldLevel = 1;
    private float _shieldLevel = 4;//全局变量改为属性伪装成字段
    public GameObject lastTriggerGo = null;

    void Awake()
    {
        if (S == null)
        {
            S = this; //设置单例对象
        }
        else
        {
            Debug.LogError("Hero.Awake()-Attempted to assign second Hero.S!");
        }
    }

    void Update()
    {
        //从用户输入中获取信息
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        //让飞船旋转一个角度,更有动感
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        print("触发器事件:" + other.gameObject.name);
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        print("Triggered: " + go.name);
        //确保此次触发的对象与上一次不同
        if (go == lastTriggerGo)
        {
            return;
        }
        //更新后供下一次OnTriggerEnter使用
        lastTriggerGo = go;

        if (go.tag == "Enemy")
        {
            shieldLevel--;
            Destroy(go);
        }
        else
        {
            print("Triggered: " + go.name);
        }
    }

    public float shieldLevel
    {
        get
        {
            return (_shieldLevel);
        }
        set
        {
            //确保shieldLevel的值永远不大于4
            _shieldLevel = Mathf.Min(value, 4);
            //如果传入的护盾值小于0,销毁_Hero
            if (value < 0)
            {
                Destroy(this.gameObject);
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }
    }
}
