/// <summary>
/// 用户数据
/// </summary>
public class UserInfo
{
    public string userName = null;
    public string passWord = null;

    public bool IsSelf()
    {
        return NetVarDataMgr.GetInstance()._NetVarData._UserInfo == this;
    }
}
