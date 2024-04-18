using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeProcess102RadioStation : PracticeProcessBase
{
    public override void Init(int taskId)
    {
        base.Init(taskId);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.RadioStation_OP_102, OnGetRadioStationOpMsg);


    }



    private void OnGetRadioStationOpMsg(IEventParam param)
    {
        //已完成所有步骤
        if (IsFinish()) return;

        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            RadioStationOp102Model model = JsonTool.ToObject<RadioStationOp102Model>(tcpReceiveEvParam.netData.Msg);
            switch (model.Type)
            {
                case RadioStationOpType102.OpenClose:
                    DoProcess(model.Operate == OperateDevice.OPEN ? RadioStation102Id.RadioStation_OPEN_102 : RadioStation102Id.RadioStation_CLOSE_102);
                    break;

            }
        }
    }

    public override void End()
    {
        base.End();
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.RadioStation_OP_102, OnGetRadioStationOpMsg);

    }
}
