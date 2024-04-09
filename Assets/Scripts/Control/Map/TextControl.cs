using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextControl : MonoBehaviour
{
    public static TextControl Instance;

    private GameObject textModel;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        textModel = transform.GetChild(0).gameObject;
    }


    /// <summary>
    /// 删除文本
    /// </summary>
    private void DestroyText()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// 创建文本
    /// </summary>
    public void CreateText(Vector3 terrainSize,Vector2 uiSize)
    {
        DestroyText();
        List<GameObject> lines = LineControl.Instance.GetLineList();
        List<GameObject> point = PointControl.Instance.GetCubeObjList();
        for(int i = 0; i < lines.Count; i++)
        {
            GameObject obj = Instantiate(textModel, transform);
            obj.SetActive(true);
            obj.transform.localPosition = point[i].transform.localPosition;
            Debug.Log(obj.transform.localEulerAngles);
            Text objText = obj.GetComponent<Text>();
            objText.text = ((int)lines[i].GetComponent<RectTransform>().sizeDelta.x * MathsMgr.UiTerrainRadio(terrainSize,uiSize).x).ToString() + "米";
        }
    }


}
