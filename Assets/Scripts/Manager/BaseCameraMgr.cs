
using UnityEngine;

/// <summary>
/// base相机类型
/// </summary>
public enum BaseCameraEnum
{
    /// <summary>
    /// 同步画面base相机
    /// </summary>
    SyncBaseCamera,

    /// <summary>
    /// 渲染画面 base相机
    /// </summary>
    RenderBaseCamera
}


public class BaseCameraMgr : MonoBehaviour
{
    private BaseCamera[] baseCameras;

    private static BaseCameraMgr instance;

    public static BaseCameraMgr GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
        baseCameras = GetComponentsInChildren<BaseCamera>();
    }

    /// <summary>
    /// 添加进base相机
    /// </summary>
    public void AddCamera(CameraBase camera)
    {
        if (camera.m_camera == null)
        {
            Logger.LogError("m_camera is null : " + camera.name);
            return;
        }
        GetCameraByType(camera.baseCameraType).AddCamera(camera.m_camera);
    }

    public void RemoveCamera(CameraBase camera)
    {
        GetCameraByType(camera.baseCameraType).RemoveCamera(camera.m_camera);
    }

    public BaseCamera GetCameraByType(BaseCameraEnum type)
    {
        foreach (BaseCamera baseCamera in baseCameras)
        {
            if (baseCamera.curType == type)
            {
                return baseCamera;
            }
        }
        return null;
    }
}
