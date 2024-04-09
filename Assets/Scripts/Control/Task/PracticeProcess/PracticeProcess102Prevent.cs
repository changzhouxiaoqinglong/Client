using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeProcess102Prevent : PracticeProcessBase
{
    public override void Init(int taskId)
    {
        base.Init(taskId);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.PREVENT_DEVICE_RADIOM_102, OnGetPreventRadiomMsg);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.POIS_ALARM_102, OnGetPreventDrugMsg);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.DIFF_PRESSURE_102, OnGetPreventPressureMsg);     
    }

    private void OnGetPreventDrugMsg(IEventParam param)
    {
        if (IsFinish()) return;
        if(param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            PoisAlarm102Model model = JsonTool.ToObject<PoisAlarm102Model>(tcpReceiveEvParam.netData.Msg);
            if(model.Type == PoisAlarmOpType102.OpenClose)
                DoProcess(model.Operate == OperateDevice.OPEN ? Prevent102Id.PREVENT_DRUG_OPEN_102 : Prevent102Id.PREVENT_DRUG_CLOSE_102);
        }
    }

    private void OnGetPreventRadiomMsg(IEventParam param)
    {
        if (IsFinish()) return;
        if(param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            RadiomeOp102Model model = JsonTool.ToObject<RadiomeOp102Model>(tcpReceiveEvParam.netData.Msg);
            if (model.Type == Prre3RadiomOpType102.OpenClose)
                DoProcess(model.Operate == OperateDevice.OPEN ? Prevent102Id.PREVENT_RADIOM_OPEN_102 : Prevent102Id.PREVENT_RADIOM_CLOSE_102);
        }
    }

    private void OnGetPreventPressureMsg(IEventParam param)
    {
        if (IsFinish()) return;
        if(param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            DiffPressureOp102Model model = JsonTool.ToObject<DiffPressureOp102Model>(tcpReceiveEvParam.netData.Msg);
            if (model.Type == DiffPressureOpType102.Gate)
                DoProcess(model.Operate == OperateDevice.OPEN ? Prevent102Id.PREVENT_PRESSURE_OPEN_102 : Prevent102Id.PREVENT_PRESSURE_CLOSE_102);
        }
    }

    protected override void JumpToNext()
    {
        base.JumpToNext();
    }

    public override void End()
    {
        base.End();
    }
}
