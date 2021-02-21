using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplePicker : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject basketPrefab;
    public int numBaskets = 3;
    public float basketBottomY = -14f;
    public float basketSpacinkgY = 2f;
    public List<GameObject> basketList;


    // Start is called before the first frame update
    void Start()
    {
        basketList = new List<GameObject>();
        for(int i = 0; i < numBaskets; i++)
        {
            GameObject tBasketGO = Instantiate(basketPrefab) as GameObject;
            Vector3 pos = Vector3.zero;
            pos.y = basketBottomY + (basketSpacinkgY * i);
            tBasketGO.transform.position = pos;
            basketList.Add(tBasketGO);
        }
     }

    public void AppleDestroyed()
    {
        GameObject[] tAppleArray = GameObject.FindGameObjectsWithTag("Apple");
        foreach (GameObject tGO in tAppleArray)
        {
            Destroy(tGO);
        }
        //销毁一个篮筐
        //获取最后一个篮筐的序号
        int basketIndex = basketList.Count - 1;
        //取得该篮筐的引用
        GameObject tBasketGO = basketList[basketIndex];
        //从列表中移除篮筐,销毁游戏对象
        basketList.RemoveAt(basketIndex);
        Destroy(tBasketGO);
        //如果篮筐都被删除,结束游戏,重来
        if (basketList.Count == 0)
        {
            SceneManager.LoadScene("_Scene_0");
        }
    } 
}
