
using UnityEngine;

/// <summary>
/// 设备接口
/// </summary>
public interface IDevice
{

}

/// <summary>
/// 设备基类
/// </summary>
public class DeviceBase : UnityMono, IDevice
{
    protected CarBase car;

    private HarmAreaMgr harmAreaMgr = null;

    /// <summary>
    /// 有害区域管理
    /// </summary>
    protected HarmAreaMgr HarmAreaMgr
    {
        get
        {
            if (harmAreaMgr == null)
            {
                if (SceneMgr.GetInstance().curScene != null)
                {
                    harmAreaMgr = (SceneMgr.GetInstance().curScene as Train3DSceneCtrBase).harmAreaMgr;
                }
            }
            return harmAreaMgr;
        }
    }

    private Train3DSceneCtrBase curScene3D;

    protected Train3DSceneCtrBase CurScene3D
    {
        get
        {
            if (curScene3D == null)
            {
                if (SceneMgr.GetInstance().curScene != null)
                {
                    curScene3D = SceneMgr.GetInstance().curScene as Train3DSceneCtrBase;
                }
            }
            return curScene3D;
        }
    }

    public void SetCurCar(CarBase car)
    {
        this.car = car;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        //任务结束 不执行update
        if (!TaskMgr.GetInstance().isInTask)
        {
            return;
        }
        OnUpdate();
    }

    protected virtual void OnUpdate()
    {

    }
}
