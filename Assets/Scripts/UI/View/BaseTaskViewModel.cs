
public class BaseTaskViewModel : ViewModelBase
{

    /// <summary>
    /// 结束训练
    /// </summary>
    public void ClickEndTrain()
    {
        TaskMgr.GetInstance().ResportEndTask();
    }

    /// <summary>
    /// 发送辐射剂量率
    /// </summary>    
    public void SendRadiomRate(float dose)
    {
        EventDispatcher.GetInstance().DispatchEvent(EventNameList.SEND_RADIOM_RATE, new FloatEvParam(dose));
    }

    /// <summary>
    /// 设置抽气时间
    /// </summary>    
    public void SetGasTime(float time)
    {
      //  EventDispatcher.GetInstance().DispatchEvent(EventNameList.SET_POIS_GAS_TIME, new FloatEvParam(time));
    }
}
