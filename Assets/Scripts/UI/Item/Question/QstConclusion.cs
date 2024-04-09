using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QstConclusion : SelectQuestionBase
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// 不需要判断对错 只需要回去答题
    /// </summary>
    public override bool QuestionJudge()
    {
        if(qstData.Id == QuestionConstant.CONCLUSIONID)
        {
            List<int> answers = GetSelectAnswer();
            if (answers.Contains(QuestionConstant.OPTIONA))
            {
                QuestionView questionView = (UIMgr.GetInstance().GetViewByType(ViewType.QuestionView) as QuestionView);
                questionView.InitJumpId = QuestionConstant.SELECTTUBE;
            }
        }
        return true;

    }
}
