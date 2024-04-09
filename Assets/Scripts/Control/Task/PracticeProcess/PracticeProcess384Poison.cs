using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeProcess384Poison : PracticeProcessBase
{
    public override void Init(int taskId)
    {
        base.Init(taskId);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.POISON_ALARM_OP_384,OnGetPoisonAlarmMsg);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.POISON_ALARM_WORK_TYPE_384,OnGetPoisonSetWorkModelMsg);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.RADIOME_OP_384, ONGetPoisonDFHMsg);
    }

    private void OnGetPoisonAlarmMsg(IEventParam param)
    {
        if (IsFinish()) return;

        if(param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            PoisonAlarmOp384Model model = JsonTool.ToObject<PoisonAlarmOp384Model>(tcpReceiveEvParam.netData.Msg);
            if (model.Type == PoisonAlarmOp384Type.Error)
                UIMgr.GetInstance().ShowToast("请打开空气侦检探头");
            switch (model.Type)
            {
                case PoisonAlarmOp384Type.AirJinYang:
                    DoProcess(model.Operate == OperateDevice.OPEN ? Poison384Id.POISON384_HEAD_OPEN : Poison384Id.POISON384_HEAD_CLOSE);
                    break;
                case PoisonAlarmOp384Type.GroundJinYang:
                    DoProcess(model.Operate == OperateDevice.OPEN ? Poison384Id.POISON384_HEAD_OPEN : Poison384Id.POISON384_HEAD_CLOSE);
                    break;
                case PoisonAlarmOp384Type.OpenStatus:
                    if(model.Operate == OperateDevice.OPEN)
                    {
                        DoProcess(Poison384Id.POISON384_OPEN);
                    }
                    else if(model.Operate == OperateDevice.CLOSE)
                    {
                        DoProcess(Poison384Id.POISON384_CLOSE);
                    }
                    break;
                default:
                    break;
            }

        }
    }

    private void ONGetPoisonDFHMsg(IEventParam param)
    {
        if (IsFinish()) return;
        if(param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            RadiomeOp384Model model = JsonTool.ToObject<RadiomeOp384Model>(tcpReceiveEvParam.netData.Msg);
            if(model.Type == RadiomOpType384.OpenClose)
            {
                if(model.Operate == OperateDevice.OPEN)
                {
                    DoProcess(Poison384Id.POISON384_DFH_OPEN);
                }
            }
        }
    }


    private void OnGetPoisonSetWorkModelMsg(IEventParam param)
    {
        if (IsFinish()) return;

        if(param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            PoisonAlarmWorkType384Model model = JsonTool.ToObject<PoisonAlarmWorkType384Model>(tcpReceiveEvParam.netData.Msg);
            if (model.Type == PoisonAlarmWorkType.UPDATE_MODEL)
                UIMgr.GetInstance().ShowToast("修改侦检模式，请将DFH控制盒开机");
            DoProcess(Poison384Id.POISON384_SET_WORK_MODEL);
        }
    }

    protected override void JumpToNext()
    {
        base.JumpToNext();
    }

    public override void End()
    {
        base.End();
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.POISON_ALARM_OP_384, OnGetPoisonAlarmMsg);
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.POISON_ALARM_WORK_TYPE_384, OnGetPoisonSetWorkModelMsg);
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.RADIOME_OP_384, ONGetPoisonDFHMsg);
    }
}
