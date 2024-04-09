using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class Loom : MonoBehaviour
{
    private static Loom instance;

    public static Loom GetInstance()
    {
        return instance;
    }

    public int maxThreads = 20;

    public int numThreads;

    public struct NoDelayedQueueItem
    {
        public Action<object> action;
        public object param;
    }

    /// <summary>
    /// 非延迟调用队列
    /// </summary>
    private List<NoDelayedQueueItem> _actions = new List<NoDelayedQueueItem>();

    public struct DelayedQueueItem
    {
        public float time;
        public Action<object> action;
        public object param;
    }

    /// <summary>
    /// 延迟调用队列
    /// </summary>
    private List<DelayedQueueItem> _delayed = new List<DelayedQueueItem>();

    List<DelayedQueueItem> _currentDelayed = new List<DelayedQueueItem>();

    /// <summary>
    /// 在主线程上调用
    /// </summary>
    public void QueueOnMainThread(Action<object> taction, object tparam)
    {
        QueueOnMainThread(taction, tparam, 0f); ;
    }

    public void QueueOnMainThread(Action<object> taction, object tparam, float time)
    {
        if (time != 0)
        {
            lock (_delayed)
            {
                _delayed.Add(new DelayedQueueItem { time = Time.time + time, action = taction, param = tparam });
            }
        }
        else
        {
            lock (_actions)
            {
                _actions.Add(new NoDelayedQueueItem { action = taction, param = tparam });
            }
        }
    }

    /// <summary>
    /// 线程调用
    /// </summary>
    public void RunAsync(Action _action)
    {
        QueueOnMainThread((object obj) =>
        {
            StartCoroutine(RunAsyncIE(_action));
        }, null);
    }

    /// <summary>
    /// 使用携程 开启线程 防止等待过程 阻塞主线程
    /// </summary>    
    private IEnumerator RunAsyncIE(Action _action)
    {
        while (numThreads >= maxThreads)
        {
            yield return new WaitForSeconds(0.1f);
        }
        Interlocked.Increment(ref numThreads);
        ThreadPool.QueueUserWorkItem(RunAction, _action);
    }

    private void RunAction(object action)
    {
        try
        {
            ((Action)action)();
        }
        catch (Exception)
        {
        }
        finally
        {
            Interlocked.Decrement(ref numThreads);
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (_actions.Count > 0)
        {
            lock (_actions)
            {
                for (int i = 0; i < _actions.Count; i++)
                {
                    _actions[i].action(_actions[i].param);
                }
                _actions.Clear();
            }
        }
        if (_delayed.Count > 0)
        {
            lock (_delayed)
            {
                _currentDelayed.Clear();
                _currentDelayed.AddRange(_delayed.Where(d => d.time <= Time.time));
                for (int i = 0; i < _currentDelayed.Count; i++)
                {
                    _delayed.Remove(_currentDelayed[i]);
                }
            }
            for (int i = 0; i < _currentDelayed.Count; i++)
            {
                _currentDelayed[i].action(_currentDelayed[i].param);
            }
        }
    }
}
