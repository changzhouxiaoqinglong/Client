/// <summary>
/// 日志配置
/// </summary>

public class LogConfig : ConfigBase<LogConfig>
{
    /// <summary>
    /// FSM日志开关
    /// </summary>
    public static bool LOG_FSM;

    public static void InitConfig()
    {
        ParseConfigByReflection("LogConfig.cfg");
    }
}
