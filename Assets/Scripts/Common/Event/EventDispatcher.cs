using System.Collections.Generic;

/// <summary>
/// 事件派发系统
/// </summary>
public class EventDispatcher : SingleTonBase<EventDispatcher>
{
    public delegate void EventHandler(IEventParam data);
    private Dictionary<string, EventHandler> eventList = new Dictionary<string, EventHandler>();

    public void AddEventListener(string eventName, EventHandler handler)
    {
        if (eventList.ContainsKey(eventName))
        {
            eventList[eventName] += handler;
        }
        else
        {
            eventList.Add(eventName, handler);
        }
    }

    public void RemoveEventListener(string eventName, EventHandler handler)
    {
        if (eventList.ContainsKey(eventName))
        {
            eventList[eventName] -= handler;
            if (eventList[eventName] == null)
            {
                eventList.Remove(eventName);
            }
        }
    }
    public void RemoveEventListenerAll(string eventName)
    {
        if (eventList.ContainsKey(eventName))
        {
            eventList.Remove(eventName);
        }
    }

    public bool DispatchEvent(string eventName, IEventParam param = null)
    {
        if (eventList.ContainsKey(eventName))
        {
            eventList[eventName].Invoke(param);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 事件派发 （线程安全）
    /// </summary>
    /// <returns></returns>
    public bool DispatchEventMainThread(string eventName, IEventParam param = null)
    {
        if (eventList.ContainsKey(eventName))
        {
            Loom.GetInstance().QueueOnMainThread((object obj) =>
            {
                if (eventList.ContainsKey(eventName))
                {
                    eventList[eventName].Invoke(param);
                }
            }, null);
            return true;
        }
        else
        {
            return false;
        }

    }
}
