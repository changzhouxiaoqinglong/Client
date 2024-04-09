using System.Collections.Generic;
using UnityEngine;
public enum ArrangeViewType
{
    /// <summary>
    /// 确认界面
    /// </summary>
    ConfirmView,
    /// <summary>
    /// 加载界面
    /// </summary>
    LoadingView,
    /// <summary>
    /// 操作提示界面
    /// </summary>
    ControlTipView
}

public class ArrangeUiMgr : MonoSingleTon<ArrangeUiMgr>
{
    private List<IArrangeView> openViews = new List<IArrangeView>();
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
    public IArrangeView OpenView(ArrangeViewType type, IViewParam param = null)
    {
        IArrangeView curOpenView = GetViewByType(type);
        //已经打开了
        if (curOpenView != null)
        {
            return curOpenView;
        }
        string viewName = type.ToString();
        IArrangeView openView;
        Transform haveView = uiRoot.Find(viewName);
        //对应面板已存在 不用重新生成
        if (haveView)
        {
            haveView.SetAsLastSibling();
            openView = haveView.GetComponent<IArrangeView>();
        }
        else
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/3DArrange/UI/" + viewName), uiRoot);
            obj.name = obj.name.Replace("(Clone)", string.Empty);
            openView = obj.GetComponent<IArrangeView>();
        }
        openView.OnOpen(param);
        CheckAddView(openView);
        return openView;
    }

    /// <summary>
    /// 界面是否已打开
    /// </summary>
    public bool IsOpenView(ArrangeViewType type)
    {
        if (openViews.Count > 0)
        {
            foreach (IArrangeView item in openViews)
            {
                if (item.ViewType == type)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void CheckAddView(IArrangeView view)
    {
        if (!openViews.Contains(view))
        {
            openViews.Add(view);
        }
    }

    /// <summary>
    /// 关闭面板
    /// </summary>
    public void CloseView(ArrangeViewType type)
    {
        IArrangeView closeView = GetViewByType(type);
        if (closeView != null)
        {
            openViews.Remove(closeView);
            closeView.Close();
            Debug.Log("CloseView");
        }
        
    }

    /// <summary>
    /// 强制销毁当前面板
    /// </summary>
    /// <param name="type"></param>
    public void ForceDestroyView(ArrangeViewType type)
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
    public IArrangeView GetViewByType(ArrangeViewType uiType)
    {
        if (openViews.Count > 0)
        {
            foreach (IArrangeView item in openViews)
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
