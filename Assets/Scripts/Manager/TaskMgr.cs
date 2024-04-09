
using System.Collections.Generic;
/// <summary>
/// 任务管理
/// </summary>
public class TaskMgr : MonoSingleTon<TaskMgr>
{
    /// <summary>
    /// 当前任务
    /// </summary>
    public ExTaskData curTaskData;

    /// <summary>
    /// 当前任务控制
    /// </summary>
    public TaskCtr curTaskCtr;

    /// <summary>
    /// 是否在任务中
    /// </summary>
    public bool isInTask = false;

    /// <summary>
    /// 开始任务
    /// </summary>
    public void StartTask(ExTaskData taskData)
    {
        curTaskData = taskData;
        curTaskCtr = new TaskCtr(taskData);
        isInTask = true;
    }

    private void Update()
    {
        if (curTaskCtr != null)
        {
            curTaskCtr.Update();
        }
    }

    /// <summary>
    /// 上报任务结束
    /// </summary>
    public void ResportEndTask()
    {
        if (isInTask)
        {
            //转发对象 (基本操作 不用转发，每个人单独结束)
            List<ForwardModel> forwardModels = new List<ForwardModel>();
            if (curTaskData.Type == TaskType.Base)
            {
                //基本训练结束要转发给设备
                forwardModels = NetManager.GetInstance().CurDeviceForward;
            }
            else
            {
                forwardModels = NetManager.GetInstance().SameMachineAllSeats;
            }
            //结束训练
            EndModel endModel = new EndModel(curTaskData.Id);
            NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(endModel), NetProtocolCode.END, forwardModels);
            DoEndTask();
        }
    }

    /// <summary>
    /// 任务结束
    /// </summary>
    public void DoEndTask()
    {
        if (!isInTask)
        {
            return;
        }
        isInTask = false;
        if (curTaskCtr != null)
        {
            curTaskCtr.EndCtr();
        }
        UIMgr.GetInstance().OpenView(ViewType.EndView);
        NetVarDataMgr.GetInstance()._NetVarData._TaskEnvVarData = null;

        //结束录制
        Record.GetInstance().StopCapture();
    }
}
