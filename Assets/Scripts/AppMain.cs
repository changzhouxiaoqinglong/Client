
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppMain : MonoBehaviour
{
    private Transform dontDestroyNode;

    private void Awake()
    {
        dontDestroyNode = GameObject.Find("DontDestroyNode").transform;
        InitConfigs();
    }

    private void Start()
    {
        ExSceneData loginScene = SceneExDataMgr.GetInstance().GetDataById(SceneConstant.ID_LOGIN_SCENE);
        CreateSendAllScreen();
        //进入登录场景
        SceneManager.LoadScene(loginScene.SceneName);
    }

    private void InitConfigs()
    {
        NetConfig.InitConfig();
        AppConfig.InitConfig();
        LogConfig.InitConfig();
    }

    /// <summary>
    /// 生成同步全屏画面组件
    /// </summary>
    private void CreateSendAllScreen()
    {
       // GameObject sendPrefab = Resources.Load<GameObject>(AssetPath.FM_SEND_FULL_SCREEN);
      //  Instantiate(sendPrefab, dontDestroyNode);
    }
}
