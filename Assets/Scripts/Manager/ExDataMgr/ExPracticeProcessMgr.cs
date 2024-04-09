
using System.Collections.Generic;
using System.Text;
/// <summary>
/// 训练流程数据管理
/// </summary>
public class ExPracticeProcessMgr : ExDataMgrBase<ExPracticeProcessMgr, ExPracticeProcess>
{
    /// <summary>
    /// 表名
    /// </summary>
    protected override string FILE_NAME { get; set; } = "PracticeProcessData";

    /// <summary>
    /// 根据训练id 获得对应的流程
    /// </summary>
    public List<ExPracticeProcess> GetProcessByTaskId(int taskId)
    {
        List<ExPracticeProcess> res = new List<ExPracticeProcess>();
        for (int i = 0; i < dataList.Count; i++)
        {
            if (dataList[i].TaskId == taskId)
            {
                res.Add(dataList[i]);
            }
        }
        return res;
    }

    /// <summary>
    /// 获得对应训练  训练流程总描述
    /// </summary>    
    public string GetProcessDesc(int taskId)
    {
        StringBuilder sb = new StringBuilder();
        List<ExPracticeProcess> list = GetProcessByTaskId(taskId);
        if (list.Count > 0)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (i != 0)
                {
                    sb.Append("\n");
                }
                sb.Append((i + 1) + ".").Append(list[i].Tip);
            }
            return sb.ToString();
        }
        else
        {
            return string.Empty;
        }
    }
}
