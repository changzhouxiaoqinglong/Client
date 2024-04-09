using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QstInitJudgePoison : SelectQuestionBase
{
    /// <summary>
    /// 显色图片
    /// </summary>
    private Image craterImage;

    /// <summary>
    /// 显色图片
    /// </summary>
    private Image bigCraterImage;

    /// <summary>
    /// 显色图片按钮组件
    /// </summary>
    private ButtonBase craterBtn;

    /// <summary>
    /// 大显色图片按钮组件
    /// </summary>
    private ButtonBase bigCraterBtn;

    /// <summary>
    /// 检测类型
    /// </summary>
    private QstPoisonParam param;

    protected override void Awake()
    {
        base.Awake();
        craterImage = transform.Find("CraterImage").GetComponent<Image>();
        bigCraterImage = transform.Find("BigCraterImage").GetComponent<Image>();
        craterBtn = transform.Find("CraterImage").GetComponent<ButtonBase>();
        craterBtn.RegistClick(BigCraterImageShow);
        bigCraterBtn = transform.Find("BigCraterImage").GetComponent<ButtonBase>();
        bigCraterBtn.RegistClick(BigCraterImageHide);
    }

    protected override void Start()
    {
        base.Start();
        param = JsonTool.ToObject<QstPoisonParam>(qstResult.Param);
        InitImage();
        InitDrugPoisonLog();
    }

    /// <summary>
    /// 大显色图片显示
    /// </summary>
    private void BigCraterImageShow(GameObject obj)
    {
        bigCraterImage.gameObject.SetActive(true);
    }

    /// <summary>
    /// 大显色图片隐藏
    /// </summary>
    private void BigCraterImageHide(GameObject obj)
    {
        bigCraterImage.gameObject.SetActive(false);
    }

    /// <summary>
    /// 初始化图片
    /// </summary>
    private void InitImage()
    {
        print("初始化图片"+qstConfig.TargetId);
        ExPoisonData data = ExPoisonDataMgr.GetInstance().GetDataById(qstConfig.TargetId);
        string path = data.GetPathByCheckType(param.CheckType);
        craterImage.sprite = Resources.Load<Sprite>(path);
        bigCraterImage.sprite = Resources.Load<Sprite>(path);
    }

    private void InitDrugPoisonLog()
    {
        (SceneMgr.GetInstance().curScene as TrainSceneCtrBase).virtualCar.SetQstDrugPoisonLog(SetQstDrugPoisonLog(param.pos, param.CheckType, qstConfig.TargetId));
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
