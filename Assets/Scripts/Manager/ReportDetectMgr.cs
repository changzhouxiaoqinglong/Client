using System.Collections.Generic;
using System.Text;
/// <summary>
/// 侦察结果类型
/// </summary>
public enum DetectResType
{
    /// <summary>
    /// 投放标志旗 侦察结果
    /// </summary>
    Flag,

    /// <summary>
    /// 毒侦察结果
    /// </summary>
    Poison,
}

/// <summary>
/// 上报侦察结果管理
/// </summary>
public class ReportDetectMgr
{
    /// <summary>
    /// 待上报侦察结果
    /// </summary>
    private Dictionary<DetectResType, List<string>> detectResDicts = new Dictionary<DetectResType, List<string>>();

    public void OnAwake()
    {
        EventDispatcher.GetInstance().AddEventListener(EventNameList.REPORT_DETECT_RES, ReportDetectRes);
        EventDispatcher.GetInstance().AddEventListener(EventNameList.ADD_REPORT_DETECT, AddReportDetectEv);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.SEND_DETCT_RES_TO_SEAT, OnGetDetectResMsg);
    }

    /// <summary>
    /// 收到网络消息 侦察结果
    /// </summary>
    private void OnGetDetectResMsg(IEventParam param)
    {
        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            //结果
            DetectResParam model = JsonTool.ToObject<DetectResParam>(tcpReceiveEvParam.netData.Msg);
            AddReportDetect(model.type, model.res);
        }
    }

    /// <summary>
    /// 增加要上报的侦察结果
    /// </summary>    
    private void AddReportDetectEv(IEventParam param)
    {
        if (param is DetectResParam detectParam)
        {
            AddReportDetect(detectParam.type, detectParam.res);
        }
    }

    private void AddReportDetect(DetectResType type, string res)
    {
        switch (type)
        {
            case DetectResType.Poison:
                if (detectResDicts.ContainsKey(type))
                {
                    res = $"侦察点{detectResDicts[type].Count + 1}" + res;
                }
                else
                {
                    res = "侦察点1" + res;
                }
                break;
        }

        if (detectResDicts.ContainsKey(type))
        {
            detectResDicts[type].Add(res);
        }
        else
        {
            detectResDicts[type] = new List<string>() { res };
        }
    }

    /// <summary>
    /// 上报侦察结果
    /// </summary>
    public void ReportDetectRes(IEventParam param)
    {
        //if (detectResDicts.Count <= 0)
        //{
        //    UIMgr.GetInstance().ShowToast("没有要上报的侦察结果");
        //    return;
        //}
        //StringBuilder res = new StringBuilder();
        //foreach (var item in detectResDicts)
        //{
        //    if (item.Value != null)
        //    {
        //        foreach (var item2 in item.Value)
        //        {
        //            res.AppendLine(item2);
        //        }
        //    }
        //}
        ////最终结果在前面加上车号
        //string resStr = $"车{AppConfig.MACHINE_ID}:\n" + res.ToString();
        //DetectResModel reportModel = new DetectResModel()
        //{
        //    Result = resStr.Trim(),
        //};
        ////车上所有人 包括自己
        //List<ForwardModel> forwardModels = new List<ForwardModel>();
        //forwardModels.AddRange(NetManager.GetInstance().SameMachineSeatsExDevice);
        //forwardModels.Add(new ForwardModel()
        //{
        //    MachineId = AppConfig.MACHINE_ID,
        //    SeatId = AppConfig.SEAT_ID,
        //});
        //NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(reportModel), NetProtocolCode.REPORT_DETECT_RES, forwardModels);
        //UIMgr.GetInstance().ShowToast("上报成功");
    }

    public void OnDestroy()
    {
        EventDispatcher.GetInstance().RemoveEventListener(EventNameList.REPORT_DETECT_RES, ReportDetectRes);
        EventDispatcher.GetInstance().RemoveEventListener(EventNameList.ADD_REPORT_DETECT, AddReportDetectEv);
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.SEND_DETCT_RES_TO_SEAT, OnGetDetectResMsg);
    }
}
