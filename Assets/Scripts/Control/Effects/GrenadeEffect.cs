
using UnityEngine;

/// <summary>
/// 导调爆炸
/// </summary>
public class GrenadeEffect : MonoBehaviour
{
    private GrenadeCamera curCamera;

    private Train3DSceneCtrBase curScene;

    /// <summary>
    /// 相机高度
    /// </summary>
    private float cameraHeight = 30;

    private void Awake()
    {
        curScene = SceneMgr.GetInstance().curScene as Train3DSceneCtrBase;
        curCamera = transform.Find("GrenadeCamera").GetComponent<GrenadeCamera>();
    }

    private void Start()
    {
        //效果朝上  z轴随机旋转
        transform.eulerAngles = new Vector3(-90, 0, Random.Range(-180.0f, 180.0f));
        //相机放外面
        curCamera.transform.SetParent(transform.parent);
        curCamera.transform.position = transform.position + new Vector3(0, cameraHeight, 0);
        //相机朝下
        curCamera.transform.eulerAngles = new Vector3(90, 0, 0);
        //激活相机
        curCamera.SetEnable();
    }

    private void Update()
    {
        //if (curScene.cameraMgr.CurMainCamera != null)
        //{
        //    transform.LookAt(curScene.cameraMgr.CurMainCamera.GetCamera().transform);
        //}
    }

    /// <summary>
    /// 还原相机
    /// </summary>
    private void CameraReduct()
    {
        if (curScene.cameraMgr.CurMainCamera == curCamera)
        {
            //主相机还原
            EventDispatcher.GetInstance().DispatchEvent(EventNameList.LAST_CAMERA_EXCHANGE);
        }
        curCamera.SetDisable();
    }

    private void OnDestroy()
    {
        CameraReduct();
        Destroy(curCamera.gameObject);
    }
}
