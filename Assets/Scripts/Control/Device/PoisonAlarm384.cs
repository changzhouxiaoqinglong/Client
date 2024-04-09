

using System.Collections.Generic;
using UnityEngine;

public class PoisonAlarm384 : PoisonAlarm
{
    private const string TAG = "[PoisonAlarm384]:";

    /// <summary>
    /// 当前工作模式
    /// </summary>
    private int curWorkTye = PoisonAlarmWorkType.AIRE_CHECK;

    protected override void Awake()
    {
        base.Awake();
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.POISON_ALARM_WORK_TYPE_384, OnGetPoisonAlarmWorkType);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.POISON_ALARM_OP_384, OnGetPoisonAlarmOp);
    }

    /// <summary>
    /// 上报当前毒数据
    /// </summary>
    protected override void ReportCurDrugData()
    {
        ReportDrugDataModel model = GetReportDrugDataModel();
        if (model != null)
        {
            //发给设备管理软件
            NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(model), NetProtocolCode.SEND_DRUG_DATA, NetManager.GetInstance().SameMachineAllSeats);
        }
    }

    /// <summary>
    /// 获得当前位置的毒上报数据
    /// </summary>
    private ReportDrugDataModel GetReportDrugDataModel()
    {
        ReportDrugDataModel res = null;
        //空气检测
        if (curWorkTye == PoisonAlarmWorkType.AIRE_CHECK)
        {
            //浓度
            float dentity = HarmAreaMgr.GetPosDrugDentity(car.GetPosition());
            DrugVarData drugVarData = HarmAreaMgr.GetPosDrugData(car.GetPosition());
            //毒数据
            ExPoisonData exPoisonData = null;
            if (drugVarData != null)
            {
                exPoisonData = ExPoisonDataMgr.GetInstance().GetDataById(drugVarData.Type);
            }
            res = new ReportDrugDataModel()
            {
                Id = drugVarData != null ? drugVarData.Id : 0,
                Type = drugVarData != null ? drugVarData.Type : PoisonType.NO_POISON,
                Dentity = dentity,
                Degree = exPoisonData != null ? exPoisonData.GetdDegreeByDentity(dentity) : DrugDegree.NONE,
                DType = exPoisonData != null ? exPoisonData.DType : DrugDType.NONE,
            };
        }
        //地面检测
        if (curWorkTye == PoisonAlarmWorkType.ROUND_CHECK)
        {
            //附近的弹坑
            CraterBase crater = CurScene3D.craterMgr.GetGroundCheckCrater(car.GetPosition());
            //浓度
            float craterDentity = 0;
            //毒数据
            ExPoisonData exPoisonData = null;
            if (crater != null)
            {
                exPoisonData = ExPoisonDataMgr.GetInstance().GetDataById(crater.VarData.Type);
                craterDentity = crater.VarData.Dentity;
            }
            res = new ReportDrugDataModel()
            {
                Id = crater != null ? crater.VarData.Id : 0,
                Type = crater != null ? crater.VarData.Type : PoisonType.NO_POISON,
                Dentity = craterDentity,
                Degree = exPoisonData != null ? exPoisonData.GetdDegreeByDentity(craterDentity) : DrugDegree.NONE,
                DType = exPoisonData != null ? exPoisonData.DType : DrugDType.NONE,
            };
        }
        return res;
    }

    /// <summary>
    /// 毒剂报警器工作模式消息
    /// </summary>
    /// <param name="param"></param>
    private void OnGetPoisonAlarmWorkType(IEventParam param)
    {
        if (param is TcpReceiveEvParam tcpParam)
        {
            PoisonAlarmOp384Model model = JsonTool.ToObject<PoisonAlarmOp384Model>(tcpParam.netData.Msg);
            curWorkTye = model.Type;
            Logger.LogDebug(TAG + "Change Work Type: " + curWorkTye);
        }
    }

    /// <summary>
    /// 收到操作毒剂报警器
    /// </summary>
    private void OnGetPoisonAlarmOp(IEventParam param)
    {
        if (param is TcpReceiveEvParam tcpParam)
        {
            PoisonAlarmOp384Model model = JsonTool.ToObject<PoisonAlarmOp384Model>(tcpParam.netData.Msg);
            //报警了  就增添加侦察结果
            if (model.Type == PoisonAlarmOp384Type.Alarm && model.Operate == OperateDevice.OPEN)
            {
                ReportDrugDataModel report = GetReportDrugDataModel();
                if (report != null && report.Type != PoisonType.NO_POISON)
                {
                    //经纬度
                    Vector3 lation = CurScene3D.terrainChangeMgr.gisPointMgr.GetGisPos(car.GetPosition());
                    string reportStr = $"（{lation.x},{lation.y}）发现染毒";
                    reportStr = reportStr + (curWorkTye == PoisonAlarmWorkType.AIRE_CHECK ? "空气" : "弹坑");
                    reportStr = reportStr + $",为{DrugDType.GetDesc(report.DType)}毒,浓度为{DrugDegree.GetDesc(report.Degree)}";
                    DetectResParam detectModel = new DetectResParam(DetectResType.Poison, reportStr);
                    //发给侦查员2
                    List<ForwardModel> forwardModels = new ForwardModelsBuilder()
                        .Append(AppConfig.MACHINE_ID, SeatType.INVEST2)
                        .Build();
                    NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(detectModel), NetProtocolCode.SEND_DETCT_RES_TO_SEAT, forwardModels);
                }
            }
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.POISON_ALARM_WORK_TYPE_384, OnGetPoisonAlarmWorkType);
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.POISON_ALARM_OP_384, OnGetPoisonAlarmOp);
    }
}
