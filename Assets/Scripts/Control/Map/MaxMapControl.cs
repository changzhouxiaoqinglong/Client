using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MaxMapControl : MonoBehaviour
{
    /// <summary>
    /// 地图x坐标
    /// </summary>
    private float anchoredX = 0;
    /// <summary>
    /// 地图y坐标
    /// </summary>
    private float anchoredY = 0;
    /// <summary>
    /// 地图缩放大小
    /// </summary>
    private float scale = 1;

    /// <summary>
    /// 边界x坐标
    /// </summary>
    private float borderX;

    /// <summary>
    /// 边界y坐标
    /// </summary>
    private float borderY;

    /// <summary>
    /// ui坐标长宽组件
    /// </summary>
    private RectTransform rect;

    /// <summary>
    /// 图片长宽数值
    /// </summary>
    private Vector2 uiSize;

    /// <summary>
    /// 面板长宽数值
    /// </summary>
    private Vector2 viewSize;

    /// <summary>
    /// 车的图标位置
    /// </summary>
    private RectTransform carPos;

    /// <summary>
    /// terrain地图的大小
    /// </summary>
    private Vector3 terrainSize;

    /// <summary>
    /// 跟随鼠标的text
    /// </summary>
    private Text mouseText;

    /// <summary>
    /// 地图图片
    /// </summary>
    private RawImage mapImage;

    /// <summary>
    /// 毒区图标路径
    /// </summary>
    public string HarmPrefabPath = "Prefabs/UI/Item/HarmAreas/EndPoint";

    /// <summary>
    /// 地图图片路径
    /// </summary>
    private string MapSpritePath = "Prefabs/Sprite/MaxMapSpirte/";

    /// <summary>
    /// terrain
    /// </summary>
    private Terrain terrain;

    /// <summary>
    /// 目标点
    /// </summary>
    private Transform endPoint;

    /// <summary>
    /// 导控下发数据
    /// </summary>
    private TaskEnvVarData taskEnvVarData;

    /// <summary>
    /// ui和terrain的比值
    /// </summary>
    private Vector2 uiRadio;

    /// <summary>
    /// 当前毒区对象
    /// </summary>
    private GameObject curHarmObj;

    private const int TempRadio = 2;

    private Vector3 initPos;

    private bool IsHarmArea;
    private void Awake()
    {
        rect = transform.GetComponent<RectTransform>();
        uiSize = rect.sizeDelta;
        viewSize = new Vector2(Screen.width, Screen.height);
        endPoint = transform.Find("EndPoint");
        carPos = transform.Find("CarIcon").GetComponent<RectTransform>();
        mouseText = transform.Find("MouseText").GetComponent<Text>();
        mapImage = transform.GetComponent<RawImage>();
        taskEnvVarData = NetVarDataMgr.GetInstance()._NetVarData._TaskEnvVarData;
        IsHarmArea = false;
    }

    private void Start()
    {
        InitMaxMap();
        InitEndPoint();
    }

    private void OnEnable()
    {
        ScenesPoint.Instance.DestroyAll();
        if (SceneMgr.GetInstance().curScene is Train3DSceneCtrBase scene3D)
        {
            Vector3 car = scene3D.miniMapMgr.MiniMapCamera.GetPoint();
            transform.GetComponent<MaxMapControl>().SetCarPos(car);
            carPos.localEulerAngles = new Vector3(0, 0, -scene3D.miniMapMgr.MiniMapCamera.GetAngle().y);
        }
    }

    private void OnDisable()
    {
        CreateItem();
    }

    void Update()
    {

        UpdateMapSizeAndPos();
        UpdateMouseDistance();
        UpdateDrugAreaPos();
        Vector2 mousePos = Mouse.current.position.ReadValue();
        if (Mouse.current.rightButton.wasPressedThisFrame) //鼠标右键按下
        {
            PointControl.Instance.SetCubeUiPos(rect, mousePos);
            Debug.Log("鼠标按下");
            GameObject pointObj = PointControl.Instance.IsDeletePoint();
            if(pointObj == null)
            {
                PointControl.Instance.CreatePoint();
            }
            else
            {
                PointControl.Instance.DeletePoint(pointObj);
            }
            LineControl.Instance.CreateAllLine(carPos.localPosition);
            TextControl.Instance.CreateText(terrainSize,uiSize);
        }
    }

    /// <summary>
    /// 获取当前车的坐标
    /// </summary>
    /// <param name="targetObj"></param>
    private void SetCarPos(Vector3 targetObj)
    {

        float radioX = uiSize.x / terrainSize.x;
        float radioY = uiSize.y / terrainSize.z;
        carPos.anchoredPosition = new Vector2((targetObj.x * radioX), (targetObj.z * radioY));
    }

    /// <summary>
    /// 地图左右移动和滚轮放大缩小
    /// </summary>
    private void UpdateMapSizeAndPos()
    {
        float x = Mouse.current.delta.ReadValue().x * 5;
        float y = Mouse.current.delta.ReadValue().y * 5;
        scale += Mouse.current.scroll.ReadValue().y / 1200f;
        if (scale <= 0.9f)
            scale = 0.9f;
        else if (scale >= 5f)
            scale = 5f;
        borderX = (uiSize.x * scale) / 2 - viewSize.x / 2;
        borderY = (uiSize.y * scale) / 2 - viewSize.y / 2;
        if (Mouse.current.leftButton.ReadValue() > 0.5f) //鼠标左键按下
        {
            anchoredX += x;
            anchoredY += y;
        }
        if (Mathf.Abs(anchoredX) >= borderX)
        {
            if (anchoredX > 0) anchoredX = borderX;
            else anchoredX = -borderX;
        }
        if (Mathf.Abs(anchoredY) >= borderY)
        {
            if (anchoredY > 0) anchoredY = borderY;
            else anchoredY = -borderY;
        }
        rect.localScale = new Vector3(scale, scale, scale);
        rect.anchoredPosition = new Vector2(anchoredX, anchoredY);
    }

    /// <summary>
    /// 创建三维物体
    /// </summary>
    private void CreateItem()
    {
        Vector2 radio = MathsMgr.UiTerrainRadio(terrainSize, uiSize);
        Debug.Log(transform.Find("Icon").GetComponentsInChildren<Transform>());
        Transform[] cubeArr = transform.Find("Icon").GetComponentsInChildren<Transform>();
        Transform[] lineArr = transform.Find("Line").GetComponentsInChildren<Transform>();
        ScenesPoint.Instance.CreateAll(cubeArr,lineArr, radio, carPos.anchoredPosition);
    }

    /// <summary>
    /// 初始化目标点
    /// </summary>
    private void InitEndPoint()
    {

        if (SceneMgr.GetInstance().curScene is Train3DSceneCtrBase scene3D)
        {
            uiRadio = MathsMgr.UiTerrainRadio(uiSize, terrainSize);
            Vector3 car = scene3D.miniMapMgr.MiniMapCamera.GetPoint();
            transform.GetComponent<MaxMapControl>().SetCarPos(car);
            foreach (var harmArea in scene3D.harmAreaMgr.areaList)
            {
                GameObject harmObj = Instantiate(Resources.Load<GameObject>(HarmPrefabPath), endPoint);
                string path = harmArea.GetImageSpritePath();
                float range = harmArea.GetHarmRange();
                harmObj.GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
                harmObj.GetComponent<Image>().color = new Color(1, 1, 1, 0.55f);
                Vector3 pos = harmArea.transform.position;
                initPos = harmArea.transform.position;
                harmObj.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(uiRadio.x * pos.x, uiRadio.y * pos.z);
                if(NetVarDataMgr.GetInstance()._NetVarData._TaskEnvVarData.Scene == SceneConstant.HILLS)
                    harmObj.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(uiRadio.x * 1000 , uiRadio.y * 1000 );
                else
                    harmObj.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(uiRadio.x * 1000 / TempRadio, uiRadio.y * 1000 / TempRadio);
                harmObj.transform.GetComponent<RectTransform>().localScale = new Vector3(range, range, 1);
                harmObj.transform.GetComponent<RectTransform>().localEulerAngles = new Vector3(0,0,taskEnvVarData.Wearth.GetWindDir());
                curHarmObj = harmObj;
                IsHarmArea = true;
            }
            foreach (var item in scene3D.craterMgr.craterList)
            {
                GameObject craterObj = Instantiate(Resources.Load<GameObject>(HarmPrefabPath), endPoint);
                string path = item.GetImagePath();
                craterObj.GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
                Vector3 pos = item.transform.position;
                craterObj.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(uiRadio.x * pos.x, uiRadio.y * pos.z);
            }
        }
    }

    /// <summary>
    /// 初始化地图图片
    /// </summary>
    private void InitMaxMap()
    {
        int sceneId = NetVarDataMgr.GetInstance()._NetVarData._TaskEnvVarData.Scene;
        ExSceneData data = SceneExDataMgr.GetInstance().GetDataById(sceneId);
        mapImage.texture = Resources.Load<Texture>(MapSpritePath + data.SceneName);
        if (SceneMgr.GetInstance().curScene is Train3DSceneCtrBase scene3D)
        {
            terrain = scene3D.terrainChangeMgr.curTerrain;
            terrainSize = terrain.transform.GetComponent<Collider>().bounds.size;
        }
    }
    
    /// <summary>
    /// 鼠标当前位置与目标点距离
    /// </summary>
    private void UpdateMouseDistance()
    {
        Vector2 pos = Mouse.current.position.ReadValue();
        Vector2 uiTempPos;
        Vector2 textSize = mouseText.transform.GetComponent<RectTransform>().sizeDelta;
        List<GameObject> objList = PointControl.Instance.GetCubeObjList();
        int distance = 0;
        MathsMgr.MousePosChangeUiPos(rect, pos, out uiTempPos);
        mouseText.transform.localPosition = uiTempPos;
        if (objList.Count == 0)
            distance = (int)MathsMgr.PointDistance(uiTempPos, carPos.localPosition);
        else
            distance = (int)MathsMgr.PointDistance(uiTempPos , objList[objList.Count - 1].transform.localPosition);
        mouseText.text = "距离为:" + distance * MathsMgr.UiTerrainRadio(terrainSize, uiSize).x;
    }
    
    /// <summary>
    /// 动态更改毒区位置
    /// </summary>
    private void UpdateDrugAreaPos()
    {
        if (!IsHarmArea) return;
        if (SceneMgr.GetInstance().curScene is Train3DSceneCtrBase scene3D)
        {
            foreach (var harmArea in scene3D.harmAreaMgr.areaList)
            {
                Vector3 pos = harmArea.transform.position;
                CreateDashedLine(taskEnvVarData.Wearth.GetWindDir(), pos);
                curHarmObj.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(uiRadio.x * pos.x, uiRadio.y * pos.z);
            }
        }
    }

    private void CreateDashedLine(float angle,Vector3 endPos)
    {
        initPos = DashedLineControl.Instance.CreateDashedLine(uiRadio, initPos, endPos, angle);
    }
}
