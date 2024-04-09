public class ExInstructId
{
    /// <summary>
    /// 侦毒ID
    /// </summary>
    public const int DrugId = 9;
}

public class ExTriggerType
{
    /// <summary>
    /// 初步判毒的触发类型
    /// </summary>
    public const int InitJudgePoison = 1;

    /// <summary>
    /// 清洁触发类型
    /// </summary>
    public const int CleanPoison = 2;

    /// <summary>
    /// 采样触发类型
    /// </summary>
    public const int SamplingPoison = 3;
}



/// <summary>
/// 车长指令 数据
/// </summary>
public class ExInstructData : ExDataBase
{
    /// <summary>
    /// 指令名
    /// </summary>
    public string Name;
}
