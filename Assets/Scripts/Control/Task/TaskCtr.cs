
/// <summary>
/// 任务控制
/// </summary>
public class TaskCtr
{
    private ExTaskData taskData;

    /// <summary>
    /// 训练日志处理
    /// </summary>
    private TrainLogHandle trainLogHandle { get; set; }

    /// <summary>
    /// 训练流程控制
    /// </summary>
    public PracticeProcessCtr practiceProcessCtr { get; set; }

    /// <summary>
    /// 时间管理
    /// </summary>
    public TrainDateMgr trainDateMgr = new TrainDateMgr();

    public TaskCtr(ExTaskData taskData)
    {
        this.taskData = taskData;
        trainLogHandle = new TrainLogHandle();
        practiceProcessCtr = new PracticeProcessCtr();
        //初始化训练流程
        practiceProcessCtr.InitProcess(taskData.Id);
    }

    public void Update()
    {
        trainDateMgr.Update();
    }

    public string GetTrainLog()
    {
        return trainLogHandle.Log.ToString();
    }

    /// <summary>
    /// 结束
    /// </summary>        
    public virtual void EndCtr()
    {
        trainLogHandle.End();
        practiceProcessCtr.End();
    }
}
