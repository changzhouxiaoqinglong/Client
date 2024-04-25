
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// 画面同步 Base相机
/// </summary>

public class SyncBaseCamera : BaseCamera
{

    private void Update()
    {
        //保持和主相机一样的旋转和位置状态，因为该相机为base相机，会渲染天空盒背景画面
        //而具体场景画面是由其他overlay相机渲染的，为防止两者叠加后 天空盒太阳等元素位置有问题，所以要保持位置旋转一致
        if (SceneMgr.GetInstance().curScene != null && SceneMgr.GetInstance().curScene is Train3DSceneCtrBase scene)
        {
           // print(scene.cameraMgr.CurMainCamera == null);
            if (scene.cameraMgr.CurMainCamera.GetCamera() != null)
            {
                transform.position = scene.cameraMgr.CurMainCamera.GetCamera().transform.position;
                transform.rotation = scene.cameraMgr.CurMainCamera.GetCamera().transform.rotation;
            }
        }
    }

    public override void AddCamera(Camera newCamera)
    {
        base.AddCamera(newCamera);
        //这里要赋值targetTexture,因为unity有bug
        //通过Base相机 所有渲染的ui都没办法正常输出到RenderTexture上
        //必须设置stack里相机的targetTexture和Base相机一致
        //(虽然stack里的overlay相机根本没有targetTexture这个选项也要这么赋值，这么改就能解决问题)
        newCamera.targetTexture = m_Camera.targetTexture;
    }

    /// <summary>
    /// 更新所有stack相机的TargetTexture
    /// </summary>
    public void RefreshAllStackTexture()
    {
        List<Camera> list = m_Camera.GetUniversalAdditionalCameraData().cameraStack;
        foreach (Camera camera in list)
        {
            camera.targetTexture = m_Camera.targetTexture;
        }
    }
}
