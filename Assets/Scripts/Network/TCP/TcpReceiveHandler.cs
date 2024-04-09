using System;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class TcpReceiveHandler
{
    /// <summary>
    /// 存储处理接收到的数据
    /// </summary>
    private TcpMessageHandler message;

    private TcpClient client;

    public TcpReceiveHandler(TcpClient client)
    {
        this.client = client;
        message = new TcpMessageHandler(client);
    }

    /// <summary>
    /// 开始接收数据
    /// </summary>
    public void StartReceive()
    {
        if (client.socket == null || client.socket.Connected == false) return;

        try
        {
            client.socket.BeginReceive(message.Datas, message.StartIndex, message.RemainSize, SocketFlags.None, ReceiveCallback, null);
        }
        catch (Exception e)
        {
            //断开连接
            Logger.LogError("BeginReceive exception:" + e.ToString());
            client.DisConnect();
        }
    }

    private void ReceiveCallback(IAsyncResult iar)
    {
        if (client == null || !client.socket.Connected)
        {
            return;
        }
        try
        {
            //接收到的数据长度
            int length = client.socket.EndReceive(iar);
            if (length > 0)
            {
                try
                {
                    message.AddReceiveNum(length);
                    //解析数据
                    message.ReadMessage();
                }
                catch (Exception e)
                {
                    Logger.LogError("handle message exception : " + e.ToString());
                }
                StartReceive();
            }
            else
            {
                Logger.LogError("Receive data length is 0 ,disconnect");
                client.DisConnect();
            }
        }
        catch (Exception e)
        {
            if (client.IsConnected())
            {
                //断开连接
                Logger.LogError("receive exception:" + e.ToString());
                client.DisConnect();
            }
        }

    }
}
