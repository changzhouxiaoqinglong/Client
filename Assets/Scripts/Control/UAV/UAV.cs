
using UnityEngine;

/// <summary>
/// 无人机
/// </summary>
public class UAV : MonoBehaviour
{
    private Train3DSceneCtrBase curScene;

    private UavFollowCamera curCamera;

    /// <summary>
    /// 移动速度
    /// </summary>
    public float moveSpeed;

    private CarBase curCar;

    /// <summary>
    /// 相机在车顶距离
    /// </summary>
    private float cameraOffCarDis = 5;

    /// <summary>
    /// 相机跟随时间
    /// </summary>
    private float cameraFollowTime = 4;

    /// <summary>
    /// 飞机销毁时间
    /// </summary>
    private float destroyTime = 10;

    private void Awake()
    {
        curScene = SceneMgr.GetInstance().curScene as Train3DSceneCtrBase;
        curCamera = transform.Find("UavCamera").GetComponent<UavFollowCamera>();
    }

    private void Start()
    {
        //随机方向
        float randomAngleY = Random.Range(0, 360);
        transform.eulerAngles = new Vector3(0, randomAngleY, 0);
        //相机放车上
        curCamera.transform.SetParent(curCar.transform);
        curCamera.transform.localPosition = new Vector3(0, cameraOffCarDis, 0);
        //激活相机
        curCamera.SetEnable();
        this.DelayInvoke(cameraFollowTime, () =>
        {
            //延时取消相机跟随并禁用
            CameraReduct();
        });
        //延时销毁
        Destroy(gameObject, destroyTime);
    }

    private void Update()
    {
        MoveLogic();
    }

    /// <summary>
    /// 移动
    /// </summary>
    private void MoveLogic()
    {
        Vector3 targetPos = transform.position + transform.forward * moveSpeed * Time.deltaTime;
        transform.position = targetPos;
    }

    public void SetCar(CarBase curCar)
    {
        this.curCar = curCar;
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
