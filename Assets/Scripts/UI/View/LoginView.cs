
using UnityEngine;
using UnityEngine.UI;

public class LoginView : ViewBase<LoginViewModel>
{
    private Transform backGroundAll;

    private InputField userName;

    private InputField password;

    protected override void Awake()
    {
        base.Awake();
        backGroundAll = transform.Find("BackGroundAll");
        userName = transform.Find("UserName").GetComponent<InputField>();
        password = transform.Find("Password").GetComponent<InputField>();
        ButtonBase loginBtn = transform.Find("loginBtn").GetComponent<ButtonBase>();
        loginBtn.RegistClick(OnClickLoginBtn);
        ButtonBase exitBtn = transform.Find("exitBtn").GetComponent<ButtonBase>();
        exitBtn.RegistClick(OnClickExitBtn);
        //LoadBackGround();
        LoadUserAndPwd();       
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.LOGIN, OnLoginRes);
    }

    private void OnClickLoginBtn(GameObject obj)
    {
        if (!NetManager.GetInstance().IsConnected(ServerType.GuideServer))
        {
            //未连接导控 先连接导控
            NetManager.GetInstance().ConnectServer(ServerType.GuideServer, (bool res) =>
            {
                //return;
                //连接成功
                if (res)
                {
                    //登录
                    RequestLogin();                 
                }
                else
                {
                    //连接失败
                    UIMgr.GetInstance().ShowToast("导控连接失败!");
                }
            });
        }
        else
        {
            //登录
            RequestLogin();
        }
    }

	

	/// <summary>
	/// 请求登录
	/// </summary>
	private void RequestLogin()
    {
        //请求登录
        LoginModel loginModel = new LoginModel()
        {
            UserName = userName.text,
            Password = password.text,
            CarId = AppConfig.CAR_ID,
        };
        NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(loginModel), NetProtocolCode.LOGIN);
        Logger.LogWarning("登录");
    }

    //登录结果
    private void OnLoginRes(IEventParam param)
    {
        Logger.Log("OnLoginRes");
        if (param is TcpReceiveEvParam tcpParam)
        {
            LoginRes loginRes = JsonTool.ToObject<LoginRes>(tcpParam.netData.Msg);
            //登录成功
            if (loginRes.IsSuccess())
            {
                DeleteUserAndPwd();
                Logger.Log("LoginSuccess." + loginRes.UserName);
                NetVarDataMgr.GetInstance()._NetVarData._UserInfo.userName = loginRes.UserName;
                NetVarDataMgr.GetInstance()._NetVarData._UserInfo.passWord = password.text;
                SaveUserAndPwd();
                //关闭界面 打开任务下发界面
                UIMgr.GetInstance().CloseView(ViewType.LoginView);
                UIMgr.GetInstance().OpenView(ViewType.TaskEnvWaiView);
            }
            //登录失败
            else
            {
                Logger.LogWarning("LoginFaile:" + loginRes.Code);
                UIMgr.GetInstance().ShowToast(loginRes.Tip);
            }
        }
    }

    /// <summary>
    /// 退出
    /// </summary>
    private void OnClickExitBtn(GameObject obj)
    {
        Application.Quit();
    }
    /// <summary>
    /// 保存账户名和密码
    /// </summary>
    private void SaveUserAndPwd()
    {
        PlayerPrefs.SetString("UserName", userName.text);
        PlayerPrefs.SetString("PassWord", password.text);
    }

    /// <summary>
    /// 加载背景图片
    /// </summary>
    private void LoadBackGround()
    {
        backGroundAll.GetChild(AppConfig.CAR_ID - 1).gameObject.SetActive(true);    
    }

    /// <summary>
    /// 加载用户名和密码
    /// </summary>
    private void LoadUserAndPwd()
    {
        if (PlayerPrefs.HasKey("UserName"))
        {
            userName.text = PlayerPrefs.GetString("UserName");
            //print(PlayerPrefs.GetString("UserName"));
        }
        if (PlayerPrefs.HasKey("PassWord"))
        {
            password.text = PlayerPrefs.GetString("PassWord");
          //  print(PlayerPrefs.GetString("PassWord"));
        }
    }
    /// <summary>
    /// 删除用户名和密码信息
    /// </summary>
    private void DeleteUserAndPwd()
    {
        PlayerPrefs.DeleteAll();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.LOGIN, OnLoginRes);
    }
}
