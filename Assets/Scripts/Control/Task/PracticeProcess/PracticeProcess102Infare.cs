using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeProcess102Infare : PracticeProcessBase
{
    public override void Init(int taskId)
    {
        base.Init(taskId);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.INFARED_TELEMETRY_102, OnGetInfareMsg);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.INFARED_TELEMETRY_PARAM_102, OnGetInfareModelMsg);
    }

    private void OnGetInfareMsg(IEventParam param)
    {
        if (IsFinish()) return;
        if(param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            InfaredTelemetryOp102Model model = JsonTool.ToObject<InfaredTelemetryOp102Model>(tcpReceiveEvParam.netData.Msg);
            switch (model.Type)
            {
                case InfaredTelemetryOpType102.Rise:
                    DoProcess(model.Operate == OperateDevice.OPEN ? Infare102Id.INFARE_UP_MODEL_102 : Infare102Id.INFARE_NO_OPERATE);
                    break;
                case InfaredTelemetryOpType102.OpenClose:
                    DoProcess(model.Operate == OperateDevice.OPEN ? Infare102Id.INFARE_OPEN_102 : Infare102Id.INFARE_CLOSE_102);
                    break;
                case InfaredTelemetryOpType102.Drop:
                    DoProcess(model.Operate == OperateDevice.OPEN ? Infare102Id.INFARE_DOWN_MODEL_102 : Infare102Id.INFARE_NO_OPERATE);
                    break;
                case InfaredTelemetryOpType102.ErrorOne:
                    UIMgr.GetInstance().ShowToast("遥测扫描前需将遥测主机上升到位");
                    break;
                default:
                    break;
            }
        }
    }

    private void OnGetInfareModelMsg(IEventParam param)
    {
        if (IsFinish()) return;
        if(param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            InfaredTelemetryParamModel model = JsonTool.ToObject<InfaredTelemetryParamModel>(tcpReceiveEvParam.netData.Msg);
            if (model.Tmode == InfaredTelemetryParamOpType.STOP || model.Tmode == InfaredTelemetryParamOpType.CHECK) return;

            DoProcess(Infare102Id.INFARE_SET_MODEL_102);
        }
    }

    protected override void JumpToNext()
    {
        base.JumpToNext();
    }

    public override void End()
    {
        base.End();
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.INFARED_TELEMETRY_102, OnGetInfareMsg);
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.INFARED_TELEMETRY_PARAM_102, OnGetInfareModelMsg);
    }
}
