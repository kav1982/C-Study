using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S;

    [Header("Set in Inspector")]
    public float minDist = 0.1f;
    public LineRenderer line;
    private GameObject _poi;
    public List<Vector3> points;

    void Awake()
    {
        S = this;
        //获取对线段渲染器LineRenderer的引用
        line = GetComponent<LineRenderer>();
        line.enabled = false;
        //初始化三维向量点的list
        points = new List<Vector3>();
    }

    public GameObject poi
    {
        get
        {
            return (_poi);
        }
        set
        {
            _poi = value;
            if (_poi != null)
            {   //poi设置为新对象时,将复位其所有内容
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    public void Clear() //用于清除线条
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }

    public void AddPoint()
    {
        Vector3 pt = _poi.transform.position;
        if (points.Count > 0 && (pt - lastPoint).magnitude < minDist)
        {
            return;
        }
        if (points.Count == 0) //如果当前点是发射点
        {
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS; ;//待定义
            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2;
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            //启用线渲染器
            line.enabled = true;
        }
        else
        {
            //正常添加点的操作
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }

    public Vector3 lastPoint
    {
        get
        {
            //如果当前没有点,归零
            if (points == null)
            {
                return (Vector3.zero);
            }
            return (points[points.Count - 1]);
        }
    }

    void FixedUpdate()
    {
        if (poi == null)
        {
            if(FollowCam.POI != null)
            {
                if (FollowCam.POI.tag == "Porjectile")
                {
                    poi = FollowCam.POI;
                }
                else
                {
                    return; 
                }
            }
            else
            {
                return;
            }
        }
        //如果存在兴趣点,则在FixedUpdate中它的位置上增加一个点
        AddPoint();
        if (FollowCam.POI == null)
        {
            poi = null;
        }
        
    }
}
