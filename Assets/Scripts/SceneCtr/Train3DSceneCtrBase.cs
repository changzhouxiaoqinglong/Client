
using UnityEngine;
/// <summary>
/// 驾驶员 有实际3D场景的 训练场景
/// </summary>
public class Train3DSceneCtrBase : TrainSceneCtrBase
{
    /// <summary>
    /// 车辆管理
    /// </summary>
    public CarMgr carMgr = new CarMgr();

    /// <summary>
    /// 有害区域 管理
    /// </summary>
    public HarmAreaMgr harmAreaMgr = new HarmAreaMgr();

    /// <summary>
    /// 弹坑 管理
    /// </summary>
    public CraterMgr craterMgr = new CraterMgr();

    [SerializeField]
    /// <summary>
    /// 地图管理
    /// </summary>
    public TerrainChangeMgr terrainChangeMgr = new TerrainChangeMgr();

    /// <summary>
    /// 插旗管理
    /// </summary>
    public FlagMgr flagMgr = new FlagMgr();

    /// <summary>
    /// 相机管理
    /// </summary>
    public CameraMgr cameraMgr = new CameraMgr();

    /// <summary>
    /// minimap 管理
    /// </summary>
    public MiniMapMgr miniMapMgr = new MiniMapMgr();

    /// <summary>
    /// 天气管理
    /// </summary>
    public UniStormMgr uniStormMgr = new UniStormMgr();

    /// <summary>
    /// 随机导调
    /// </summary> 
    public GuideMgr guideMgr = new GuideMgr();

    protected override void Awake()
    {
        base.Awake();
        carMgr.CurScene = this;
        flagMgr.CurScene = this;
        craterMgr.CurScene = this;
        guideMgr.CurScene = this;
    }

    protected override void Start()
    {
        base.Start();
        terrainChangeMgr.Start();
        //更新车辆人物信息
        carMgr.UpdateCars();
        //初始化有害区域
        harmAreaMgr.InitHarmArea();
        //初始化弹坑
        craterMgr.InitCraters();
        //弹坑位置下陷
        print(craterMgr.craterList.Count);
        terrainChangeMgr.TerrainHeightDown(craterMgr.craterList, 2f, 3f);
        //调整天气
        this.DelayInvoke(0, () =>
        {
            uniStormMgr.ChangeWeather(NetVarDataMgr.GetInstance()._NetVarData._TaskEnvVarData.Wearth.Type);
        });
        //画面同步 发送组件
        InitFmSendScreen();

      //  if (AppConfig.CAR_ID == CarIdConstant.ID_102)
       //     InitFmSendYaoCeScreen();

       //开始记录训练时间
       TaskMgr.GetInstance().curTaskCtr.trainDateMgr.IsStartTimer = true;
    }

    private void Update()
    {
        harmAreaMgr.OnUpdate();
        uniStormMgr.Update();
        guideMgr.Update();
    }

    /// <summary>
    /// 生成同步画面 发送组件
    /// </summary>
    private void InitFmSendScreen()
    {
        GameObject sendPrefab = Resources.Load<GameObject>(AssetPath.FM_SEND_SCREEN);
        GameObject sendScreen = Instantiate(sendPrefab);
        //编码脚本
        GameViewEncoder encoder = sendScreen.GetComponentInChildren<GameViewEncoder>();
        if (encoder != null)
        {
            SyncBaseCamera syncCamera = BaseCameraMgr.GetInstance().GetCameraByType(BaseCameraEnum.SyncBaseCamera) as SyncBaseCamera;
            //设置同步渲染相机
            encoder.RenderCam = syncCamera.m_Camera;
            //设置同步渲染分辨率
            encoder.Resolution = new Vector3(Screen.width, Screen.height);
            this.InvokeByYield(() =>
            {
                //更新相机targetTexture
                syncCamera.RefreshAllStackTexture();
                EventDispatcher.GetInstance().DispatchEvent(EventNameList.SYN_BASECAMERA_RENDER);
            }, new WaitForSeconds(0));
        }
    }

    /// <summary>
    /// 生成同步画面 发送组件
    /// </summary>
    private void InitFmSendYaoCeScreen()
    {
        GameObject sendPrefab = Resources.Load<GameObject>(AssetPath.FM_SEND_SCREEN);
        GameObject sendScreen = Instantiate(sendPrefab);
        //编码脚本
        GameViewEncoder encoder = sendScreen.GetComponentInChildren<GameViewEncoder>();
        if (encoder != null)
        {
          
          //  encoder.RenderCam = syncCamera.m_Camera;
            //设置同步渲染分辨率
           // encoder.Resolution = new Vector3(Screen.width, Screen.height);
            
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        carMgr.OnDestroy();
        cameraMgr.OnDestroy();
        flagMgr.OnDestroy();
        guideMgr.OnDestroy();
        UIMgr.GetInstance().ForceDestroyView(ViewType.MapView);
    }
}
