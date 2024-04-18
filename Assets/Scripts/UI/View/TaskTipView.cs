
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 任务提示界面
/// </summary>

public class TaskTipView : ViewBase<TaskTipViewModel>
{
    /// <summary>
    /// 任务描述
    /// </summary>
    private Text taskDesc;

    protected override void Awake()
    {
        base.Awake();
        taskDesc = transform.Find("Content/desc").GetComponent<Text>();
        ButtonBase okBtn = transform.Find("Content/okBtn").GetComponent<ButtonBase>();
        okBtn.RegistClick(OnClickClose);
    }

	protected override void OnClickClose(GameObject obj)
	{
		base.OnClickClose(obj);
        EventDispatcher.GetInstance().DispatchEvent(EventNameList.OnClick_tasktipview);
    }

	protected override void Start()
    {
        base.Start();
        taskDesc.text = NetVarDataMgr.GetInstance()._NetVarData._TaskEnvVarData.ExTaskData.Desc;
        
    }
}
