using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeProcess384Power : PracticeProcessBase
{
    public override void Init(int taskId)
    {
        base.Init(taskId);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.POISON_ALARM_OP_384, OnGetPowerOpMsg);


    }



    private void OnGetPowerOpMsg(IEventParam param)
    {
        //已完成所有步骤
        if (IsFinish()) return;

        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            PowerOp384Model model = JsonTool.ToObject<PowerOp384Model>(tcpReceiveEvParam.netData.Msg);
            switch (model.Type)
            {
                case PowerOp384Type.OpenClose:
                    DoProcess(model.Operate == OperateDevice.OPEN ? Power384Id.Power_OPEN_384 : Power384Id.Power_CLOSE_384);
                    break;

            }
        }
    }

    public override void End()
    {
        base.End();
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.POISON_ALARM_OP_384, OnGetPowerOpMsg);

    }
}
