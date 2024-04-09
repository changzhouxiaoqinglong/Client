using UnityEngine;
using System;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
/// <summary>
/// 解析表数据
/// </summary>
public class ExcelSerializer
{
    public static List<T> Deserialize<T>(string excelName)
    {
        T[] array = (T[])CreateArray(typeof(T), ParseExcel(excelName));
        return new List<T>(array);
    }

    private static object CreateArray(Type type, List<string[]> rows)
    {
        Array array_value = Array.CreateInstance(type, rows.Count - 1);
        Dictionary<string, int> table = new Dictionary<string, int>();

        for (int i = 0; i < rows[0].Length; i++)
        {
            string id = rows[0][i];
            string id2 = "";
            for (int j = 0; j < id.Length; j++)
            {
                if ((id[j] >= 'a' && id[j] <= 'z') || (id[j] >= '0' && id[j] <= '9'))
                    id2 += ((char)id[j]).ToString();
                else if (id[j] >= 'A' && id[j] <= 'Z')
                    id2 += ((char)(id[j] - 'A' + 'a')).ToString();
            }

            table.Add(id, i);
            if (!table.ContainsKey(id2))
                table.Add(id2, i);
        }

        for (int i = 1; i < rows.Count; i++)
        {
            object rowdata = Create(rows[i], table, type);
            array_value.SetValue(rowdata, i - 1);
        }
        return array_value;
    }

    /// <summary>
    /// 读取表数据
    /// </summary>
    private static List<string[]> ParseExcel(string excelName)
    {
        List<string[]> res = new List<string[]>();
        TextAsset textAsset = Resources.Load<TextAsset>("Excels/" + excelName);
        string excelTxt = textAsset.text.Trim();
        string[] textRows = excelTxt.Split('\n');
        //从第三行开读
        for (int i = 2; i < textRows.Length; i++)
        {
            string str = textRows[i];
            string[] strs = str.Split('\t');
            for (int j = 0; j < strs.Length; j++)
            {
                strs[j] = strs[j].Trim().Trim('\"');
            }
            res.Add(strs);
        }
        return res;
    }

    private static object Create(string[] cols, Dictionary<string, int> table, Type type)
    {
        object v = Activator.CreateInstance(type);

        FieldInfo[] fieldinfo = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (FieldInfo tmp in fieldinfo)
        {
            if (table.ContainsKey(tmp.Name))
            {
                int idx = table[tmp.Name];
                if (idx < cols.Length)
                    SetValue(v, tmp, cols[idx]);
            }
        }
        return v;
    }

    private static void SetValue(object v, FieldInfo fieldinfo, string value)
    {
        if (value == null || value == "")
            return;

        if (fieldinfo.FieldType.IsArray)
        {
            Type elementType = fieldinfo.FieldType.GetElementType();
            string[] elem = value.Split(',');
            Array array_value = Array.CreateInstance(elementType, elem.Length);
            for (int i = 0; i < elem.Length; i++)
            {
                if (elementType == typeof(string))
                    array_value.SetValue(elem[i], i);
                else
                    array_value.SetValue(Convert.ChangeType(elem[i], elementType), i);
            }
            fieldinfo.SetValue(v, array_value);
        }
        else if (fieldinfo.FieldType.IsEnum)
            fieldinfo.SetValue(v, Enum.Parse(fieldinfo.FieldType, value.ToString()));
        else if (value.IndexOf('.') != -1 &&
            (fieldinfo.FieldType == typeof(Int32) || fieldinfo.FieldType == typeof(Int64) || fieldinfo.FieldType == typeof(Int16)))
        {
            float f = (float)Convert.ChangeType(value, typeof(float));
            fieldinfo.SetValue(v, Convert.ChangeType(f, fieldinfo.FieldType));
        }
#if UNITY_EDITOR
        else if (fieldinfo.FieldType == typeof(UnityEngine.Sprite))
        {
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(value.ToString());
            fieldinfo.SetValue(v, sprite);
        }
#endif
        else if (fieldinfo.FieldType == typeof(string))
            fieldinfo.SetValue(v, value);
        else
            fieldinfo.SetValue(v, Convert.ChangeType(value, fieldinfo.FieldType));
    }
}