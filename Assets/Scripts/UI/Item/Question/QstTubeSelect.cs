using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QstTubeSelect : SelectQuestionBase
{
    public int tube;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    public override bool QuestionJudge()
    {
        bool isCorrect = base.QuestionJudge();
        (SceneMgr.GetInstance().curScene as TrainSceneCtrBase).virtualCar.TubeCountAdd();
        if(qstData.Id == QuestionConstant.SELECTTUBE)
        {
            List<int> list = GetSelectAnswer();
            tube = list[0];
        }
        return isCorrect;
    }
}
