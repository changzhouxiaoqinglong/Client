        
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 成绩 界面
/// </summary>
public class ScoreView : ViewBase<ScoreViewModel>
{
    /// <summary>
    /// 文本内容
    /// </summary>
    private Text descText;

    /// <summary>
    /// 背景图片
    /// </summary>
    private Transform backGroundAll;

    private ButtonBase backBtn;
    private ScoreItemMgr scoreItemMgr;
    private Transform content;

    protected override void Awake()
    {
        base.Awake();
        backGroundAll = transform.Find("BackGroundAll");
        descText = transform.Find("Content/Scroll View/descText").GetComponent<Text>();
        content = transform.Find("Content/Scroll View/Viewport/Content");
        backBtn = transform.Find("Content/Back").GetComponent<ButtonBase>();
        backBtn.RegistClick(OnClickBack);
        scoreItemMgr = gameObject.GetComponent<ScoreItemMgr>();
        EventDispatcher.GetInstance().AddEventListener(EventNameList.ON_GET_TRAIN_SCORE, OnGetScore);
    }

    protected override void Start()
    {
        base.Start();
        //LoadBackGround();
        this.DelayInvoke(0, () =>
        {
            //更新文本
            UpdateDesc();
        });
    }

    /// <summary>
    /// 加载背景图片
    /// </summary>
    private void LoadBackGround()
    {
        backGroundAll.GetChild(AppConfig.CAR_ID - 1).gameObject.SetActive(true);
    }


    /// <summary>
    /// 获得分数了
    /// </summary>
    private void OnGetScore(IEventParam param)
    {
        UpdateDesc();
    }

    /// <summary>
    /// 更新文本
    /// </summary>
    private void UpdateDesc()
    {
        GetScoreModel scoreModel = NetVarDataMgr.GetInstance()._NetVarData.ScoreModels;
        if (scoreModel != null)
        {
            //先把提示字符串清空
            descText.gameObject.SetActive(false);
            //创建分数表格
            CreateScoreItem(scoreModel);
        }
        else
        {
            descText.gameObject.SetActive(true);
        }
    }

    private void OnClickBack(GameObject obj)
    {
        if (Record.GetInstance().isUpLoading) {
            UIMgr.GetInstance().ShowToast("正在上传回放视频，请稍后!");
            return;
        }
        ExSceneData endScene = SceneExDataMgr.GetInstance().GetDataById(SceneConstant.ID_LOGIN_SCENE);
        SceneManager.LoadScene(endScene.SceneName);
        UIMgr.GetInstance().CloseView(ViewType);
    }
    /// <summary>
    /// 创建成绩信息
    /// </summary>
    private void CreateScoreItem(GetScoreModel scoreModel)
    {
        if (scoreItemMgr.scoreItems.Count + 1 > scoreModel.DeductItems.Count){ Debug.Log("return"); return; }
        //先创建得分
        scoreItemMgr.CreateScoreItem(content, "得分：", scoreModel.Score.ToString());
        //创建扣分项
        for (int i = 0; i < scoreModel.DeductItems.Count; i++)
        {
            DeductItem deduct = scoreModel.DeductItems[i];
            string leftText = deduct.Desc;
            string rightText = deduct.DeductScore;
            scoreItemMgr.CreateScoreItem(content, leftText, rightText);
        }
        ////排序
        //scoreItemMgr.OrderScoreText(content);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventDispatcher.GetInstance().RemoveEventListener(EventNameList.ON_GET_TRAIN_SCORE, OnGetScore);
    }
}
