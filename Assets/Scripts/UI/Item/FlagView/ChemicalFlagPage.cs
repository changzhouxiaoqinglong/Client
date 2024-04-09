
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Dropdown;

/// <summary>
/// 插化学旗子  界面
/// </summary>
public class ChemicalFlagPage : MonoBehaviour
{
    private FlagView flagView;

    /// <summary>
    /// 102 和02b节点
    /// </summary>
    private GameObject root02B102;

    /// <summary>
    /// 毒 下拉框
    /// </summary>
    private Dropdown drugType;

    /// <summary>
    /// 384节点
    /// </summary>
    private GameObject root384;

    /// <summary>
    /// 毒 大类
    /// </summary>
    private Dropdown drugDType;

    /// <summary>
    /// 毒浓度程度
    /// </summary>
    private Dropdown drugDgree;

    /// <summary>
    /// 确定按钮
    /// </summary>
    private ButtonBase okBtn;

    private void Awake()
    {
        root02B102 = transform.Find("02b102").gameObject;
        drugType = root02B102.transform.Find("drugType/Dropdown").GetComponent<Dropdown>();
        root384 = transform.Find("384").gameObject;
        drugDType = root384.transform.Find("drugDType/Dropdown").GetComponent<Dropdown>();
        drugDgree = root384.transform.Find("drugDegree/Dropdown").GetComponent<Dropdown>();
        okBtn = transform.Find("okBtn").GetComponent<ButtonBase>();
        okBtn.RegistClick(OnClickOk);
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        //初始化毒类型下拉框
        InitDrugDropDown();
        root02B102.SetActive(AppConfig.CAR_ID == CarIdConstant.ID_02B || AppConfig.CAR_ID == CarIdConstant.ID_102);
        root384.SetActive(AppConfig.CAR_ID == CarIdConstant.ID_384C);
    }

    public void SetFlagView(FlagView flagView)
    {
        this.flagView = flagView;
    }

    private void InitDrugDropDown()
    {
        drugType.options.Clear();
        foreach (var item in ExPoisonDataMgr.GetInstance().dataList)
        {
            OptionData option = new OptionData(item.Name);
            drugType.options.Add(option);
        }
        drugType.RefreshShownValue();
    }

    private void OnClickOk(GameObject obj)
    {
        //旗子信息
        string flagInfo = "";
        if (root02B102.activeInHierarchy)
        {
            ExPoisonData poisonData = ExPoisonDataMgr.GetInstance().dataList[drugType.value];
            flagInfo += "当前毒剂类型为" + poisonData.Name;
        }
        if (root384.activeInHierarchy)
        {
            flagInfo += $"当前毒剂类型为{DrugDType.GetDesc(drugDType.value)},浓度为{DrugDegree.GetDesc(drugDgree.value)}";
        }
        gameObject.SetActive(false);
        flagView.SendFlagMsg(HarmAreaType.DRUG, flagInfo);
    }
}
