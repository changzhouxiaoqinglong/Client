
public class ResBase
{
    public int Code;

    /// <summary>
    /// 提示消息
    /// </summary>

    public string Tip;

    public bool IsSuccess()
    {
        return Code == NetResCode.RES_SUCCESS;
    }
}
