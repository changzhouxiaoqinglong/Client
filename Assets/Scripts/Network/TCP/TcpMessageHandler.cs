using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

public class TcpMessageHandler
{
    private TcpClient tcpClient;

    /// <summary>
    /// 用于存储接收到的数据
    /// </summary>
    private byte[] datas = new byte[512000];

    public byte[] Datas
    {
        get { return datas; }
    }

    /// <summary>
    /// 当前存储数据 开始接收的位置（也就是当前收到数据的尾部）
    /// </summary>
    private int startIndex = 0;

    public int StartIndex { get { return startIndex; } }

    /// <summary>
    /// 剩余能接收的大小
    /// </summary>
    public int RemainSize
    {
        get { return datas.Length - startIndex; }
    }

    public TcpMessageHandler(TcpClient tcpClient)
    {
        this.tcpClient = tcpClient;
    }

    /// <summary>
    /// 收到数据 增加数据长度
    /// </summary>
    public void AddReceiveNum(int addNum)
    {
        startIndex += addNum;
        if (startIndex >= datas.Length)
        {
            //接收的数据过多 超过了数组长度，需要考虑扩大初始接收数组长度
            Logger.LogError("tcp receive buffermax");
        }
    }

    /// <summary>
    /// 解析数据
    /// </summary>
    public void ReadMessage()
    {
        //包头大小
        int headLen = NetConfig.TCP_MESSAGE_HEAD_LEN+4;
        while (true)
        {
            //数据不足
            if (startIndex <= headLen)
            {
                break;
            }
            //包体大小
            int bodyLen = BitConverter.ToInt32(datas, 4);
            //数据包总大小
            int packLen = bodyLen + headLen;
            if (startIndex >= packLen)
            {
                //包体内容
                string str = Encoding.UTF8.GetString(datas, headLen, bodyLen);
                if (str[16] != '0')  //{"ProtocolCode":0      
                    Logger.LogDebug("tcp receive：" + str);
               
                try
                {
                    NetData netData = JsonTool.ToObject<NetData>(str);
                    NetManager.GetInstance().DispatchNetMsgEvent(tcpClient.ServerType, netData.ProtocolCode, new TcpReceiveEvParam(netData));
                    tcpClient.OnReceiveNetData(netData);
                }
                catch (Exception e)
                {
                    Logger.LogError("handle tcp message fail:" + e.ToString());
                }
                Array.Copy(datas, packLen, datas, 0, startIndex - packLen);
                startIndex -= packLen;
            }
            else
            {
                // 数据包不完整 等待下次接收
                Logger.LogWarning("pack not complete,wait for next");
                break;
            }
        }
    }

    /// <summary>
    /// 封装消息
    /// </summary>
    public static byte[] PackMessage(string message)
    {
        byte[] devicedata = BitConverter.GetBytes(0x352EF853);
        //消息内容
        byte[] bodyData = Encoding.UTF8.GetBytes(message);
        //消息长度 4个字节
        byte[] headerBytes = BitConverter.GetBytes(bodyData.Length);//以字节数组的形式返回指定 32 位有符号整数值,返回长度为 4 的字节数组。

        byte[] temBytes = new byte[headerBytes.Length + bodyData.Length + devicedata.Length];
        Buffer.BlockCopy(devicedata, 0, temBytes, 0, devicedata.Length);
        Buffer.BlockCopy(headerBytes, 0, temBytes, devicedata.Length, headerBytes.Length);
        Buffer.BlockCopy(bodyData, 0, temBytes, devicedata.Length + headerBytes.Length, bodyData.Length);
        return temBytes;
    }
}
