
using Spore.DataBinding;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TaskEvWaitViewModel : ViewModelBase
{
    /// <summary>
    /// 下发的任务环境信息
    /// </summary>
    [AutoBinding(BindConstant.UpTaskEnvVarData)]
    public BindableProperty<TaskEnvVarData> taskEnvVarData { get; set; } = new BindableProperty<TaskEnvVarData>();

    /// <summary>
    /// 场景加载进度
    /// </summary>
    [AutoBinding(BindConstant.UpTaskEnvSceneProgress)]
    public BindableProperty<float> sceneLoadProgress { get; set; } = new BindableProperty<float>();

    public IEnumerator LoadSceneAsyn(ExSceneData sceneData)
    {
        AsyncOperation asynLoad = SceneManager.LoadSceneAsync(sceneData.SceneName);
        while (!asynLoad.isDone)
        {
            sceneLoadProgress.Value = asynLoad.progress;
            yield return null;
        }
        asynLoad.allowSceneActivation = true;
        //关闭界面
        UIMgr.GetInstance().CloseView(ViewType.TaskEnvWaiView);
        SceneMgr.GetInstance().CurSceneData = sceneData;
    }
}
