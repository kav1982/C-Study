using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsCheck : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float radius = 1f;

    [Header("Set Dynamically")]
    public float camWidth;
    public float camHeight;

    void Awake()
    {
        //获取正交主相机的Size
        camHeight = Camera.main.orthographicSize;
        //Camera.main.aspect 摄像机的纵横比
        //获得从原点到屏幕左右的距离
        camWidth = camHeight * Camera.main.aspect;
    }

    // 调用Update之后会在每一帧调用LateUpdate
    // 避免两个Update函数之间的竞态条件
    void LateUpdate()
    {
        Vector3 pos = transform.position;

        if (pos.x > camWidth - radius)
            pos.x = camWidth - radius;
        if (pos.x < -camWidth + radius)
            pos.x = -camWidth + radius;
        if (pos.y > camHeight - radius)
            pos.y = camHeight - radius;
        if (pos.y < -camHeight + radius)
            pos.y = -camHeight + radius;

        transform.position = pos;
    }

    //使用OnDrawGizmos方法在场景面板中绘制边界
    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Vector3 boundSize = new Vector3(camWidth * 2, camHeight * 2, 0.1f);
        Gizmos.DrawWireCube(Vector3.zero, boundSize);
    }
}
