/// <summary>
/// 不继承MonoBehaviour的单例基类
/// </summary>
public class SingleTonBase<T> where T : SingleTonBase<T>, new()
{
    private static T instance;
    private static readonly object sysLock = new object();

    public static T GetInstance()
    {
        lock (sysLock)
        {
            if (instance == null)
            {
                instance = new T();
            }
        }
        return instance;
    }
}
