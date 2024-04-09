using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ArrangeMain : MonoBehaviour
{
    void Start()
    {
        try
        {
            string[] CommandLineArgs = Environment.GetCommandLineArgs();
            string pathstr = string.Empty;
            foreach (string arg in CommandLineArgs)
            {
                Debug.Log("Commond : " + arg);
                pathstr += "+" + arg;
            }

            GameObject canvas = GameObject.Find("Canvas");
            GameObject view = canvas.transform.Find("ErrorView").gameObject;
            Text viewText = canvas.transform.Find("ErrorView/Content/desc").GetComponent<Text>();
            //test
            //XmlManger.GetInstance().filePath = "D:\\FZC02B战术训练_单车道路辐射_车1.xml";
            //XmlManger.GetInstance().battleSchemeID = "20230708193310023";
            ////
            string[] strs = CommandLineArgs[1].Split('%');
            XmlManger.GetInstance().filePath = strs[1];
            XmlManger.GetInstance().battleSchemeID = strs[2];

            XmlManger.GetInstance().ReadXML();
            if (XmlManger.GetInstance().errorMsg != string.Empty)
            {
                view.SetActive(true);
                viewText.text = XmlManger.GetInstance().errorMsg;
                return;
            }
            ArrangeUiMgr.GetInstance().OpenView(ArrangeViewType.LoadingView);
        }
        catch (Exception ex)
        {
            GameObject canvas = GameObject.Find("Canvas");
            canvas.transform.Find("ErrorView").gameObject.SetActive(true);
            canvas.transform.Find("ErrorView/Content/desc").GetComponent<Text>().text += ex.Message;
        }
    }
}
