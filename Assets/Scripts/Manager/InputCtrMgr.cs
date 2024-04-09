

using UnityEngine;
/// <summary>
/// 输入控制管理
/// </summary>
public class InputCtrMgr : MonoSingleTon<InputCtrMgr>
{
    /// <summary>
    /// 屏蔽输入计数（>0 就禁用输入系统）
    /// </summary>
    private int DisableCounter = 0;

    /// <summary>
    /// 输入控制总开关
    /// </summary>
    public bool IsEnabled
    {
        get
        {
            return DisableCounter <= 0;
        }
    }

    /// <summary>
    /// 当前启用的输入控制
    /// </summary>
    public IInputCtr curInputCtr;

    /// <summary>
    /// 修改当前输入控制
    /// </summary>
    public void ExchangeInputCtr(IInputCtr changeInput)
    {
        if (curInputCtr == changeInput)
        {
            return;
        }
        //禁用当前输入
        curInputCtr?.SetDisable();
        curInputCtr = changeInput;
    }

    /// <summary>
    /// 增加屏蔽数
    /// </summary>
    public void AddDisableCount()
    {
        DisableCounter++;
        Logger.LogDebug("AddInputDisableCount: " + DisableCounter);
    }

    /// <summary>
    /// 解除屏蔽数
    /// </summary>
    public void RemoveDisableCount()
    {
        DisableCounter--;
        DisableCounter = DisableCounter < 0 ? 0 : DisableCounter;
        Logger.LogDebug("RemoveInputDisableCount: " + DisableCounter);
    }
}
