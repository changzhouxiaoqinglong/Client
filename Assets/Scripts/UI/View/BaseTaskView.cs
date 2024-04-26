
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 基本训练界面
/// </summary>
public class BaseTaskView : ViewBase<BaseTaskViewModel>
{
    /// <summary>
    /// 结束按钮
    /// </summary>
    private ButtonBase endBtn;

    /// <summary>
    /// 设置剂量率节点
    /// </summary>
    private GameObject doseRoot;

    /// <summary>
    /// 剂量率输入框
    /// </summary>
    private InputField doseInput;

    /// <summary>
    /// 剂量率发送按钮
    /// </summary>
    private ButtonBase sendDoseBtn;

    /// <summary>
    /// 设置抽气时间节点
    /// </summary>
    private GameObject gasTimeRoot;

    /// <summary>
    /// 设置抽气时间输入框
    /// </summary>
    private InputField gasTimeInput;

    /// <summary>
    /// 设置抽气时间按钮
    /// </summary>
    private ButtonBase gasTimeBtn;

    /// <summary>
    /// 指令按钮
    /// </summary>
    private ButtonBase instructBtn;

    /// <summary>
    /// 背景图片切换
    /// </summary>
    private Transform backGroundAll;
    #region 384 设置
    /// <summary>
    /// 384 设置总节点
    /// </summary>
    private GameObject set384Root;

    /// <summary>
    /// 384毒 设置ui节点
    /// </summary>
    private GameObject setDrug384Root;

    /// <summary>
    /// 384设置毒 程度
    /// </summary>
    private Dropdown drugDegreeDrop;

    /// <summary>
    /// 384设置毒 类
    /// </summary>
    private Dropdown drugDtypeDrop;

    /// <summary>
    /// 设置按钮
    /// </summary>
    private ButtonBase drugSet384Btn;
    #endregion

    #region 102设置
    /// <summary>
    /// 102设置ui  总节点
    /// </summary>
    private GameObject set102Root;

    /// <summary>
    /// 102 毒剂设置节点
    /// </summary>
    private GameObject setDrug102Root;

    /// <summary>
    /// 102 毒剂类型设置下拉框
    /// </summary>
    private Dropdown setDrugTypeDrop102;

    /// <summary>
    /// 102 毒剂浓度设置输入框
    /// </summary>
    private InputField setDrugDentityInput102;

    /// <summary>
    /// 102 毒剂设置按钮
    /// </summary>
    private ButtonBase setDrugBtn;

    /// <summary>
    /// 102 设置遥测数据节点
    /// </summary>
    private GameObject setInfare102Root;

    /// <summary>
    /// 102 红外遥测 毒类型下拉框
    /// </summary>
    private Dropdown infareTypeDrop102;

    /// <summary>
    /// 102 设置红外遥测 按钮
    /// </summary>
    private ButtonBase setInfareBtn102;
    #endregion
    Transform ProcessCtr;
    protected override void Awake()
    {
        base.Awake();

        //  transform.Find("MessageView/提示信息框").gameObject.transform.localScale=Vector3.zero;
        //   transform.Find("MessageView/底部信息框").transform.localScale = Vector3.zero;
        ProcessCtr = transform.Find("ProcessCtr");
        backGroundAll = transform.Find("BackGroundAll");
        endBtn = transform.Find("endBtn").GetComponent<ButtonBase>();
        endBtn.RegistClick(OnClickEnd);
        doseRoot = transform.Find("Dose").gameObject;
        doseInput = doseRoot.transform.Find("Dose").GetComponent<InputField>();
        sendDoseBtn = doseRoot.transform.Find("DoseBtn").GetComponent<ButtonBase>();
        sendDoseBtn.RegistClick(OnClickDoseBtn);
        gasTimeRoot = transform.Find("SetGasTime").gameObject;
        gasTimeInput = gasTimeRoot.transform.Find("gasTime").GetComponent<InputField>();
        gasTimeBtn = gasTimeRoot.transform.Find("set").GetComponent<ButtonBase>();
        gasTimeBtn.RegistClick(OnClickGasTimeBtn);
        instructBtn = transform.Find("instructBtn").GetComponent<ButtonBase>();
        instructBtn.RegistClick(OnClickInstructBtn);
        set384Root = transform.Find("384Set").gameObject;
        setDrug384Root = set384Root.transform.Find("384DrugSet").gameObject;
        drugDegreeDrop = setDrug384Root.transform.Find("drugDegree/Dropdown").GetComponent<Dropdown>();
        drugDtypeDrop = setDrug384Root.transform.Find("drugDtype/Dropdown").GetComponent<Dropdown>();
        drugSet384Btn = setDrug384Root.transform.Find("Button").GetComponent<ButtonBase>();
        drugSet384Btn.RegistClick(OnClickSetDrugBtn384);

        set102Root = transform.Find("102Set").gameObject;
        setDrug102Root = set102Root.transform.Find("102DrugSet").gameObject;
        setDrugTypeDrop102 = setDrug102Root.transform.Find("drugType/Dropdown").GetComponent<Dropdown>();
        setDrugBtn = setDrug102Root.transform.Find("Button").GetComponent<ButtonBase>();
        setDrugBtn.RegistClick(OnClickSetDrugBtn102);

        setInfare102Root = set102Root.transform.Find("102InfareSet").gameObject;
        infareTypeDrop102 = setInfare102Root.transform.Find("drugType/Dropdown").GetComponent<Dropdown>();
        setInfareBtn102 = setInfare102Root.transform.Find("Button").GetComponent<ButtonBase>();
        setInfareBtn102.RegistClick(OnClickSetInfareBtn102);
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
        //辐射仪基本训练 才会展示发送剂量率的ui
        //doseRoot.SetActive(IsShowSetDoseBtn());
        //车载侦毒器基本训练 要设置抽气时间
        //gasTimeRoot.SetActive(IsShowGasTimeBtn());//新版修改 抽气放在device里
        instructBtn.gameObject.SetActive(AppConfig.SEAT_ID == SeatType.MASTER);
        endBtn.gameObject.SetActive(IsShowEndBtn());
       // set384Root.SetActive(AppConfig.CAR_ID == CarIdConstant.ID_384C);//新版修改 去掉384设置ui
        setDrug384Root.SetActive(TaskMgr.GetInstance().curTaskData.Id == ExTaskId.BASE_POISON_384 && AppConfig.CAR_ID == CarIdConstant.ID_384C);

       // set102Root.SetActive(AppConfig.CAR_ID == CarIdConstant.ID_102);//新版修改 去掉102设置ui
        setDrug102Root.SetActive((TaskMgr.GetInstance().curTaskData.Id == ExTaskId.BASE_POISON_102
            || TaskMgr.GetInstance().curTaskData.Id == ExTaskId.BASE_PREVENT_102)
            && AppConfig.CAR_ID == CarIdConstant.ID_102);
        setInfare102Root.SetActive(TaskMgr.GetInstance().curTaskData.Id == ExTaskId.BASE_INFARE_102 && AppConfig.CAR_ID == CarIdConstant.ID_102);
        // LoadBackGround();
        EventDispatcher.GetInstance().AddEventListener(EventNameList.OnClick_tasktipview, ShowPanel);
    }

    /// <summary>
    /// 加载背景图片
    /// </summary>
    private void LoadBackGround()
    {
        backGroundAll.GetChild(AppConfig.CAR_ID - 1).gameObject.SetActive(true);
    }

    /// <summary>
    /// 发送剂量率
    /// </summary>
    private void OnClickDoseBtn(GameObject obj)
    {
        ViewModel.SendRadiomRate(doseInput.text.ToFloat());
    }

    /// <summary>
    /// 设置抽气时间
    /// </summary>
    private void OnClickGasTimeBtn(GameObject obj)
    {
        ViewModel.SetGasTime(gasTimeInput.text.ToFloat());
    }

    /// <summary>
    /// 点击结束按钮
    /// </summary>
    private void OnClickEnd(GameObject obj)
    {
        UIMgr.GetInstance().OpenView(ViewType.ChoiceConfirmView);
        //ViewModel.ClickEndTrain();
    }

    /// <summary>
    /// 是否展示设置剂量率的按钮
    /// </summary>
    /// <returns></returns>
    private bool IsShowSetDoseBtn()
    {
        int curTaskId = TaskMgr.GetInstance().curTaskData.Id;
        return curTaskId == ExTaskId.BASE_RADIOMETE_02B || curTaskId == ExTaskId.BASE_RADIOMETE_384
            || curTaskId == ExTaskId.BASE_RADIOMETE_102 || curTaskId == ExTaskId.BASE_PREVENT_102
             || curTaskId == ExTaskId.BASE_RADIOMETE_106;
    }

    /// <summary>
    /// 是否展示设置抽气时间按钮
    /// </summary>
    private bool IsShowGasTimeBtn()
    {
        return TaskMgr.GetInstance().curTaskData.Id == ExTaskId.BASE_CAR_POISON_DETECT_02B;
    }

    /// <summary>
    /// 点击指令按钮
    /// </summary>
    private void OnClickInstructBtn(GameObject obj)
    {
        UIMgr.GetInstance().OpenView(ViewType.InstructView);
    }

    /// <summary>
    /// 是否展示结束按钮
    /// </summary>
    private bool IsShowEndBtn()
    {
     //   return TaskMgr.GetInstance().curTaskData.Type != TaskType.Coord || AppConfig.SEAT_ID == SeatType.MASTER;
        return TaskMgr.GetInstance().curTaskData.Type != TaskType.Coord || AppConfig.SEAT_ID == SeatType.INVEST1; //新版修改 合并234
    }

    #region 384设置ui
    /// <summary>
    /// 点击设置毒数据
    /// </summary>
    private void OnClickSetDrugBtn384(GameObject obj)
    {
        ReportDrugDataModel model = new ReportDrugDataModel()
        {
            Degree = drugDegreeDrop.value,
            DType = drugDtypeDrop.value,
        };
        //发给设备管理软件
        NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(model), NetProtocolCode.SEND_DRUG_DATA, NetManager.GetInstance().SameMachineAllSeats);
    }
    #endregion

    #region 102设置 ui
    /// <summary>
    /// 点击设置毒数据
    /// </summary>
    private void OnClickSetDrugBtn102(GameObject obj)
    {
        if (TaskMgr.GetInstance().curTaskData.Id == ExTaskId.BASE_PREVENT_102 || TaskMgr.GetInstance().curTaskData.Id == ExTaskId.BASE_POISON_102)
        {
            DefenseReportDrugDataModel model = new DefenseReportDrugDataModel()
            {
                Type = setDrugTypeDrop102.value + 1,
            };
            //发给设备管理软件
            NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(model), NetProtocolCode.DEFENSE_SEND_DRUG_DATA, NetManager.GetInstance().CurDeviceForward);
        }
        else
        {
            ReportDrugDataModel model = new ReportDrugDataModel()
            {
                Type = setDrugTypeDrop102.value + 1,
            };
            //发给设备管理软件
            NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(model), NetProtocolCode.SEND_DRUG_DATA, NetManager.GetInstance().CurDeviceForward);
        }
    }

    /// <summary>
    /// 点击设置红外遥测数据
    /// </summary>
    private void OnClickSetInfareBtn102(GameObject obj)
    {
        InfaredTelemetry102DrugModel model = new InfaredTelemetry102DrugModel()
        {
            Type = infareTypeDrop102.value + 1,
        };
        //发给设备管理软件
        NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(model), NetProtocolCode.INFARED_TELEMETRY_DRUG_DATA_102, NetManager.GetInstance().CurDeviceForward);
    }
	#endregion

    void ShowPanel(IEventParam param)
	{
        // transform.localScale = Vector3.one;
        // transform.Find("MessageView/提示信息框").transform.localScale = Vector3.one;
        // transform.Find("MessageView/底部信息框").transform.localScale = Vector3.one;
        transform.Find("MessageView").transform.gameObject.SetActive(true);
        ProcessCtr.gameObject.SetActive(true);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
       
        EventDispatcher.GetInstance().RemoveEventListener(EventNameList.OnClick_tasktipview, ShowPanel);
    }
}
