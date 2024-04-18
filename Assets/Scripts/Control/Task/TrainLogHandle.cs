
using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 训练 日志处理
/// </summary>
public class TrainLogHandle
{
    /// <summary>
    /// 日志
    /// </summary>
    public StringBuilder Log { get; set; } = new StringBuilder();

    /// <summary>
    /// 需要处理的网络日志 协议号 和 对应数据类
    /// </summary>
    private Dictionary<int, Type> handleNetLogDic = new Dictionary<int, Type>()
    {
        //操作毒剂报警器
        {NetProtocolCode.POISON_ALARM_OP, typeof(PoisonAlarmOpModel)},

        //操作毒剂报警器 384
        {NetProtocolCode.POISON_ALARM_OP_384, typeof(PoisonAlarmOp384Model)},

        //毒剂报警器工作模式 384
        {NetProtocolCode.POISON_ALARM_WORK_TYPE_384, typeof(PoisonAlarmWorkType384Model)},

        //毒剂报警器 进样状态
        //{NetProtocolCode.POISON_IN_STATUS, typeof(PoisonInStatusModel)},

        //操作辐射仪02b
        {NetProtocolCode.RADIOME_OP, typeof(RadiomeOpModel)},

        //设置辐射仪 剂量率阈值02b
        {NetProtocolCode.SET_RADIOM_RATE_THRESHOLD, typeof(SetRadiomThreHold02b)},

        //设置辐射仪 累计剂量率阈值02b
        {NetProtocolCode.SET_TT_RADIOM_RATE_THRESHOLD, typeof(SetTotalRadiomThreHold02b)},

        //操作车载辐射仪102
        {NetProtocolCode.CAR_RADIOM_OP_102, typeof(RadiomeOp102Model)},

        //设置辐射仪 剂量率阈值 102
        {NetProtocolCode.SET_RADIOM_RATE_THRESHOLD_102, typeof(SetRadiomThreShold102Model)},
        
        //设置辐射仪 累计剂量率阈值 102
        {NetProtocolCode.SET_TT_RADIOM_RATE_THRESHOLD_102, typeof(SetTTRadiomThreShold102Model)},

        //操作辐射仪384
        {NetProtocolCode.RADIOME_OP_384, typeof(RadiomeOp384Model)},

        //设置辐射仪 剂量率阈值 384
        {NetProtocolCode.SET_RADIOM_RATE_THRESHOLD_384, typeof(SetRadiomThreShold384Model)},
        
        ////设置辐射仪 累计剂量率阈值 384
        {NetProtocolCode.SET_TT_RADIOM_RATE_THRESHOLD_384, typeof(SetTotalRadiomThreHold384)},

         //设置辐射仪 剂量率阈值106
        {NetProtocolCode.SET_RADIOM_RATE_THRESHOLD_106, typeof(SetRadiomThreShold106)},

        //设置辐射仪 累计剂量率阈值106
        {NetProtocolCode.SET_TT_RADIOM_RATE_THRESHOLD_106, typeof(SetTotalRadiomThreHold106)},

        //设置生物106
        {NetProtocolCode.SET_Biology_RATE_THRESHOLD_106, typeof(SetBiologyThreShold106Model)},

        //操作电源
        {NetProtocolCode.POWER_OP, typeof(PowerOpModel)},

        //操作电台
        {NetProtocolCode.RadioStation_OP, typeof(RadioStationOpModel)},

        //答题上报结果
        {NetProtocolCode.QUESTION_REPORT, typeof(QstReport)},

        //操作北斗
        {NetProtocolCode.BEIDOU_OP, typeof(BeiDouOpModel)},

        //操作气象器
        {NetProtocolCode.METEOR_OP, typeof(MeteorOpModel)},

        //操作车载侦毒器
        {NetProtocolCode.OP_CAR_DETECT_POISON, typeof(CarDetectPoisonOpModel)},

        //防护
        {NetProtocolCode.PROTECT, typeof(ProtectModel)},
        
        //上报侦察结果
        {NetProtocolCode.REPORT_DETECT_RES, typeof(DetectResModel)},

        //车长指令
        {NetProtocolCode.MASTER_INSTRUCT, typeof(MasterInstructModel)},

        //三防毒报102
        {NetProtocolCode.POIS_ALARM_102, typeof(PoisAlarm102Model)},
        
        //三防差压计102
        {NetProtocolCode.DIFF_PRESSURE_102, typeof(DiffPressureOp102Model)},

        //102三防辐射仪
        {NetProtocolCode.PREVENT_DEVICE_RADIOM_102,  typeof(PreRadiomOp102Model)},

        //102车载质谱仪
        {NetProtocolCode.CAR_MASS_SPECT_102,  typeof(CarMassSpectOp102Model)},

        //102红外遥测模拟器
        {NetProtocolCode.INFARED_TELEMETRY_102,  typeof(InfaredTelemetryOp102Model)},

        //102红外遥测模拟器参数
        //{NetProtocolCode.INFARED_TELEMETRY_PARAM_102,  typeof(InfaredTelemetryParamModel)},

        //102电源
        {NetProtocolCode.POWER_102,  typeof(PowerOp102Model)},

        //102电气象仪
        {NetProtocolCode.METEOR_102,  typeof(MeteorOp102Model)},

          //102电台
        {NetProtocolCode.RadioStation_OP_102,  typeof(RadioStationOp102Model)},

        //384电源
        {NetProtocolCode.POWER_OP_384,  typeof(PowerOp384Model)},
        //384电台
        {NetProtocolCode.RadioStation_OP_384,  typeof(RadioStationOp384Model)},

         //106电源
        {NetProtocolCode.POWER_OP_106,  typeof(PowerOp106Model)},
        //106电台
        {NetProtocolCode.RadioStation_OP_106,  typeof(RadioStationOp106Model)},
         //操作106毒剂报警器
        {NetProtocolCode.POISON_ALARM_OP_106, typeof(PoisonAlarmOp106Model)},
    };

    public TrainLogHandle()
    {
        InitEvent();
    }

    /// <summary>
    /// 初始化事件监听
    /// </summary>
    private void InitEvent()
    {
        EventDispatcher.GetInstance().AddEventListener(EventNameList.ADD_TASK_LOG, AddLog);
        foreach (var item in handleNetLogDic)
        {
            NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, item.Key, HandleNetTaskLog);
        }
    }

    /// <summary>
    /// 添加日志
    /// </summary>
    private void AddLog(IEventParam param)
    {
        if (param is StringEvParam strParam)
        {
            AddLog(strParam.value);
        }
    }

    private void AddLog(string log)
    {
        if (log.IsNullOrEmpty())
        {
            return;
        }
        Log.AppendLine(log);
        EventDispatcher.GetInstance().DispatchEvent(EventNameList.REF_SHOW_TASK_LOG);
    }

    public void End()
    {
        EventDispatcher.GetInstance().RemoveEventListener(EventNameList.ADD_TASK_LOG, AddLog);
        foreach (var item in handleNetLogDic)
        {
            NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, item.Key, HandleNetTaskLog);
        }
    }

    /// <summary>
    /// 处理网络消息产生的日志
    /// </summary>
    private void HandleNetTaskLog(IEventParam param)
    {
        if (param is TcpReceiveEvParam revParam)
        {
            if (handleNetLogDic.ContainsKey(revParam.netData.ProtocolCode))
            {
                Type type = handleNetLogDic[revParam.netData.ProtocolCode];
                //消息对象
                object netModel = JsonTool.ToObject(revParam.netData.Msg, type);
                if (netModel == null)
                {
                    netModel = Activator.CreateInstance(type);
                }
                if (netModel is ITaskLogModel model)
                {
                    string log = model.GetTaskLog(revParam.netData.SeatId);
                    if (!log.IsNullOrEmpty())
                    {
                        //添加log
                        AddLog(log);
                        if (revParam.netData.ProtocolCode == NetProtocolCode.MASTER_INSTRUCT)
                        {
                            //车长命令 跳字提示
                            UIMgr.GetInstance().ShowToast(log);
                        }
                    }
                }
            }
        }
    }
}
