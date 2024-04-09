using System;
using System.Collections.Generic;

public interface IClient
{
    ServerType ServerType { get; set; }

    /// <summary>
    /// 断开连接
    /// </summary>
    void DisConnect();

    bool IsConnected();

    /// <summary>
    /// 发送消息
    /// </summary>
    void SendMsg(string message, int protocolCode, List<ForwardModel> forwards = null);

    /// <summary>
    /// 发送消息
    /// </summary>
    void SendMsg(NetData data);

    /// <summary>
    /// 连接服务器
    /// </summary>
    void ConnectServer(Action<bool> callBack);
}
