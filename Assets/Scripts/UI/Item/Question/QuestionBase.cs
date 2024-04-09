using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBase : MonoBehaviour
{
    /// <summary>
    /// 标题
    /// </summary>
    public string title;

    /// <summary>
    /// 题目
    /// </summary>
    public string topic;

    /// <summary>
    /// 题目对象
    /// </summary>
    public ExQuestionData qstData;

    /// <summary>
    /// 题目答案
    /// </summary>
    public ExQuestionConfig qstConfig;

    /// <summary>
    /// 题目请求结果数据
    /// </summary>
    public QstRequestResult qstResult;


    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
    }

    public void SetQstDataAndConfig(ExQuestionData data,ExQuestionConfig config,QstRequestResult result)
    {
        qstData = data;
        qstConfig = config;
        qstResult = result;
    }

    public virtual bool QuestionJudge()
    {
        return false;
    }
    public virtual List<int> GetSelectAnswer()
    {
        return null;
    }

}
