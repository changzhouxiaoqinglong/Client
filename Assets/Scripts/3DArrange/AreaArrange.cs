using System;
using UnityEngine;
using NWH.VehiclePhysics;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using RTG;
using UnityEngine.EventSystems;

public class AreaArrange : TrainSceneCtrBase
{
    [SerializeField]
    /// <summary>
    /// 地图管理
    /// </summary>
    public TerrainChangeMgr terrainChangeMgr = new TerrainChangeMgr();
    private Camera _camera;//摄像机
    private ButtonBase saveBtn;//保存按钮
    private ButtonBase rListBtn;//右侧列表按钮
    private ButtonBase controlTipBtn;//提示按钮
    private ButtonBase startPosBtn;//初始点

    private Tut_5_CustomObjectLocalPivot targetObjMethod;//操作物体的方法

    //数组
    private List<GameObject> carList = new List<GameObject>();
    private List<GameObject> toxiList = new List<GameObject>();
    private List<GameObject> radiateList = new List<GameObject>();
    private List<GameObject> craterList = new List<GameObject>();

    private Transform arrangeObjPath;//想定物体的父物体路径
    private Transform content;//列表路径
    private GameObject item;//单个列表
    private bool isExpand=true;//是否展开
    private float time = 0;//双击计时
    private string choiceBtn;//选中的按钮名字


    protected override void Start()
    {
        terrainChangeMgr.Start();
        //获取按钮，设置按钮点击事件。
        saveBtn = GameObject.Find("Arrange/Canvas/MessageView/SaveBtn").GetComponent<ButtonBase>();
        saveBtn.onClick.AddListener(ClickSaveBtn);
        rListBtn = GameObject.Find("Arrange/Canvas/MessageView/Btn").GetComponent<ButtonBase>();
        rListBtn.onClick.AddListener(ClickLListBtn);
        controlTipBtn = GameObject.Find("Arrange/Canvas/MessageView/ControlTipBtn").GetComponent<ButtonBase>();
        controlTipBtn.onClick.AddListener(ClickControlTipBtn);
        startPosBtn = GameObject.Find("Arrange/Canvas/MessageView/StartPosBtn").GetComponent<ButtonBase>();
        startPosBtn.onClick.AddListener(ClickStartPosBtn);

        arrangeObjPath = GameObject.Find("Arrange/Car").transform;
        _camera = GameObject.Find("Arrange/Camera").GetComponent<Camera>();
        targetObjMethod = GameObject.Find("Arrange/Tutorial").GetComponent<Tut_5_CustomObjectLocalPivot>();
        content = GameObject.Find("Arrange/Canvas/MessageView/Container/taskDescMsg/Viewport/Content").transform;
        item = Resources.Load<GameObject>("Prefabs/3DArrange/UI/item");
        CreateArrangeObj();
        CreateList();
        ChangeCamera(arrangeObjPath.GetChild(0).gameObject);
    }

    void Update()
    {
    }
    /// 创建车辆、弹坑、毒剂、辐射
    /// </summary>
    /// <param name="arrange"></param>
    /// <returns></returns>
    private void CreateArrangeObj()
    {
        for (int i = 0; i < XmlManger.GetInstance().carArangeList.Count; i++)
        {
            Car_Arrange arrange = XmlManger.GetInstance().carArangeList[i];
            Debug.Log(arrange.posX);
            Debug.Log(arrange.posY);
            Vector3 vector3 = terrainChangeMgr.GetTerrainPosByGis(new Vector2(Convert.ToSingle(arrange.posX), Convert.ToSingle(arrange.posY)));
            Debug.Log(vector3);
            vector3 = new Vector3(vector3.x, vector3.y + 1, vector3.z);
            GameObject carObj = Resources.Load<GameObject>("Prefabs/3DArrange/Car/" + arrange.carName);
            GameObject _car = Instantiate(carObj, arrangeObjPath);
            _car.AddComponent<EntityBase>();
            _car.name = "Car"+i;
            _car.transform.position = vector3;
            _car.transform.localEulerAngles = new Vector3(_car.transform.localEulerAngles.x,Convert.ToSingle(arrange.rotate),_car.transform.localEulerAngles.z);
            carList.Add(_car);
        }
        for (int i = 0; i < XmlManger.GetInstance().toxiArangeList.Count; i++)
        {
            Toxi_Arrange arrange = XmlManger.GetInstance().toxiArangeList[i];
            Vector3 vector3 = terrainChangeMgr.GetTerrainPosByGis(arrange.pos);
            vector3 = new Vector3(vector3.x, vector3.y, vector3.z);
            GameObject toxiObj = Resources.Load<GameObject>("Prefabs/3DArrange/Toxi");
            GameObject _toxi = Instantiate(toxiObj, arrangeObjPath);
            _toxi.AddComponent<EntityBase>();
            _toxi.name = "Toxi" + i;
            _toxi.transform.localScale = new Vector3(arrange.range * 2, arrange.range,arrange.range * 2);
            _toxi.transform.position = vector3;
            toxiList.Add(_toxi);
        }
        for (int i = 0; i < XmlManger.GetInstance().radiateArangeList.Count; i++)
        {
            Radiate_Arrange arrange = XmlManger.GetInstance().radiateArangeList[i];
            Vector3 vector3 = terrainChangeMgr.GetTerrainPosByGis(arrange.pos);
            vector3 = new Vector3(vector3.x, vector3.y, vector3.z);
            GameObject radiateObj = Resources.Load<GameObject>("Prefabs/3DArrange/Radiate");
            GameObject _radiate = Instantiate(radiateObj, arrangeObjPath);
            _radiate.AddComponent<EntityBase>();
            _radiate.name = "Radiate" + i;
            _radiate.transform.localScale = new Vector3(arrange.range * 2, arrange.range, arrange.range * 2);
            _radiate.transform.position = vector3;
            radiateList.Add(_radiate);
        }
        for (int i = 0; i < XmlManger.GetInstance().craterArangeList.Count; i++)
        {
            Crater_Arrange arrange = XmlManger.GetInstance().craterArangeList[i];
            Vector3 vector3 = terrainChangeMgr.GetTerrainPosByGis(arrange.pos);
            vector3 = new Vector3(vector3.x, vector3.y, vector3.z);
            GameObject craterObj = Resources.Load<GameObject>("Prefabs/3DArrange/Crater/"+arrange.harmType);
            GameObject _crater = Instantiate(craterObj, arrangeObjPath);
            _crater.AddComponent<EntityBase>();
            _crater.name = "Crater" + i;
            _crater.transform.position = vector3;
            _crater.transform.localEulerAngles = arrange.rotate;
            craterList.Add(_crater);
        }

        //GameObject car = arrangeObjPath.transform.Find(carList[0].name).gameObject;
        //Vector3 vector = new Vector3(car.transform.position.x, car.transform.position.y + 20f, car.transform.position.z);
        //_camera.transform.localPosition = vector;
        //_camera.GetComponent<CameraMove>().mouseX = 0;
        //_camera.GetComponent<CameraMove>().mouseY = 90;
        Debug.Log(carList.Count);
    }
    /// <summary>
    /// 创建车辆、弹坑、毒剂、辐射列表按钮
    /// </summary>
    private void CreateList()
    {
        for (int i = 0; i < carList.Count; i++)
        {
            GameObject obj = Instantiate(item, content);
            obj.SetActive(true);
            obj.name = "Car" + i;
            obj.transform.Find("Text").GetComponent<Text>().text = XmlManger.GetInstance().carArangeList[i].showName;
            ButtonBase btnBase = obj.GetComponent<ButtonBase>();
            btnBase.RegistClick(ClickRightBtn);
        }
        for (int i = 0; i < toxiList.Count; i++)
        {
            GameObject obj = Instantiate(item, content);
            obj.SetActive(true);
            obj.name = "Toxi" + i;
            obj.transform.Find("Text").GetComponent<Text>().text = HarmType.GetTypeStr(XmlManger.GetInstance().toxiArangeList[i].harmType)+"毒区";
            ButtonBase btnBase = obj.GetComponent<ButtonBase>();
            btnBase.RegistClick(ClickRightBtn);
        }
        for (int i = 0; i < radiateList.Count; i++)
        {
            GameObject obj = Instantiate(item, content);
            obj.SetActive(true);
            obj.name = "Radiate" + i;
            obj.transform.Find("Text").GetComponent<Text>().text ="辐射区域"+i; 
            ButtonBase btnBase = obj.GetComponent<ButtonBase>();
            btnBase.RegistClick(ClickRightBtn);
        }
        for (int i = 0; i < craterList.Count; i++)
        {
            GameObject obj = Instantiate(item, content);
            obj.SetActive(true);
            obj.name = "Crater" + i;
            obj.transform.Find("Text").GetComponent<Text>().text = HarmType.GetTypeStr(XmlManger.GetInstance().craterArangeList[i].harmType) + "弹坑";
            ButtonBase btnBase = obj.GetComponent<ButtonBase>();
            btnBase.RegistClick(ClickRightBtn);
        }
    }
    /// <summary>
    /// 右侧列表收缩按钮方法
    /// </summary>
    /// <summary>
    private void ClickLListBtn()
    {
        Debug.Log("ClickBtn");
        isExpand = !isExpand;
        if (isExpand)
        {
           GameObject.Find("Arrange/Canvas/MessageView").transform.DOLocalMoveX(635f, 1f);
        }
        else
        {
            GameObject.Find("Arrange/Canvas/MessageView").transform.DOLocalMoveX(960f, 1f);
        }

    }
    /// <summary>
    /// 点击保存按钮 ，保存车辆、弹坑、毒剂、辐射位置信息
    /// </summary>
    private void ClickSaveBtn()
    {

        int i = 0;
        foreach (GameObject _car in carList)
        {
            Vector2 vector2 = terrainChangeMgr.gisPointMgr.GetGisPos(_car.transform.position);
            Car_Arrange a = XmlManger.GetInstance().carArangeList[i];
            Debug.Log(vector2.x);
            a.posX = vector2.x.ToString();
            a.posY = vector2.y.ToString();
            a.rotate = (_car.transform.eulerAngles.y % 360).ToString();
            i++;
        }
        i = 0;
        foreach (GameObject _toxi in toxiList)
        {
            Vector2 vector2 = terrainChangeMgr.gisPointMgr.GetGisPos(_toxi.transform.position);
            Toxi_Arrange a = XmlManger.GetInstance().toxiArangeList[i];
            a.pos = vector2;
            i++;
        }
        i = 0;
        foreach (GameObject _radiate in radiateList)
        {
            Vector2 vector2 = terrainChangeMgr.gisPointMgr.GetGisPos(_radiate.transform.position);
            Radiate_Arrange a = XmlManger.GetInstance().radiateArangeList[i];
            a.pos = vector2;
            i++;
        }
        i = 0;
        foreach (GameObject _crater in craterList)
        {
            Vector2 vector2 = terrainChangeMgr.gisPointMgr.GetGisPos(_crater.transform.position);
            Crater_Arrange a = XmlManger.GetInstance().craterArangeList[i];
            a.pos = vector2;
            a.rotate = new Vector3(_crater.transform.eulerAngles.x % 360,
                _crater.transform.eulerAngles.y % 360,
                _crater.transform.eulerAngles.z % 360);
            Debug.Log("_carter:" + a.rotate);
            i++;
        }
        XmlManger.GetInstance().WriteXML();       
        ArrangeUiMgr.GetInstance().OpenView(ArrangeViewType.ConfirmView);
    }
    /// <summary>
    /// 点击提示按钮
    /// </summary>
    private void ClickControlTipBtn()
    {
        ArrangeUiMgr.GetInstance().OpenView(ArrangeViewType.ControlTipView);
    }
    /// <summary>
    /// 点击右侧列表按钮
    /// </summary>
    /// <param name="obj"></param>
    private void ClickRightBtn(GameObject obj)
    {
        if (Time.time - time <= 0.3f&&choiceBtn==obj.name)
        {
            ChangeCamera(obj);
        }
        else
        {
            choiceBtn = obj.name;
            time = Time.time;
        }
        targetObjMethod.ChoiceObj(arrangeObjPath.transform.Find(obj.name).gameObject);
    }
    /// <summary>
    /// 点击初始点按钮
    /// </summary>
    private void ClickStartPosBtn()
    {
        if (GameObject.Find("Environment/Start") == null)
        {
            startPosBtn.gameObject.SetActive(false);
            return;
        }
        GameObject startObj = GameObject.Find("Environment/Start");
        Vector3 startPos = startObj.transform.position;
        Vector3 startRot = startObj.transform.localEulerAngles;
        CameraMove cameraMove = _camera.GetComponent<CameraMove>();
        cameraMove.mouseX = startObj.transform.rotation.x;
        cameraMove.mouseY = startObj.transform.rotation.y;
        cameraMove.transform.position = startPos; 
    }
    /// <summary>
    /// 改变摄像机的目标点
    /// </summary>
    /// <param name="obj"></param>
    private void ChangeCamera(GameObject obj)
    {
        GameObject arrangeObj = arrangeObjPath.transform.Find(obj.name).gameObject;
        Vector3 vector = Vector3.zero;
        if (obj.name.Contains("Toxi") || obj.name.Contains("Radiate"))
        {
            vector = new Vector3(arrangeObj.transform.position.x, arrangeObj.transform.position.y + (arrangeObj.transform.localScale.x * 1.5f), arrangeObj.transform.position.z);
        }
        else
        {
            vector = new Vector3(arrangeObj.transform.position.x, arrangeObj.transform.position.y + 20f, arrangeObj.transform.position.z);
        }
        CameraMove cameraMove = _camera.GetComponent<CameraMove>();
        cameraMove.mouseX = 0;
        cameraMove.mouseY = 90;
        cameraMove.transform.position = vector;
    }
    /// <summary>
    /// 选中目标，显示位置工具
    /// </summary>
    /// <param name="btn"></param>
    public void ChoiceState(GameObject btn)
    {
        if (content != null&&content.childCount > 0)
        {
            for (int i = 0; i < content.childCount; i++)
            {
                if (content.GetChild(i).name == btn.name)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(content.GetChild(i).gameObject);
                }
            }
        }
    }
    
}
