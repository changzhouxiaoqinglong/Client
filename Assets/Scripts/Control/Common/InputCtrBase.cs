
using UnityEngine;

public interface IInputCtr
{
    /// <summary>
    /// 是否启用
    /// </summary>
    bool IsEnabled
    {
        get; set;
    }

    /// <summary>
    /// 启用
    /// </summary>
    void SetEnable();

    /// <summary>
    /// 禁用
    /// </summary>
    void SetDisable();

    /// <summary>
    /// 获得控制目标
    /// </summary>    
    Transform GetTarget();
}

/// <summary>
/// 输入控制基类
/// </summary>
public class InputCtrBase : MonoBehaviour, IInputCtr
{
    protected bool isEnabled = false;

    /// <summary>
    /// 是否启用该输入控制
    /// </summary>
    public bool IsEnabled
    {
        get
        {
            return isEnabled && InputCtrMgr.GetInstance().IsEnabled;
        }
        set
        {
            isEnabled = value;
        }
    }


    /// <summary>
    /// 启用
    /// </summary>
    public virtual void SetEnable()
    {
        IsEnabled = true;
        InputCtrMgr.GetInstance().ExchangeInputCtr(this);
    }

    /// <summary>
    /// 禁用
    /// </summary>
    public virtual void SetDisable()
    {
        IsEnabled = false;
    }

    public virtual Transform GetTarget()
    {
        return transform;
    }
}
