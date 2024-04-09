using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectQuestionBase : QuestionBase
{
    /// <summary>
    /// 标题文本
    /// </summary>
    private Text titleText;

    /// <summary>
    /// 题目文本
    /// </summary>
    private Text topicText;

    /// <summary>
    /// 单选数据
    /// </summary>
    private Toggle[] toggles;

    

    protected override void Awake()
    {
        base.Awake();
        titleText = transform.Find("QstTitle").GetComponent<Text>();
        topicText = transform.Find("QstTopic").GetComponent<Text>();
        toggles = transform.Find("Options").GetComponentsInChildren<Toggle>();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        InitQuestion();
    }

    private void InitQuestion()
    {
        titleText.text = qstData.Title;
        topicText.text = qstData.Question;
    }

    /// <summary>
    /// 判断当前题目对错
    /// </summary>
    public override bool QuestionJudge()
    {
        List<int> correctAnswers = qstConfig.GetAnswerList();
        List<int> userAnswers = GetSelectAnswer();
        int count = 0;
        bool IsCorrect = false;
        if (correctAnswers.Count == userAnswers.Count)
        {
            foreach (var item in userAnswers)
            {
                if(correctAnswers.Contains(item))
                {
                    count++;
                    continue;
                }
            }
            if (count == correctAnswers.Count)
            {
                IsCorrect = true;
            }
            QstReport qst = new QstReport()
            {
                QuestionId = qstConfig.QstId,
                IsCorrect = IsCorrect,
            };
            NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(qst), NetProtocolCode.QUESTION_REPORT, NetManager.GetInstance().SameMachineSeatsExDevice);
            EventDispatcher.GetInstance().DispatchEvent(EventNameList.ADD_TASK_LOG, new StringEvParam(qst.GetTaskLog(AppConfig.SEAT_ID)));
        }
        return IsCorrect;
    }

    public bool QuestionJudge(List<int> correctAnswer)
    {
        List<int> correctAnswers = correctAnswer;
        List<int> userAnswers = GetSelectAnswer();
        int count = 0;
        bool IsCorrect = false;
        if (correctAnswers.Count == userAnswers.Count)
        {
            foreach (var item in userAnswers)
            {
                if (correctAnswers.Contains(item))
                {
                    count++;
                    continue;
                }
            }
            if (count == correctAnswers.Count)
            {
                IsCorrect = true;
            }
            QstReport qst = new QstReport()
            {
                QuestionId = qstConfig.QstId,
                IsCorrect = IsCorrect,
            };
            NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(qst), NetProtocolCode.QUESTION_REPORT, NetManager.GetInstance().SameMachineSeatsExDevice);
            EventDispatcher.GetInstance().DispatchEvent(EventNameList.ADD_TASK_LOG, new StringEvParam(qst.GetTaskLog(AppConfig.SEAT_ID)));
        }
        return IsCorrect;
    }

    public override List<int> GetSelectAnswer()
    {
        List<int> answers = new List<int>();
        for(int i=0;i<toggles.Length;i++)
        {
            if(toggles[i].isOn)
            {
                answers.Add(i + 1);
            }
        }
        return answers;
    }



}
