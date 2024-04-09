

using System.IO;
using UnityEngine;

public class AppConstant
{
    /// <summary>
    /// 配置文件路径
    /// </summary>
    public static readonly string CONFIG_PATH = Path.Combine(Application.streamingAssetsPath, "Configs");

    /// <summary>
    /// 辐射剂量率 检测间隔
    /// </summary>
    public const float RADIOM_CHECK_OFFTIME = 0.5f;

    /// <summary>
    /// 化学 检测间隔
    /// </summary>
    public const float DRUG_CHECK_OFFTIME = 0.5f;

    /// <summary>
    /// 北斗数据上报间隔 s
    /// </summary>
    public const float BEIDOU_SEND_OFFTIME = 1;

    /// <summary>
    /// 气象仪 数据上报间隔 s
    /// </summary>
    public const float SEND_METEOR_OFF_TIME = 5;

    /// <summary>
    /// 剂量率单位
    /// </summary>
    public const string RADIOM_UNIT = "uGy/h";

    /// <summary>
    /// 接近测量点 上报速度 延时时间
    /// </summary>
    public const float CHECK_SPEED_DELAY = 1.5f;
}
