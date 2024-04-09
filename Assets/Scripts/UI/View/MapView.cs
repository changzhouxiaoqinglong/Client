using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapView :  ViewBase<MapViewModel>
{
    /// <summary>
    /// 关闭地图按钮
    /// </summary>
    private ButtonBase closeMapBtn;
    


    protected override void Awake()
    {
        base.Awake();
        closeMapBtn = transform.Find("closeMapBtn").GetComponent<ButtonBase>();
        closeMapBtn.RegistClick(OnClickCloseMapBtn);
    }
    private void OnClickCloseMapBtn(GameObject obj)
    {
        UIMgr.GetInstance().CloseView(ViewType.MapView);
    }
}
