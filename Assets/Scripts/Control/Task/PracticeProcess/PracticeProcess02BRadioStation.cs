using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeProcess02BRadioStation : PracticeProcessBase
{
    public override void Init(int taskId)
    {
        base.Init(taskId);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.RadioStation_OP, OnGetRadioStationOpMsg);


    }



    private void OnGetRadioStationOpMsg(IEventParam param)
    {
        //已完成所有步骤
        if (IsFinish()) return;

        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            RadioStationOpModel model = JsonTool.ToObject<RadioStationOpModel>(tcpReceiveEvParam.netData.Msg);
            switch (model.Type)
            {
                case RadioStationOpType.OpenClose:
                    DoProcess(model.Operate == OperateDevice.OPEN ? RadioStationId.RadioStation_OPEN_02B : RadioStationId.RadioStation_CLOSE_02B);
                    break;

            }
        }
    }

    public override void End()
    {
        base.End();
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.RadioStation_OP, OnGetRadioStationOpMsg);

    }
}
