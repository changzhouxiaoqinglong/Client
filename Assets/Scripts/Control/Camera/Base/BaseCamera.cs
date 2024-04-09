using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// 基础相机，RenderType为Base urp项目相机和普通项目不同的地方
/// 多个相机想融合渲染，要通过base相机的stack来做
/// 所以统一使用base相机做处理
/// </summary>
public class BaseCamera : UnityMono
{
    [HideInInspector]
    public Camera m_Camera;

    /// <summary>
    /// 当前类型
    /// </summary>
    public BaseCameraEnum curType;

    protected override void Awake()
    {
        base.Awake();
        m_Camera = GetComponent<Camera>();
    }

    /// <summary>
    /// 加入新的相机
    /// </summary>
    public virtual void AddCamera(Camera newCamera)
    {
        List<Camera> list = m_Camera.GetUniversalAdditionalCameraData().cameraStack;
        if (!list.Contains(newCamera))
        {
            list.Add(newCamera);
        }
        list.Sort((a, b) =>
        {
            return (int)CameraBase.GetCameraEnum(a.name) - (int)CameraBase.GetCameraEnum(b.name);
        });
    }

    /// <summary>
    /// 移除相机
    /// </summary>
    public void RemoveCamera(Camera removeCam)
    {
        m_Camera.GetUniversalAdditionalCameraData().cameraStack.Remove(removeCam);
    }
}
