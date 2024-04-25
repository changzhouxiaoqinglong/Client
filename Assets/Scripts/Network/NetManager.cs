using System;
using System.Collections.Generic;
using UnityEngine;
using static EventDispatcher;

/// <summary>
/// 连接的服务器
/// </summary>
public enum ServerType
{
    /// <summary>
    /// 导控
    /// </summary>
    GuideServer,
}

public class NetManager : MonoSingleTon<NetManager>
{
    private const string TAG = "[NetManager]:";

    #region 转发数据组
    /// <summary>
    /// 同车 所有席位 包括设备管理软件（不包括自己）
    /// </summary>
    public List<ForwardModel> SameMachineAllSeats = new List<ForwardModel>();

    /// <summary>
    /// 同车 所有席位 不包括设备管理软件（不包括自己）
    /// </summary>
    public List<ForwardModel> SameMachineSeatsExDevice = new List<ForwardModel>();

    /// <summary>
    /// 同一训练，所有席位 包括设备管理软件（不包括自己）
    /// </summary>
    public List<ForwardModel> SameTrainAllSeats = new List<ForwardModel>();

    /// <summary>
    /// 同一训练，所有席位 不包括设备管理软件（不包括自己）
    /// </summary>
    public List<ForwardModel> SameTrainSeatsExDevice = new List<ForwardModel>();

    /// <summary>
    /// 同一训练，所有驾驶位 不包括设备管理软件（不包括自己）
    /// </summary>
    public List<ForwardModel> SameTrainDriveSeatsExDevice = new List<ForwardModel>();

    /// <summary>
    /// 设备管理软件
    /// </summary>
    public List<ForwardModel> CurDeviceForward = new List<ForwardModel>();
    #endregion

    /// <summary>
    /// 当前的网络连接
    /// </summary>
    private Dictionary<ServerType, IClient> clients = new Dictionary<ServerType, IClient>();

    /// <summary>
    /// 添加网络连接
    /// </summary>
    private void AddClient(ServerType type, IClient client)
    {
        lock (clients)
        {
            if (clients.ContainsKey(type))
            {
                Logger.LogError(TAG + "Add Client Fail  Have Client");
                client.DisConnect();
                return;
            }
            clients[type] = client;
        }
    }

    /// <summary>
    /// 建立连接
    /// </summary>
    public void ConnectServer(ServerType type, Action<bool> callBack = null)
    {
        IClient client = null;
        switch (type)
        {
            case ServerType.GuideServer:
                if (clients.ContainsKey(type))
                {
                    client = clients[type];
                }
                else
                {
                    client = new GuideTcpClient(NetConfig.GUIDE_IP, NetConfig.GUIDE_PORT, ServerType.GuideServer);
                    AddClient(type, client);
                }
                break;
            default:
                break;
        }
        if (client != null)
        {
            client.ConnectServer(callBack);
        }
    }

    public void DisConnect(ServerType type)
    {
        if (clients.ContainsKey(type))
        {
            clients[type].DisConnect();
        }
    }

    private void Awake()
    {
        CurDeviceForward.Add(new ForwardModel()
        {
            MachineId = AppConfig.MACHINE_ID,
            SeatId = SeatType.DEVICE,
        });
    }

    /// <summary>
    /// 对应服务器是否是连接状态
    /// </summary>
    public bool IsConnected(ServerType type)
    {
        return clients.ContainsKey(type) && clients[type].IsConnected();
    }
    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="type">服务器</param>
    /// <param name="message">消息内容</param>
    /// <param name="protocolCode">协议号</param>
    /// <param name="forwards">转发信息</param>
    public void SendMsg(ServerType type, string message, int protocolCode, List<ForwardModel> forwards = null)
    {
        if (clients.ContainsKey(type))
        {
            clients[type].SendMsg(message, protocolCode, forwards);
        }
        else
        {
            Logger.LogError("Servertyp : " + type + "    not connect can not SendMsg");
        }
    }

    public void SendMsg(ServerType type, NetData data)
    {
        if (clients.ContainsKey(type))
        {
            clients[type].SendMsg(data);
        }
        else
        {
            Logger.LogError("Servertyp : " + type + "    not connect can not SendMsg");
        }
    }

    #region  网络事件监听
    /// <summary>
    /// 添加网络数据接收监听
    /// </summary>
    /// <param name="type">对应服务器</param>
    /// <param name="protocolCode">监听的协议号</param>
    /// /// <param name="handler">监听事件</param>
    public void AddNetMsgEventListener(ServerType type, int protocolCode, EventDispatcher.EventHandler handler)
    {      
        EventDispatcher.GetInstance().AddEventListener(GetNetMsgEventKey(type, protocolCode), handler);
    }

    /// <summary>
    /// 移除网络数据接收监听
    /// </summary>
    /// <param name="type">对应服务器</param>
    /// <param name="protocolCode">监听的协议号</param>
    /// <param name="handler">监听事件</param>
    public void RemoveNetMsgEventListener(ServerType type, int protocolCode, EventDispatcher.EventHandler handler)
    {
        EventDispatcher.GetInstance().RemoveEventListener(GetNetMsgEventKey(type, protocolCode), handler);
    }

    /// <summary>
    /// 接收到网络数据 事件派发
    /// </summary>
    public void DispatchNetMsgEvent(ServerType type, int protocolCode, IEventParam param)
    {
        EventDispatcher.GetInstance().DispatchEventMainThread(GetNetMsgEventKey(type, protocolCode), param);
    }

    #endregion

    /// <summary>
    /// 获取网络消息事件 对应的key
    /// </summary>
    /// <param name="type">对应服务器</param>
    /// <param name="protocolCode">监听的协议号</param>
    private string GetNetMsgEventKey(ServerType type, int protocolCode)
    {
        return EventNameList.ON_RECEIVE_NET_MSG + "_" + type + "_" + protocolCode;
    }

    /// <summary>
    /// 获取事件对应的Key
    /// </summary>
    /// <param name="triggerType"></param>
    /// <returns></returns>
    private string GetTriggerTypeEventKey(int triggerType)
    {
        return EventNameList.TRIGGER_TYPE + "_" + triggerType;
    }

    /// <summary>
    /// 请求答题事件派发
    /// </summary>
    public void DispatchQstRequestMsgEvent(int triggerType, IEventParam param)
    {
        EventDispatcher.GetInstance().DispatchEvent(GetTriggerTypeEventKey(triggerType), param);
    }

    /// <summary>
    /// 监听请求答题事件
    /// </summary>
    public void AddQstRequstMsgEvent(int triggerType, EventDispatcher.EventHandler handler)
    {
        EventDispatcher.GetInstance().AddEventListener(GetTriggerTypeEventKey(triggerType), handler);
    }

    /// <summary>
    /// 删除请求答题事件
    /// </summary>
    public void RemoveQstRequstMsgEvent(int triggerType, EventDispatcher.EventHandler handler)
    {
        EventDispatcher.GetInstance().RemoveEventListener(GetTriggerTypeEventKey(triggerType), handler);
    }

    /// <summary>
    /// 更新转发数据组
    /// </summary>
    public void UpdateForwardList(List<TrainMachineVarData> trainMachineDatas)
    {
        SameTrainAllSeats.Clear();
        SameTrainSeatsExDevice.Clear();
        SameTrainDriveSeatsExDevice.Clear();
        SameMachineAllSeats.Clear();
        SameMachineSeatsExDevice.Clear();
        foreach (TrainMachineVarData machine in trainMachineDatas)
        {
            foreach (TrainSeatVarData seat in machine.TrainUserDatas)
            {
                //不需要转发自身
                if (seat.MachineId != AppConfig.MACHINE_ID || seat.SeatId != AppConfig.SEAT_ID)
                {
                    //同一训练，所有席位 包括设备管理软件（不包括自己）
                    SameTrainAllSeats.Add(seat.ForwardModel);
                    //同一训练，所有席位 不包括设备管理软件（不包括自己）
                    SameTrainSeatsExDevice.Add(seat.ForwardModel);
                    if (seat.SeatId == SeatType.DRIVE)
                    {
                        //同一训练，所有驾驶席位 不包括设备管理软件（不包括自己）
                        SameTrainDriveSeatsExDevice.Add(seat.ForwardModel);
                    }
                    if (seat.MachineId == AppConfig.MACHINE_ID)
                    {
                        //同车 所有席位 包括设备管理软件（不包括自己）
                        SameMachineAllSeats.Add(seat.ForwardModel);
                        //同车 所有席位 不包括设备管理软件（不包括自己）
                        SameMachineSeatsExDevice.Add(seat.ForwardModel);
                    }
                }
            }
        }
        SameTrainAllSeats.AddRange(CurDeviceForward);
        SameMachineAllSeats.AddRange(CurDeviceForward);
    }

    //断线重连
    public void ReConnect() {
        ConnectAgain();
    }

    void ConnectAgain()
    {
        Debug.Log("----------  开始重连");
        //判断如果在登录场景，就不重连
        if (UIMgr.GetInstance().IsOpenView(ViewType.LoginView)) return;
        //如果已经处于连接状态
        if (NetManager.GetInstance().IsConnected(ServerType.GuideServer))
        {
            return;
        }

        //连接导控
        ConnectServer(ServerType.GuideServer, (bool res) =>
        {
            //连接成功
            if (res)
            {
                UIMgr.GetInstance().ShowToast("重连成功!");

                //请求登录
                LoginModel loginModel = new LoginModel()
                {
                    UserName = NetVarDataMgr.GetInstance()._NetVarData._UserInfo.userName,
                    Password = NetVarDataMgr.GetInstance()._NetVarData._UserInfo.passWord,
                    CarId = AppConfig.CAR_ID,
                };
                NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(loginModel), NetProtocolCode.LOGIN);
            }
            else
            {
                //连接失败
                //UIMgr.GetInstance().ShowToast("重连失败!");
                //if (reConnectTime <= 3)
                {
                    ConnectAgain();
                }
            }
        });
    }



    private void OnDestroy()
    {
        foreach (var item in clients.Values)
        {
            item.DisConnect();
        }
    }
}