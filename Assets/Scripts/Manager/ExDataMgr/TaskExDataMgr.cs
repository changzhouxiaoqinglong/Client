/// <summary>
/// 任务表数据管理
/// </summary>
public class TaskExDataMgr : ExDataMgrBase<TaskExDataMgr, ExTaskData>
{
    /// <summary>
    /// 表名
    /// </summary>
    protected override string FILE_NAME { get; set; } = "TaskData";

    public TaskExDataMgr() : base()
    {

    }

    /// <summary>
    /// 是否有训练流程控制提示
    /// </summary>
/*    public bool IsHaveProcessCtrTip(int taskId)
    {
        return taskId == ExTaskId.BASE_POISON_ALARM_02B || taskId == ExTaskId.BASE_RADIOMETE_02B;
    }*/
}
