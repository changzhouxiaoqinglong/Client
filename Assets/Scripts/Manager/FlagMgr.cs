
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 插旗管理
/// </summary>

public class FlagMgr
{
    private const string TAG = "[FlagMgr]:";

    /// <summary>
    /// 旗子节点
    /// </summary>
    private Transform flagRoot;
    private Transform FlagRoot
    {
        get
        {
            if (flagRoot == null)
            {
                GameObject flagRootGo = GameObject.Find("FlagRoot");
                if (flagRootGo)
                {
                    flagRoot = flagRootGo.transform;
                }
                else
                {
                    flagRoot = new GameObject("FlagRoot").transform;
                }
            }
            return flagRoot;
        }
    }

    /// <summary>
    /// 场景插的旗子
    /// </summary>
    public List<GameObject> flags = new List<GameObject>();

    public Train3DSceneCtrBase CurScene
    {
        get; set;
    }

    public FlagMgr()
    {
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.FLAG, OnGetFlagMsg);
    }

    /// <summary>
    /// 插旗
    /// </summary>
    /// <param name="flagType">旗子类型</param>
    /// <param name="rotateY">旗子y旋转</param>
    /// <param name="pos">插旗位置</param>
    public void InsertFlagLogic(int flagType, Vector3 pos, string info)
    {
        Logger.Log(TAG + "InsertFlagLogic");
        GameObject flag = InsertFlag(flagType, pos);
        CustVect3 custPos = pos.ToCustVect3();
        Vector3 lation = (SceneMgr.GetInstance().curScene as Train3DSceneCtrBase).terrainChangeMgr.gisPointMgr.GetGisPos(pos);

        //发送插旗消息
        FlagModel model = new FlagModel()
        {
            FlagType = flagType,
            Pos = custPos,
            Rotate = flag.transform.eulerAngles.ToCustVect3(),
            Longicude = lation.x,
            Latitude=lation.y
        };
        //发送插旗数据给导控 并且转发给其他驾驶位进行同步
        NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(model), NetProtocolCode.FLAG, NetManager.GetInstance().SameTrainDriveSeatsExDevice);
        //同步态势
        SituationSyncLogic.SyncFlag(pos, flagType, info);
        AddFlagDetectRes(flagType, pos, info);
    }

    private GameObject InsertFlag(int flagType, Vector3 pos)
    {
        //相机方向
        Vector3 cameraDir = CurScene.cameraMgr.CurMainCamera.GetCamera().transform.position - pos;
        //旗子朝向相机方向
        float rotateY = Quaternion.LookRotation(cameraDir).eulerAngles.y;
        Quaternion quaternion = Quaternion.Euler(0, rotateY, 0);
        return InsertFlag(flagType, pos, quaternion);
    }

    private GameObject InsertFlag(int flagType, Vector3 pos, Quaternion quaternion)
    {
        Logger.Log(TAG + "InsertFlag");
        string res = GetFlagResByType(flagType);
        GameObject flagPrefab = Resources.Load<GameObject>(res);
        GameObject flagObj = Object.Instantiate(flagPrefab, FlagRoot);
        flagObj.transform.position = pos;
        flagObj.transform.rotation = quaternion;
        flags.Add(flagObj);
        return flagObj;
    }

    /// <summary>
    /// 添加标志旗 侦察结果
    /// </summary>
    private void AddFlagDetectRes(int flagType, Vector3 pos, string info)
    {
        //旗子经纬度
        Vector2 gisPos = CurScene.terrainChangeMgr.gisPointMgr.GetGisPos(pos);
        //侦察结果
        string res = $"投放{HarmAreaType.GetTypeStr(flagType)}标志旗在（{gisPos.x},{gisPos.y}）位置";
        if (!info.IsNullOrEmpty())
        {
            res += "，" + info;
        }
        switch (flagType)
        {
            case HarmAreaType.NUCLEAR:
                //辐射剂量率
                float radiomValue = CurScene.harmAreaMgr.GetPosRadiomRate(pos);
                res += $"，当前辐射剂量率为:{radiomValue}  {AppConstant.RADIOM_UNIT}";
                break;
            case HarmAreaType.BIOLOGY:

                break;
            case HarmAreaType.DRUG:

                break;
        }
        //侦察结果数据
        DetectResParam detectModel = new DetectResParam(DetectResType.Flag, res);
        //发给侦查员2
        List<ForwardModel> forwardModels = new ForwardModelsBuilder()
            .Append(AppConfig.MACHINE_ID, SeatType.INVEST2)
            .Build();
        NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(detectModel), NetProtocolCode.SEND_DETCT_RES_TO_SEAT, forwardModels);
    }

    /// <summary>
    /// 获得对应旗子的资源路径
    /// </summary>
    private string GetFlagResByType(int flagType)
    {
        switch (flagType)
        {
            //化学
            case HarmAreaType.DRUG:
                return "Prefabs/Flags/FlagChemical";
            //核
            case HarmAreaType.NUCLEAR:
                return "Prefabs/Flags/FlagNuclear";
            //生物
            case HarmAreaType.BIOLOGY:
                return "Prefabs/Flags/FlagBiology";
            default:
                Logger.LogError("not exist flagtype : " + flagType);
                return string.Empty;
        }
    }

    /// <summary>
    /// 收到插旗消息   同步插旗
    /// </summary>
    private void OnGetFlagMsg(IEventParam param)
    {
        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            FlagModel model = JsonTool.ToObject<FlagModel>(tcpReceiveEvParam.netData.Msg);
            //插旗
            InsertFlag(model.FlagType, model.Pos.ToVector3(), model.Rotate.ToQuaternion());
        }
    }

    public void OnDestroy()
    {
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.FLAG, OnGetFlagMsg);
    }
}
