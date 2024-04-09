using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeProcess384Radio : PracticeProcessBase
{
    public override void Init(int taskId)
    {
        base.Init(taskId);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.RADIOME_OP_384, OnGetRadiomMsg);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.SET_RADIOM_RATE_THRESHOLD_384,OnGetRadiomRateThresholeMsg);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.SET_TT_RADIOM_RATE_THRESHOLD_384, OnGetRadiomRateThresholeMsg);
    }

    private void OnGetRadiomRateThresholeMsg(IEventParam param)
    {
        if (IsFinish()) return;

        if(param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            if (tcpReceiveEvParam.netData.ProtocolCode == NetProtocolCode.SET_RADIOM_RATE_THRESHOLD_384)
                DoProcess(Radiom384Id.RADIOM384_RATE_THRESHOLD);
            if (tcpReceiveEvParam.netData.ProtocolCode == NetProtocolCode.SET_TT_RADIOM_RATE_THRESHOLD_384)
                DoProcess(Radiom384Id.TT_RADIOM384_RATE_THRESHOLD);
        }
    }


    /// <summary>
    /// 操作车载辐射仪
    /// </summary>
    private void OnGetRadiomMsg(IEventParam param)
    {
        //已完成所有步骤
        if (IsFinish()) return;

        if(param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            RadiomeOp384Model model = JsonTool.ToObject<RadiomeOp384Model>(tcpReceiveEvParam.netData.Msg);
            if(model.Type == RadiomOpType384.OpenClose)
                DoProcess(model.Operate == OperateDevice.OPEN ? Radiom384Id.RADIOM384_OPEN : Radiom384Id.RADIOM384_CLOSE);
        }   
    }


    protected override void JumpToNext()
    {
        base.JumpToNext();
    }

    public override void End()
    {
        base.End();
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.RADIOME_OP_384, OnGetRadiomMsg);
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.SET_RADIOM_RATE_THRESHOLD_384, OnGetRadiomRateThresholeMsg);
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.SET_TT_RADIOM_RATE_THRESHOLD_384, OnGetRadiomRateThresholeMsg);
    }
}
