using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class JsonTool
{
    public static string ToJson(object obj)
    {
        var json = JsonConvert.SerializeObject(obj);
        return json;
    }

    /// <summary>
    /// 将Json字符串反序列化成目标类型对象。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    public static T ToObject<T>(string json)
    {
        var obj = JsonConvert.DeserializeObject<T>(json);
        return obj;
    }

    public static List<T> ToListObject<T>(string json)
    {
        var obj = JsonConvert.DeserializeObject<List<T>>(json);
        return obj;
    }

    public static object ToObject(string json)
    {
        var obj = JsonConvert.DeserializeObject(json);
        return obj;
    }

    public static object ToObject(string json, Type type)
    {
        var obj = JsonConvert.DeserializeObject(json, type);
        return obj;
    }
}
