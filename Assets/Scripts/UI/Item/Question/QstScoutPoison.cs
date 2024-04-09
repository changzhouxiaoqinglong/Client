using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QstScoutPoison : SelectQuestionBase
{

    private QstPoisonParam param;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        param = JsonTool.ToObject<QstPoisonParam>(qstResult.Param);
        InitDrugPoisonLog();
    }

    private void InitDrugPoisonLog()
    {
        (SceneMgr.GetInstance().curScene as TrainSceneCtrBase).virtualCar.SetQstDrugPoisonLog(SetQstDrugPoisonLog(param.pos,param.CheckType,qstConfig.TargetId));
    }

    /// <summary>
    /// 设置日志数据
    /// </summary>
    private QstDrugPoisonLog SetQstDrugPoisonLog(CustVect2 pos, int checkType, int poisonType)
    {
        QstDrugPoisonLog tempLog = new QstDrugPoisonLog();
        tempLog.GisPos = pos;
        tempLog.CheckType = checkType;
        tempLog.PoisonType = poisonType;
        tempLog.TubeCount = 0;
        return tempLog;
    }

}
