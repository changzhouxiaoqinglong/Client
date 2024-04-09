using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeProcess102Radio : PracticeProcessBase
{
    public override void Init(int taskId)
    {
        base.Init(taskId);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.CAR_RADIOM_OP_102, OnGetRadiomMsg);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.SET_RADIOM_RATE_THRESHOLD_102, OnGetRadiomSetRateMsg);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.SET_TT_RADIOM_RATE_THRESHOLD_102, OnGetRadiomSetRateMsg);
    }

    private void OnGetRadiomMsg(IEventParam param)
    {
        if (IsFinish()) return;

        if(param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            RadiomeOp102Model model = JsonTool.ToObject<RadiomeOp102Model>(tcpReceiveEvParam.netData.Msg);
            if(model.Type == RadiomOpType102.OpenClose)
                DoProcess(model.Operate == OperateDevice.OPEN ? Radiom102Id.RADIOM_OPEN_102 : Radiom102Id.RADIOM_CLOSE_102);
        }
    }

    private void OnGetRadiomSetRateMsg(IEventParam param)
    {
        if (IsFinish()) return;
        if(param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            if (tcpReceiveEvParam.netData.ProtocolCode == NetProtocolCode.SET_RADIOM_RATE_THRESHOLD_102)
                DoProcess(Radiom102Id.RADIOM_RATE_THRESHOLD_102);
            if (tcpReceiveEvParam.netData.ProtocolCode == NetProtocolCode.SET_TT_RADIOM_RATE_THRESHOLD_102)
                DoProcess(Radiom102Id.TT_RADIOM_RATE_THRESHOLD_102);
        }
    }


    protected override void JumpToNext()
    {
        base.JumpToNext();
    }

    public override void End()
    {
        base.End();
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.CAR_RADIOM_OP_102, OnGetRadiomMsg);
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.SET_RADIOM_RATE_THRESHOLD_102, OnGetRadiomSetRateMsg);
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.SET_TT_RADIOM_RATE_THRESHOLD_102, OnGetRadiomSetRateMsg);
    }
}
