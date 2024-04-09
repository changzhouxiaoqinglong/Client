

using System.Threading;

public class TcpHeartBeat
{

    private const string TAG = "[TcpHeartBeat]:";

    /// <summary>
    /// 是否正在处理发送
    /// </summary>
    private bool isHandleSend = false;

    /// <summary>
    /// 是否正在处理接收
    /// </summary>
    private bool isHandleReceive = false;

    private TcpClient tcpClient;

    /// <summary>
    /// 接收心跳 间隔计时器
    /// </summary>
    private int revTimer = 0;

    public TcpHeartBeat(TcpClient tcpClient)
    {
        this.tcpClient = tcpClient;
    }

    public void StartHeartBeat()
    {
        if (!isHandleSend)
        {
            //开始处理发送心跳包
            Loom.GetInstance().RunAsync(SendHeartBeat);
        }
    }
    #region 发送
    private NetData heartBeatData;

    private NetData HeartBeatData
    {
        get
        {
            if (heartBeatData == null)
            {
                heartBeatData = new NetData(NetProtocolCode.HEART_BEAT, "");
            }
            return heartBeatData;
        }
    }
    /// <summary>
    /// 处理发送心跳
    /// </summary>
    private void SendHeartBeat()
    {
        isHandleSend = true;
        while (true)
        {
            if (!tcpClient.IsConnected())
            {
                break;
            }
            NetManager.GetInstance().SendMsg(tcpClient.ServerType, HeartBeatData);
            //间隔时间上报心跳包
            Thread.Sleep(NetConfig.HEART_BEAT_OFFTIME);
        }
        isHandleSend = false;
    }
    #endregion

    #region 接收
    /// <summary>
    /// 重置计时
    /// </summary>
    public void ResetTimer()
    {
        revTimer = 0;
    }

    /// <summary>
    /// 开始处理心跳接收计时  超时断开连接
    /// </summary>
    public void StartRevTimer()
    {
        if (!isHandleReceive)
        {
            Loom.GetInstance().RunAsync(RevTimerHandle);
        }
    }

    /// <summary>
    /// 接收计时 超时断开连接
    /// </summary>
    private void RevTimerHandle()
    {
        isHandleReceive = true;
        while (true)
        {
            Thread.Sleep(1000);
            if (!tcpClient.IsConnected())
            {
                Logger.LogWarning(TAG + "Disconnected HeartBeat Stop");
                break;
            }
            revTimer += 1;
            if (revTimer >= NetConfig.HEART_BEAT_TIME_OUT)
            {
                //断开连接
                tcpClient.DisConnect();
                Logger.LogError(TAG + "HeartBeat TimeOut Disconnected");
                break;
            }
        }
        isHandleReceive = false;
        ResetTimer();

        //开始重连
        NetManager.GetInstance().ReConnect();
    }
    #endregion
}
