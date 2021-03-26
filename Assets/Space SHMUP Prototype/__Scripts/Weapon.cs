using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    none,       //默认没有武器
    blaster,    //简单爆破
    spread,     //同时射击2发子弹
    phaser,     //[IN] 波浪形射击
    missile,    //[IN] 自控导弹
    laser,      //[IN] 持续摧毁
    shield      //提升护盾
}

[System.Serializable]
public class WeaponDefinition
{
    public WeaponType type = WeaponType.none;
    public string letter;                           //升级道具中显示的字母
    public Color color = Color.white;               //Collar & 升级道具的颜色
    public GameObject projectilePrefab;             //弹丸的预设
    public Color projectileColor = Color.white;     //弹丸颜色
    public float damegeOnHit = 0;                   //造成的伤害点数
    public float continuousDamage = 0;              //每秒伤害数(laser)
    public float delayBetweenShots = 0;             //发射间隔
    public float velocity = 20;                     //弹丸的速度
}

public class Weapon : MonoBehaviour
{

}
