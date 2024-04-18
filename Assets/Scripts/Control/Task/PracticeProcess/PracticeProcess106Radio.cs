using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeProcess106Radio : PracticeProcessBase
{
    public override void Init(int taskId)
    {
        base.Init(taskId);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.RADIOME_OP_106, OnGetRadioOpMsg);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.SET_RADIOM_RATE_THRESHOLD_106, OnGetRadioDoseThresholdMsg);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.SET_TT_RADIOM_RATE_THRESHOLD_106, OnGetRadioTotalDoseThresholdMsg);
    }

    private void OnGetRadioDoseThresholdMsg(IEventParam param)
    {
        //已完成所有步骤
        if (IsFinish()) return;

        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            DoProcess(RadioId106.RADIOM_RATE_THRESHOLD_106);
        }
    }

    private void OnGetRadioTotalDoseThresholdMsg(IEventParam param)
    {
        //已完成所有步骤
        if (IsFinish()) return;

        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            DoProcess(RadioId106.TT_RADIOM_RATE_THRESHOLD_106);
        }
    }


    private void OnGetRadioOpMsg(IEventParam param)
    {
        //已完成所有步骤
        if (IsFinish()) return;

        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            RadiomeOp106Model model = JsonTool.ToObject<RadiomeOp106Model>(tcpReceiveEvParam.netData.Msg);
            switch (model.Type)
            {
                case RadiomOpType106.OpenClose:
                    DoProcess(model.Operate == OperateDevice.OPEN ? RadioId106.RADIO_OPEN_106 : RadioId106.RADIO_CLOSE_106);
                    break;
               // case RadiomOpType106.Check:
               //     DoProcess(model.Operate == OperateDevice.OPEN ? RadioId106.RADIO_CHECK_106 : RadioId106.RADIO_NO_OPERATE);
               //     break;
            }
        }
    }

    protected override void JumpToNext()
    {
        base.JumpToNext();
    }

    public override void End()
    {
        base.End();
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.RADIOME_OP_106, OnGetRadioOpMsg);
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.SET_RADIOM_RATE_THRESHOLD_106, OnGetRadioDoseThresholdMsg);
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.SET_TT_RADIOM_RATE_THRESHOLD_106, OnGetRadioTotalDoseThresholdMsg);
    }
}
