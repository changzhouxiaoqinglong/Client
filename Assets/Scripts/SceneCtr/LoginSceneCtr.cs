
/// <summary>
/// 登录场景
/// </summary>
public class LoginSceneCtr : SceneCtrBase   
{
    protected override void Start()
    {
        base.Start();
        if (NetVarDataMgr.GetInstance()._NetVarData._UserInfo.userName == null)
        {
            UIMgr.GetInstance().OpenView(ViewType.LoginView);
        }
        else
        {
            //已登录，直接等待任务下发
            UIMgr.GetInstance().OpenView(ViewType.TaskEnvWaiView);
        }
    }
}
