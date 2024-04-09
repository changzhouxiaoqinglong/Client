
using System.Collections.Generic;
/// <summary>
/// 题目数据管理
/// </summary>
public class ExQuestionDataMgr : ExDataMgrBase<ExQuestionDataMgr, ExQuestionData>
{
    /// <summary>
    /// 表名
    /// </summary>
    protected override string FILE_NAME { get; set; } = "QuestionData";

    /// <summary>
    /// 获取题目id
    /// </summary>
    /// <param name="triggerType"></param>
    /// <returns></returns>
    public List<int> GetQstId(int triggerType)
    {
        List<int> data = new List<int>();
        foreach(var item in dataList)
        {
            if(item.TriggerType == triggerType)
            {
                data.Add(item.Id);
            }
        }   
        return data;
    }
    
    /// <summary>
    /// 获取题目列表
    /// </summary>
    /// <param name="triggerType"></param>
    /// <returns></returns>
    public List<ExQuestionData> GetQuestionList(int triggerType)
    {
        List<ExQuestionData> data = new List<ExQuestionData>();
        foreach (var item in dataList)
        {
            if (item.TriggerType == triggerType)
            {
                data.Add(item);
            }
        }
        return data;
    }

    public ExQuestionData GetQuestionItem(int qstId)
    {
        ExQuestionData data = new ExQuestionData();
        foreach (var item in dataList)
        {
            if(item.Id == qstId)
            {
                return item;
            }
        }
        return null;
    }

}
