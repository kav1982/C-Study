using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    [Header("Set in INspector")]
    public static float bottomY = -20f;
    
    // Update is called once per frame
    void Update()
    {
        //当苹果下落到-20f的最低点时,删除对象
        if (transform.position.y < bottomY)
        {
            //Destory(this); 只能销毁Apple的C#脚本Apple(Script)组件
            //Destroy(this.gameObject);销毁整个Apple游戏对象(当前类的实例)
            Destroy(this.gameObject);
            ApplePicker apScript = Camera.main.GetComponent<ApplePicker>();
            apScript.AppleDestroyed();
        }
    }
}
