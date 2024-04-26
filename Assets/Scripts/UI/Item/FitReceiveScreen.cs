
using UnityEngine;

/// <summary>
/// 接收同步画面 适配ui大小
/// </summary>
public class FitReceiveScreen : MonoBehaviour
{
    /// <summary>
    /// 当前适配的 同步画面宽高
    /// </summary>
    private Rect curFitSysRect;

    private RectTransform curRectTransform;

    private Rect canvasRect;
    private void Awake()
    {
        curRectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        canvasRect = (UIMgr.GetInstance().uiRoot as RectTransform).rect;
    }
    /// <summary>
    /// 适配宽高
    /// </summary>    
    public void FitScreen(float width, float height)
    {
        if (curFitSysRect.width != width || curFitSysRect.height != height)
        {
            //画布宽高
            float canvasWidth = canvasRect.width;
            float canvasHeight = canvasRect.height;
            //适配高度
            float fitHeight = height / width * canvasWidth;
            //高度超了  就适配宽度
            if (fitHeight > canvasHeight)
            {
             //   curRectTransform.sizeDelta = new Vector2(width / height * canvasHeight, canvasHeight);
            }
            else
            {
            //    curRectTransform.sizeDelta = new Vector2(canvasWidth, height / width * canvasWidth);
            }
            curFitSysRect.width = width;
            curFitSysRect.height = height;
        }
    }
}
