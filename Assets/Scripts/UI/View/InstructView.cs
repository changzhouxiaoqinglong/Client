
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Dropdown;
/// <summary>
/// 指令界面
/// </summary>
public class InstructView : ViewBase<InstructViewModel>
{
    /// <summary>
    /// 指令下拉列表
    /// </summary>
    private Dropdown dropDown;

    /// <summary>
    /// 指令
    /// </summary>
    private List<ExInstructData> instructList;

    /// <summary>
    /// 指令下发选项
    /// </summary>
    private InstructSelect[] sendSelectItems;
    protected override void Awake()
    {
        base.Awake();
        dropDown = transform.Find("Content/Dropdown").GetComponent<Dropdown>();
        instructList = TaskMgr.GetInstance().curTaskData.GetInstructList();
        ButtonBase sendBtn = transform.Find("Content/sendBtn").GetComponent<ButtonBase>();
        sendBtn.RegistClick(OnClickSend);
        ButtonBase cancelBtn = transform.Find("Content/cancel").GetComponent<ButtonBase>();
        cancelBtn.RegistClick(OnClickClose);
        sendSelectItems = transform.Find("Content/select").GetComponentsInChildren<InstructSelect>();
    }

    protected override void Start()
    {
        base.Start();
        InitDropDown();
    }

    /// <summary>
    /// 初始化下拉列表
    /// </summary>
    private void InitDropDown()
    {
        dropDown.options.Clear();
        foreach (var item in instructList)
        {
            OptionData option = new OptionData(item.Name);
            dropDown.options.Add(option);
        }
        dropDown.value = 0;
        dropDown.RefreshShownValue();
    }

    private void OnClickSend(GameObject obj)
    {
        SendInstruct();
    }

    /// <summary>
    /// 发送指令
    /// </summary>
    private void SendInstruct()
    {
        List<ForwardModel> forwardModels = GetForwardModels();
        if (forwardModels.Count <= 0)
        {
            UIMgr.GetInstance().ShowToast("请选择发送对象！");
        }
        else
        {
            int instructId = instructList[dropDown.value].Id;
            //指令数据
            MasterInstructModel model = new MasterInstructModel()
            {
                Id = instructId,
            };
            //发送指令
            NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(model), NetProtocolCode.MASTER_INSTRUCT, forwardModels);
            UIMgr.GetInstance().ShowToast("发送成功!");
        }
    }

    /// <summary>
    /// 获得转发对象
    /// </summary>
    private List<ForwardModel> GetForwardModels()
    {
        List<ForwardModel> forwardModels = new List<ForwardModel>();
        foreach (var item in sendSelectItems)
        {
            if (item.IsBeSelect())
            {
                forwardModels.Add(new ForwardModel()
                {
                    MachineId = AppConfig.MACHINE_ID,
                    SeatId = item.seatId,
                });
            }
        }
        return forwardModels;
    }

   /* /// <summary>
    /// 侦毒特殊处理
    /// </summary>
    /// <param name="forwardModels"></param>
    private void DrugInstruct()
    {
        //指令数据
        QstRequest model = new QstRequest()
        {
            //直接发给一号侦察员
            SeatId = SeatType.INVEST1,
            TriggerType = ExTriggerType.InitJudgePoison,
            
        };
        List<ForwardModel> forwardModels = new ForwardModelsBuilder()
                    .Append(AppConfig.MACHINE_ID, SeatType.DRIVE)
                    .Build();
        //发送指令
        NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(model), NetProtocolCode.REQUEST_QUESTION, forwardModels);
    }*/
}
