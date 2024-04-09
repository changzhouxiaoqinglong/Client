using UnityEngine;

public class SingleTons : MonoBehaviour
{
    private static SingleTons instance;

    public static SingleTons GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// 添加单例
    /// </summary>
    /// <typeparam name="T">单例类型</typeparam>
    public T AddSingleTon<T>() where T : MonoBehaviour
    {
        T singleTon = GetComponent<T>();
        if (singleTon)
        {
            return singleTon;
        }
        else
        {
            return gameObject.AddComponent<T>();
        }
    }
}
