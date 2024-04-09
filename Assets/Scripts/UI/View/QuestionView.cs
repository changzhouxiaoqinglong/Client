using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionView : ViewBase<QuestionViewModel>
{
    /// <summary>
    /// 题目位置
    /// </summary>
    private Transform questionItem;

    /// <summary>
    /// 下一题
    /// </summary>
    private ButtonBase nextQueBtn;

    /// <summary>
    /// 存放题目数组
    /// </summary>
    public List<QuestionBase> qstList = new List<QuestionBase>();

    /// <summary>
    /// 题目配置
    /// </summary>
    private List<ExQuestionData> qstDataList = new List<ExQuestionData>();

    /// <summary>
    /// 题目答案配置
    /// </summary>
    private List<ExQuestionConfig> configList = new List<ExQuestionConfig>();

    /// <summary>
    /// 当前题目列表
    /// </summary>
    public QstRequestResult curQstRequstResult = null;

    /// <summary>
    /// 当前题目索引
    /// </summary>
    private int curIndex = -1;

    /// <summary>
    /// 管子类型
    /// </summary>
    public int tubeType = -1;

    /// <summary>
    /// 测量法是否正确
    /// </summary>
    public bool meathedIsCorrect = false;

    /// <summary>
    /// 场景音频播放器
    /// </summary>
    private AudioSource audioSource;

    public AudioClip[] audioClips;

    /// <summary>
    /// 初始要跳的题目id
    /// </summary>
    public int InitJumpId
    {
        get; set;
    } = -1;

    public Action EndCallBack { get; set; }


    protected override void Awake()
    {
        base.Awake();
        questionItem = transform.Find("QuestionItem");
        nextQueBtn = transform.Find("NextQueBtn").GetComponent<ButtonBase>();
        nextQueBtn.RegistClick(OnclickNextQueBtn);
        audioSource = GetComponent<AudioSource>();
    }


    protected override void Start()
    {
        base.Start();
        InitQuestion();
        QstJump();
        NextQueBtn();
    }

    /// <summary>
    /// 点击下一题按钮
    /// </summary>
    /// <param name="obj"></param>
    private void OnclickNextQueBtn(GameObject obj)
    {
        int checkType = NetVarDataMgr.GetInstance()._NetVarData._TaskEnvVarData.CheckType;
        //考试模式
        if (checkType == CheckTypeConst.EXAMINE)
        {
            ExamModeQuestion();
        }
        //训练模式
        else if(checkType == CheckTypeConst.PRACTICE)
        {
            TrainModeQuestion();
        }
    }

    /// <summary>
    /// 训练模式答题
    /// </summary>
    private void TrainModeQuestion()
    {
        if (curIndex >= 0 && curIndex < qstList.Count)
        {
            if (qstList[curIndex].QuestionJudge())//如果换管检测 index就跳到真毒管选择，否则就关掉答题
            {
                PlayAudio(QuestionConstant.CORRECTAUDIO);
                qstList[curIndex].gameObject.SetActive(false);
                QstJump();
                NextQueBtn();
            }
            else
            {
                PlayAudio(QuestionConstant.ERRORAUDIO);
                UIMgr.GetInstance().ShowToast("答案选择错误");
            }
        }
        
    }

    /// <summary>
    /// 播放音频(0正确，1错误)
    /// </summary>
    private void PlayAudio(int index)
    {
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }

    /// <summary>
    /// 考试模式答题
    /// </summary>
    private void ExamModeQuestion()
    {
        if (curIndex >= 0 && curIndex < qstList.Count)
        {
            qstList[curIndex].QuestionJudge();
            qstList[curIndex].gameObject.SetActive(false);
        }
        QstJump();
        NextQueBtn();
    }


    /// <summary>
    /// 初始化题目
    /// </summary>
    private void InitQuestion()
    {
        print("InitQuestion");
        QstRequestResult qstResult = curQstRequstResult;
        print(qstResult.TargetId);
        List<int> qstid = ExQuestionDataMgr.GetInstance().GetQstId(qstResult.TriggerType);//获取QuestionData里的此TriggerType的item的id
        qstDataList = ExQuestionDataMgr.GetInstance().GetQuestionList(qstResult.TriggerType);
        configList = ExQuestionConfigMgr.GetInstance().GetTriggerData(qstid, qstResult.TargetId);
        for (int i = 0; i < configList.Count; i++)
        {
            string content = qstDataList[i].ContentType;
            GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Item/Question/" + content), questionItem);
            obj.GetComponent<QuestionBase>().SetQstDataAndConfig(qstDataList[i], configList[i],qstResult);//题目 答案 请求答题结果
            qstList.Add(obj.GetComponent<QuestionBase>());
            obj.SetActive(false);
        }
    }

    /// <summary>
    /// 跳转题目
    /// </summary>
    private void QstJump()
    {
        if (InitJumpId == -1) return;
        curIndex = InitJumpId - 2;  
        InitJumpId = -1;
    }

    /// <summary>
    /// 下一题事件
    /// </summary>
    private void NextQueBtn()
    {
        curIndex++;
        if(curIndex == qstList.Count-1)
        {
            nextQueBtn.transform.Find("Text").GetComponent<Text>().text = "确定";
        }
        if (curIndex >= qstList.Count)
        {
            nextQueBtn.transform.Find("Text").GetComponent<Text>().text = "下一题";
            UIMgr.GetInstance().CloseView(ViewType.QuestionView);
            if(qstList.Count > 1 && AppConfig.CAR_ID == CarIdConstant.ID_02B)
                SendMsgToINVEST2();
        }
        else
        {
            qstList[curIndex].gameObject.SetActive(true);
        }
    }

    /// <summary>
    ///  设置当前题目种类
    /// </summary>
    /// <param name="qst"></param>
    public void SetQstRequestResult(QstRequestResult qst)
    {
        curQstRequstResult = qst;
        print("SetQstRequestResult");
    }

    /// <summary>
    /// 上传结果给侦查员2
    /// </summary>
    private void SendMsgToINVEST2()
    {
        QstDrugPoisonLog qstRequestLog = (SceneMgr.GetInstance().curScene as TrainSceneCtrBase).virtualCar.GetQstDrugPoisonLog();
        Debug.Log(qstRequestLog);
        string str = qstRequestLog.ReportQstLogStr(qstRequestLog.CheckType,qstRequestLog.PoisonType,qstRequestLog.UserSelectPoison);
        DetectResParam detectModel = new DetectResParam(DetectResType.Poison, str);
        List<ForwardModel> forwardModels = new ForwardModelsBuilder()
                        .Append(AppConfig.MACHINE_ID, SeatType.INVEST2)
                        .Build();
        NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(detectModel), NetProtocolCode.SEND_DETCT_RES_TO_SEAT, forwardModels);
    }

}
