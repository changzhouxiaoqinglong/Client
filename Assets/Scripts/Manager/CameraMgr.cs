
/// <summary>
/// 相机管理
/// </summary>

public class CameraMgr
{
    /// <summary>
    /// 当前启用的相机(主相机)
    /// </summary>
    public MainCameraItemBase CurMainCamera
    {
        get; set;
    }

    /// <summary>
    /// 记录上次启用的相机
    /// </summary>
    private MainCameraItemBase LastMainCamera
    {
        get; set;
    }

    public CameraMgr()
    {
       // UnityEngine.Debug.Log("CameraMgrCameraMgrCameraMgr");
       // UnityEngine.Debug.Log(CurMainCamera==null);
        EventDispatcher.GetInstance().AddEventListener(EventNameList.CAMERA_EXCHANGE, ExchangeCamera);//开始训练有错误的情况下 相机切换就不执行了
        EventDispatcher.GetInstance().AddEventListener(EventNameList.LAST_CAMERA_EXCHANGE, ExchangeLastCamera);
       // UnityEngine.Debug.Log(CurMainCamera == null);
       // UnityEngine.Debug.Log("adddddddddd222222111");
    }

    /// <summary>
    /// 切换相机
    /// </summary>
    private void ExchangeCamera(IEventParam param)
    {
        if (param is CameraExchangeEvParam cameraParam)
        {
            MainCameraItemBase cameraItem = cameraParam.cameraItem;
            if (CurMainCamera == cameraItem)
            {
                return;
            }
            //禁用当前相机
            CurMainCamera?.SetDisable();
            if (CurMainCamera != null && CurMainCamera.NeedRecordLastMainCamera())
            {
                LastMainCamera = CurMainCamera;
            }
            CurMainCamera = cameraItem;
          //  UnityEngine.Debug.Log(" 切换相机 ");
        }
    }

    /// <summary>
    /// 启用上次的相机
    /// </summary>
    private void ExchangeLastCamera(IEventParam param)
    {
        if (LastMainCamera != null)
        {
            LastMainCamera.SetEnable();
        }
        else
        {
            Logger.LogWarning("LastCamera is null!");
        }
    }

    public void OnDestroy()
    {
        EventDispatcher.GetInstance().RemoveEventListener(EventNameList.CAMERA_EXCHANGE, ExchangeCamera);
        EventDispatcher.GetInstance().RemoveEventListener(EventNameList.LAST_CAMERA_EXCHANGE, ExchangeLastCamera);
    }
}
