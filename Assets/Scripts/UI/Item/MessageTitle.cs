
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 消息标签类型
/// </summary>
public enum MsgTitleType
{
    /// <summary>
    /// 任务描述
    /// </summary>
    TaskDesc,//新版修改 不要了

    /// <summary>
    /// 日志
    /// </summary>
    Log,

    /// <summary>
    /// 流程
    /// </summary>
    Process,

    /// <summary>
    /// 基本信息
    /// </summary>
    Information,//新版修改 
}

/// <summary>
/// 消息展示界面标签
/// </summary>
public class MessageTitle : MonoBehaviour
{
    /// <summary>
    /// 展示信息的节点
    /// </summary>
    private GameObject showMsgNode;

    /// <summary>
    /// 信息节点 滑动条
    /// </summary>
    private ScrollRect scrollRect;

    /// <summary>
    /// 消息文本
    /// </summary>
    private Text msgText;

    public MsgTitleType curType;

    private void Awake()
    {
      //  GetComponent<ButtonBase>().RegistClick(OnClickThis);
        scrollRect = GetComponent<ScrollRect>();
        msgText = scrollRect.transform.Find("Viewport/Content").GetComponent<Text>();
    }

    public void SetShow(bool isShow)
    {
        showMsgNode.SetActive(isShow);
    }

    public void SetText(string text)
    {
        msgText.text = text;
        
        if (curType== MsgTitleType.Process)//任务流程 滑动到最上面 因为有的流程特别长  
		{
            this.DelayInvoke(0.1f, () =>
            {
                //更新滑动条到最顶端
                scrollRect.verticalNormalizedPosition = 1;
            });
        }
        else
		{
            this.DelayInvoke(0.1f, () =>
            {
                //更新滑动条到最底端
                scrollRect.verticalNormalizedPosition = 0;
            });
        }
        
       
    }

    private void OnClickThis(GameObject obj)
    {
        EventDispatcher.GetInstance().DispatchEvent(EventNameList.CLICK_MSG_TITLE, new ClickMsgTitleEvParam(curType));
    }

	private void Update()
	{
		//if(Input.GetKeyDown(KeyCode.Space))
		//{
  //          scrollRect.verticalNormalizedPosition = 1;
  //      }
  //      if (Input.GetKeyDown(KeyCode.A))
  //      {
  //          scrollRect.verticalNormalizedPosition = 0;
  //      }

    }
}
