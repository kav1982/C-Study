using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Basket : MonoBehaviour
{
    [Header("Set Dynamically")]
    public Text scoreGT;
    //[System.Obsolete]
    // Start is called before the first frame update
    void Start()
    {
        GameObject scoreGO = GameObject.Find("ScoreCounter");
        scoreGT = scoreGO.GetComponent<Text>();
        scoreGT.text = "0";
    }

    // Update is called once per frame
    void Update()
    {   
        //2D图像上的鼠标位置
        Vector3 mousePos2D = Input.mousePosition;
        //Z轴移动的距离
        mousePos2D.z = -Camera.main.transform.position.z;
        //ScreenToWorldPoint 将2D位置转换为3D位置
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        Vector3 pos = this.transform.position;
        //将鼠标的横线位置赋值给pos
        pos.x = mousePos3D.x;
        //pos曲线赋值
        this.transform.position = pos;
    }
    
    //检测碰撞,销毁碰撞时,标签为Apple的Obj
    void OnCollisionEnter(Collision coll)
    {
        //将检测碰撞到的游戏对象赋值给临时变量collidedWith
        GameObject collidedWith = coll.gameObject;
        //删掉带有苹果标签的物体
        if(collidedWith.tag == "Apple")
        {
            Destroy(collidedWith);
        }
        //显示计分板
        //将scoreGT转换为整数值 
        int score = int.Parse(scoreGT.text);
        //每次接住苹果+100分
        score += 100;
        //转换为字符串显示在屏幕上
        scoreGT.text = score.ToString();
        //如果新得分高于最高得分.替换当前最高得分
        if (score > HighScore.score)
        {
            HighScore.score = score;
        }
    }

}
