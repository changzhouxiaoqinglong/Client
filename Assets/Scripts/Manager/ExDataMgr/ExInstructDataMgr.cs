
/// <summary>
/// 车长指令数据管理
/// </summary>
public class ExInstructDataMgr : ExDataMgrBase<ExInstructDataMgr, ExInstructData>
{
    /// <summary>
    /// 表名
    /// </summary>
    protected override string FILE_NAME { get; set; } = "InstructData";
}
