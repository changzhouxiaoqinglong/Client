using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
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
        if (m_NetworkManager.Client != null)
        {
            if (m_NetworkManager.Client.IsConnected)
                GetComponent<RawImage>().DOFade(1, 0);
            else
                GetComponent<RawImage>().DOFade(0, 0);
        }
    }
}
