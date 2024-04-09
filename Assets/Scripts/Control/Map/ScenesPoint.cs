using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenesPoint : MonoBehaviour
{
    public static ScenesPoint Instance;

    private const float Distance = 40;
    void Awake()
    {
        Instance = this;
    }


    /// <summary>
    /// 创建点和线
    /// </summary>
    /// <param name="points"></param>
    /// <param name="lines"></param>
    /// <param name="radio"></param>
    public void CreateAll(Transform[] points,Transform[] lines,Vector2 radio,Vector3 carPos)
    {
        Point3dControl.Instance.CreatePoint(points, radio,carPos);
        Line3dControl.Instance.CreateLine(lines, radio);
        curScene = SceneMgr.GetInstance().curScene as Train3DSceneCtrBase;
    }


    /// <summary>
    /// 销毁点和线
    /// </summary>
    public void DestroyAll()
    {
        Point3dControl.Instance.DestroyPoint();
        Line3dControl.Instance.DestroyLine();
    }



    /// <summary>
    /// 新版修改 生成点一直放在minmapcam下方，因为新版地形高度差很大
    /// </summary>
    private Train3DSceneCtrBase curScene;
    private void Start()
	{
        distance = 500;
    }

    [SerializeField]
     float distance;
	void Update()
    {
        if(curScene!=null)
		{
            transform.localPosition = new Vector3(transform.localPosition.x, curScene.miniMapMgr.MiniMapCamera.transform.position.y - distance, transform.localPosition.z);
        }
        
    }
        
}
