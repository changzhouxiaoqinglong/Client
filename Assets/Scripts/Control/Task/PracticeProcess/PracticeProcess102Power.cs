using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeProcess102Power : PracticeProcessBase
{
    public override void Init(int taskId)
    {
        base.Init(taskId);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.POWER_102, OnGetPowerOpMsg);


    }



    private void OnGetPowerOpMsg(IEventParam param)
    {
        //已完成所有步骤
        if (IsFinish()) return;

        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            PowerOp102Model model = JsonTool.ToObject<PowerOp102Model>(tcpReceiveEvParam.netData.Msg);
            switch (model.Type)
            {
                case PowerOpType102.OpenClose:
                    DoProcess(model.Operate == OperateDevice.OPEN ? Power102Id.Power_OPEN_102 : Power102Id.Power_CLOSE_102);
                    break;

            }
        }
    }

    public override void End()
    {
        base.End();
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.POWER_102, OnGetPowerOpMsg);

    }
}
