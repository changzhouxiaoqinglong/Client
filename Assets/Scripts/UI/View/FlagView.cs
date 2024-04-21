
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 插旗界面
/// </summary>

public class FlagView : ViewBase<FlagViewModel>
{
    private ButtonBase closeBtn;

    /// <summary>
    /// 核按钮
    /// </summary>
    private ButtonBase nuclearBtn;

    /// <summary>
    /// 生按钮
    /// </summary>
    private ButtonBase biologyBtn;

    /// <summary>
    /// 化按钮
    /// </summary>
    private ButtonBase chemicalBtn;

    /// <summary>
    /// 化学插旗界面
    /// </summary>
    private ChemicalFlagPage chemicalPage;

    protected override void Awake()
    {
        base.Awake();
        Transform content = transform.Find("Content");
        closeBtn = content.Find("closeBtn").GetComponent<ButtonBase>();
        closeBtn.RegistClick(OnClickClose);
        nuclearBtn = content.Find("nuclear").GetComponent<ButtonBase>();
        nuclearBtn.RegistClick(OnClickNuclear);
        biologyBtn = content.Find("biology").GetComponent<ButtonBase>();
        biologyBtn.RegistClick(OnClickBiology);
        chemicalBtn = content.Find("chemical").GetComponent<ButtonBase>();
        chemicalBtn.RegistClick(OnClickChemical);
        chemicalPage = transform.Find("ChemicalPage").GetComponent<ChemicalFlagPage>();
        chemicalPage.SetFlagView(this);
    }

    /// <summary>
    /// 插核旗
    /// </summary>
    private void OnClickNuclear(GameObject obj)
    {
        SendFlagMsg(HarmAreaType.NUCLEAR);
    }

    /// <summary>
    /// 插生物旗
    /// </summary>
    private void OnClickBiology(GameObject obj)
    {
        SendFlagMsg(HarmAreaType.BIOLOGY);
    }

    /// <summary>
    /// 插化学旗
    /// </summary>
    private void OnClickChemical(GameObject obj)
    {
      //  chemicalPage.gameObject.SetActive(true);
        SendFlagMsg(HarmAreaType.DRUG);
    }

    /// <summary>
    /// 发送插旗消息
    /// </summary>
    public void SendFlagMsg(int flagType, string info = "")
    {
        FlagToDriveModel model = new FlagToDriveModel()
        {
            FlagType = flagType,
            Info = info,
        };
        //发给驾驶员
        List<ForwardModel> forwardModels = new ForwardModelsBuilder()
            .Append(AppConfig.MACHINE_ID, SeatType.DRIVE)
            .Build();
        NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(model), NetProtocolCode.FLAG_TO_DRIVER, forwardModels);
        UIMgr.GetInstance().CloseView(ViewType);
    }
}
