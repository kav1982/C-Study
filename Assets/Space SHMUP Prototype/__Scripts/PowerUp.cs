using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("Set ink Inspector")]

    public Vector2 rotMinMax = new Vector2(15, 90);
    public Vector2 driftMinMax = new Vector2(0.25f, 2);
    public float lifeTime = 6f;                             //升级道具存在的时间长度
    public float fadeTime = 4f;                             //升级道具渐隐所用的时间

    [Header("Set Dynamically")]
    public WeaponType type;                                 //升级道具的类型
    public GameObject cube;                                 //对于Cube对象的引用
    public TextMesh letter;                                 //对文本网格的引用
    public Vector3 rotPerSecond;                            //欧拉角旋转的速度
    public float birthTime;

    private Rigidbody rigid;
    private BoundsCheck bndCheck;
    private Renderer cubeRend;

    void Awake()
    {
        cube = transform.Find("Cube").gameObject;
        letter = GetComponent<TextMesh>();
        rigid = GetComponent<Rigidbody>();
        bndCheck = GetComponent<BoundsCheck>();
        cubeRend = cube.GetComponent<Renderer>();

        //设置一个随机速度
        Vector3 vel = Random.onUnitSphere;                  //获取一个随机的XYZ速度
        vel.z = 0;                                          //使运动处于XY平面
        vel.Normalize();                                    //使速度大小变为1
            vel *= Random.Range(driftMinMax.x, driftMinMax.y);
        rigid.velocity = vel;
        //将游戏对象的旋转设置为[0,0,0]
        transform.rotation = Quaternion.identity;
        //Quaternion.identity的旋转为0
      
        rotPerSecond = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y),
                            Random.Range(rotMinMax.x, rotMinMax.y),
                            Random.Range(rotMinMax.x, rotMinMax.y));

        birthTime = Time.time;
    }

    void Update()
    {
        cube.transform.rotation =
            Quaternion.Euler(rotPerSecond * Time.time);

        //间隔一定时间后,让升级道具渐隐
        //根据默认值,升级道具存在10秒,然后再4秒内消失
        float u = (Time.time - (birthTime + lifeTime)) / fadeTime;
        //在lifeTime秒数内, u将<=0.当超过fadeTmie的时间后,u将转化为1

        //如果u >= 1 ,消除升级道具
        if (u >= 1)
        {
            Destroy(this.gameObject);
            return;
        }

        //使用变量u确定Cube和文字的不透明度
        if (u > 0)
        {
            Color c = cubeRend.material.color;
            c.a = 1f - u;
            cubeRend.material.color = c;

            //让字母也渐隐,只不过程度不一样
            c = letter.color;
            c.a = 1f - (u * 0.5f);
            letter.color = c;
        }

        if (!bndCheck.isOnScreen)
        {
            //如果升级道具完全退出屏幕则销毁
            Destroy(gameObject);
        }      
    }
    public void SetType(WeaponType wt)
    {
        WeaponDefinition def = Main.GetWeaponDefinition(wt);
        cubeRend.material.color = def.color;
        letter.text = def.letter;
        type = wt;
    }

    public void AbsorbedBy(GameObject target)
    {
        //Hero类在收集到道具后调用本函数
        Destroy(this.gameObject);
    }

}
