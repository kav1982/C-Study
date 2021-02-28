using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour
{
    [Header("Set in Inspector")]
    public int numClouds = 40;
    public GameObject cloudPrefab;
    public GameObject[] cloudPrefabs;
    public Vector3 cloudPosMin = new Vector3(-50, -5, 10);
    public Vector3 cloudPosMax = new Vector3(150, 100, 10);
    public float cloudScaleMin = 1;
    public float cloudScaleMax = 3;
    public float cloudSpeedMult = 0.5f;

    public GameObject[] cloudInstances;

    void Awake()
    {
        //创建一个数组,储存云的实例
        cloudInstances = new GameObject[numClouds];
        //查找父物体对象
        GameObject anchor = GameObject.Find("CloudAnchor");
        //遍历cloud并创建实例
        GameObject cloud;
        for (int i=0; i<numClouds; i++)
        {
            //创建cloudPrefab实例
            cloud = Instantiate<GameObject>(cloudPrefab);
            //云朵位置
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);
            //云朵缩放
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);  
            //较小的云朵离地面较近
            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);
            //较小的云朵深度值较远
            cPos.z = 100 - 90 * scaleU;
            //将以上变换应用到云朵
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;
            //设置父物体层级
            cloud.transform.parent = anchor.transform;
            //将云朵添加到数组中
            cloudInstances[i] = cloud;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        foreach (GameObject cloud in cloudInstances)
        {
            //获取云的缩放和位置数据
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;
            //云朵越大移动速度越快
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;

            //云循环
            //如果已经位于最左侧
            if (cPos.x <= cloudPosMin.x)
            {
                //侧将它放置到最右侧
                cPos.x = cloudPosMax.x;
            }
            //将新的位置应用到云朵上
            cloud.transform.position = cPos;
        }
    }
}
