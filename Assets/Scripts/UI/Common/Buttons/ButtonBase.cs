
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonBase : Button
{
    public delegate void OnUIEvent(GameObject obj);
    #region 交互事件
    /// <summary>
    /// 点击事件
    /// </summary>
    private OnUIEvent clickEv;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        onClick.AddListener(OnClick);
    }

    #region 注册事件
    /// <summary>
    /// 注册点击事件
    /// </summary>
    public void RegistClick(OnUIEvent uiEvent)
    {
        clickEv = uiEvent;
    }

    #endregion

    #region 事件响应
    protected virtual void OnClick()
    {
        clickEv?.Invoke(gameObject);
    }
    #endregion
}
