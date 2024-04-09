using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QstExamineMeathed : SelectQuestionBase
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
    /// 特殊处理跳转抽气面板
    /// </summary>
    public override bool QuestionJudge()
    {
        bool isCorrect = false;
        QstPoisonParam qstpoisonParam = JsonTool.ToObject<QstPoisonParam>(qstResult.Param);
        if(qstData.Id == QuestionConstant.EXAMINEID)
        {
            if (qstpoisonParam.DrugType == QstPoisonDrugType.OUT_CAR_DRUG)
            {
                List<int> list = new List<int>();
                if(qstConfig.TargetId == PoisonType.VX_POISON)
                    list.Add(3);
                else
                    list.Add(4);
                isCorrect = base.QuestionJudge(list);
                QuestionView questionView = (UIMgr.GetInstance().GetViewByType(ViewType.QuestionView) as QuestionView);
                int curTube = SelectTube(questionView.qstList);
                questionView.tubeType = curTube;
                questionView.meathedIsCorrect = isCorrect;
            }
            else
            {
                isCorrect = base.QuestionJudge();
                OpenDetPoisonBleedView();

            }
        }
        return isCorrect;
    }

    private void OpenDetPoisonBleedView()
    {
        DetPoisonBleedView detPoisonBleedView = (UIMgr.GetInstance().OpenView(ViewType.DetPoisonBleedView) as DetPoisonBleedView);
        QuestionView questionView = (UIMgr.GetInstance().GetViewByType(ViewType.QuestionView) as QuestionView);
        QstRequestResult curQstRequstResult = questionView.curQstRequstResult;
        int curTube = SelectTube(questionView.qstList);
        QstPoisonColorParam qstPoisonColorParam = new QstPoisonColorParam()
        {
            qstResult = curQstRequstResult,
            tubeType = curTube,
        };
        detPoisonBleedView.poisonColorParam = qstPoisonColorParam;
        UIMgr.GetInstance().CloseView(ViewType.QuestionView);
    }
    private int SelectTube(List<QuestionBase> list)
    {
        int curTube = -1;
        foreach (var item in list)
        {
            QstTubeSelect temp = (item as QstTubeSelect);
            if (temp != null)
            {
                curTube = temp.tube;
                Debug.Log(curTube);
                break;
            }
        }
        return curTube;
    }

}
