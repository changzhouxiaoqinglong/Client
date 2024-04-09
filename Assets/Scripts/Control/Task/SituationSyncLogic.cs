using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 态势类型
/// </summary>
public class SituateType
{
    /// <summary>
    /// 车位置
    /// </summary>
    public const int CAR_POS = 1;

    /// <summary>
    /// 车长规划路线信息
    /// </summary>
    public const int ROUTE = 2;

    /// <summary>
    /// 标志旗信息
    /// </summary>
    public const int FLAG = 3;

    /// <summary>
    /// 毒区或辐射区域位置
    /// </summary>
    public const int Harm = 4;
}

/// <summary>
/// 态势同步
/// </summary>
public class SituationSyncLogic
{
    private static SituationSyncModel GetCommonModel(int type)
    {
        return new SituationSyncModel()
        {
            Type = type,
            ExerciseId = NetVarDataMgr.GetInstance()._NetVarData._TrainStartModel.TrainID,
            SysTemTime = TimeTool.TransDateToYYYY(DateTime.Now),
            SimulateTime = TaskMgr.GetInstance().curTaskCtr.trainDateMgr.GetCurDateYYYYStr(),
        };
    }

    /// <summary>
    /// 同步车位置
    /// </summary>
    public static void SyncCarPos(CustVect3 pos)
    {
        SituationSyncModel model = GetCommonModel(SituateType.CAR_POS);
        model.PosList.Add(pos);
        NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(model), NetProtocolCode.SITUATION_SYNC);
    }

    /// <summary>
    /// 同步插旗子
    /// </summary>
    public static void SyncFlag(Vector3 pos, int flagType, string info)
    {
        Train3DSceneCtrBase train3D = SceneMgr.GetInstance().curScene as Train3DSceneCtrBase;
        //旗子经纬度
        Vector3 gisPos = train3D.terrainChangeMgr.gisPointMgr.GetGisPos(pos);
        SituationSyncModel model = GetCommonModel(SituateType.FLAG);
        model.PosList.Add(gisPos.ToCustVect3());
        model.SignType = flagType;
        if (info.IsNullOrEmpty())
        {
            info = $"车{AppConfig.MACHINE_ID}在位置（{gisPos.x}, {gisPos.y}）投掷标志旗" + info;
        }
        else
        {
            info = $"车{AppConfig.MACHINE_ID}在位置（{gisPos.x}, {gisPos.y}）投掷标志旗," + info;
        }
        if (flagType == HarmAreaType.NUCLEAR)
        {
            //辐射剂量率
            float radiomValue = train3D.harmAreaMgr.GetPosRadiomRate(pos);
            info += $",当前辐射剂量率为:{radiomValue}  {AppConstant.RADIOM_UNIT}";
        }
        model.FlagInfo = info;
        NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(model), NetProtocolCode.SITUATION_SYNC);
    }

    /// <summary>
    /// 同步路线
    /// </summary>
    public static void SyncRoute(List<CustVect3> pos)
    {
        SituationSyncModel model = GetCommonModel(SituateType.ROUTE);
        model.PosList = pos;
        NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(model), NetProtocolCode.SITUATION_SYNC);
    }

    /// <summary>
    /// 同步毒区或辐射区域
    /// </summary>
    public static void SyncHarm(CustVect3 pos)
    {
        SituationSyncModel model = GetCommonModel(SituateType.Harm);
        Train3DSceneCtrBase train3D = SceneMgr.GetInstance().curScene as Train3DSceneCtrBase;
        Vector3 gisPos = train3D.terrainChangeMgr.gisPointMgr.GetGisPos(pos.ToVector3());
        model.PosList.Add(gisPos.ToCustVect3());
        List<HarmData> harmDatas = NetVarDataMgr.GetInstance()._NetVarData._TaskEnvVarData.HarmDatas;
        if (harmDatas.Count == 0) return;
        //毒
        if (harmDatas[0].HarmType == HarmAreaType.DRUG)
        {
            DrugVarData drug = JsonTool.ToObject<DrugVarData>(harmDatas[0].Content);
            model.EnvironId = drug.Id;
        }
        //辐射
        else if (harmDatas[0].HarmType == HarmAreaType.NUCLEAR)
        {
            RadiatVarData radiate = JsonTool.ToObject<RadiatVarData>(harmDatas[0].Content);
            model.EnvironId = radiate.Id;
        }
        NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(model), NetProtocolCode.SITUATION_SYNC);
    }
}
