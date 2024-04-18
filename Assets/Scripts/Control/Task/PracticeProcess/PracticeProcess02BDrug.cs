using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeProcess02BDrug : PracticeProcessBase
{
    public override void Init(int taskId)
    {
        base.Init(taskId);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.OP_CAR_DETECT_POISON, OnGetDrugMsg);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.SET_CAR_POIS_GAS_TIME,OnGetGasTimeMsg);
    }

    private void OnGetGasTimeMsg(IEventParam param)
    {
        //已完成所有步骤
        if (IsFinish()) return;

        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            DoProcess(DrugId.DRUG_DET_BLEED_TIME);
        }
    }

    private void OnGetDrugMsg(IEventParam param)
    {
        //已完成所有步骤
        if (IsFinish()) return;

        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            CarDetectPoisonOpModel model = JsonTool.ToObject<CarDetectPoisonOpModel>(tcpReceiveEvParam.netData.Msg);
            switch (model.Type)
            {
                case CarDetectPoisonOpType.Pump: 
                    DoProcess(model.Operate == OperateDevice.OPEN ? DrugId.DRUG_PUMP_OPEN : DrugId.DRUG_PUMP_CLOSE); 
                    break;
                case CarDetectPoisonOpType.Heat: 
                    DoProcess(model.Operate == OperateDevice.OPEN ? DrugId.DRUG_HEAT_OPEN : DrugId.DRUG_HEAT_CLOSE); 
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
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.OP_CAR_DETECT_POISON, OnGetDrugMsg);
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.SET_CAR_POIS_GAS_TIME, OnGetGasTimeMsg);

    }
}
