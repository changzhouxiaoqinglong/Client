using System;
using System.Collections;
using UnityEngine;

public static class MonoExtension
{
    /// <summary>
    /// 携程延迟调用 扩展
    /// </summary>
    /// <param name="delayTime">延迟时间</param>
    /// <param name="action">调用的方法</param>
    public static Coroutine DelayInvoke(this MonoBehaviour mono, float delayTime, Action action)
    {
        return mono.InvokeByYield(action, new WaitForSeconds(delayTime));
    }

    /// <summary>
    /// 调用携程 传yield条件参数
    /// </summary>
    public static Coroutine InvokeByYield(this MonoBehaviour mono, Action action, YieldInstruction yieldInst)
    {
        return mono.StartCoroutine(InvokeCoroutineByYield(action, yieldInst));
    }

    private static IEnumerator InvokeCoroutineByYield(Action action, YieldInstruction yieldInst)
    {
        yield return yieldInst;
        action?.Invoke();
    }
}
