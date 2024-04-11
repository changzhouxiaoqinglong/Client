public interface IEventParam { }

public class StringEvParam : IEventParam
{
    public string value;
    public int index;
    public StringEvParam(string value,int _index=-1)
    {
        this.value = value;       
        this.index = _index;
    }
}

public class IntEvParam : IEventParam
{
    public int value;
    public IntEvParam(int value)
    {
        this.value = value;
    }
}

public class FloatEvParam : IEventParam
{
    public float value;
    public FloatEvParam(float value)
    {
        this.value = value;
    }
}

public class ObjectEvParam : IEventParam
{
    public object value;
    public ObjectEvParam(object value)
    {
        this.value = value;
    }
}

/// <summary>
/// tcp接收消息 事件参数
/// </summary>
public class TcpReceiveEvParam : IEventParam
{
    public NetData netData;

    public TcpReceiveEvParam(NetData netData)
    {
        this.netData = netData;
    }
}

/// <summary>
/// 点击消息界面标签 事件参数
/// </summary>
public class ClickMsgTitleEvParam : IEventParam
{
    public MsgTitleType type;

    public ClickMsgTitleEvParam(MsgTitleType type)
    {
        this.type = type;
    }
}

/// <summary>
/// 相机切换
/// </summary>
public class CameraExchangeEvParam : IEventParam
{
    public MainCameraItemBase cameraItem;

    public CameraExchangeEvParam(MainCameraItemBase cameraItem)
    {
        this.cameraItem = cameraItem;
    }
}

/// <summary>
/// 侦察结果
/// </summary>
public class DetectResParam : IEventParam
{
    /// <summary>
    /// 类型
    /// </summary>
    public DetectResType type;

    /// <summary>
    /// 结果
    /// </summary>
    public string res;

    public DetectResParam(DetectResType type, string res)
    {
        this.type = type;
        this.res = res;
    }
}