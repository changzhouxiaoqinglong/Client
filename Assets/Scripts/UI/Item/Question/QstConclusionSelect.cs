using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QstConclusionSelect : SelectQuestionBase
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
        
        return true;

    }
}
