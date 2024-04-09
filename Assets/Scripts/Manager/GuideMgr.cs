
using System.Collections;
using UniStorm;
using UnityEngine;
/// <summary>
/// 随机导调 管理
/// </summary>
public class GuideMgr
{
    public Train3DSceneCtrBase CurScene
    {
        get; set;
    }

    //计划导调
    private PlanGuideHandle planGuideHandle = new PlanGuideHandle();

    #region 无人机
    /// <summary>
    /// 无人机高度
    /// </summary>
    private float uavHeight = 50;

    /// <summary>
    /// 无人机资源路径
    /// </summary>
    private const string UavResPath = "Prefabs/RandomGuide/UAV";

    /// <summary>
    /// 无人机位置随机范围
    /// </summary>
    private float UavPosRandomRange = 30;
    #endregion

    #region 爆炸
    /// <summary>
    /// 爆炸资源路径
    /// </summary>
    private const string GrenadeResPath = "Prefabs/RandomGuide/Grenad_02";

    /// <summary>
    /// 爆炸位置随机范围
    /// </summary>
    private float GrenadePosRandomRangeMin = 32;

    private float GrenadePosRandomRangeMax = 50;
    #endregion

    /// <summary>
    /// 节点
    /// </summary>
    private Transform randomGuideRoot;
    private Transform RandomGuideRoot
    {
        get
        {
            if (randomGuideRoot == null)
            {
                GameObject randomGuideRootGo = GameObject.Find("RandomGuideRoot");
                if (randomGuideRootGo)
                {
                    randomGuideRoot = randomGuideRootGo.transform;
                }
                else
                {
                    randomGuideRoot = new GameObject("RandomGuideRoot").transform;
                }
            }
            return randomGuideRoot;
        }
    }
    public GuideMgr()
    {
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.RANDOM_GUIDE, OnGetRandomGuideMsg);
        planGuideHandle._GuideMgr = this;  
    }

    public void Update() {
        planGuideHandle.Update();
    }
    /// <summary>
    /// 收到随机导调消息
    /// </summary>
    private void OnGetRandomGuideMsg(IEventParam param)
    {
        if (param is TcpReceiveEvParam tcpParam)
        {
            RandomGuideModel model = JsonTool.ToObject<RandomGuideModel>(tcpParam.netData.Msg);
            TriggerGuideEv(model.Type);
        }
    }

    public void TriggerGuideEv(int type) {
        switch (type)
        {
            case RandomGuideType.RAIN_STORM:
                UniStormManager.Instance.RAIN_STORM();
                break;
            case RandomGuideType.ENEMY_ATTACK:
                CreateGrenade();
                break;
            case RandomGuideType.ENEMY_AIR_DETECT:
                CreateUAV();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 生成无人机
    /// </summary>
    public void CreateUAV()
    {
        CarBase curCar = CurScene.carMgr.GetCarByMachineId(AppConfig.MACHINE_ID);
        //随机位置
        Vector3 randomPos = curCar.GetRandomNearPos(0, UavPosRandomRange);
        GameObject uavPrefab = Resources.Load<GameObject>(UavResPath);
        GameObject uavObj = Object.Instantiate(uavPrefab, RandomGuideRoot);
        uavObj.transform.position = new Vector3(randomPos.x, randomPos.y + uavHeight, randomPos.z);
        uavObj.GetComponent<UAV>().SetCar(curCar);
    }

    /// <summary>
    /// 生成爆炸
    /// </summary>
    public void CreateGrenade()
    {
        CarBase curCar = CurScene.carMgr.GetCarByMachineId(AppConfig.MACHINE_ID);
        //随机位置
        Vector3 randomPos = curCar.GetRandomNearPos(GrenadePosRandomRangeMin, GrenadePosRandomRangeMax);
        //对应地面位置
        Vector3 terrainPos = CurScene.terrainChangeMgr.GetTerrainPosByPos(randomPos);
        GameObject grenadePrefab = Resources.Load<GameObject>(GrenadeResPath);
        GameObject grenadeObj = Object.Instantiate(grenadePrefab, RandomGuideRoot);
        //位置 稍微往上点
        grenadeObj.transform.position = new Vector3(terrainPos.x, terrainPos.y + 3, terrainPos.z);
    }
   
    public void OnDestroy()
    {
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.RANDOM_GUIDE, OnGetRandomGuideMsg);
    }

}
