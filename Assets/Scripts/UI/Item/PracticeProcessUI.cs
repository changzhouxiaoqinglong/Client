
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 训练流程ui
/// </summary>
public class PracticeProcessUI : MonoBehaviour
{
    /// <summary>
    /// 当前提示语
    /// </summary>
    private Text curTip;

    /// <summary>
    /// 错误提示
    /// </summary>
    private Text errorTip;

    /// <summary>
    /// 提示字大小值
    /// </summary>
    private Vector3 tipAnimScale = new Vector3(1.3f, 1.3f, 1.3f);
    public Text messagecontent;
    string inittext;
    private void Awake()
    {       
        //不需要流程控制提示
        if (!TaskMgr.GetInstance().curTaskCtr.practiceProcessCtr.IsHaveProcess() ||
            NetVarDataMgr.GetInstance()._NetVarData._TaskEnvVarData.CheckType != CheckTypeConst.PRACTICE)
        {
            gameObject.SetActive(false);
            return;
        }
        curTip = transform.Find("curTip").GetComponent<Text>();
        errorTip = transform.Find("errorTip").GetComponent<Text>();
        EventDispatcher.GetInstance().AddEventListener(EventNameList.PRACTICE_PROCESS_TIP, OnGetTipEv);
        EventDispatcher.GetInstance().AddEventListener(EventNameList.PRACTICE_PROCESS_ERROR_TIP, OnGetErrorTipEv);
        EventDispatcher.GetInstance().AddEventListener(EventNameList.CLEAR_ERROR_PROCESS_TIP, ClearErrorTipEv);
    }

    private void Start()
    {
        Init();
        
    }

    private void SetCurTip(string str)
    {
        curTip.text = str;
        if (!str.IsNullOrEmpty())
        {
            DoTipAnim(curTip.transform);
        }
    }

    private void SetErrorTip(string str)
    {
        errorTip.text = str;
        if (!str.IsNullOrEmpty())
        {
            DoTipAnim(errorTip.transform);
        }
    }

    private void Init()
    {
        //SetCurTip(TaskMgr.GetInstance().curTaskCtr.practiceProcessCtr.curProcess.GetCurTip());
        inittext = messagecontent.text;
    }

    /// <summary>
    /// 提示语
    /// </summary>
    private void OnGetTipEv(IEventParam param)
    {
        if (param is StringEvParam strParam)
        {
            SetCurTip(strParam.value);
           // print(strParam.index-1);
            
         
            System.Text.RegularExpressions.MatchCollection matches = System.Text.RegularExpressions.Regex.Matches(inittext, "\n");
          //  print(matches.Count);
        
            string temp = inittext;
            temp=temp.Insert(0, @"<color=green>");
            if("全部完成!" != strParam.value)
            temp=temp.Insert(matches[strParam.index - 1].Index+13, @"</color>");
            else
                temp = temp.Insert(temp.Length, @"</color>");
            messagecontent.text = temp;
        }
    }

    /// <summary>
    /// 错误提示语
    /// </summary>
    private void OnGetErrorTipEv(IEventParam param)
    {
        
        if (param is StringEvParam strParam)
        {
            UIMgr.GetInstance().ShowToast(strParam.value);
            SetErrorTip(strParam.value);
        }
    }

    /// <summary>
    /// 清空错误提示语
    /// </summary>
    private void ClearErrorTipEv(IEventParam param)
    {
        SetErrorTip(string.Empty);
    }

    /// <summary>
    /// 提示动画
    /// </summary>
    private void DoTipAnim(Transform trans)
    {
        trans.DOKill();
        trans.DOScale(tipAnimScale, 0.5f).
            Append(trans.DOScale(Vector3.one, 0.5f));
    }

    private void OnDestroy()
    {
        EventDispatcher.GetInstance().RemoveEventListener(EventNameList.PRACTICE_PROCESS_TIP, OnGetTipEv);
        EventDispatcher.GetInstance().RemoveEventListener(EventNameList.PRACTICE_PROCESS_ERROR_TIP, OnGetErrorTipEv);
        EventDispatcher.GetInstance().RemoveEventListener(EventNameList.CLEAR_ERROR_PROCESS_TIP, ClearErrorTipEv);
    }
}
