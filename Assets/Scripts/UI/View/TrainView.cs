
using DG.Tweening;
using Spore.DataBinding;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 训练界面
/// </summary>
public class TrainView : ViewBase<TrainViewModel>
{
    /// <summary>
    /// 渲染画面的rawImage
    /// </summary>
    private RawImage mainScreen;

    /// <summary>
    /// 接收同步画面 节点
    /// </summary>
    private Transform sysScreen;

    /// <summary>
    /// 结束按钮
    /// </summary>
    private ButtonBase endBtn;

    /// <summary>
    /// 左侧提示按钮
    /// </summary>
    private ButtonBase leftTipBtn;

    /// <summary>
    /// 右侧按钮框
    /// </summary>
    private ButtonBase rightCtlBtn;

    /// <summary>
    /// 底部信息框max
    /// </summary>
    private ButtonBase MaxInfoBtn;

    /// <summary>
    /// 底部信息框min
    /// </summary>
    private ButtonBase MinInfoBtn;

    /// <summary>
    /// 日志界面
    /// </summary>
    private GameObject messageView;

    /// <summary>
    /// 右侧按钮界面
    /// </summary>
    private GameObject rightCtlView;

    /// <summary>
    /// 表盘ui
    /// </summary>
    private GameObject gauges;

    /// <summary>
    /// 防护按钮
    /// </summary>
    private GameObject protectBtn;

    /// <summary>
    /// 取消防护按钮
    /// </summary>
    private GameObject cancelProtectBtn;

    /// <summary>
    /// 徒步侦察按钮
    /// </summary>
    private ButtonBase walkCheckBtn;

    /// <summary>
    /// 上车按钮
    /// </summary>
    private ButtonBase inCarBtn;

    /// <summary>
    /// 标志旗按钮
    /// </summary>
    private ButtonBase flagBtn;

    /// <summary>
    /// 指令按钮
    /// </summary>
   // private ButtonBase instructBtn;//新版修改 去掉

    /// <summary>
    /// 切换视角
    /// </summary>
    private ButtonBase changeViewBtn;

    /// <summary>
    /// 徒步检测辐射剂量率
    /// </summary>
    private Text outRadiomText;

    /// <summary>
    /// 小地图
    /// </summary>
    private GameObject miniMap;

    /// <summary>
    /// 点击地图按钮
    /// </summary>
    private ButtonBase maxMapBtn;

    /// <summary>
    /// 上报侦察结果按钮
    /// </summary>
    private ButtonBase reportDetectBtn;

    /// <summary>
    /// 侦毒按钮
    /// </summary>
    private ButtonBase drugPoisonBtn;

    /// <summary>
    /// 开关车门按钮
    /// </summary>
    private ButtonBase carDoorBtn;

    /// <summary>
    /// 消洗按钮
    /// </summary>
   // private ButtonBase cleanBtn;//新版修改 去掉

    /// <summary>
    /// 采样按钮
    /// </summary>
    private ButtonBase samplingBtn;

    /// <summary>
    /// 测量
    /// </summary>
    private ButtonBase measureBtn;

    /// <summary>
    /// 生物
    /// </summary>
    private ButtonBase BiologyBtn;

    /// <summary>
    /// 显示日志
    /// </summary>
    private bool isShow = true;

    /// <summary>
    /// 显示操作按钮
    /// </summary>
    private bool isShowCtl = true;

    /// <summary>
    /// 显示底部操作信息按钮
    /// </summary>
    private bool isMaxInfo = true;

    /// <summary>
    /// 人物是否在车上
    /// </summary>
    private bool isInCar = true;

    /// <summary>
    /// 当前任务检测类型下拉框
    /// </summary>
    private Dropdown dropdown ;

    protected override void Awake()
    {
        base.Awake();
        dropdown = transform.Find("右侧按钮框/Dropdown").GetComponent<Dropdown>();
        mainScreen = transform.Find("mainScreen").GetComponent<RawImage>();
        sysScreen = transform.Find("sysScreen");
        endBtn = transform.Find("endBtn").GetComponent<ButtonBase>();
        endBtn.RegistClick(OnClickEnd);
        leftTipBtn = transform.Find("MessageView/提示信息框/Btn").GetComponent<ButtonBase>();
        leftTipBtn.RegistClick(OnClickMessageBtn);
        rightCtlBtn = transform.Find("右侧按钮框/Btn").GetComponent<ButtonBase>();
        rightCtlBtn.RegistClick(OnClickCtlBtn);
        //MaxInfoBtn = transform.Find("MessageView/底部信息框/BtnMax").GetComponent<ButtonBase>();
        //MaxInfoBtn.RegistClick(OnClickMaxInfoBtn);
        //MinInfoBtn = transform.Find("MessageView/底部信息框/BtnMin").GetComponent<ButtonBase>();
        //MinInfoBtn.RegistClick(OnClickMinInfoBtn);
        messageView = transform.Find("MessageView/提示信息框").gameObject;
        rightCtlView = transform.Find("右侧按钮框").gameObject;
        gauges = transform.Find("Gauges").gameObject;
        protectBtn = transform.Find("右侧按钮框/VerticalLayout/protectBtn").gameObject;
        ButtonBase protect = protectBtn.GetComponent<ButtonBase>();
        protect.RegistClick(OnClickProtect);
        cancelProtectBtn = transform.Find("右侧按钮框/VerticalLayout/cancelProtect").gameObject;
        ButtonBase cancelProtect = cancelProtectBtn.GetComponent<ButtonBase>();
        cancelProtect.RegistClick(OnClickCancelProtect);
        walkCheckBtn = transform.Find("右侧按钮框/VerticalLayout/walkCheckBtn").GetComponent<ButtonBase>();
        walkCheckBtn.RegistClick(OnClickWalkCheck);
        inCarBtn = transform.Find("右侧按钮框/VerticalLayout/inCarBtn").GetComponent<ButtonBase>();
        inCarBtn.RegistClick(OnClickInCar);
        flagBtn = transform.Find("右侧按钮框/VerticalLayout/flagBtn").GetComponent<ButtonBase>();
        flagBtn.RegistClick(OnClickFlagBtn);
        outRadiomText = transform.Find("outRadiomRate").GetComponent<Text>();
       // instructBtn = transform.Find("instructBtn").GetComponent<ButtonBase>();
       // instructBtn.RegistClick(OnClickInstructBtn);
        changeViewBtn = transform.Find("changeViewBtn").GetComponent<ButtonBase>();
        changeViewBtn.RegistClick(OnClickChangeBtn);
        miniMap = transform.Find("MiniMap").gameObject;
        maxMapBtn = miniMap.transform.Find("maxMapBtn").GetComponent<ButtonBase>();
        maxMapBtn.RegistClick(OnClickMaxMapBtn);
        reportDetectBtn = transform.Find("右侧按钮框/VerticalLayout/reportDetectRes").GetComponent<ButtonBase>();
        reportDetectBtn.RegistClick(OnClickReportDetectBtn);
        drugPoisonBtn = transform.Find("右侧按钮框/VerticalLayout/DrugPoisonBtn").GetComponent<ButtonBase>();
        drugPoisonBtn.RegistClick(OnclickDrugPoisonBtn);


        BiologyBtn = transform.Find("右侧按钮框/VerticalLayout/BiologyBtn").GetComponent<ButtonBase>();
        BiologyBtn.RegistClick(OnclickBiologyBtn);

        carDoorBtn = transform.Find("右侧按钮框/VerticalLayout/OpenCarDoorBtn").GetComponent<ButtonBase>();
        carDoorBtn.RegistClick(OnClickOpenCarDoorBtn);
       // cleanBtn = transform.Find("CleanBtn/Btn").GetComponent<ButtonBase>();
       // cleanBtn.RegistClick(OnClickCleanPoisonBtn);
        samplingBtn = transform.Find("右侧按钮框/VerticalLayout/SamplingBtn").GetComponent<ButtonBase>();
        samplingBtn.RegistClick(OnClickSamplingPoisonBtn);
        measureBtn = transform.Find("右侧按钮框/VerticalLayout/measureBtn").GetComponent<ButtonBase>();
        measureBtn.RegistClick(OnclickMeasureBtn);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.OUT_CAR_RES, OutCarWalkRes);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.IN_CAR_RES, InCarRes);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.RESULT_QUESTION, OnGetQstResult);
        EventDispatcher.GetInstance().AddEventListener(EventNameList.HIDE_OUT_RADIOM_RATE, HideOutRadiomRate);
        EventDispatcher.GetInstance().AddEventListener(EventNameList.OUTCAR_RADIOMRATE, OutCarRadiomRate);
        EventDispatcher.GetInstance().AddEventListener(EventNameList.SYN_BASECAMERA_RENDER, SynBaseCameraRender);
    }

    protected override void Start()
    {
        base.Start();
        InitUi();
    }

    /// <summary>
    /// 初始化ui
    /// </summary>
    private void InitUi()
    {
        
        #region 新版修改
        //驾驶员
        gauges.SetActive(AppConfig.SEAT_ID == SeatType.DRIVE);
        changeViewBtn.gameObject.SetActive(AppConfig.SEAT_ID == SeatType.DRIVE);
        miniMap.SetActive(AppConfig.SEAT_ID == SeatType.DRIVE);
        //  carDoorBtn.gameObject.SetActive(AppConfig.SEAT_ID == SeatType.DRIVE);

        //合并
        transform.Find("右侧按钮框").gameObject.SetActive(AppConfig.SEAT_ID == SeatType.INVEST1);
        protectBtn.gameObject.SetActive(AppConfig.SEAT_ID == SeatType.INVEST1);
        //cancelProtectBtn.gameObject.SetActive(AppConfig.SEAT_ID == SeatType.INVEST1);
        carDoorBtn.gameObject.SetActive(AppConfig.SEAT_ID == SeatType.INVEST1);
        endBtn.transform.gameObject.SetActive(AppConfig.SEAT_ID == SeatType.INVEST1);
        //instructBtn.gameObject.SetActive(AppConfig.SEAT_ID == SeatType.INVEST1);
        walkCheckBtn.transform.gameObject.SetActive(AppConfig.SEAT_ID == SeatType.INVEST1);
        //inCarBtn.transform.gameObject.SetActive(AppConfig.SEAT_ID == SeatType.INVEST1);
        measureBtn.transform.gameObject.SetActive(AppConfig.SEAT_ID == SeatType.INVEST1);
       // cleanBtn.gameObject.SetActive(AppConfig.SEAT_ID == SeatType.INVEST1);
        samplingBtn.gameObject.SetActive(AppConfig.SEAT_ID == SeatType.INVEST1);
        flagBtn.transform.gameObject.SetActive(AppConfig.SEAT_ID == SeatType.INVEST1);
        reportDetectBtn.transform.gameObject.SetActive(AppConfig.SEAT_ID == SeatType.INVEST1);

        drugPoisonBtn.gameObject.SetActive(IsQstAnswer());
        measureBtn.transform.gameObject.SetActive(IsNuclearMeasure());
        BiologyBtn.transform.gameObject.SetActive(IsBiologyMeasure());
        if (TaskMgr.GetInstance().curTaskData.CheckType == HarmAreaType.DRUG)//毒
		{
            dropdown.options = new List<Dropdown.OptionData>() { new Dropdown.OptionData("化学侦察") };
		}
        else if (TaskMgr.GetInstance().curTaskData.CheckType == HarmAreaType.NUCLEAR)//辐射
        {
            dropdown.options = new List<Dropdown.OptionData>() { new Dropdown.OptionData("辐射侦察") };
        }
        else//生物
		{
            dropdown.options = new List<Dropdown.OptionData>() { new Dropdown.OptionData("化学侦察") };
        }

            #endregion

            /*
           #region old
           //驾驶员
           gauges.SetActive(AppConfig.SEAT_ID == SeatType.DRIVE);
           changeViewBtn.gameObject.SetActive(AppConfig.SEAT_ID == SeatType.DRIVE);
           miniMap.SetActive(AppConfig.SEAT_ID == SeatType.DRIVE);
           carDoorBtn.gameObject.SetActive(AppConfig.SEAT_ID == SeatType.DRIVE);

           //车长
           endBtn.transform.parent.gameObject.SetActive(AppConfig.SEAT_ID == SeatType.MASTER);
           instructBtn.gameObject.SetActive(AppConfig.SEAT_ID == SeatType.MASTER);

           //侦1
           walkCheckBtn.transform.parent.gameObject.SetActive(AppConfig.SEAT_ID == SeatType.INVEST1);
           inCarBtn.transform.parent.gameObject.SetActive(AppConfig.SEAT_ID == SeatType.INVEST1);
           measureBtn.transform.parent.gameObject.SetActive(AppConfig.SEAT_ID == SeatType.INVEST1);
           cleanBtn.gameObject.SetActive(AppConfig.SEAT_ID == SeatType.INVEST1);
           samplingBtn.gameObject.SetActive(AppConfig.SEAT_ID == SeatType.INVEST1);

           //侦2
           flagBtn.transform.parent.gameObject.SetActive(AppConfig.SEAT_ID == SeatType.INVEST2);
           reportDetectBtn.transform.parent.gameObject.SetActive(AppConfig.SEAT_ID == SeatType.INVEST2);

           //print(IsQstAnswer());
           //print(IsNuclearMeasure());
           drugPoisonBtn.gameObject.SetActive(IsQstAnswer());//化学才有侦毒，但是都能下车
           measureBtn.transform.parent.gameObject.SetActive(IsNuclearMeasure());//辐射才有测量，但是都能下车
           #endregion
            */
            //非驾驶位 需要接收同步画面
            if (AppConfig.SEAT_ID != SeatType.DRIVE)
       {
           //生成接收画面
           GameObject sysScreenPre = Resources.Load<GameObject>(AssetPath.FM_RECEIVE_SCREEN);
           Instantiate(sysScreenPre, sysScreen);
           print("生成接收画面");

            if(AppConfig.CAR_ID == CarIdConstant.ID_102)
			{
                //  FM_RECEIVE_YaoCeSCREEN
                GameObject yaoceScreenPre = Resources.Load<GameObject>(AssetPath.FM_RECEIVE_YaoCeSCREEN);
                Instantiate(yaoceScreenPre, sysScreen);
                print("生成遥测接收画面");
            }

           
        }
       else
       {
           InitMainScreen();
       }
   }

   private bool IsQstAnswer()
   {
       return AppConfig.SEAT_ID == SeatType.INVEST1 && TaskMgr.GetInstance().curTaskData.CheckType == HarmAreaType.DRUG;
   }

   private bool IsNuclearMeasure()
   {
       return AppConfig.SEAT_ID == SeatType.INVEST1 && TaskMgr.GetInstance().curTaskData.CheckType == HarmAreaType.NUCLEAR;
   }

    private bool IsBiologyMeasure()
    {
        return AppConfig.SEAT_ID == SeatType.INVEST1 && TaskMgr.GetInstance().curTaskData.CheckType == HarmAreaType.BIOLOGY;
    }

    #region 按钮展示逻辑
    /// <summary>
    /// 是否可以下车侦察
    /// </summary>
    /*    private bool IsCanWalkCheck()
       {
           //2号侦查员可以下车
           if (AppConfig.SEAT_ID == SeatType.INVEST1)
           {
               ExTaskData curTaskData = TaskMgr.GetInstance().curTaskData;
               ExCarData carData = CarExDataMgr.GetInstance().GetDataById(AppConfig.CAR_ID);
               //辐射任务  如果没有便携式辐射仪 就不下车
               if (curTaskData.CheckType == HarmAreaType.NUCLEAR && !carData.IsHaveDevice(DeviceType.EASY_RADIOMETER))
               {
                   return false;
               }
               return true;
           }
           else
           {
               return false;
           }
       }*/
    #endregion

    /// <summary>
    /// 同步base相机 开始渲染和处理同步画面
    /// </summary>
    private void SynBaseCameraRender(IEventParam param)
    {
        InitMainScreen();
    }

    private void InitMainScreen()
    {
        if (AppConfig.SEAT_ID == SeatType.DRIVE)
        {
            //驾驶位 需要通过rawimage获得同步相机渲染的画面
            Camera syncCamera = BaseCameraMgr.GetInstance().GetCameraByType(BaseCameraEnum.SyncBaseCamera).m_Camera;
            mainScreen.texture = syncCamera.targetTexture;
            mainScreen.gameObject.SetActive(mainScreen.texture != null);
        }
    }

    /// <summary>
    /// 显示日志
    /// </summary>
    private void OnClickMessageBtn(GameObject obj)
    {
        isShow = !isShow;
        if (isShow)
        {
            messageView.transform.DOLocalMoveX(-797, 0.5f);
        }
        else
        {
            messageView.transform.DOLocalMoveX(-1095f, 0.5f);
        }
        //messageView.SetActive(!messageView.activeSelf);

    }

    /// <summary>
    /// 显示右侧控制按钮
    /// </summary>
    private void OnClickCtlBtn(GameObject obj)
    {
        isShowCtl = !isShowCtl;
        if (isShowCtl)
        {
            rightCtlView.GetComponent<RectTransform>().DOAnchorPosX(-163, 0.5f);
        }
        else
        {
            rightCtlView.GetComponent<RectTransform>().DOAnchorPosX(133f, 0.5f);
        }
        //messageView.SetActive(!messageView.activeSelf);
        
    }

    private void OnClickMaxInfoBtn(GameObject obj)
    {
        if (isMaxInfo) return;
        isMaxInfo = !isMaxInfo;
        MaxInfoBtn.transform.Find("select").gameObject.SetActive(isMaxInfo);
        MaxInfoBtn.transform.Find("defult").gameObject.SetActive(!isMaxInfo);
        MinInfoBtn.transform.Find("select").gameObject.SetActive(!isMaxInfo);
        MinInfoBtn.transform.Find("defult").gameObject.SetActive(isMaxInfo);
        if (isMaxInfo)
		{
            transform.Find("MessageView/底部信息框").GetComponent<RectTransform>().DOAnchorPosY(151,0.5f);
        }
    }

    private void OnClickMinInfoBtn(GameObject obj)
    {
        if (!isMaxInfo) return;
        isMaxInfo = !isMaxInfo;
        MaxInfoBtn.transform.Find("select").gameObject.SetActive(isMaxInfo);
        MaxInfoBtn.transform.Find("defult").gameObject.SetActive(!isMaxInfo);
        MinInfoBtn.transform.Find("select").gameObject.SetActive(!isMaxInfo);
        MinInfoBtn.transform.Find("defult").gameObject.SetActive(isMaxInfo);
        if (!isMaxInfo)
        {
            transform.Find("MessageView/底部信息框").GetComponent<RectTransform>().DOAnchorPosY(-80, 0.5f);
        }
    }

    private void OnClickEnd(GameObject obj)
    {
        //打开二次确认界面
        UIMgr.GetInstance().OpenView(ViewType.ChoiceConfirmView);
        //ViewModel.TrainEnd();
    }

    private void DoIsProtect(bool isProtect)
    {
        //车上所有人 包括自己
        List<ForwardModel> forwardModels = new List<ForwardModel>();
        forwardModels.AddRange(NetManager.GetInstance().SameMachineSeatsExDevice);
        forwardModels.Add(new ForwardModel()
        {
            MachineId = AppConfig.MACHINE_ID,
            SeatId = AppConfig.SEAT_ID,
        });
        ProtectModel model = new ProtectModel()
        {
            IsProtect = isProtect,
        };
        //发送防护消息
        NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(model), NetProtocolCode.PROTECT, forwardModels);
        cancelProtectBtn.SetActive(isProtect);
        protectBtn.SetActive(!isProtect);
    }

    /// <summary>
    /// 防护
    /// </summary>
    private void OnClickProtect(GameObject obj)
    {
        DoIsProtect(true);
    }

    /// <summary>
    /// 取消防护
    /// </summary>
    private void OnClickCancelProtect(GameObject obj)
    {
        DoIsProtect(false);
    }

    /// <summary>
    /// 徒步侦察
    /// </summary>
    private void OnClickWalkCheck(GameObject obj)
    {
        //发给驾驶员
        List<ForwardModel> forwardModels = new ForwardModelsBuilder()
            .Append(AppConfig.MACHINE_ID, SeatType.DRIVE)
            .Build();
        //下车
        NetManager.GetInstance().SendMsg(ServerType.GuideServer, "", NetProtocolCode.OUT_CAR, forwardModels);
    }

    /// <summary>
    /// 下车结果
    /// </summary>
    private void OutCarWalkRes(IEventParam param)
    {
        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            ResBase res = JsonTool.ToObject<ResBase>(tcpReceiveEvParam.netData.Msg);
            //下车成功 更新按钮
            if (res.IsSuccess())
            {
                walkCheckBtn.gameObject.SetActive(false);
                inCarBtn.gameObject.SetActive(true);
               // measureBtn.gameObject.SetActive(true);
                measureBtn.gameObject.SetActive(IsNuclearMeasure());
                isInCar = false;
            }
        }
    }

    /// <summary>
    /// 上车
    /// </summary>
    private void OnClickInCar(GameObject obj)
    {
        //发给驾驶员
        List<ForwardModel> forwardModels = new ForwardModelsBuilder()
            .Append(AppConfig.MACHINE_ID, SeatType.DRIVE)
            .Build();
        //上车指令
        NetManager.GetInstance().SendMsg(ServerType.GuideServer, "", NetProtocolCode.IN_CAR, forwardModels);
    }

    /// <summary>
    /// 上车结果
    /// </summary>
    private void InCarRes(IEventParam param)
    {
        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            ResBase res = JsonTool.ToObject<ResBase>(tcpReceiveEvParam.netData.Msg);
            //上车成功 更新按钮
            if (res.IsSuccess())
            {
                walkCheckBtn.gameObject.SetActive(true);
                inCarBtn.gameObject.SetActive(false);
                measureBtn.gameObject.SetActive(false);
                isInCar = true;
            }
        }
    }

    /// <summary>
    /// 标志旗
    /// </summary>
    private void OnClickFlagBtn(GameObject obj)
    {
        UIMgr.GetInstance().OpenView(ViewType.FlagView);
    }

    /// <summary>
    /// 点击指令按钮
    /// </summary>
    private void OnClickInstructBtn(GameObject obj)
    {
        UIMgr.GetInstance().OpenView(ViewType.InstructView);
    }

    /// <summary>
    /// 开关门事件
    /// </summary>
    /// <param name="obj"></param>
    private void OnClickOpenCarDoorBtn(GameObject obj)
    {
        #region 新版修改,没做是否开门成功的消息
       

        Text text = obj.transform.Find("Text").GetComponent<Text>();
        if (text.text == "开门")
        {
            //发给驾驶员
            List<ForwardModel> forwardModels = new ForwardModelsBuilder()
                .Append(AppConfig.MACHINE_ID, SeatType.DRIVE)
                .Build();
            //开门指令
            NetManager.GetInstance().SendMsg(ServerType.GuideServer, "", NetProtocolCode.OpenDoor, forwardModels);

            text.text = "关门";
        }
        else
        {
            //发给驾驶员
            List<ForwardModel> forwardModels = new ForwardModelsBuilder()
                .Append(AppConfig.MACHINE_ID, SeatType.DRIVE)
                .Build();
            //开门指令
            NetManager.GetInstance().SendMsg(ServerType.GuideServer, "", NetProtocolCode.CloseDoor, forwardModels);

            text.text = "开门";
        }

        #endregion

        /*
        #region old
        Text text = obj.transform.Find("Text").GetComponent<Text>();
        if (text.text == "开门")
        {
            EventDispatcher.GetInstance().DispatchEvent(EventNameList.OPEN_CAR_DOOR);
            text.text = "关门";
        }
        else
        {
            EventDispatcher.GetInstance().DispatchEvent(EventNameList.CLOSE_CAR_DOOR);
            text.text = "开门";
        }
        #endregion
        */
    }

    float index = 0;   
    /// <summary>
    /// 点击切换视角按钮
    /// </summary>
    private void OnClickChangeBtn(GameObject obj)
    {
        
        switch (++index % 3)
		{
            case 0:
                changeViewBtn.GetComponentInChildren<Text>().text = "第三人称视角";
                break;
            case 1:
                changeViewBtn.GetComponentInChildren<Text>().text = "自由视角";
                break;
            case 2:
                changeViewBtn.GetComponentInChildren<Text>().text = "驾驶视角";
                break;
        }

       
        EventDispatcher.GetInstance().DispatchEvent(EventNameList.CHANGE_VIEW);
    }

    /// <summary>
    /// 点击大地图按钮
    /// </summary>
    private void OnClickMaxMapBtn(GameObject obj)
    {
        UIMgr.GetInstance().OpenView(ViewType.MapView);

    }
    /// <summary>
    /// 点击侦毒按钮
    /// </summary>
    private void OnclickDrugPoisonBtn(GameObject obj)
    {
        print("OnclickDrugPoisonBtn");
        RequestQuestion(ExTriggerType.InitJudgePoison);
    }

    /// <summary>
    /// 点击生物按钮
    /// </summary>
    private void OnclickBiologyBtn(GameObject obj)
    {
        print("OnclickBiologyBtn");
     //   RequestQuestion(ExTriggerType.InitJudgePoison);
    }

    /// <summary>
    /// 点击清洁按钮
    /// </summary>
    /// <param name="obj"></param>
    private void OnClickCleanPoisonBtn(GameObject obj)
    {
        //RequestQuestion(ExTriggerType.CleanPoison);
        DirectOpenQuestionView(ExTriggerType.CleanPoison);
    }

    /// <summary>
    /// 点击采样按钮
    /// </summary>
    /// <param name="obj"></param>
    private void OnClickSamplingPoisonBtn(GameObject obj)
    {
        //RequestQuestion(ExTriggerType.SamplingPoison);
        if (!isInCar)
        {
            RequestQuestion(ExTriggerType.SamplingPoison);
            return;
        }
        DirectOpenQuestionView(ExTriggerType.SamplingPoison);
    }

    /// <summary>
    /// 直接打开答题面板，用于辐射和化学科目中采样和洗消的题目
    /// </summary>
    /// <param name="trigger"></param>
    private void DirectOpenQuestionView(int trigger)
    {
        QstRequestResult qstRequestResult = new QstRequestResult();
        qstRequestResult.TriggerType = trigger;
        qstRequestResult.TargetId = PoisonType.NO_POISON;
        UIMgr.GetInstance().OpenView(ViewType.QuestionView);
        QuestionView questionView = (UIMgr.GetInstance().GetViewByType(ViewType.QuestionView) as QuestionView);
        questionView.SetQstRequestResult(qstRequestResult);
    }


    /// <summary>
    /// 点击上报侦察结果按钮
    /// </summary>
    private void OnClickReportDetectBtn(GameObject obj)
    {
        //打开二次确认界面
        UIMgr.GetInstance().OpenView(ViewType.ChoiceReportDetect);
        //ViewModel.TrainEnd();
    }

    /// <summary>
    /// 隐藏徒步侦察 辐射率
    /// </summary>
    private void HideOutRadiomRate(IEventParam param)
    {
        outRadiomText.gameObject.SetActive(false);
    }

    /// <summary>
    /// 徒步检测到 辐射率
    /// </summary>
    private void OutCarRadiomRate(IEventParam param)
    {
        if (param is FloatEvParam floatParam)
        {
            ViewModel.OutCarRadiomRate.Value = floatParam.value;
            outRadiomText.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 刷新显示下车侦察辐射率值
    /// </summary>
    [AutoBinding(BindConstant.UpOutCarRadiomRate)]
    private void UpdateRadiomRate(float oldValue, float newValue)
    {
        outRadiomText.text = $"当前徒步侦察探测的剂量率：\n{ newValue } {AppConstant.RADIOM_UNIT}";
    }

    /// <summary>
    /// 请求答题
    /// </summary>
    private void RequestQuestion(int type)
    {
        print("RequestQuestion");
        QstRequest model = new QstRequest()
        {
            TriggerType = type,
            SeatId = SeatType.INVEST1,
        };
        List<ForwardModel> forwardModels = new ForwardModelsBuilder()
            .Append(AppConfig.MACHINE_ID, SeatType.DRIVE)
            .Build();
        NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(model), NetProtocolCode.REQUEST_QUESTION, forwardModels);
    }

    /// <summary>
    /// 显示答题面板
    /// </summary>
    private void OnGetQstResult(IEventParam param)
    {
        print("OnGetQstResult");
        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            QstRequestResult qstRequestResult = JsonTool.ToObject<QstRequestResult>(tcpReceiveEvParam.netData.Msg);
            if (qstRequestResult != null)
            {
                if(qstRequestResult.IsOk)
                {
                    UIMgr.GetInstance().OpenView(ViewType.QuestionView);
                    QuestionView questionView = (UIMgr.GetInstance().GetViewByType(ViewType.QuestionView) as QuestionView);
                    questionView.SetQstRequestResult(qstRequestResult);
                }
                else
                {
                    if(!qstRequestResult.Tip.IsNullOrEmpty())
                    {
                        UIMgr.GetInstance().ShowToast(qstRequestResult.Tip);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 一号侦查员下车测量
    /// </summary>
    private void OnclickMeasureBtn(GameObject obj)
    {
        List<ForwardModel> forwardModels = new ForwardModelsBuilder()
            //发给驾驶员
            .Append(AppConfig.MACHINE_ID, SeatType.DRIVE)
            .Build();
        //测量消息
        NetManager.GetInstance().SendMsg(ServerType.GuideServer, "", NetProtocolCode.OUT_CAR_MEASURE, forwardModels);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.OUT_CAR_RES, OutCarWalkRes);
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.IN_CAR_RES, InCarRes);
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.RESULT_QUESTION, OnGetQstResult);
        EventDispatcher.GetInstance().RemoveEventListener(EventNameList.HIDE_OUT_RADIOM_RATE, HideOutRadiomRate);
        EventDispatcher.GetInstance().RemoveEventListener(EventNameList.OUTCAR_RADIOMRATE, OutCarRadiomRate);
        EventDispatcher.GetInstance().RemoveEventListener(EventNameList.SYN_BASECAMERA_RENDER, SynBaseCameraRender);
    }
}
