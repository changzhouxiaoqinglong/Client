
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 小地图
/// </summary>
public class MiniMap : MonoBehaviour
{
    //private Transform miniCarIcon;//车辆图标
    private Transform miniPlayerIcon;//人物图标
    private Text jingWeiText;
    private float mapSize;

    /// <summary>
    /// 地图缩放 最小值
    /// </summary>
    private const float MIN_SIZE = 50;

    /// <summary>
    /// 地图缩放最大值
    /// </summary>
    private const float MAX_SIZE = 2500;

    /// <summary>
    /// 大地图按钮
    /// </summary>
    private ButtonBase maxMapBtn;

    /// <summary>
    /// 车角度
    /// </summary>
    private Vector3 carAngle;
    // Start is called before the first frame update

    /// <summary>
    /// 小地图渲ui
    /// </summary>
    private RawImage rawImage;

    /// <summary>
    /// 小地图宽高最低长度
    /// </summary>
    private const int MIN_MAP_SIZE = 300;

    private ButtonBase plusBtn;

    private ButtonBase subBtn;

    /// <summary>
    /// 小地图缩放 变动值
    /// </summary>
    private const float MAP_SCALE_CHANGE = 25;
    private void Awake()
    {
        maxMapBtn = transform.Find("maxMapBtn").GetComponent<ButtonBase>();
        //miniCarIcon = transform.Find("CarIcon");
        miniPlayerIcon = transform.Find("PlayIcon");
        jingWeiText = transform.Find("JingWeiText").GetComponent<Text>();
        rawImage = transform.Find("mask/rawImage").GetComponent<RawImage>();
        plusBtn = transform.Find("plusBtn").GetComponent<ButtonBase>();
        plusBtn.RegistClick(OnClickPlusBtn);
        subBtn = transform.Find("subBtn").GetComponent<ButtonBase>();
        mapSize = 100;
        subBtn.RegistClick(OnClickSubBtn);
    }

    private void Start()
    {
        InitMinimapSize();
    }

    private void UpdateCarText()
    {
        if (SceneMgr.GetInstance().curScene is Train3DSceneCtrBase scene3D)
        {
            List<Vector3> tempList = Line3dControl.Instance.GetListPoint();
            if (tempList.Count == 0) return;
            Vector3 car = scene3D.miniMapMgr.MiniMapCamera.GetPoint();
            if (MathTool.IsPointInPolygon(car, tempList))
            {
                transform.Find("WarningText").gameObject.SetActive(false);
            }
            else
            {
                transform.Find("WarningText").gameObject.SetActive(true);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        //更新经纬度
        UpdateLation();
        UpdateCarText();
    }

    /// <summary>
    /// 更新经纬度
    /// </summary>
    private void UpdateLation()
    {
        if (SceneMgr.GetInstance().curScene is Train3DSceneCtrBase scene3D)
        {
            carAngle = scene3D.miniMapMgr.MiniMapCamera.GetAngle();
            Vector3 lation = scene3D.terrainChangeMgr.gisPointMgr.GetGisPos(scene3D.miniMapMgr.MiniMapCamera.GetPoint());
            if (lation != null)
                jingWeiText.text = "经度：" + lation.x + "，纬度：" + lation.y;
            //if (GameObject.Find("RigidBodyFPSController") == true)
            //{
            //    miniPlayerIcon.eulerAngles = new Vector3(0, 0, -carAngle.y);
            //    miniCarIcon.gameObject.SetActive(false);
            //    miniPlayerIcon.gameObject.SetActive(true);
            //}
            //else
            {
/*                miniCarIcon.eulerAngles = new Vector3(0, 0, -carAngle.y);
                miniCarIcon.gameObject.SetActive(true);*/
                miniPlayerIcon.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 初始化小地图尺寸
    /// </summary>
    private void InitMinimapSize()
    {
        //屏幕宽高
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;
        int widthRatio = screenWidth / MIN_MAP_SIZE;
        int heightRatio = screenHeight / MIN_MAP_SIZE;
        int minRatio = Mathf.Min(widthRatio, heightRatio);
        //根据屏幕宽高 算出ui 保证不小于MIN_MAP_SIZE的最小宽高
        float resHeight = screenHeight * 1.0f / minRatio;
        float resWidth = screenWidth * 1.0f / minRatio;
        //修改ui大小
        rawImage.rectTransform.sizeDelta = new Vector2(resWidth, resHeight);
        //修改renderTexture分辨率 后续如果小地图精度过低，可适当在这里增加renderTexture分辨率
        if (rawImage.texture != null && rawImage.texture is RenderTexture renderTexture)
        {
            renderTexture.width = (int)resWidth;
            renderTexture.height = (int)resHeight;
        }
    }

    private void OnClickPlusBtn(GameObject obj)
    {
        ChangeMapSize(-MAP_SCALE_CHANGE);
    }

    private void OnClickSubBtn(GameObject obj)
    {
        ChangeMapSize(MAP_SCALE_CHANGE);
    }

    //放大缩小
    public void ChangeMapSize(float value)
    {
        mapSize += value;
        mapSize = Mathf.Clamp(mapSize, MIN_SIZE, MAX_SIZE);
        if (SceneMgr.GetInstance().curScene is Train3DSceneCtrBase scene3D)
        {
            scene3D.miniMapMgr.MiniMapCamera.SetOrthSize(mapSize);

        }
    }



}
