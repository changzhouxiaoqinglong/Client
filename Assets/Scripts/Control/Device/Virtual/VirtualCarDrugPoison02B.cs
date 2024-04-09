using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 题目显色参数
/// </summary>
public class QstPoisonColorParam
{
    /// <summary>
    /// 毒类型的类
    /// </summary>
    public QstRequestResult qstResult;

    /// <summary>
    /// 管类型
    /// </summary>
    public int tubeType;
}

public class VirtualCarDrugPoison02B : VirtualDeviceBase
{
    /// <summary>
    /// 加热开关状态
    /// </summary>
    public bool curHeatState = false;

    /// <summary>
    /// 泵开关状态
    /// </summary>
    public bool curPumpState = false;


    public bool isOk = false;

    public VirtualCarDrugPoison02B() : base()
    {
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.OP_CAR_DETECT_POISON, OnGetDetectPoisonMsg);
    }


    private void OnGetDetectPoisonMsg(IEventParam param)
    {
        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            //操作数据
            CarDetectPoisonOpModel model = JsonTool.ToObject<CarDetectPoisonOpModel>(tcpReceiveEvParam.netData.Msg);
            if (model.Operate == OperateDevice.OPEN)
            {
                DeviceState(model, true);
            } 
            else
            {
                DeviceState(model,false);
            }
        }
    }

    /// <summary>
    /// 设备状态
    /// </summary>
    private void DeviceState(CarDetectPoisonOpModel model,bool state)
    {
        if (model.Type == CarDetectPoisonOpType.Pump)
        {
            curPumpState = state;
        }
        else if (model.Type == CarDetectPoisonOpType.Heat)
        {
            curHeatState = state;
        }
        Debug.Log(curPumpState);
    }

    public override void OnDestory()
    {
        base.OnDestory();
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.OP_CAR_DETECT_POISON, OnGetDetectPoisonMsg);
    }
}
