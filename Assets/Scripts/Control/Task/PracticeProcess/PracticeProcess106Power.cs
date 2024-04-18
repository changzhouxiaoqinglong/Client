using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeProcess106Power : PracticeProcessBase
{
    public override void Init(int taskId)
    {
        base.Init(taskId);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.POWER_OP_106, OnGetPowerOpMsg);


    }



    private void OnGetPowerOpMsg(IEventParam param)
    {
        //已完成所有步骤
        if (IsFinish()) return;

        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            PowerOp106Model model = JsonTool.ToObject<PowerOp106Model>(tcpReceiveEvParam.netData.Msg);
            switch (model.Type)
            {
                case PowerOp106Type.OpenClose:
                    DoProcess(model.Operate == OperateDevice.OPEN ? Power106Id.Power_OPEN_106 : Power106Id.Power_CLOSE_106);
                    break;

            }
        }
    }

    public override void End()
    {
        base.End();
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.POWER_OP_106, OnGetPowerOpMsg);

    }
}
