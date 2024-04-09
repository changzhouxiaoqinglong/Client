using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapMgr
{
    private MiniMapCamera miniMapCamera;
    public MiniMapCamera MiniMapCamera
    {
        get
        {
            if (miniMapCamera == null)
            {
                CreateCamera();
            }
            return miniMapCamera;
        }
    }

    private void CreateCamera()
    {
        GameObject miniCamObj = Resources.Load<GameObject>(AssetPath.MINI_MAP_CAMERA);
        GameObject camObj = Object.Instantiate(miniCamObj);
        miniMapCamera = camObj.GetComponent<MiniMapCamera>();
    }
}
