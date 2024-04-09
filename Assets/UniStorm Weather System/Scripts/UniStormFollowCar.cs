
using UnityEngine;

public class UniStormFollowCar : MonoBehaviour
{
    /// <summary>
    /// 当前训练场景
    /// </summary>
    private Train3DSceneCtrBase curScene;

    public Train3DSceneCtrBase CurScene
    {
        get
        {
            if (curScene == null)
            {
                curScene = SceneMgr.GetInstance().curScene as Train3DSceneCtrBase;
            }
            return curScene;
        }
    }
    //esc唤出特效面板  

    // Update is called once per frame
    void Update()
    {
        if (InputCtrMgr.GetInstance().curInputCtr != null)
        {
            if (InputCtrMgr.GetInstance().curInputCtr.GetTarget() != null)
            {
                //使特效物体一直跟随着控制目标
                gameObject.transform.position = InputCtrMgr.GetInstance().curInputCtr.GetTarget().position;
            }
        }
    }
}
