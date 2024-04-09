
using System.Collections.Generic;
/// <summary>
/// 题目配置 数据
/// </summary>
public class ExQuestionConfig : ExDataBase
{
    /// <summary>
    /// 题目id 对应ExQuestionData
    /// </summary>
    public int QstId;

    /// <summary>
    /// 目标id（毒 就是毒id）
    /// </summary>
    public int TargetId;

    /// <summary>
    /// 答案
    /// </summary>
    public string Answer;

    /// <summary>
    /// 车辆Id
    /// </summary>
    public int CarId;

    public List<int> GetAnswerList()
    {
        List<int> answerList = new List<int>();
        string[] answerStr = Answer.Split(',');
        for(int i = 0; i < answerStr.Length; i++)
        {
            answerList.Add(int.Parse(answerStr[i]));
        }
        return answerList;
    }
}
