
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 结束训练界面
/// </summary>
public class EndView : ViewBase<EndViewModel>
{
    private ButtonBase okBtn;

    protected override void Awake()
    {
        base.Awake();
        okBtn = transform.Find("Content/okBtn").GetComponent<ButtonBase>();
        okBtn.RegistClick(OnClickOk);
    }

    private void OnClickOk(GameObject obj)
    {
        UIMgr.GetInstance().CloseAllViews();
        ExSceneData endScene = SceneExDataMgr.GetInstance().GetDataById(SceneConstant.END_SCENE);
        SceneManager.LoadScene(endScene.SceneName);
    }
}
