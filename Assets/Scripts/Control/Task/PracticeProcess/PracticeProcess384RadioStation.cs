using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeProcess384RadioStation : PracticeProcessBase
{
    public override void Init(int taskId)
    {
        base.Init(taskId);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.RadioStation_OP_384, OnGetRadioStationOpMsg);


    }



    private void OnGetRadioStationOpMsg(IEventParam param)
    {
        //已完成所有步骤
        if (IsFinish()) return;

        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            RadioStationOp384Model model = JsonTool.ToObject<RadioStationOp384Model>(tcpReceiveEvParam.netData.Msg);
            switch (model.Type)
            {
                case RadioStationOpType384.OpenClose:
                    DoProcess(model.Operate == OperateDevice.OPEN ? RadioStation384Id.RadioStation_OPEN_384 : RadioStation384Id.RadioStation_CLOSE_384);
                    break;

            }
        }
    }

    public override void End()
    {
        base.End();
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.RadioStation_OP_384, OnGetRadioStationOpMsg);

    }
}
