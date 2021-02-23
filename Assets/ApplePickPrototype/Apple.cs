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
            Destroy(this.gameObject);
            ApplePicker apScript = Camera.main.GetComponent<ApplePicker>();
            apScript.AppleDestroyed();
        }
    }
}
