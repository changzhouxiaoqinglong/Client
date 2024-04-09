
/// <summary>
/// tcp连接导控
/// </summary>
public class GuideTcpClient : TcpClient
{
    public GuideTcpClient(string ip, int port, ServerType ServerType) : base(ip, port, ServerType)
    {

    }
}
