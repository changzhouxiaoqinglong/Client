
using Spore.DataBinding;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 等待任务下发界面
/// </summary>
public class TaskEnvWaitView : ViewBase<TaskEvWaitViewModel>
{
    /// <summary>
    /// 文本
    /// </summary>
    private Text descText;

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

    /// <summary>
    /// 已经开始任务
    /// </summary>
    private bool haveStartTrain;
    /// <summary>
    /// 返回按钮
    /// </summary>
    private ButtonBase backBtn;

    /// <summary>
    /// 图片背景
    /// </summary>
    private Transform backGroundAll;

    Transform loadbg;

    protected override void Awake()
    {
        base.Awake();
        backGroundAll = transform.Find("BackGroundAll");
        loadbg = transform.Find("loadbg");
        descText = transform.Find("desc/Text").GetComponent<Text>();
        progressImage = transform.Find("progressBg").GetComponent<Image>();
        loadingText = transform.Find("progressBg/loading").gameObject;
        progressText = transform.Find("progressBg/progressText").GetComponent<Text>();
        backBtn = transform.Find("desc/Back").GetComponent<ButtonBase>();
        backBtn.RegistClick(OnClickBack);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.TASK_ENV, OnGetTaskMsg);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.TRAIN_START, OnGetStartTrain);
       // LoadBackGround();
    }

    /// <summary>
    /// 背景图片
    /// </summary>
    private void LoadBackGround()
    {
        backGroundAll.GetChild(AppConfig.CAR_ID - 1).gameObject.SetActive(true);
    }

    /// <summary>
    /// 收到下发的任务环境信息
    /// </summary>
    private void OnGetTaskMsg(IEventParam param)
    {
        if (haveStartTrain)
        {
            return;
        }
        //隐藏返回按钮
        backBtn.gameObject.SetActive(false);
        Logger.Log("OnGetTaskMsg.");
        if (param is TcpReceiveEvParam tcpParam)
        {
           // Logger.Log(tcpParam.netData.Msg);
            TaskEnvVarData data = JsonTool.ToObject<TaskEnvVarData>(tcpParam.netData.Msg);
            ////修改导控返回数据，添加弹坑
            //CraterVarData tempData = new CraterVarData();
            //tempData.Id = 1;
            //tempData.Pos = new CustVect3(2834.29f, 24.816f, 206.8f);
            //data.CraterDatas.Add(tempData);

            NetVarDataMgr.GetInstance()._NetVarData._TaskEnvVarData = data;
            ViewModel.taskEnvVarData.Value = data;

            //开启录屏
            Record.GetInstance().StartCapture();
        }
    }


    /// <summary>
    /// 收到开始训练的消息
    /// </summary>
    private void OnGetStartTrain(IEventParam param)
    {
        if (haveStartTrain)
        {
            return;
        }
        if (param is TcpReceiveEvParam tcpParam)
        {
            //收到的数据
            TrainStartModel startModel = JsonTool.ToObject<TrainStartModel>(tcpParam.netData.Msg);

            foreach(TrainMachineVarData machinedata in startModel.TrainMachineDatas)
			{
                foreach(TrainSeatVarData user in machinedata.TrainUserDatas)
				{
                    user.MachineId = machinedata.MachineId;

                }
			}


            print(startModel.TrainMachineDatas[0].TrainUserDatas==null);
            NetVarDataMgr.GetInstance()._NetVarData._TrainStartModel = startModel;
            //设置参加训练车人数据
            NetVarDataMgr.GetInstance()._NetVarData.TrainMachineDatas = startModel.TrainMachineDatas;

            //进入场景 开始任务
            if (ViewModel.taskEnvVarData.Value == null)
            {
                Logger.LogWarning("have no taskenv  can not start train");
                return;
            }
            TaskEnvVarData data = ViewModel.taskEnvVarData.Value;
            //场景数据
            ExSceneData sceneData;
            if (data.ExTaskData.IsNeedInBaseTranScene())
            {
                //基本训练固定场景
                sceneData = SceneExDataMgr.GetInstance().GetDataById(SceneConstant.ID_BASE_SCENE);
            }
            else
            {
                if (AppConfig.SEAT_ID == SeatType.DRIVE)
                {
                    //驾驶位正常进任务场景
                    sceneData = SceneExDataMgr.GetInstance().GetDataById(data.Scene);
                }
                else
                {
                    //非驾驶位  进入无3d场景 画面通过驾驶位同步渲染
                    sceneData = SceneExDataMgr.GetInstance().GetDataById(SceneConstant.ID_NO3D_SCENE);
                }
            }
            ///开始任务
            TaskMgr.GetInstance().StartTask(data.ExTaskData);
            
            //加载场景
            StartCoroutine(ViewModel.LoadSceneAsyn(sceneData));
            haveStartTrain = true;
            NetVarDataMgr.GetInstance()._NetVarData.ScoreModels = null;
        }
    }

    /// <summary>
    /// 更新任务环境信息
    /// </summary>
    [AutoBinding(BindConstant.UpTaskEnvVarData)]
    private void UpdateTaskEnvData(TaskEnvVarData oldValue, TaskEnvVarData newValue)
    {
        descText.text = newValue.TaskDesc;
    }

    /// <summary>
    /// 更新场景加载进度
    /// </summary>
    [AutoBinding(BindConstant.UpTaskEnvSceneProgress)]
    private void UpdateSceneLoadProgress(float oldValue, float newValue)
    {
        loadingText.SetActive(true);
        loadbg.gameObject.SetActive(true);
        progressText.text = Mathf.Floor(newValue * 100) + "%";
        progressImage.fillAmount = newValue;
    }


    private void OnClickBack(GameObject obj)
    {
        ////先断开连接
        NetManager.GetInstance().DisConnect(ServerType.GuideServer);
        CloseThis();
        //重新打开登录界面
        UIMgr.GetInstance().OpenView(ViewType.LoginView);

    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.TASK_ENV, OnGetTaskMsg);
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.TRAIN_START, OnGetStartTrain);
    }
}