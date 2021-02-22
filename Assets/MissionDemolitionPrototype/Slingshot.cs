using System.Collections;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject prefabProjectile;

    [Header("Set Dynamically")]
    public GameObject launchPoint;
    public Vector3 LaunchPos;
    public GameObject projectile;
    public bool aimingMode;

    void Awake()
    {
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        LaunchPos = launchPointTrans.position;
    }
    void OnMouseEnter()
    {
        launchPoint.SetActive(true);
        //print("Slingshot:OnMouseEnter()");
    }
    void OnMouseExit()
    {
        launchPoint.SetActive(false);
        //print("Slingshot:OnMouseExit()");
    }
    // Start is called before the first frame update
    void OnMouseDown()
    {
        aimingMode = true;
        //实例化一个弹丸
        projectile = Instantiate(prefabProjectile) as GameObject;
        //实例的初始位置位于launchPos
        projectile.transform.position = LaunchPos;
        //设置当前isKinematic的属性为真
        projectile.GetComponent<Rigidbody>().isKinematic = true;
    }
}
