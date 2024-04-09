
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
        EventDispatcher.GetInstance().AddEventListener(EventNameList.CAMERA_EXCHANGE, ExchangeCamera);
        EventDispatcher.GetInstance().AddEventListener(EventNameList.LAST_CAMERA_EXCHANGE, ExchangeLastCamera);
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
