
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

public abstract class ConfigBase<T> where T : ConfigBase<T>
{
    /// <summary>
    /// 通过反射解析配置数据
    /// </summary>
    public static void ParseConfigByReflection(string configFileName)
    {
        string configContext = AssetManager.ReadConfigText(configFileName);

        if (configContext != null)
        {
            string[] configs = configContext.Split('\n');
            Dictionary<string, string> configDic = new Dictionary<string, string>();

            foreach (var config in configs)
            {
                if (!config.Trim().StartsWith("//"))
                {
                    string[] pair = config.Split('=');
                    configDic.Add(pair[0].Trim(), pair[1].Trim());
                }
            }

            foreach (var pair in configDic)
            {
                FieldInfo fieldInfo = typeof(T).GetField(pair.Key, BindingFlags.Static | BindingFlags.Public);

                if (fieldInfo != null)
                {
                    if (fieldInfo.FieldType.IsSubclassOf(typeof(Enum)))
                    {
                        fieldInfo.SetValue(typeof(T), Enum.Parse(fieldInfo.FieldType, pair.Value));
                    }
                    else if (fieldInfo.FieldType == typeof(int))
                    {
                        fieldInfo.SetValue(typeof(T), int.Parse(pair.Value));
                    }
                    else if (fieldInfo.FieldType == typeof(string))
                    {
                        fieldInfo.SetValue(typeof(T), pair.Value);
                    }
                    else if (fieldInfo.FieldType == typeof(bool))
                    {
                        fieldInfo.SetValue(typeof(T), bool.Parse(pair.Value));
                    }
                    else if (fieldInfo.FieldType == typeof(float))
                    {
                        fieldInfo.SetValue(typeof(T), float.Parse(pair.Value));
                    }
                    //Logger.LogDebug(fieldInfo.Name + " " + fieldInfo.GetValue(typeof(T)));
                }
            }
        }
    }
}
