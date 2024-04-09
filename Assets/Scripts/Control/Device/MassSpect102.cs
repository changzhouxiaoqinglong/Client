
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ReportDrugData
{
    /// <summary>
    /// 上报毒数据
    /// </summary>
    public ReportDrugDataModel reportData;

    /// <summary>
    /// 毒来源
    /// </summary>
    public int poisonOrigin;
}

/// <summary>
/// 质谱仪
/// </summary>
public class MassSpect102 : DeviceBase
{
    /// <summary>
    /// 计时器
    /// </summary>
    private float checkTimer = 0;

    protected override void Awake()
    {
        base.Awake();
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.CAR_MASS_SPECT_102, OnGetMassSpectOp);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        //化学训练才起作用
        if (TaskMgr.GetInstance().curTaskData.CheckType != HarmAreaType.DRUG)
        {
            return;
        }
        if (!UIMgr.GetInstance().IsOpenView(ViewType.DrugDgreeTest))
        {
            CountDrugData();
        }
        if (Keyboard.current.ctrlKey.isPressed)
        {
            if (Keyboard.current.tKey.isPressed)
            {
                UIMgr.GetInstance().OpenView(ViewType.DrugDgreeTest);
            }
        }
    }

    /// <summary>
    /// 计算上报化学区域信息
    /// </summary>
    private void CountDrugData()
    {
        if (car.IsSelfCar())
        {
            checkTimer += Time.deltaTime;
            if (checkTimer >= AppConstant.DRUG_CHECK_OFFTIME)
            {
                checkTimer = 0;
                ReportCurDrugData();
            }
        }
    }

    /// <summary>
    /// 上报当前化学信息
    /// </summary>
    private void ReportCurDrugData()
    {
        ReportDrugData model = GetReportDrugDataModel();
        if (model != null)
        {
            //发给设备管理软件
            NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(model.reportData), NetProtocolCode.SEND_DRUG_DATA, NetManager.GetInstance().SameMachineAllSeats);
        }
    }

    /// <summary>
    /// 获得当前位置的毒上报数据
    /// </summary>
    private ReportDrugData GetReportDrugDataModel()
    {
        ReportDrugData res;
        //空气检测
        float dentity = HarmAreaMgr.GetPosDrugDentity(car.GetPosition());
        if (dentity > 0)
        {
            DrugVarData drugVarData = HarmAreaMgr.GetPosDrugData(car.GetPosition());
            //毒数据
            ExPoisonData exPoisonData = null;
            if (drugVarData != null)
            {
                exPoisonData = ExPoisonDataMgr.GetInstance().GetDataById(drugVarData.Type);
            }
            res = new ReportDrugData()
            {
                reportData = new ReportDrugDataModel()
                {
                    Id = drugVarData != null ? drugVarData.Id : 0,
                    Type = drugVarData != null ? drugVarData.Type : PoisonType.NO_POISON,
                    Dentity = dentity,
                    Degree = exPoisonData != null ? exPoisonData.GetdDegreeByDentity(dentity) : DrugDegree.NONE,
                    DType = exPoisonData != null ? exPoisonData.DType : DrugDType.NONE,
                },
                poisonOrigin = PoisonOrigin.AIR,
            };
        }
        else
        {
            //地面检测
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
            res = new ReportDrugData()
            {
                reportData = new ReportDrugDataModel()
                {
                    Id = crater != null ? crater.VarData.Id : 0,
                    Type = crater != null ? crater.VarData.Type : PoisonType.NO_POISON,
                    Dentity = craterDentity,
                    Degree = exPoisonData != null ? exPoisonData.GetdDegreeByDentity(craterDentity) : DrugDegree.NONE,
                    DType = exPoisonData != null ? exPoisonData.DType : DrugDType.NONE,
                },
                poisonOrigin = PoisonOrigin.CRATER
            };
        }
        return res;
    }

    /// <summary>
    /// 收到质谱仪操作消息
    /// </summary>
    private void OnGetMassSpectOp(IEventParam param)
    {
        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            
            CarMassSpectOp102Model model = JsonTool.ToObject<CarMassSpectOp102Model>(tcpReceiveEvParam.netData.Msg);
            Logger.Log("监听到质谱仪信息,type为" + model.Type.ToString());
            //报警了  就添加侦察结果
            if (model.Type == CarMasssSpectOpType102.Alarm && model.Operate == OperateDevice.OPEN)
            {
                ReportDrugData drugData = GetReportDrugDataModel();
                if (drugData != null && drugData.reportData != null && drugData.reportData.Type != PoisonType.NO_POISON)
                {
                    //经纬度
                    Vector3 lation = CurScene3D.terrainChangeMgr.gisPointMgr.GetGisPos(car.GetPosition());
                    ExPoisonData exPoisonData = ExPoisonDataMgr.GetInstance().GetDataById(drugData.reportData.Type);
                    string reportStr = $"（{lation.x},{lation.y}）发现染毒";
                    reportStr = reportStr + (drugData.poisonOrigin == PoisonOrigin.AIR ? "空气" : "弹坑");
                    reportStr = reportStr + $",为{exPoisonData.Name}";
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
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.CAR_MASS_SPECT_102, OnGetMassSpectOp);
    }
}
