using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointControl : MonoBehaviour
{
    public static PointControl Instance;

    private List<GameObject> cubeObj = new List<GameObject>();

    private Transform cubeModel;

    private Vector2 uiPos;
    private void Awake()
    {
        Instance = this;
        cubeModel = transform.GetChild(0);
    }
    public List<GameObject> GetCubeObjList()
    {
        return cubeObj;
    }
 
    /// <summary>
    /// 屏幕坐标转世界坐标并且赋值
    /// </summary>
    /// <param name="mapTrans"></param>
    /// <param name="mousePos"></param>
    public void SetCubeUiPos(RectTransform mapTrans,Vector2 mousePos)
    {
        Vector2 uiPos;
        bool isflag = MathsMgr.MousePosChangeUiPos(mapTrans, mousePos, out uiPos);
        this.uiPos = uiPos;
    }


    /// <summary>
    /// 创建点
    /// </summary>
    public void CreatePoint()
    {
        GameObject obj = Instantiate(cubeModel.gameObject, transform);
        obj.transform.localPosition = uiPos;
        obj.SetActive(true);
        cubeObj.Add(obj);
    }

    
    /// <summary>
    /// 是否删除点
    /// </summary>
    /// <returns></returns>
    public GameObject IsDeletePoint()
    {
        for (int i = 0; i < cubeObj.Count; i++)
        {
            GameObject cube = cubeObj[i];
            float x = cube.transform.localPosition.x;
            float y = cube.transform.localPosition.y;
            float sizeX = cube.GetComponent<RectTransform>().sizeDelta.x;
            float sizeY = cube.GetComponent<RectTransform>().sizeDelta.y;
            if (uiPos.x >= x - sizeX && uiPos.x <= x + sizeX && uiPos.y >= y - sizeY && uiPos.y <= y + sizeY)
            {
                return cube;
            }
        }
        return null;
    }

    /// <summary>
    /// 删除目标点
    /// </summary>
    /// <param name="cube"></param>
    public void DeletePoint(GameObject cube)
    {
        cubeObj.Remove(cube);
        Destroy(cube);
    }
}
