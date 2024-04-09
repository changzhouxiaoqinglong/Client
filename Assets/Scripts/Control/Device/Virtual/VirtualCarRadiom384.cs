/// <summary>
/// 虚拟车载辐射仪
/// </summary>
public class VirtualCarRadiom384 : VirtualDeviceBase
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
                string log = "DFH辐射仪：" + (value ? "剂量率报警" : "停止剂量率报警");
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
                string log = "DFH辐射仪：" + (value ? "累积剂量报警" : "停止累积剂量报警");
                EventDispatcher.GetInstance().DispatchEvent(EventNameList.ADD_TASK_LOG, new StringEvParam(log));
            }
        }
    }

    public VirtualCarRadiom384() : base()
    {
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.RADIOME_OP_384, OnGetCarRadiomOpMsg);
    }

    /// <summary>
    /// 操作车载辐射仪
    /// </summary>
    private void OnGetCarRadiomOpMsg(IEventParam param)
    {
        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            //操作数据
            RadiomeOp384Model model = JsonTool.ToObject<RadiomeOp384Model>(tcpReceiveEvParam.netData.Msg);
            //剂量率报警
            if (model.Type == RadiomOpType384.RateAlarm)
            {
                switch (model.Operate)
                {
                    //都不报警
                    case RadiomOpAlarmOperate384.NONE:
                        CurRadiomAlarm = false;
                        CurTTRadiomAlarm = false;
                        break;
                    //剂量率报警
                    case RadiomOpAlarmOperate384.RADIOM_ALARM:
                        CurRadiomAlarm = true;
                        CurTTRadiomAlarm = false;
                        break;
                    //累计剂量率报警
                    case RadiomOpAlarmOperate384.TT_RADIOM_ALARM:
                        CurRadiomAlarm = false;
                        CurTTRadiomAlarm = true;
                        break;
                    //同时报警
                    case RadiomOpAlarmOperate384.BOTH:
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
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.RADIOME_OP_384, OnGetCarRadiomOpMsg);
    }
}
