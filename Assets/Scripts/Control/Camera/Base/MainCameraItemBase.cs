
using UnityEngine;
/// <summary>
/// 受管理主相机项 基类(同时只会有一个主相机启用)
/// </summary>
public abstract class MainCameraItemBase : MonoBehaviour
{
    public abstract Camera GetCamera();

    /// <summary>
    /// 启用
    /// </summary>
    public virtual void SetEnable()
    {
        EventDispatcher.GetInstance().DispatchEvent(EventNameList.CAMERA_EXCHANGE, new CameraExchangeEvParam(this));
        //天气配置 需要的远景裁剪面值
        SetCameraStormPlane();
    }

    /// <summary>
    /// 禁用
    /// </summary>
    public virtual void SetDisable()
    {

    }

    /// <summary>
    /// 设置相机 天气需要的 远景裁剪面值
    /// </summary>
    protected virtual void SetCameraStormPlane()
    {
        GetCamera().farClipPlane = UniStorm.UniStormSystem.CAMERA_FAR_PLANES;
    }

    /// <summary>
    /// 是否记录相机切换
    /// </summary>
    public virtual bool NeedRecordLastMainCamera()
    {
        return true;
    }
}
