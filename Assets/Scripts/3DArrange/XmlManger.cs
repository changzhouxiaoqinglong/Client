using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
public class XmlManger : MonoSingleTon<XmlManger>
{
    //xml操作
    public string filePath = String.Empty;//Xml文件路径
    public string battleSchemeID = string.Empty;//作战方案ID
    private List<string> twoPoint = new List<string>();//存放xml多余节点
    private XmlDocument xml = new XmlDocument();//xml操作

    //地区名字
    public string areaName = string.Empty;

    //方案节点
    private XmlElement battleSchemeNode;

    //Node节点
    private XmlNode areaNode = null;//地区节点
    private XmlNode carNode = null;//车节点
    private XmlNode toxiNode = null;//毒剂节点
    private XmlNode radiateNode = null;//辐射节点
    private XmlNode craterNode = null;//弹坑节点

    //各类型数组
    public List<Car_Arrange> carArangeList = new List<Car_Arrange>();//存放车类
    public List<Toxi_Arrange> toxiArangeList = new List<Toxi_Arrange>();//存放毒剂类
    public List<Radiate_Arrange> radiateArangeList = new List<Radiate_Arrange>();//存放辐射类
    public List<Crater_Arrange> craterArangeList = new List<Crater_Arrange>();//存放弹坑类

    public string errorMsg = string.Empty;
    /// <summary>
    /// 读取xml文件
    /// </summary>
    public void ReadXML()
    {
        Debug.Log("读取路径：" + filePath);
        if (File.Exists(filePath))
        {
            try
            {
                //将xml文件转为字符串数组(方便筛选出XMl其余根元素)
                byte[] data = File.ReadAllBytes(filePath);
                string xmlText = System.Text.Encoding.GetEncoding("GB2312").GetString(data);
                string[] strs = xmlText.Split('\n');

                //保留第一个根元素到新字符串数组内
                List<string> newstrs = new List<string>();
                foreach (string str in strs)
                {
                    if (!str.Contains("create") && !str.Contains("Create") && str != string.Empty)
                    {
                        newstrs.Add(str);
                    }
                    else
                    {
                        if (str == string.Empty) continue;
                        twoPoint.Add(str);
                    }
                }

                //将新的xml字符串数组合并成字符串，并根据字串加载Xml
                string xmlStr = string.Join("\n", newstrs);
                Debug.Log(xmlStr);
                xml.LoadXml(xmlStr);

                //查找方案地区
                areaNode = xml.SelectSingleNode("Experiment/Scenario/battleArea");
                XmlNodeList xmlNodeList = areaNode.ChildNodes;
                foreach (XmlElement x1 in xmlNodeList)
                {
                    if (x1.GetAttribute("name") != null && x1.GetAttribute("name") != string.Empty)
                    {
                        switch (x1.GetAttribute("name"))
                        {
                            case "丘陵":
                                areaName = "Hills";
                                //arange.areaName = "丘陵";
                                break;
                            case "山地":
                                areaName = "Mountain";
                                //arange.areaName = "山地";
                                break;
                            case "平原":
                                areaName = "Plain";
                                //arange.areaName = "平原";
                                break;
                            case "高原":
                                areaName = "Plateau";
                                //arange.areaName = "高原";
                                break;
                            case "盆地":
                                areaName = "Basin";
                                //arange.areaName = "盆地";
                                break;
                            case "沙漠戈壁":
                                areaName = "Gobi";
                                //arange.areaName = "戈壁";
                                break;
                            case "城市":
                                areaName = "city";
                                //arange.areaName = "城市";
                                break;
                            default:
                                areaName = null;
                                break;
                        }
                    }
                }
                if (areaName == string.Empty)
                {
                    errorMsg = "找不到方案地区";
                    return;
                }

                //指定作战方案
                foreach (XmlElement x1 in xml.SelectSingleNode("Experiment").ChildNodes)
                {
                    Debug.Log(x1.InnerXml);
                    if (x1.GetAttribute("ID") == battleSchemeID)
                    {
                        battleSchemeNode = x1;
                    }
                }
                if (battleSchemeNode == null)
                {
                    errorMsg = "找不到指定作战方案ID";
                    return;
                }

                //查找车名和车位置和旋转角度
                carNode = battleSchemeNode.SelectSingleNode("battleGroup/redGroup");
                if (carNode != null)
                {
                    Debug.Log(carNode.InnerXml);
                    Debug.Log(carNode.ChildNodes);
                    xmlNodeList = carNode.ChildNodes;

                    foreach (XmlElement x1 in xmlNodeList)
                    {
                        Debug.Log(x1.InnerXml);
                        if (x1.GetAttribute("posX") == "-999999") continue;
                        if (x1.GetAttribute("name") != null && x1.GetAttribute("name") != string.Empty)
                        {
                            string carStr;
                            if (x1.GetAttribute("name").Contains("02B"))
                            {
                                carStr = "02B";
                            }
                            else if (x1.GetAttribute("name").Contains("102"))
                            {
                                carStr = "102";
                            }
                            else if (x1.GetAttribute("name").Contains("384C"))
                            {
                                carStr = "384C";
                            }
                            else
                            {
                                continue;
                            }
                            Car_Arrange arrange3D = new Car_Arrange()
                            {
                                id = x1.GetAttribute("id"),
                                carName = carStr,
                                showName = x1.GetAttribute("name"),
                                posX = x1.GetAttribute("posX"),
                                posY = x1.GetAttribute("posY"),
                                rotate = x1.GetAttribute("rotate")
                            };
                            carArangeList.Add(arrange3D);
                            Debug.Log("carlist+1");
                        }
                        else
                        {
                            continue;
                        }
                    }
                }


                //查找毒剂
                toxiNode = battleSchemeNode.SelectSingleNode("Toxi");
                if (toxiNode != null)
                {
                    xmlNodeList = toxiNode.ChildNodes;
                    foreach (XmlElement x1 in xmlNodeList)
                    {
                        string[] pos = x1.GetAttribute("Pos").Split(',');
                        Debug.Log(x1.InnerXml);
                        Toxi_Arrange arrange = new Toxi_Arrange()
                        {
                            id = x1.GetAttribute("id"),
                            harmType = Convert.ToInt32(x1.GetAttribute("Type")),
                            pos = new Vector2(Single.Parse(pos[0]), Single.Parse(pos[1])),
                            range = Convert.ToInt32(x1.GetAttribute("Range")),
                        };
                        toxiArangeList.Add(arrange);
                    }
                }

                //查找辐射
                radiateNode = battleSchemeNode.SelectSingleNode("Radiate");
                if (radiateNode != null)
                {
                    xmlNodeList = radiateNode.ChildNodes;
                    foreach (XmlElement x1 in xmlNodeList)
                    {
                        string[] pos = x1.GetAttribute("Pos").Split(',');
                        Radiate_Arrange arrange = new Radiate_Arrange()
                        {
                            id = x1.GetAttribute("ID"),
                            pos = new Vector2(Single.Parse(pos[0]), Single.Parse(pos[1])),
                            range = Convert.ToInt32(x1.GetAttribute("Range")),
                        };
                        radiateArangeList.Add(arrange);
                    }
                }

                //查找弹坑
                craterNode = battleSchemeNode.SelectSingleNode("Crater");
                if (craterNode != null)
                {
                    xmlNodeList = craterNode.ChildNodes;
                    foreach (XmlElement x1 in xmlNodeList)
                    {
                        string[] pos = x1.GetAttribute("Pos").Split(',');
                        Crater_Arrange arrange = new Crater_Arrange()
                        {
                            id = x1.GetAttribute("id"),
                            harmType = Convert.ToInt32(x1.GetAttribute("ToxiType")),
                            pos = new Vector2(Single.Parse(pos[0]), Single.Parse(pos[1])),
                        };
                        if (x1.GetAttribute("Rotate") == null || x1.GetAttribute("Rotate") == String.Empty)
                        {
                            arrange.rotate = Vector3.zero;
                        }
                        else
                        {
                            string[] rotateStr = x1.GetAttribute("Rotate").Split(',');
                            switch (rotateStr.Length)
                            {
                                case 0: arrange.rotate = Vector3.zero; break;
                                case 1: arrange.rotate = new Vector3(Single.Parse(rotateStr[0]), 0, 0); break;
                                case 2: arrange.rotate = new Vector3(Single.Parse(rotateStr[0]), Single.Parse(rotateStr[1]), 0); break;
                                case 3: arrange.rotate = new Vector3(Single.Parse(rotateStr[0]), Single.Parse(rotateStr[1]), Single.Parse(rotateStr[2])); break;
                                default: arrange.rotate = Vector3.zero; break;
                            }
                        }
                        Debug.Log(arrange.rotate);
                        craterArangeList.Add(arrange);
                    }
                }

                Debug.Log(carArangeList.Count);
                //Debug
                Debug.Log("场景名：" + areaName);
                foreach (Car_Arrange a in carArangeList)
                {
                    Debug.Log("车名：" + a.carName);
                    Debug.Log("posX：" + a.posX);
                    Debug.Log("posY：" + a.posY);
                    Debug.Log("Rotate：" + a.rotate);
                }
                foreach (Toxi_Arrange a in toxiArangeList)
                {
                    Debug.Log(a);
                }
                foreach (Radiate_Arrange a in radiateArangeList)
                {
                    Debug.Log(a);
                }
                foreach (Crater_Arrange a in craterArangeList)
                {
                    Debug.Log(a);
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
        }
        else
        {
            errorMsg = "该路径读取不到XML文件";
            return;
        }
    }

    /// <summary>
    /// 写入xml
    /// </summary>
    /// <param name="vector2">经纬度</param>
    /// <param name="rotate">旋转角度</param>
    public void WriteXML()
    {

        int i = 0;
        //写入车辆信息
        if (carNode != null)
        {
            foreach (XmlElement x1 in carNode.ChildNodes)
            {
                if (i + 1 > carArangeList.Count) break;
                x1.SetAttribute("posX", carArangeList[i].posX);
                x1.SetAttribute("posY", carArangeList[i].posY);
                x1.SetAttribute("rotate", carArangeList[i].rotate);
                i++;
            }
        }
        //写入毒剂信息
        if (toxiNode != null)
        {
            i = 0;
            foreach (XmlElement x1 in toxiNode.ChildNodes)
            {
                x1.SetAttribute("Pos", toxiArangeList[i].pos.x.ToString() + "," + toxiArangeList[i].pos.y.ToString());
                i++;
            }
        }
        //写入辐射信息
        if (radiateNode != null)
        {
            i = 0;
            foreach (XmlElement x1 in radiateNode.ChildNodes)
            {
                x1.SetAttribute("Pos", radiateArangeList[i].pos.x.ToString() + "," + radiateArangeList[i].pos.y.ToString());
                i++;
            }
        }
        //写入弹坑信息    
        if (craterNode != null)
        {
            i = 0;
            foreach (XmlElement x1 in craterNode.ChildNodes)
            {
                Debug.Log(x1.GetAttribute("Rotate"));
                x1.SetAttribute("Pos", craterArangeList[i].pos.x.ToString() + "," + craterArangeList[i].pos.y.ToString());
                x1.SetAttribute("Rotate", craterArangeList[i].rotate.x.ToString() + "," + craterArangeList[i].rotate.y.ToString() +","+ craterArangeList[i].rotate.z.ToString());
                Debug.Log(craterArangeList[i].rotate.x.ToString() + "," + craterArangeList[i].rotate.y.ToString() + "," + craterArangeList[i].rotate.z.ToString());
                Debug.Log(x1.GetAttribute("Rotate"));   
                i++;
            }
        }

        //将xml转为string
        string afterXMlText = ConvertXmlToString(xml);
        Debug.Log(afterXMlText);
        afterXMlText += "\n";
        //将第二个根节点拼接到字符串内
        afterXMlText += String.Join("\n", twoPoint);
        //转码为byte 格式为gb2312
        byte[] xmlTextbyte = System.Text.Encoding.GetEncoding("GB2312").GetBytes(afterXMlText);
        Debug.Log(System.Text.Encoding.GetEncoding("GB2312").GetString(xmlTextbyte));
        //覆写原文件
        File.WriteAllBytes(filePath, xmlTextbyte);
    }

    /// <summary>
    /// xml文档转为字符串
    /// </summary>
    /// <param name="xmldoc"></param>
    /// <returns></returns>
    private string ConvertXmlToString(XmlDocument xmldoc)
    {
        MemoryStream stream = new MemoryStream();
        XmlTextWriter writer = new XmlTextWriter(stream, System.Text.Encoding.GetEncoding("GB2312"));
        writer.Formatting = Formatting.Indented;
        xmldoc.Save(writer);
        StreamReader sr = new StreamReader(stream, System.Text.Encoding.GetEncoding("GB2312"));
        stream.Position = 0;
        string xmlString = sr.ReadToEnd();
        sr.Close();
        stream.Close();
        return xmlString;
    }
}

//车类
public class Car_Arrange
{
    //唯一标识
    public string id;
    //车名
    public string carName = string.Empty;
    //要显示的车名称
    public string showName = string.Empty;
    //经纬度坐标
    public string posX = string.Empty;
    public string posY = string.Empty;
    //旋转角度（Y）
    public string rotate = string.Empty;
}
//毒区类
public class Toxi_Arrange
{
    //唯一标识
    public string id;
    //毒类型
    public int harmType;
    //位置
    public Vector2 pos;
    //范围
    public int range;
}
//辐射类
public class Radiate_Arrange
{
    //唯一标识
    public string id;
    //位置
    public Vector2 pos;
    //范围
    public int range;
}
//弹坑类
public class Crater_Arrange
{
    //唯一标识
    public string id;
    //毒类型
    public int harmType;
    //位置
    public Vector2 pos;
    //旋转角
    public Vector3 rotate;
}
//毒类型
public class HarmType
{
    /// <summary>
    /// 无毒
    /// </summary>
    public const int NODRUG = 1;
    /// <summary>
    /// 沙林
    /// </summary>
    public const int SHALIN = 2;
    /// <summary>
    /// 梭曼
    /// </summary>
    public const int SOMAN = 3;
    /// <summary>
    /// 芥子气
    /// </summary>
    public const int JIEZIQI = 4;
    /// <summary>
    /// VX
    /// </summary>
    public const int VX = 5;
    /// <summary>
    /// DMMP
    /// </summary>
    public const int DMMP = 6;

    public static string GetTypeStr(int type)
    {
        switch (type)
        {
            case NODRUG:
                return "无毒";
            case SHALIN:
                return "沙林";
            case SOMAN:
                return "梭曼";
            case JIEZIQI:
                return "芥子气";
            case VX:
                return "VX";
            case DMMP:
                return "DMMP";
            default:
                return "";
        }
    }
}

//题目选项毒类型
public class QstPoisonType
{
    /// <summary>
    /// 路易试剂
    /// </summary>
    public const int LUYISHIJI = 1;

    /// <summary>
    /// 沙林
    /// </summary>
    public const int SHALIN = 2;

    /// <summary>
    /// 芥路混合
    /// </summary>
    public const int JIELUHUNHE = 3;

    /// <summary>
    /// 芥子气
    /// </summary>
    public const int JIEZIQI = 4;

    /// <summary>
    /// 氢氯酸
    /// </summary>
    public const int QINGLVSUAN = 5;

    /// <summary>
    /// 光气
    /// </summary>
    public const int GUANGQI = 6;

    /// <summary>
    /// 不显色
    /// </summary>
    public const int NOCOLOR = 7;

    /// <summary>
    /// VX
    /// </summary>
    public const int VX = 8;

    public static string GetPoisonType(int poison)
    {
        switch (poison)
        {
            case LUYISHIJI:
                return "路易试剂";
            case SHALIN:
                return "沙林";
            case JIELUHUNHE:
                return "芥路混合";
            case JIEZIQI:
                return "芥子气";
            case QINGLVSUAN:
                return "氢氯酸";
            case GUANGQI:
                return "光气";
            case NOCOLOR:
                return "未检测出毒";
            case VX:
                return "VX";
            default:
                return "";
        }
    }

}