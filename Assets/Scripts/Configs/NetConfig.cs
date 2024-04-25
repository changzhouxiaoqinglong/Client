/// <summary>
/// 网络配置
/// </summary>
public class NetConfig : ConfigBase<NetConfig>
{
    /// <summary>
    /// 导控服务器 ip地址
    /// </summary>
    public static string GUIDE_IP;

    /// <summary>
    /// 导控服务器 端口号
    /// </summary>
    public static int GUIDE_PORT;

    /// <summary>
    /// TCP连接 消息包头的大小 包头保存包体大小 int型 占4字节
    /// </summary>
    public static int TCP_MESSAGE_HEAD_LEN;

    /// <summary>
    /// 心跳上报间隔 ms
    /// </summary>
    public static int HEART_BEAT_OFFTIME;

    /// <summary>
    /// 断开连接 心跳超时时间 s
    /// </summary>
    public static int HEART_BEAT_TIME_OUT;

    /// <summary>
    /// 同步驾驶画面Server端IP
    /// </summary>
    public static string SYS_SCREEN_IP;

    /// <summary>
    /// 同步驾驶画面Server端监听端口
    /// </summary>
    public static int FM_SERVER_PORT;

    /// <summary>
    /// 同步驾驶画面Client端监听端口
    /// </summary>
    public static int FM_CLIENT_PORT;

    /// <summary>
    /// 同步102遥测画面Server端监听端口
    /// </summary>
    public static int FM_102yaoce_SERVER_PORT;

    /// <summary>
    /// 同步102遥测画面Client端监听端口
    /// </summary>
    public static int FM_102yaoce_CLIENT_PORT;


    /// <summary>
    /// 同步全屏画面Server端监听端口
    /// </summary>
    public static int FM_SERVER_FULL_PORT;

    /// <summary>
    /// 同步全屏画面Client端监听端口
    /// </summary>
    public static int FM_CLIENT_FULL_PORT;

    /// <summary>
    /// 状态同步每秒次数
    /// </summary>
    public static int SYNC_SECOND_TIMES;

    /// <summary>
    /// 位置态势同步上报间隔s
    /// </summary>
    public static int SITUATION_SYNC_POS_OFF_TIME;

    /// <summary>
    /// FTP文件保留时间
    /// </summary>
    public static int FTP_SAVE_TIME;

    public static void InitConfig()
    {
        ParseConfigByReflection("NetConfig.cfg");
    }
}
