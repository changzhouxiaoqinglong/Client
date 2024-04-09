
/// <summary>
/// 自定义输入值
/// </summary>
public class CustomInput
{
    /// <summary>
    /// 水平输入值
    /// </summary>
    public static float Horizontal { get; set; } = 0;

    /// <summary>
    /// 竖直输入值
    /// </summary>
    public static float Vertical { get; set; } = 0;

    /// <summary>
    /// 驾驶水平输入值
    /// </summary>
    public static float DriveHorizontal { get; set; } = 0;

    /// <summary>
    /// 驾驶竖直输入值
    /// </summary>
    public static float DriveVertical { get; set; } = 0;

    /// <summary>
    /// 车辆启动输入值
    /// </summary>
    public static bool QiDongValue { get; set; } = false;

    /// <summary>
    /// 离合输入值
    /// </summary>
    public static float ClutchValue { get; set; } = 0;

    /// <summary>
    /// 挡位
    /// </summary>
    public static int ShiftLevel { get; set; } = 0;

    /// <summary>
    /// 手刹
    /// </summary>
    public static float HandBrakeValue { get; set; } = 0;

    /// <summary>
    /// 左转向
    /// </summary>
    public static bool LeftBlinkerValue { get; set; } = false;

    /// <summary>
    /// 右转向
    /// </summary>
    public static bool RightBlinkerValue { get; set; } = false;

    /// <summary>
    /// 喇叭
    /// </summary>
    public static bool HornValue { get; set; } = false;
}
