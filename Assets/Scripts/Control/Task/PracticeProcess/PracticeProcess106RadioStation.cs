using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeProcess106RadioStation : PracticeProcessBase
{
    public override void Init(int taskId)
    {
        base.Init(taskId);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.RadioStation_OP_106, OnGetRadioStationOpMsg);


    }



    private void OnGetRadioStationOpMsg(IEventParam param)
    {
        //已完成所有步骤
        if (IsFinish()) return;

        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            RadioStationOp106Model model = JsonTool.ToObject<RadioStationOp106Model>(tcpReceiveEvParam.netData.Msg);
            switch (model.Type)
            {
                case RadioStationOpType106.OpenClose:
                    DoProcess(model.Operate == OperateDevice.OPEN ? RadioStation106Id.RadioStation_OPEN_106 : RadioStation106Id.RadioStation_CLOSE_106);
                    break;

            }
        }
    }

    public override void End()
    {
        base.End();
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.RadioStation_OP_106, OnGetRadioStationOpMsg);

    }
}
