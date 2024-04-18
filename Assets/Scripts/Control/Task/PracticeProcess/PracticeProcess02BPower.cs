using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeProcess02BPower : PracticeProcessBase
{
    public override void Init(int taskId)
    {
        base.Init(taskId);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.POWER_OP, OnGetPowerOpMsg);
       

    }

    

    private void OnGetPowerOpMsg(IEventParam param)
    {
        //已完成所有步骤
        if (IsFinish()) return;

        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            PowerOpModel model = JsonTool.ToObject<PowerOpModel>(tcpReceiveEvParam.netData.Msg);
            switch (model.Type)
            {
                case PowerOpType.OpenClose:
                    DoProcess(model.Operate == OperateDevice.OPEN ? PowerId.Power_OPEN_02B : PowerId.Power_CLOSE_02B);
                    break;
              
            }
        }
    }

    public override void End()
    {
        base.End();
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.POWER_OP, OnGetPowerOpMsg);

    }
}
