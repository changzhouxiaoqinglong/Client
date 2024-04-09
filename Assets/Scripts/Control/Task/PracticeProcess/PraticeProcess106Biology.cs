using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PraticeProcess106Biology : PracticeProcessBase
{
    public override void Init(int taskId)
    {
        base.Init(taskId);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.Biology_OP_106, OnGetRadioOpMsg);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.SET_Biology_RATE_THRESHOLD_106, OnGetRadioDoseThresholdMsg);

    }

    private void OnGetRadioDoseThresholdMsg(IEventParam param)
    {
        //已完成所有步骤
        if (IsFinish()) return;

        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            DoProcess(BiologyId106.Biology_RATE_THRESHOLD_106);
        }
    }

    private void OnGetRadioOpMsg(IEventParam param)
    {
        //已完成所有步骤
        if (IsFinish()) return;

        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            BiologyOp106Model model = JsonTool.ToObject<BiologyOp106Model>(tcpReceiveEvParam.netData.Msg);
            switch (model.Type)
            {
                case BiologyOp106Type.kaiguanji:
                    DoProcess(model.Operate == OperateDevice.OPEN ? BiologyId106.Biology_OPEN_106 : BiologyId106.Biology_CLOSE_106);
                    break;
                case BiologyOp106Type.alarm:
                    DoProcess(model.Operate == OperateDevice.OPEN ? BiologyId106.Biology_ALARM_OPEN_106 : BiologyId106.Biology_ALARM_CLOSE_106);
                    break;
            }
        }
    }

    public override void End()
    {
        base.End();
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.Biology_OP_106, OnGetRadioOpMsg);
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.SET_Biology_RATE_THRESHOLD_106, OnGetRadioDoseThresholdMsg);
    }
}
