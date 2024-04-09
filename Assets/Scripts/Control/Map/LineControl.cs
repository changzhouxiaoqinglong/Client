using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineControl : MonoBehaviour
{
    public static LineControl Instance;

    private GameObject lineModel;

    private List<GameObject> lines = new List<GameObject>();
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        lineModel = transform.GetChild(0).gameObject;
    }

    private void DestroyLine()
    {
        for(int i = 1;i<transform.childCount;i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        lines.Clear();
    }

    /// <summary>
    /// 删除所有的线并创建所有的线
    /// </summary>
    /// <param name="carPos"></param>
    public void CreateAllLine(Vector3 carPos)
    {
        DestroyLine();
        List<GameObject> cubeObj = PointControl.Instance.GetCubeObjList();
        if (cubeObj.Count == 0) return;

        CreateLine(carPos, cubeObj[0].transform.localPosition);
        for(int i = 1;i<cubeObj.Count;i++)
        {
            Vector3 startPos = cubeObj[i - 1].transform.localPosition;
            Vector3 endPos = cubeObj[i].transform.localPosition;
            CreateLine(startPos,endPos);
        }
    }


    /// <summary>
    /// 创建一条线
    /// </summary>
    /// <param name="startPos">开始位置</param>
    /// <param name="endPos">终点位置</param>
    private void CreateLine(Vector3 startPos,Vector3 endPos)
    {
        GameObject obj = Instantiate(lineModel, transform);
        RectTransform objRect = obj.GetComponent<RectTransform>();
        obj.SetActive(true);
        obj.transform.localPosition = new Vector3(startPos.x + objRect.sizeDelta.y / 2, startPos.y, startPos.z);
        objRect.sizeDelta = new Vector2(MathsMgr.PointDistance(startPos, endPos), objRect.sizeDelta.y);
        objRect.localEulerAngles = new Vector3(0, 0, MathsMgr.PointAngle(startPos, endPos));
        lines.Add(obj);
    }

    public List<GameObject> GetLineList()
    {
        return lines;
    }
}
