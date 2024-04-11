
using System.Collections.Generic;

/// <summary>
/// 训练流程基类
/// </summary>
public class PracticeProcessBase
{
    /// <summary>
    /// 当前所在流程索引
    /// </summary>
    protected int curIndex = 0;

    /// <summary>
    /// 当前训练所有流程,PracticeProcessDatan表里的 比如2011的9个
    /// </summary>
    public List<ExPracticeProcess> processList = new List<ExPracticeProcess>();

    public virtual void Init(int taskId)
    {
        processList = ExPracticeProcessMgr.GetInstance().GetProcessByTaskId(taskId);
    }

    /// <summary>
    /// 获得当前步骤提示语
    /// </summary>
    public string GetCurTip()
    {
        if (curIndex >= 0 && curIndex < processList.Count)
        {
            return processList[curIndex].Tip;
        }
        else
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// 操作了流程
    /// precessid就是PracticeProcessData表里的id
    /// </summary>
    protected virtual void DoProcess(int processId)
    {      
        int index = GetProcessIndex(processId);//得到当前taskid对应的训练流程里，
                                               //id等于processId的那个
                                               
        
        Logger.LogWarning("操作了流程:  " + processId+ "  index: " + index);
        if (index >= 0)
        {
            if (index == curIndex)
            {
                //是当前操作，跳到下一步
                JumpToNext();
            }
            else if (index < curIndex)
            {
                //前面的步骤不处理
            }
            else
            {
                //后面的步骤 说明操作错误了 提示错误
                string errorTip = processList[index].GetErrorTip();
                if (!errorTip.IsNullOrEmpty())
                {
                    EventDispatcher.GetInstance().DispatchEvent(EventNameList.PRACTICE_PROCESS_ERROR_TIP, new StringEvParam(errorTip));
                }
            }
        }
    }

    protected virtual void JumpToNext()
    {
        curIndex++;
        if (curIndex < processList.Count)
        {
            //更新提示
            EventDispatcher.GetInstance().DispatchEvent(EventNameList.PRACTICE_PROCESS_TIP, new StringEvParam(processList[curIndex].Tip, curIndex));
        }
        else
        {
            //完成
            EventDispatcher.GetInstance().DispatchEvent(EventNameList.PRACTICE_PROCESS_TIP, new StringEvParam("全部完成!", curIndex));
        }
        EventDispatcher.GetInstance().DispatchEvent(EventNameList.CLEAR_ERROR_PROCESS_TIP);
    }

    /// <summary>
    /// 通过流程id 获取流程索引
    /// </summary>
    protected int GetProcessIndex(int processId)
    {
        for (int i = 0; i < processList.Count; i++)
        {
            if (processList[i].Id == processId)
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// 是否完成
    /// </summary>
    protected bool IsFinish()
    {
        return curIndex >= processList.Count;
    }

    public virtual void End()
    {

    }
}
