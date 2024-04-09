
using Spore.DataBinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 等待任务下发界面
/// </summary>
public class LoadingView : ArrangeViewBase<LoadingViewModel>
{
    /// <summary>
    /// 进度条
    /// </summary>
    private Image progressImage;

    /// <summary>
    /// 加载中
    /// </summary>
    private GameObject loadingText;

    /// <summary>
    /// 进度值
    /// </summary>
    private Text progressText;


    protected override void Awake()
    {
        base.Awake();
        progressImage = transform.Find("progressBg").GetComponent<Image>();
        loadingText = transform.Find("progressBg/loading").gameObject;
        progressText = transform.Find("progressBg/progressText").GetComponent<Text>();
        StartCoroutine(ViewModel.LoadSceneAsyn(XmlManger.GetInstance().areaName));
    }
    /// <summary>
    /// 更新场景加载进度
    /// </summary>
    [AutoBinding(BindConstant.UpTaskEnvSceneProgress)]
    private void UpdateSceneLoadProgress(float oldValue, float newValue)
    {
        loadingText.SetActive(true);
        progressText.text = Mathf.Floor(newValue * 100) + "%";
        Debug.Log(progressText.text);
        progressImage.fillAmount = newValue;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}