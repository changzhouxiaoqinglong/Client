
using System.Collections.Generic;
/// <summary>
/// 训练场景基类
/// </summary>
public class TrainSceneCtrBase : SceneCtrBase
{
    private ReportDetectMgr reportMgr = new ReportDetectMgr();
    /// <summary>
    /// 请求答题结果列表
    /// </summary>

    /// <summary>
    /// 虚拟车对象（无实体）
    /// </summary>
    public VirtualCarBase virtualCar;

    protected override void Awake()
    {
        base.Awake();
        reportMgr.OnAwake();
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.END, OnGetEndTaskEv);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.REQUEST_QUESTION, SendQstRequest);
        EventDispatcher.GetInstance().AddEventListener(EventNameList.SEND_RADIOM_RATE, SendRadiomRate);
       // EventDispatcher.GetInstance().AddEventListener(EventNameList.SET_POIS_GAS_TIME, SetGasTime);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.GUIDE_PROCESS_CTR, OnGetGuideProcessMsg);
    }

    protected override void Start()
    {
        base.Start();
        if (this is BaseSceneCtr)
        {
            print("基本训练界面");
            //基本训练界面
            UIMgr.GetInstance().OpenView(ViewType.BaseTaskView);
        }
        else
        {
            UIMgr.GetInstance().OpenView(ViewType.TrainView);
        }
        //任务提示界面
        UIMgr.GetInstance().OpenView(ViewType.TaskTipView);
        InitVirtualCar();
    }

    /// <summary>
    /// 收到任务结束消息
    /// </summary>
    private void OnGetEndTaskEv(IEventParam param)
    {
        TaskMgr.GetInstance().DoEndTask();
    }

    /// <summary>
    /// 发送辐射剂量率
    /// </summary>    
    public void SendRadiomRate(IEventParam param)
    {
        if (param is FloatEvParam floatParam)
        {
            SetDoseRateModel model = new SetDoseRateModel()
            {
                DoseRate = floatParam.value,
            };
            //发给设备
            NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(model), NetProtocolCode.SEND_RADIOM_RATE, NetManager.GetInstance().CurDeviceForward);
        }
    }

    /// <summary>
    /// 设置车载侦毒器抽气时间
    /// </summary>    
    public void SetGasTime(IEventParam param)
    {
        print("设置车载侦毒器抽气时间");
        if (param is FloatEvParam floatParam)
        {
            SetCarPoisonGasTime model = new SetCarPoisonGasTime()
            {
                Time = floatParam.value,
            };
            List<ForwardModel> forwardModels = new List<ForwardModel>();
            forwardModels.Add(new ForwardModel()
            {
                MachineId = AppConfig.MACHINE_ID,
                SeatId = AppConfig.SEAT_ID,
            });
            NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(model), NetProtocolCode.SET_CAR_POIS_GAS_TIME,forwardModels);
        }
    }

    /// <summary>
    /// 派发请求答题给驾驶员
    /// </summary>
    /// <param name="param"></param>
    private void SendQstRequest(IEventParam param)
    {
        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            QstRequest qst = JsonTool.ToObject<QstRequest>(tcpReceiveEvParam.netData.Msg);
            int triggerType = qst.TriggerType;
            NetManager.GetInstance().DispatchQstRequestMsgEvent(triggerType, param);

        }
    }


    /// <summary>
    /// 监听导控控制训练进程消息
    /// </summary>
    /// <param name="param"></param>
    private void OnGetGuideProcessMsg(IEventParam param)
    {
        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            GuideProcessCtrModel model = JsonTool.ToObject<GuideProcessCtrModel>(tcpReceiveEvParam.netData.Msg);
            switch (model.Operate)
            {
                case GuideProcessType.Pause:
                    //暂停
                    UIMgr.GetInstance().OpenView(ViewType.PauseView);
                    break;
                case GuideProcessType.End:
                    //任务结束
                    TaskMgr.GetInstance().DoEndTask();
                    break;
            }
        }
    }

    /// <summary>
    /// 初始化虚拟车对象
    /// </summary>
    private void InitVirtualCar()
    {
        switch (AppConfig.CAR_ID)
        {
            case CarIdConstant.ID_102:
                virtualCar = new VirtualCar102();
                break;
            case CarIdConstant.ID_384C:
                virtualCar = new VirtualCar384();
                break;
            case CarIdConstant.ID_02B:
                virtualCar = new VirtualCar02B();
                break;
            default:
                virtualCar = new VirtualCarBase();
                break;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        reportMgr.OnDestroy();
        virtualCar.OnDestroy();
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.END, OnGetEndTaskEv);
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.REQUEST_QUESTION, SendQstRequest);
        EventDispatcher.GetInstance().RemoveEventListener(EventNameList.SEND_RADIOM_RATE, SendRadiomRate);
      //  EventDispatcher.GetInstance().RemoveEventListener(EventNameList.SET_POIS_GAS_TIME, SetGasTime);
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.GUIDE_PROCESS_CTR, OnGetGuideProcessMsg);
    }

}