
using UnityEngine;

/// <summary>
/// 继承MonoBehaviour的单例
/// </summary>
public class MonoSingleTon<T> : MonoBehaviour where T : MonoSingleTon<T>
{
    private static T instance;
    private static readonly object sysLock = new object();

    public static T GetInstance()
    {
        lock (sysLock)
        {
            if (instance == null)
            {
                instance = SingleTons.GetInstance().AddSingleTon<T>();
            }
        }
        return instance;
    }
}
