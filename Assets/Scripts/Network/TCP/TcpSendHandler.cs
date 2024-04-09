using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class TcpSendHandler
{
    private const string TAG = "[TcpSendHandler]:";

    /// <summary>
    /// 待发送消息
    /// </summary>
    private Queue<byte[]> sendMessages = new Queue<byte[]>();

    private Socket socket;

    /// <summary>
    /// 是否正在发送消息
    /// </summary>
    private bool isSending = false;

    public TcpSendHandler(Socket socket)
    {
        this.socket = socket;
    }

    /// <summary>
    /// 添加发送消息
    /// </summary>
    public void AddSendMsg(byte[] message)
    {
        lock (sendMessages)
        {
            sendMessages.Enqueue(message);
            CheckDoSendMsg();
        }
    }

    /// <summary>
    /// 检测发送消息
    /// </summary>
    private void CheckDoSendMsg()
    {
        if (socket == null || !socket.Connected)
        {
            Logger.LogWarning(TAG + "Not Connect Server Cannot SendMsg!");
            Reset();
            return;
        }
        if (isSending || sendMessages.Count <= 0)
        {
            return;
        }
        byte[] data = sendMessages.Dequeue();
        if (data != null && data.Length > 0)
        {
            try
            {
                isSending = true;
                socket.BeginSend(data, 0, data.Length, SocketFlags.None, SendCallBack, null);
            }
            catch (System.Exception e)
            {
                isSending = false;
                Logger.LogError(TAG + "send tcp message exception : " + e.ToString());
                CheckDoSendMsg();
            }
        }
    }

    private void SendCallBack(IAsyncResult iar)
    {
        socket.EndSend(iar);
        isSending = false;
        CheckDoSendMsg();
    }

    public void Reset()
    {
        sendMessages.Clear();
    }
}
