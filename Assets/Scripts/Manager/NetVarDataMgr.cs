
using System.Collections.Generic;

/// <summary>
/// 网络获取的动态数据 
/// </summary>
public class NetVarData
{
    /// <summary>
    /// 当前用户数据
    /// </summary>
    public UserInfo _UserInfo { get; set; } = new UserInfo();

    /// <summary>
    /// 任务环境数据
    /// </summary>
    public TaskEnvVarData _TaskEnvVarData { get; set; }

    /// <summary>
    /// 开始训练数据
    /// </summary>
    public TrainStartModel _TrainStartModel{ get; set; }

    /// <summary>
    /// 参加训练的车 席位信息
    /// </summary>
    private List<TrainMachineVarData> trainMachineDatas;
    public List<TrainMachineVarData> TrainMachineDatas
    {
        get
        {
            return trainMachineDatas;
        }
        set
        {
            trainMachineDatas = value;
            //更新
            EventDispatcher.GetInstance().DispatchEvent(EventNameList.UPDATE_CAR_PLAYER);
            NetManager.GetInstance().UpdateForwardList(trainMachineDatas);
            //UnityEngine.Debug.LogError("参加训练的车 席位信息");
        }
    }

    /// <summary>
    /// 训练得分
    /// </summary>
    private GetScoreModel scoreModels;

    public GetScoreModel ScoreModels
    {
        get { return scoreModels; }
        set
        {
            scoreModels = value;
        }
    }
}

/// <summary>
/// 网络获取的动态数据管理
/// </summary>
public class NetVarDataMgr : MonoSingleTon<NetVarDataMgr>
{
    /// <summary>
    /// 网络数据
    /// </summary>
    public NetVarData _NetVarData { get; set; } = new NetVarData();

    private void Start()
    {
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.GET_SCORE, OnGetScoreMsg);
    }

    private void OnGetScoreMsg(IEventParam param)
    {
        if (param is TcpReceiveEvParam tcpParam)
        {
            _NetVarData.ScoreModels = JsonTool.ToObject<GetScoreModel>(tcpParam.netData.Msg);
            EventDispatcher.GetInstance().DispatchEvent(EventNameList.ON_GET_TRAIN_SCORE);
        }
    }

    private void OnDestroy()
    {
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.GET_SCORE, OnGetScoreMsg);
    }
}
