using Spore.DataBinding;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 关闭类型
/// </summary>
public enum CloseType
{
    /// <summary>
    /// 销毁
    /// </summary>
    Destroy,

    /// <summary>
    /// 隐藏
    /// </summary>
    Hide,
}

public class ViewBase<TViewModel> : UnityMono, IView where TViewModel : ViewModelBase, new()
{
    public ViewType viewType;

    /// <summary>
    /// 界面打开时是否会屏蔽输入
    /// </summary>
    public bool DisableInput = false;

    /// <summary>
    /// 面板类型
    /// </summary>
    public ViewType ViewType
    {
        get
        {         
            return viewType;
        }
        set
        {        
            viewType = value;
        }
    }

    /// <summary>
    /// 关闭类型
    /// </summary>
    public CloseType closeType;

    public ViewBase()
    {
        BindingContext.ValueChanged += OnBindingContextChanged;
    }

    protected override void Awake()
    {
        base.Awake();
        //绑定ViewModel层
        ViewModel = new TViewModel();
    }

    protected override void Start()
    {
        base.Start();
        //检测下是否加入ui管理里了，因为如果面板是直接放场景里，没有通过OpenView打开，
        //就要在这里判断下，加入ui管理
        UIMgr.GetInstance().CheckAddView(this);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (DisableInput)
        {
            //屏蔽输入
            InputCtrMgr.GetInstance().AddDisableCount();
        }
    }

    #region Binding
    private List<BindingData> _currentBindingDatas = new List<BindingData>();

    /// <summary>
    /// View所绑定的ViewModel。
    /// </summary>
    private BindableProperty<TViewModel> BindingContext = new BindableProperty<TViewModel>();

    public TViewModel ViewModel
    {
        get
        {
            return BindingContext.Value;
        }
        set
        {
            BindingContext.Value = (TViewModel)value;
        }
    }

    protected virtual void OnBindingContextChanged(TViewModel oldViewModel, TViewModel newViewModel)
    {
        AutoBindingTool.Unbinding(_currentBindingDatas);
        AutoBindingTool.Binding(BindingContext.Value, this, _currentBindingDatas);
    }
    #endregion


    public virtual void Close()
    {
        switch (closeType)
        {
            case CloseType.Destroy:
                Destroy(gameObject);
                break;
            case CloseType.Hide:
                gameObject.SetActive(false);
                break;
        }
    }

    public virtual void OnOpen(IViewParam param)
    {
        ViewModel.ViewParam = param;
        switch (closeType)
        {
            case CloseType.Destroy:
                break;
            case CloseType.Hide:
                gameObject.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// 点击关闭按钮通用逻辑
    /// </summary>
    protected virtual void OnClickClose(GameObject obj)
    {
        CloseThis();
    }

    protected void CloseThis()
    {
        UIMgr.GetInstance().CloseView(ViewType);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        //这里多判断一次  防止没通过UIMgr关闭的情况
        if (UIMgr.GetInstance().IsOpenView(ViewType))
        {
            UIMgr.GetInstance().CloseView(ViewType);
            Logger.LogWarning(ViewType.ToString() + " not use UIMgr close view");
        }
        if (DisableInput)
        {
            //解除输入屏蔽
            InputCtrMgr.GetInstance().RemoveDisableCount();
        }
    }
}