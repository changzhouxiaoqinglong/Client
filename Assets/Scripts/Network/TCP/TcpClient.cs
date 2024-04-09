using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
public enum ConnectStatus
{
    /// <summary>
    /// 未连接
    /// </summary>
    Disconnected,

    /// <summary>
    /// 正在连接中
    /// </summary>
    Connecting,

    /// <summary>
    /// 已连接
    /// </summary>
    Connected,
}

/// <summary>
/// tcp客户端
/// </summary>
public class TcpClient : IClient
{
    private const string TAG = "[TcpClient]:";
    public Socket socket;

    /// <summary>
    /// 服务器 ip 和 端口
    /// </summary>
    private IPEndPoint ipEndPoint;

    /// <summary>
    /// 当前连接状态
    /// </summary>
    public ConnectStatus curStatus = ConnectStatus.Disconnected;

    /// <summary>
    /// 发送处理
    /// </summary>
    private TcpSendHandler sendHandler;

    /// <summary>
    /// 接收处理
    /// </summary>
    private TcpReceiveHandler receiveHandler;

    /// <summary>
    /// 心跳处理
    /// </summary>
    private TcpHeartBeat heartBeat;

    /// <summary>
    /// 连接的服务器类型
    /// </summary>
    public ServerType ServerType { get; set; }

    /// <summary>
    /// 连接回调
    /// </summary>
    private Action<bool> connectCallBack = null;

    public TcpClient(string ip, int port, ServerType ServerType)
    {
        ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        this.ServerType = ServerType;
    }

    private void SetConnectStatus(ConnectStatus status)
    {
        curStatus = status;
    }


    /// <summary>
    /// 连接服务器
    /// </summary>
    public void ConnectServer(Action<bool> callBack)
    {
        if (IsConnected())
        {
            Logger.LogWarning(TAG + "Connect fail, already connected!");
            return;
        }
        if (curStatus == ConnectStatus.Connecting)
        {
            Logger.LogWarning(TAG + "is Connecting do not connect again");
            return;
        }
        //先断开当前的连接
        DisConnect();
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        sendHandler = new TcpSendHandler(socket);
        receiveHandler = new TcpReceiveHandler(this);
        heartBeat = new TcpHeartBeat(this);
        SetConnectStatus(ConnectStatus.Connecting);
        try
        {
            connectCallBack = callBack;
            //开始连接
            socket.BeginConnect(ipEndPoint, ConnectCallBack, null);
        }
        catch (Exception e)
        {
            SetConnectStatus(ConnectStatus.Disconnected);
            Logger.LogError(TAG + "Connect fail!!!" + e.ToString());
            //连接失败                            
            OnConnect(false);
        }
    }

    private void ConnectCallBack(IAsyncResult iar)
    {
        try
        {
            socket.EndConnect(iar);
            Logger.LogDebug(TAG + "Connect success:" + ipEndPoint.Address.ToString());
            SetConnectStatus(ConnectStatus.Connected);
            //连接成功
            OnConnect(true);
        }
        catch (Exception e)
        {
            SetConnectStatus(ConnectStatus.Disconnected);
            Logger.LogError(TAG + "Connect fail!!!" + e.ToString());
            //连接失败                            
            OnConnect(false);
        }
        if (IsConnected())
        {
            heartBeat.ResetTimer();
            //开始心跳
            heartBeat.StartHeartBeat();
            //开始心跳计时
            heartBeat.StartRevTimer();
            //开始接收消息
            receiveHandler.StartReceive();
            //发送初始数据
            InitModel initModel = new InitModel();
            NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(initModel), NetProtocolCode.INIT_MSG);
            Logger.LogWarning("初始数据");
        }
    }

    /// <summary>
    /// 连接回调
    /// </summary>
    protected virtual void OnConnect(bool res)
    {
        //回调逻辑在主线程执行
        Loom.GetInstance().QueueOnMainThread((object obj) =>
        {
            connectCallBack?.Invoke(res);
        }, null);
    }

    /// <summary>
    /// 收到网络数据（一般收到数据 都用事件派发 在主线程上处理 除非特殊的网络相关逻辑 直接在这里处理）
    /// </summary>
    public void OnReceiveNetData(NetData netData)
    {
        switch (netData.ProtocolCode)
        {
            case NetProtocolCode.HEART_BEAT:
                //重置心跳计时 因为计时都是在线程里做的，如果把重置逻辑放到主线程，
                //会导致如果调了unity的暂停逻辑，心跳计时不会被重置 导致超时断开连接
                heartBeat.ResetTimer();
                break;
            default:
                break;
        }
    }

    public bool IsConnected()
    {
        return curStatus == ConnectStatus.Connected;
    }

    public void DisConnect()
    {
        Logger.Log(TAG + "DisConnect!");
        try
        {
            if (socket != null)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Disconnect(true);
                socket.Close();
                socket.Dispose();
            }
        }
        catch (Exception e)
        {
            
        }
        SetConnectStatus(ConnectStatus.Disconnected);
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="message">消息Msg内容</param>
    /// <param name="protocolCode">协议号</param>
    /// <param name="forwards">转发信息</param>
    public void SendMsg(string message, int protocolCode, List<ForwardModel> forwards = null)
    {
        NetData data = new NetData(protocolCode, message);
        if (forwards != null)
        {
            data.Forwards = forwards;
        }
        SendMsg(data);
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="message">消息Msg内容</param>
    /// <param name="protocolCode">协议号</param>
    /// <param name="forwards">转发信息</param>
    public void SendMsg(NetData data)
    {
        string jsonMsg = JsonTool.ToJson(data);
       // Logger.LogDebug(TAG + "SendMsg:" + jsonMsg);
        sendHandler.AddSendMsg(TcpMessageHandler.PackMessage(jsonMsg));
    }
}
