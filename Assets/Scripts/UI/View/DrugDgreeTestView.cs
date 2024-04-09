using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 毒剂程度报警测试
/// </summary>
public class DrugDgreeTestView : ViewBase<DrugDgreeTestViewModel>
{
    /// <summary>
    /// 02b节点
    /// </summary>
    private GameObject root02B;
    /// <summary>
    /// 102节点
    /// </summary>
    private GameObject root102;

    /// <summary>
    /// 02b毒 下拉框
    /// </summary>
    private Dropdown drugType02B;
    /// <summary>
    /// 102毒 下拉框
    /// </summary>
    private Dropdown drugType102;


    /// <summary>
    /// 毒浓度程度
    /// </summary>
    private InputField drugDgree02B;
    /// <summary>
    /// 毒浓度程度
    /// </summary>
    private InputField drugDgree102;

    /// <summary>
    /// 确定按钮
    /// </summary>
    private ButtonBase okBtn;
    /// <summary>
    /// 返回按钮
    /// </summary>
    private ButtonBase noBtn;

    private ReportDrugData model;

    private bool isSend = false;//是否满足发送信息条件
    private float totlaTime = 0;//计时
    private float continueTime = 0f;//持续报警时间
    private float timeSum = 0f;//计算经过时间

    protected override void Awake()
    {
        base.Awake();
        root02B = transform.Find("02B").gameObject;
        drugType02B = root02B.transform.Find("drugDType/Dropdown").GetComponent<Dropdown>();
        drugDgree02B = root02B.transform.Find("drugDegree/InputField").GetComponent<InputField>();

        root102 = transform.Find("102").gameObject;
        drugType102 = root102.transform.Find("drugDType/Dropdown").GetComponent<Dropdown>();
        drugDgree102 = root102.transform.Find("drugDegree/InputField").GetComponent<InputField>();

        okBtn = transform.Find("Content/okBtn").GetComponent<ButtonBase>();
        okBtn.RegistClick(OnClickOk);
        noBtn = transform.Find("Content/closeBtn").GetComponent<ButtonBase>();
        noBtn.RegistClick(OnClickcloseBtn);

    }
    protected override void Start()
    {
        base.Start();
        Init();
    }
    void Update()
    {
        //Debug.Log("isSend:"+isSend);
        if (isSend)
        {
            //Debug.Log("毒剂测试发送信息");
            totlaTime += Time.deltaTime;
            timeSum += Time.deltaTime;
            if (totlaTime >= 1&&timeSum<=continueTime)
            {
                Debug.Log("毒剂测试发送信息");
                totlaTime = 0;
                NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(model.reportData), NetProtocolCode.SEND_DRUG_DATA, NetManager.GetInstance().SameMachineAllSeats);
            } 
        }
    }
    private void Init()
    {
        root02B.SetActive(AppConfig.CAR_ID == CarIdConstant.ID_02B);
        root102.SetActive(AppConfig.CAR_ID == CarIdConstant.ID_102);
    }
    private void OnClickOk(GameObject obj)
    {
        int drugType = 0;
        if (AppConfig.CAR_ID == CarIdConstant.ID_02B)
        {
            switch (drugType02B.value)
            {
                case 0: drugType = 0; break;
                case 1: drugType = 2; break;
                case 2: drugType = 5; break;
                default: break;
            }
        }
        else if (AppConfig.CAR_ID == CarIdConstant.ID_102)
        {
            switch (drugType102.value)
            {
                case 0: drugType = 0; break;
                case 1: drugType = 2; break;
                case 2: drugType = 5; break;
                case 3: drugType = 4; break;
                default: break;
            }
        }
        model = new ReportDrugData()
        {
            reportData = new ReportDrugDataModel()
            {
                Id = 0,
                Type = drugType,
                Dentity = AppConfig.CAR_ID == CarIdConstant.ID_02B ? float.Parse(drugDgree02B.text) : float.Parse(drugDgree102.text),
                Degree = DrugDegree.MIDDLE,
                DType = DrugDType.DTYPE2,
            },
            poisonOrigin = PoisonOrigin.AIR,
        };
        isSend = CheckSendState();
        timeSum = 0f;
    }
    private bool CheckSendState()
    {
        bool flag = false;
        Debug.Log("AppConfig.CAR_ID_issend:" + AppConfig.CAR_ID);
        Debug.Log("model.reportData.Typeissend:" + model.reportData.Type);
        Debug.Log("model.reportData.Dentity issend:" + model.reportData.Dentity);
        if (AppConfig.CAR_ID == CarIdConstant.ID_02B)
        {
            if ((model.reportData.Type == 2 && model.reportData.Dentity > 1.5) || (model.reportData.Type == 5 && model.reportData.Dentity > 4))
            {
                flag = true;
                continueTime = 10f;
            }
        }
        else if (AppConfig.CAR_ID == CarIdConstant.ID_102)
        {
            if (model.reportData.Type == 2 && model.reportData.Dentity > 1)
            {
                flag = true;
                continueTime = 30f;
            }else if(model.reportData.Type == 5 && model.reportData.Dentity > 1.5) 
            {
                flag = true;
                continueTime = 80f;
            }
            else if (model.reportData.Type == 4)
            {
                if (root102.transform.Find("UnitDropdown/Dropdown").GetComponent<Dropdown>().value == 0 && model.reportData.Dentity > 20)
                {
                    flag = true;
                    continueTime = 60f;
                }
                else if (root102.transform.Find("UnitDropdown/Dropdown").GetComponent<Dropdown>().value == 1 && model.reportData.Dentity > 10)
                {
                    flag = true;
                    continueTime = 60f;
                }
            }
        }
        return flag;
    }
    private void OnClickcloseBtn(GameObject obj)
    {
        UIMgr.GetInstance().CloseView(ViewType.DrugDgreeTest);
    }
    public void OnValueChange()
    {
        Debug.Log(drugType102.value);
        if (drugType102.value == 3)
        {
            Debug.Log("OnValueChange");
            root102.transform.Find("UnitDropdown").gameObject.SetActive(true);
            root102.transform.Find("Unit").gameObject.SetActive(false);
            Debug.Log("OnValueChange2");
        }
        else
        {
            root102.transform.Find("UnitDropdown").gameObject.SetActive(false);
            root102.transform.Find("Unit").gameObject.SetActive(true);
        }
    }
}
