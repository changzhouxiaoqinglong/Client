using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMReceive102YaoCeScreen : MonoBehaviour
{
    private FMNetworkManager m_NetworkManager;

    private GameViewDecoder decoder;

    /// <summary>
    /// 同步ui大小适配
    /// </summary>
    private FitReceiveScreen fitScreen;

    private void Awake()
    {
        m_NetworkManager = transform.Find("Manager").GetComponent<FMNetworkManager>();
        decoder = transform.Find("Decoder").GetComponent<GameViewDecoder>();
        fitScreen = GetComponent<FitReceiveScreen>();
        m_NetworkManager.ServerListenPort = NetConfig.FM_102yaoce_SERVER_PORT;
        m_NetworkManager.ClientListenPort = NetConfig.FM_102yaoce_CLIENT_PORT;
    }

    private void Update()
    {
        if (decoder.ReceivedTexture != null)
        {
            fitScreen.FitScreen(decoder.ReceivedTexture.width, decoder.ReceivedTexture.height);
        }
    }
}
