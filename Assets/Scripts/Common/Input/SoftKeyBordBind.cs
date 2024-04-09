using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftKeyBordBind : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //如果不是驾驶员席 且 分辨率为1024*768 则输入框绑定软键盘
        if (AppConfig.SEAT_ID != 1 && Screen.width <= 1024)
        {
            gameObject.AddComponent<SoftKeyBoardInput>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
