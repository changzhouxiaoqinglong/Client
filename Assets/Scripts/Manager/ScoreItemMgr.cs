using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 成绩信息管理
/// </summary>
public class ScoreItemMgr:MonoBehaviour
{
    public GameObject scoreItemPrefab;
    public List<GameObject> scoreItems= new List<GameObject>();
    public Rect objRect;
    //文本框行高
    private const float ScoreItemHeight=40;
    private void Start()
    {
        objRect = scoreItemPrefab.GetComponent<RectTransform>().rect;
    }
    /// <summary>
    /// 创建单条成绩信息
    /// </summary>
    /// <param name="root">父节点</param>
    /// <param name="leftStr">左框文字信息</param>
    /// <param name="rightStr">右框文字信息</param>
    public void CreateScoreItem(Transform root, string leftStr, string rightStr)
    {
        GameObject obj = Instantiate(scoreItemPrefab, root);
        Text leftText = obj.transform.Find("LeftText/Text").GetComponent<Text>();
        Text rightText = obj.transform.Find("RightText/Text").GetComponent<Text>();
        leftText.text = leftStr;
        rightText.text = rightStr;
        LongText(leftText, rightText, obj);
        scoreItems.Add(obj);
    }

    /// <summary>
    /// 判断左侧和右侧是否多行，统一适配高度
    /// </summary>
    /// <param name="leftText">左侧文本框</param>
    /// <param name="rightText">右侧文本框</param>
    /// <param name="obj">整个成绩列表</param>
    public void LongText(Text leftText,Text rightText,GameObject obj)
    {
        if (leftText.preferredHeight > ScoreItemHeight || rightText.preferredHeight > ScoreItemHeight)
        {
            float textheight;
            //判断左侧和右侧文本框那个更高。
            if(leftText.preferredHeight> rightText.preferredHeight)
                textheight = leftText.preferredHeight;
            else
                textheight = rightText.preferredHeight;

            //获取成绩列表的所有UI
            RectTransform objrect = obj.GetComponent<RectTransform>();
            RectTransform leftObj = obj.transform.Find("LeftText").GetComponent<RectTransform>();
            RectTransform rightObj = obj.transform.Find("RightText").GetComponent<RectTransform>();

            //给所有的UI统一高度
            float textHeight = textheight + 30f;
            leftObj.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, textHeight);
            rightObj.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, textHeight);
            objrect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, textHeight);
        }
    }
    /// <summary>
    /// 成绩信息排序
    /// </summary>
    public void OrderScoreText(Transform parent)
    {
        float parentHeight = 10f;
        for(int i = 0; i < scoreItems.Count; i++)
        {
            float curHeight = scoreItems[i].GetComponent<RectTransform>().rect.height;
            Vector3 vector = new Vector3(322, parentHeight-= curHeight, 0);
            scoreItems[i].GetComponent<RectTransform>().localPosition = vector;
        }
        parent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, parentHeight);
    }
}
 