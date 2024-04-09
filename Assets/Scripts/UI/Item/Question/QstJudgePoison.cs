
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QstJudgePoison : SelectQuestionBase
{
    /// <summary>
    /// 显色图片
    /// </summary>
    private Image poisonImage;

    /// <summary>
    /// 显色大图片
    /// </summary>
    private Image bigPoisonImage;

    /// <summary>
    /// 显色图片按钮组件
    /// </summary>
    private ButtonBase poisonButton;

    /// <summary>
    /// 显色大图片按钮组件
    /// </summary>
    private ButtonBase bigPoisonButton;

    private QstPoisonParam param;

    protected override void Awake()
    {
        base.Awake();
        poisonImage = transform.Find("PoisonImage").GetComponent<Image>();
        bigPoisonImage = transform.Find("BigPoisonImage").GetComponent<Image>();
        poisonButton = transform.Find("PoisonImage").GetComponent<ButtonBase>();
        poisonButton.RegistClick(BigPoisonImageShow);
        bigPoisonButton = transform.Find("BigPoisonImage").GetComponent<ButtonBase>();
        bigPoisonButton.RegistClick(BigPoisonImageHide);
    }

    /// <summary>
    /// 大显色图片显示
    /// </summary>
    private void BigPoisonImageShow(GameObject obj)
    {
        bigPoisonImage.gameObject.SetActive(true);
    }

    /// <summary>
    /// 大显色图片隐藏
    /// </summary>
    private void BigPoisonImageHide(GameObject obj)
    {
        bigPoisonImage.gameObject.SetActive(false);
    }

    protected void OnEnable()
    {
        this.DelayInvoke(0, () =>
        {
            InitImage();
        });
    }

    protected override void Start()
    {
        base.Start();
        
        param = JsonTool.ToObject<QstPoisonParam>(qstResult.Param);
    }

    /// <summary>
    /// 初始化图片
    /// </summary>
    private void InitImage()
    {
        QuestionView questionview = (UIMgr.GetInstance().GetViewByType(ViewType.QuestionView) as QuestionView);
        if (questionview == null) return;
        string path;
        if (param.DrugType == QstPoisonDrugType.OUT_CAR_DRUG)
        {
            if(questionview.meathedIsCorrect)
            {
                path = ExTubePoisonCheckMgr.GetInstance().GetTubePoisonCheck(questionview.tubeType, qstConfig.TargetId, param.DegreeLow, param.CheckType);
            }
            else
            {
                path = ExTubePoisonCheckMgr.GetInstance().GetTubePoisonCheck(questionview.tubeType, PoisonType.NO_POISON, DrugDegree.NONE, param.CheckType);
            }
        }
        else
        {
            bool isOk = (SceneMgr.GetInstance().curScene as TrainSceneCtrBase).virtualCar.GetDevice<VirtualCarDrugPoison02B>().isOk;
            if (isOk)
            {
                path = ExTubePoisonCheckMgr.GetInstance().GetTubePoisonCheck(questionview.tubeType, qstConfig.TargetId, param.DegreeLow, param.CheckType);
            }
            else
            {
                path = ExTubePoisonCheckMgr.GetInstance().GetTubePoisonCheck(questionview.tubeType, PoisonType.NO_POISON, DrugDegree.NONE, param.CheckType);
            }
        }
        poisonImage.sprite = Resources.Load<Sprite>(path);
        bigPoisonImage.sprite = Resources.Load<Sprite>(path);
    }

    public override bool QuestionJudge()
    {
        bool isCorrect = base.QuestionJudge();
        
        if (qstData.Id == QuestionConstant.JUDGEID)
        {
            List<int> list = GetSelectAnswer();
            (SceneMgr.GetInstance().curScene as TrainSceneCtrBase).virtualCar.SetQstDrugPoison(list[0]);
        }
        return isCorrect;
    }
}
