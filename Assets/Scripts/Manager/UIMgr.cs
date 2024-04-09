using System.Collections.Generic;
using UnityEngine;
public enum ViewType
{
    /// <summary>
    /// 登录
    /// </summary>
    LoginView,

    /// <summary>
    /// 任务环境下发
    /// </summary>
    TaskEnvWaiView,

    /// <summary>
    /// 基础训练界面
    /// </summary>
    BaseTaskView,

    /// <summary>
    /// 结束界面
    /// </summary>
    EndView,

    /// <summary>
    /// 训练界面
    /// </summary>
    TrainView,

    /// <summary>
    /// 插旗界面
    /// </summary>
    FlagView,

    /// <summary>
    /// 任务提示界面
    /// </summary>
    TaskTipView,

    /// <summary>
    /// 成绩界面
    /// </summary>
    ScoreView,

    /// <summary>
    /// 指令界面
    /// </summary>
    InstructView,

    /// <summary>
    /// 大地图界面
    /// </summary>
    MapView,

    /// <summary>
    /// 答题界面
    /// </summary>
    QuestionView,

    /// <summary>
    /// 结束任务二次确认界面
    /// </summary>
    ChoiceConfirmView,

    /// <summary>
    /// 上报侦察结果二次确认界面
    /// </summary>
    ChoiceReportDetect,

    /// <summary>
    /// 暂停界面
    /// </summary>
    PauseView,

    /// <summary>
    /// 设置抽气时间面板
    /// </summary>
    DetPoisonBleedView,

    /// <summary>
    /// 毒剂程度测试
    /// </summary>
    DrugDgreeTest
}

public class UIMgr : MonoSingleTon<UIMgr>
{
    private List<IView> openViews = new List<IView>();
    /// <summary>
    /// ui面板父节点
    /// </summary>
    [HideInInspector]
    public Transform uiRoot;

    /// <summary>
    /// toast节点
    /// </summary>
    private Transform toastRoot;
    private void Awake()
    {
        uiRoot = GameObject.Find("UI/Canvas/UIRoot").transform;
        toastRoot = GameObject.Find("UI/Canvas/Toast").transform;
    }

    /// <summary>
    /// 打开面板
    /// </summary>
    /// <param name="type">面板类型</param>
    public IView OpenView(ViewType type, IViewParam param = null)
    {
        IView curOpenView = GetViewByType(type);
        //已经打开了
        if (curOpenView != null)
        {
            return curOpenView;
        }
        string viewName = type.ToString();
        IView openView;
        Transform haveView = uiRoot.Find(viewName);
        //对应面板已存在 不用重新生成
        if (haveView)
        {
            haveView.SetAsLastSibling();
            openView = haveView.GetComponent<IView>();
        }
        else
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/UI/View/" + viewName), uiRoot);
            obj.name = obj.name.Replace("(Clone)", string.Empty);
            openView = obj.GetComponent<IView>();
        }
        openView.OnOpen(param);
        CheckAddView(openView);
        return openView;
    }

    /// <summary>
    /// 界面是否已打开
    /// </summary>
    public bool IsOpenView(ViewType type)
    {
        if (openViews.Count > 0)
        {
            foreach (IView item in openViews)
            {
                if (item.ViewType == type)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void CheckAddView(IView view)
    {
        if (!openViews.Contains(view))
        {
            openViews.Add(view);
        }
    }

    /// <summary>
    /// 关闭面板
    /// </summary>
    public void CloseView(ViewType type)
    {
        IView closeView = GetViewByType(type);
        if (closeView != null)
        {
            openViews.Remove(closeView);
            closeView.Close();
        }
    }

    /// <summary>
    /// 强制销毁当前面板
    /// </summary>
    /// <param name="type"></param>
    public void ForceDestroyView(ViewType type)
    {
        CloseView(type);
        string viewName = type.ToString();
        Transform haveView = uiRoot.Find(viewName);
        if (haveView != null)
        {
            Destroy(haveView.gameObject);
        }
    }

    /// <summary>
    /// 关闭所有面板
    /// </summary>
    public void CloseAllViews()
    {
        for (int i = openViews.Count - 1; i >= 0; i--)
        {
            Debug.Log("close : " + openViews[i].ViewType.ToString());
            CloseView(openViews[i].ViewType);
        }
    }

    /// <summary>
    /// 根据面板类型 获得当前打开的对应的面板
    /// </summary>
    public IView GetViewByType(ViewType uiType)
    {
        if (openViews.Count > 0)
        {
            foreach (IView item in openViews)
            {
                if (item.ViewType == uiType)
                {
                    return item;
                }
            }
        }
        return null;
    }




    public void ShowToast(string tip)
    {
        GameObject toastObj = Resources.Load<GameObject>(AssetPath.UI_TOAST);
        Toast toast = Instantiate(toastObj, toastRoot).GetComponent<Toast>();
        toast.ShowTip(tip);
    }
}
