using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 同步遥测画面
/// </summary>
public class FMSendYaoCeScreen : MonoBehaviour
{
    private FMNetworkManager m_NetworkManager;

    private void Awake()
    {
        m_NetworkManager = transform.Find("Manager").GetComponent<FMNetworkManager>();
         m_NetworkManager.ServerListenPort = NetConfig.FM_102yaoce_SERVER_PORT;
         m_NetworkManager.ClientListenPort = NetConfig.FM_102yaoce_CLIENT_PORT;
      
    }
}
