using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.UI.Dropdown;

public class UiArrange : MonoBehaviour
{
    private Dropdown dropDown;
    private Button enterBtn;
    private FileInfo[] xmlFiles;
    private Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.transform.Find("Panel/Text").GetComponent<Text>();
        dropDown = gameObject.transform.Find("Panel/Dropdown").GetComponent<Dropdown>();
        enterBtn = gameObject.transform.Find("Panel/EnterBtn").GetComponent<Button>();
        enterBtn.onClick.AddListener(ReadXml);
        GetXMLFileName();
    }
    private void GetXMLFileName()
    {
        Debug.Log("StartSearch");
        if (Directory.Exists(FilePathConfig.FilePath))
        {
            DirectoryInfo dir = new DirectoryInfo(FilePathConfig.FilePath);
            xmlFiles = dir.GetFiles("*", SearchOption.AllDirectories);
            foreach (FileInfo file in xmlFiles)
            {
                OptionData option = new OptionData(file.Name);
                dropDown.options.Add(option);
            }
            dropDown.RefreshShownValue();
        }
        else
        {
            Debug.Log("FilePathError!");
        }
    }
    private void ReadXml()
    {
        XmlManger.GetInstance().filePath = xmlFiles[dropDown.value].ToString();
        //SceneManager.LoadScene(XmlManger.GetInstance().areaStr);
        gameObject.SetActive(false);
        ArrangeUiMgr.GetInstance().OpenView(ArrangeViewType.LoadingView);
    }
}
