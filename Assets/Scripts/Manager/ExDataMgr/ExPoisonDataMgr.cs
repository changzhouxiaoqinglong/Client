
/// <summary>
/// 毒数据管理
/// </summary>
public class ExPoisonDataMgr : ExDataMgrBase<ExPoisonDataMgr, ExPoisonData>
{
    /// <summary>
    /// 表名
    /// </summary>
    protected override string FILE_NAME { get; set; } = "PoisonData";
}
