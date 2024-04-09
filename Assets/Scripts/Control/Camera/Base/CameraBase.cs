using UnityEngine;
/// <summary>
/// 相机枚举(必须和相机名字一致  而且顺序影响渲染顺序)
/// </summary>
public enum CameraEnum
{
    /// <summary>
    /// 跟随人物
    /// </summary>
    followCamera,

    /// <summary>
    /// 跟随车子
    /// </summary>
    VehicleCameraFollow,

    /// <summary>
    /// 车子拖拽相机
    /// </summary>
    VehicleCameraDrag,

    /// <summary>
    /// 司机视角
    /// </summary>
    VehicleCameraDriver,

    /// <summary>
    /// 同步ui相机
    /// </summary>
    SynUiCamera,

    /// <summary>
    /// 随机导调 无人机相机
    /// </summary>
    UavCamera,

    /// <summary>
    /// 随机导调 爆炸效果 相机
    /// </summary>
    GrenadeCamera,
}

/// <summary>
/// 相机基类
/// </summary>
public class CameraBase : UnityMono
{
    public Camera m_camera = null;

    /// <summary>
    /// 目标base相机类型
    /// </summary>
    public BaseCameraEnum baseCameraType = BaseCameraEnum.SyncBaseCamera;

    protected override void Awake()
    {
        base.Awake();
        m_camera = GetComponent<Camera>();
    }

    protected override void Start()
    {
        base.Start();
        BaseCameraMgr.GetInstance().AddCamera(this);
    }

    /// <summary>
    /// 当前相机类型
    /// </summary>
    protected CameraEnum GetCurEnum()
    {
        return GetCameraEnum(name);
    }

    /// <summary>
    /// 获得对应相机枚举类型
    /// </summary>
    /// <param name="cameraName"></param>
    public static CameraEnum GetCameraEnum(string cameraName)
    {
        cameraName = cameraName.Replace("(Clone)", string.Empty);
        CameraEnum curEnum;
        if (!System.Enum.TryParse(cameraName, out curEnum))
        {
            Logger.LogError("have no enum : " + cameraName);
        }
        return curEnum;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        BaseCameraMgr.GetInstance().RemoveCamera(this);
    }
}
