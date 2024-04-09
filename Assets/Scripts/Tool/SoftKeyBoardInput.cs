using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.Events;

[RequireComponent(typeof(InputField))]
public class SoftKeyBoardInput : MonoBehaviour
{
    private InputField input;
    private Process proc;
    private EventTrigger et;
    private void Awake()
    {
        et = transform.GetComponent<EventTrigger>();
        if (et == null)
            et = gameObject.AddComponent<EventTrigger>();
        input = transform.GetComponent<InputField>();
    }

    void Start()
    {
        input.onEndEdit.AddListener(EditEnd);

        
        et.triggers = new List<EventTrigger.Entry>();      // 实例化委托列表
        EventTrigger.Entry Sel = new EventTrigger.Entry(); // 注册事件
        Sel.eventID = EventTriggerType.Select;       // 实例化eventID
        Sel.callback = new EventTrigger.TriggerEvent();    // 实例化callback
        Sel.callback.AddListener(new UnityAction<BaseEventData>(OnSelectIpt));// 绑定事件
        et.triggers.Add(Sel);
    }

    //选中输入框
    void OnSelectIpt(BaseEventData baseEventData)
    {
        proc = Process.Start(@"C:\Windows\System32\osk.exe");
        UnityEngine.Debug.Log("Selected!");
    }

    //编辑完成
    void EditEnd(string str)
    {
        if (proc != null && !proc.HasExited)
            proc.CloseMainWindow();
        UnityEngine.Debug.Log("EditEnd!");
    }
}
