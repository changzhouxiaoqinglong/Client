
using System.Collections.Generic;
/// <summary>
/// 题目配置管理
/// </summary>
public class ExQuestionConfigMgr : ExDataMgrBase<ExQuestionConfigMgr, ExQuestionConfig>
{
    /// <summary>
    /// 表名
    /// </summary>
    protected override string FILE_NAME { get; set; } = "QuestionConfig";


    public List<ExQuestionConfig> GetTriggerData(List<int> exdata, int targetId)
    {
        List<ExQuestionConfig> data = new List<ExQuestionConfig>();
        foreach (var item in dataList)
        {
            if(item.TargetId == targetId && exdata.Contains(item.QstId) && item.CarId == AppConfig.CAR_ID)
            {
                data.Add(item);
            }
        }
        return data;
    }

}
