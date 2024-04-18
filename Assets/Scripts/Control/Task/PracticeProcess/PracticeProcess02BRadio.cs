using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeProcess02BRadio : PracticeProcessBase
{

    public override void Init(int taskId)
    {
        base.Init(taskId);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.RADIOME_OP, OnGetRadioOpMsg);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.SET_RADIOM_RATE_THRESHOLD, OnGetRadioDoseThresholdMsg);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.SET_TT_RADIOM_RATE_THRESHOLD, OnGetRadioTotalDoseThresholdMsg);
    }

    private void OnGetRadioDoseThresholdMsg(IEventParam param)
    {
        //已完成所有步骤
        if (IsFinish()) return;
      
        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            DoProcess(RadioId.RADIOM_RATE_THRESHOLD);           
        }
    }

    private void OnGetRadioTotalDoseThresholdMsg(IEventParam param)
    {
        //已完成所有步骤
        if (IsFinish()) return;

        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            DoProcess(RadioId.TT_RADIOM_RATE_THRESHOLD);
        }
    }


    private void OnGetRadioOpMsg(IEventParam param)
    {
        //已完成所有步骤
        if (IsFinish()) return;

        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            RadiomeOpModel model = JsonTool.ToObject<RadiomeOpModel>(tcpReceiveEvParam.netData.Msg);
            switch (model.Type)
            {
                case RadiomOpType.OpenClose:
                    DoProcess(model.Operate == OperateDevice.OPEN ? RadioId.RADIO_OPEN_02B : RadioId.RADIO_CLOSE_02B);
                    break;
                case RadiomOpType.Check:
                    DoProcess(model.Operate == OperateDevice.OPEN ? RadioId.RADIO_CHECK_02B :RadioId.RADIO_NO_OPERATE) ;
                    break;
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
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.RADIOME_OP, OnGetRadioOpMsg);
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.SET_RADIOM_RATE_THRESHOLD, OnGetRadioDoseThresholdMsg);
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.SET_TT_RADIOM_RATE_THRESHOLD, OnGetRadioTotalDoseThresholdMsg);
    }

}
