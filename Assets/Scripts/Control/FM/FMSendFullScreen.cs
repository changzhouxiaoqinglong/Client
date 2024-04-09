
using UnityEngine;

/// <summary>
/// 同步 发送全屏画面
/// </summary>
public class FMSendFullScreen : MonoBehaviour
{
    private FMNetworkManager m_NetworkManager;

    private void Awake()
    {
        m_NetworkManager = transform.Find("Manager").GetComponent<FMNetworkManager>();
        m_NetworkManager.ServerListenPort = NetConfig.FM_SERVER_FULL_PORT;
        m_NetworkManager.ClientListenPort = NetConfig.FM_CLIENT_FULL_PORT;
    }
}
