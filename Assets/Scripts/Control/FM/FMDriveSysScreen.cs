using UnityEngine;

/// <summary>
/// 驾驶员同步画面给驾驶员 组件
/// </summary>
public class FMDriveSysScreen : MonoBehaviour
{
    private FMNetworkManager m_NetworkManager;

    private void Awake()
    {
        m_NetworkManager = transform.Find("Manager").GetComponent<FMNetworkManager>();
        m_NetworkManager.ServerListenPort = NetConfig.FM_SERVER_PORT;
        m_NetworkManager.ClientListenPort = NetConfig.FM_CLIENT_PORT;
    }
}
