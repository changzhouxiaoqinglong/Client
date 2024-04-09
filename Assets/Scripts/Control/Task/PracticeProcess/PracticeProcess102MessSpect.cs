using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeProcess102MessSpect : PracticeProcessBase
{
    public override void Init(int taskId)
    {
        base.Init(taskId);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.CAR_MASS_SPECT_102, OnGetMassSpectMsg);
    }

    private void OnGetMassSpectMsg(IEventParam param)
    {
        if (IsFinish()) return;
        if(param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            CarMassSpectOp102Model model = JsonTool.ToObject<CarMassSpectOp102Model>(tcpReceiveEvParam.netData.Msg);
            
            switch (model.Type)
            {
                case CarMasssSpectOpType102.NitrogenTap:
                    DoProcess(model.Operate == OperateDevice.OPEN ? MessSpect102Id.MESS_SPECT_NITRO_GENTIP_OPEN_102 : MessSpect102Id.MESS_SPECT_NITRO_GENTIP_CLOSE_102);
                    break;
                case CarMasssSpectOpType102.Power:
                    DoProcess(model.Operate == OperateDevice.OPEN ? MessSpect102Id.MESS_SPECT_POWER_OPEN_102 : MessSpect102Id.MESS_SPECT_POWER_CLOSE_102);
                    break;
                case CarMasssSpectOpType102.ZPY:
                    DoProcess(model.Operate == OperateDevice.OPEN ? MessSpect102Id.MESS_SPECT_ZPY_OPEN_102 : MessSpect102Id.MESS_SPECT_ZPY_CLOSE_102);
                    break;
                case CarMasssSpectOpType102.SampPoleCap:
                    DoProcess(model.Operate == OperateDevice.OPEN ? MessSpect102Id.MESS_SPECT_SAMP_POLE_CAP_OPEN_102 : MessSpect102Id.MESS_SPECT_SAMP_POLE_CAP_CLOSE_102);
                    break;
                case CarMasssSpectOpType102.ErrorOne:
                    UIMgr.GetInstance().ShowToast("启动质谱仪需先打开总气瓶");
                    break;
/*                case CarMasssSpectOpType102.ErrorTwo:
                    UIMgr.GetInstance().ShowToast("请打开探头盖板");
                    break;*/
                case CarMasssSpectOpType102.ErrorThree:
                    UIMgr.GetInstance().ShowToast("质谱仪非法开机");
                    break;
                default:
                    break;
            }
        }
    }


    protected override void JumpToNext()
    {
        base.JumpToNext();
    }

    public override void End()
    {
        base.End();
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.CAR_MASS_SPECT_102, OnGetMassSpectMsg);
    }
}
