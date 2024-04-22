
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
/// <summary>
/// 信息展示界面
/// </summary>
public class MessageView : MonoBehaviour
{
    /// <summary>
    /// 标签
    /// </summary>
    public MessageTitle[] titles;

    /// <summary>
    /// 当前选中的标签
    /// </summary>
    private MessageTitle curSelect;

    /// <summary>
    /// 流程标签
    /// </summary>
    private MessageTitle processTitle;

    private Transform basicInfo;
    /// <summary>
    /// 显示底部操作信息按钮
    /// </summary>
    private bool isMaxInfo = true;

    /// <summary>
    /// 底部信息框max
    /// </summary>
    private ButtonBase MaxInfoBtn;

    /// <summary>
    /// 底部信息框min
    /// </summary>
    private ButtonBase MinInfoBtn;
    private void Awake()
    {
        titles = GetComponentsInChildren<MessageTitle>();
        processTitle = transform.Find("提示信息框/Scroll View").GetComponent<MessageTitle>();
        basicInfo = transform.Find("底部信息框/BasicInformation/Viewport/Content");
        // EventDispatcher.GetInstance().AddEventListener(EventNameList.CLICK_MSG_TITLE, OnClickTitle);
        if (NetVarDataMgr.GetInstance()._NetVarData._TaskEnvVarData.CheckType == CheckTypeConst.PRACTICE)
            EventDispatcher.GetInstance().AddEventListener(EventNameList.REF_SHOW_TASK_LOG, RefreshLog);
    }

    private void Start()
    {
        /*
		#region old
		//初始选中任务描述标签
		SelectTitle(MsgTitleType.TaskDesc);
        InitTaskDesc();
        //刷新日志
        RefreshLog(null);
        //初始化流程
        InitProcessUI();
        #endregion
        */

        #region 新版修改
        //初始选中任务描述标签
       // SelectTitle(MsgTitleType.TaskDesc);
        //  InitTaskDesc();任务说明不要了
        InitBasicInformation();
        //刷新日志
        RefreshLog(null);
        //初始化流程，就是提示信息
        InitProcessUI();//

        MaxInfoBtn = transform.Find("底部信息框/BtnMax").GetComponent<ButtonBase>();
        MaxInfoBtn.RegistClick(OnClickMaxInfoBtn);
        MinInfoBtn = transform.Find("底部信息框/BtnMin").GetComponent<ButtonBase>();
        MinInfoBtn.RegistClick(OnClickMinInfoBtn);

        #endregion
    }

    /// <summary>
    /// 初始化任务描述
    /// </summary>
    public void InitTaskDesc()
    {
        GetTitleByType(MsgTitleType.TaskDesc).SetText(NetVarDataMgr.GetInstance()._NetVarData._TaskEnvVarData.TaskDesc);
    }

    /// <summary>
    /// 初始化训练流程
    /// </summary>
    private void InitProcessUI()
    {
        print("初始化训练流程_CheckType"+ NetVarDataMgr.GetInstance()._NetVarData._TaskEnvVarData.CheckType);
        //单击和考核没有基础训练
        if (NetVarDataMgr.GetInstance()._NetVarData._TaskEnvVarData.CheckType != CheckTypeConst.PRACTICE)
        {
            processTitle.transform.parent.gameObject.SetActive(false);
            //processTitle.SetText("当前为考核模式不显示具体操作信息");
            return;
        }
        int taskId = TaskMgr.GetInstance().curTaskData.Id;
        //本次训练流程数据
        List<ExPracticeProcess> processList = ExPracticeProcessMgr.GetInstance().GetProcessByTaskId(taskId);
        if (processList.Count > 0)
        {
            processTitle.SetText(ExPracticeProcessMgr.GetInstance().GetProcessDesc(taskId));
        }
        else
        {
            processTitle.transform.parent.gameObject.SetActive(false);
        }
    }

    /// <summary>
    ///  初始化基本信息
    /// </summary>
    private void InitBasicInformation()
	{   
     float WindDirOff = 5;
     float WindSpOff = 0.3f;
     var weather = NetVarDataMgr.GetInstance()._NetVarData._TaskEnvVarData.Wearth;
        //风向浮动
        float windDir = weather.GetWindDir();
        windDir += WindDirOff * Random.Range(-1f, 1f);
        windDir = Mathf.Clamp(windDir, 0, 359);

        //风速浮动
        float windSp = weather.GetWindSp();
        windSp += WindSpOff * Random.Range(-1f, 1f);
        windSp = windSp < 0 ? 0 : windSp;
		// basicInfo.Find("tasktype/value")?.GetComponent<Text>().text=;
		// basicInfo.Find("currentpos")?;

		

        basicInfo.Find("weather/value").GetComponent<Text>().text = weather.GetDes()+ "温度" +weather.Temperate+","+ "湿度" + weather.Humidity + ","+ GetWindDir();

        basicInfo.Find("tasktype/value").GetComponent<Text>().text = TaskMgr.GetInstance().curTaskData.Desc;



    }


    /// <summary>
    /// 获取当前风向
    /// </summary>
    /// <returns></returns>
    private string GetWindDir() {
        string windDir = "";
        Wearth curWearth = NetVarDataMgr.GetInstance()._NetVarData._TaskEnvVarData.Wearth;
        switch (curWearth.WindDir) {
            case 0:
                windDir = "北风";
                break;
            case 1:
                windDir = "东北风";
                break;
            case 2:
                windDir = "东风";
                break;
            case 3:
                windDir = "东南风";
                break;
            case 4:
                windDir = "南风";
                break;
            case 5:
                windDir = "西南风";
                break;
            case 6:
                windDir = "西风";
                break;
            case 7:
                windDir = "西北风";
                break;
            default:
                break;
        }
        return windDir;
    }

    /// <summary>
    /// 刷新日志
    /// </summary>
    private void RefreshLog(IEventParam param)
    {
        if (NetVarDataMgr.GetInstance()._NetVarData._TaskEnvVarData.CheckType != CheckTypeConst.PRACTICE)
		{
            GetTitleByType(MsgTitleType.Log).SetText("当前为考核模式不显示具体操作信息");
            return;
        }
        GetTitleByType(MsgTitleType.Log).SetText(TaskMgr.GetInstance().curTaskCtr.GetTrainLog());
    }

    private void SelectTitle(MsgTitleType type)
    {
        MessageTitle title = GetTitleByType(type);
        if (title)
        {
            SelectTitle(title);
        }
    }

    /// <summary>
    /// 选择标签
    /// </summary>
    private void SelectTitle(MessageTitle title)
    {
        if (curSelect != title)
        {
            curSelect?.SetShow(false);
            curSelect = title;
            curSelect.SetShow(true);
        }
    }

    /// <summary>
    /// 根据类型找到对应的标签
    /// </summary>
    private MessageTitle GetTitleByType(MsgTitleType type)
    {
        foreach (MessageTitle title in titles)
        {
            if (title.curType == type)
            {
                return title;
            }
        }
        return null;
    }

    /// <summary>
    /// 点击标签
    /// </summary>
    private void OnClickTitle(IEventParam param)
    {     
        if (param is ClickMsgTitleEvParam titleParam)
        {
            SelectTitle(titleParam.type);
        }
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
            transform.Find("底部信息框").GetComponent<RectTransform>().DOAnchorPosY(151, 0.5f);
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
            transform.Find("底部信息框").GetComponent<RectTransform>().DOAnchorPosY(-80, 0.5f);
        }
    }

    private void OnDestroy()
    {
      //  EventDispatcher.GetInstance().RemoveEventListener(EventNameList.CLICK_MSG_TITLE, OnClickTitle);
        EventDispatcher.GetInstance().RemoveEventListener(EventNameList.REF_SHOW_TASK_LOG, RefreshLog);
    }
}
