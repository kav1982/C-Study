using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    static public int score = 1000;

    void Awake()
    {
        // PlayerPrefs,字典,通过关键字HighScore引用值key
        // 如果存在Keyworld"HighScore"返回true
        // Returns true if key exists in the preferences.
        if (PlayerPrefs.HasKey("HighScore"))
        {
            score = PlayerPrefs.GetInt("HighScore");
        }
        PlayerPrefs.SetInt("HighScore", score);
    }
    
    // Update is called once per frame
    void Update()
    {
        Text gt = this.GetComponent<Text>();
        //使用+连接会隐式调用ToString方法转换类型
        gt.text = "High Score:" + score;
    }
}
