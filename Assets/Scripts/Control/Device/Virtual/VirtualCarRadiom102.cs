/// <summary>
/// 虚拟车载辐射仪
/// </summary>
public class VirtualCarRadiom102 : VirtualDeviceBase
{
    /// <summary>
    /// 当前剂量率状态
    /// </summary>
    private bool curRadiomAlarm;
    public bool CurRadiomAlarm
    {
        set
        {
            if (value != curRadiomAlarm)
            {
                curRadiomAlarm = value;
                //添加日志
                string log = "车载辐射仪：" + (value ? "剂量率报警" : "停止剂量率报警");
                EventDispatcher.GetInstance().DispatchEvent(EventNameList.ADD_TASK_LOG, new StringEvParam(log));
            }
        }
    }

    /// <summary>
    /// 当前累计剂量率报警状态
    /// </summary>
    private bool curTTRadiomAlarm;
    public bool CurTTRadiomAlarm
    {
        set
        {
            if (value != curTTRadiomAlarm)
            {
                curTTRadiomAlarm = value;
                //添加日志
                string log = "车载辐射仪：" + (value ? "累积剂量报警" : "停止累积剂量报警");
                EventDispatcher.GetInstance().DispatchEvent(EventNameList.ADD_TASK_LOG, new StringEvParam(log));
            }
        }
    }

    public VirtualCarRadiom102() : base()
    {
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.CAR_RADIOM_OP_102, OnGetCarRadiomOpMsg);
    }

    /// <summary>
    /// 操作车载辐射仪
    /// </summary>
    private void OnGetCarRadiomOpMsg(IEventParam param)
    {
        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            //操作数据
            RadiomeOp102Model model = JsonTool.ToObject<RadiomeOp102Model>(tcpReceiveEvParam.netData.Msg);
            //剂量率报警
            if (model.Type == RadiomOpType102.RateAlarm)
            {
                switch (model.Operate)
                {
                    //都不报警
                    case RadiomOpAlarmOperate102.NONE:
                        CurRadiomAlarm = false;
                        CurTTRadiomAlarm = false;
                        break;
                    //剂量率报警
                    case RadiomOpAlarmOperate102.RADIOM_ALARM:
                        CurRadiomAlarm = true;
                        CurTTRadiomAlarm = false;
                        break;
                    //累计剂量率报警
                    case RadiomOpAlarmOperate102.TT_RADIOM_ALARM:
                        CurRadiomAlarm = false;
                        CurTTRadiomAlarm = true;
                        break;
                    //同时报警
                    case RadiomOpAlarmOperate102.BOTH:
                        CurRadiomAlarm = true;
                        CurTTRadiomAlarm = true;
                        break;
                }
            }
        }
    }

    public override void OnDestory()
    {
        base.OnDestory();
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.CAR_RADIOM_OP_102, OnGetCarRadiomOpMsg);
    }
}
