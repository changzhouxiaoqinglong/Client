/// <summary>
/// 配置
/// </summary>
public class AppConfig : ConfigBase<AppConfig>
{
    /// <summary>
    /// 机号
    /// </summary>
    public static int MACHINE_ID;

    /// <summary>
    /// 席位号
    /// </summary>
    public static int SEAT_ID;

    /// <summary>
    /// 车型
    /// </summary>
    public static int CAR_ID;

    public static void InitConfig()
    {
        ParseConfigByReflection("AppConfig.cfg");
    }
}
