using UnityEngine;

/// <summary>
/// 接收驾驶位同步画面 组件
/// </summary>
public class FMReceiveDriveScreen : MonoBehaviour
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
        m_NetworkManager.ServerListenPort = NetConfig.FM_SERVER_PORT;
        m_NetworkManager.ClientListenPort = NetConfig.FM_CLIENT_PORT;
    }

    private void Update()
    {
        if (decoder.ReceivedTexture != null)
        {
            fitScreen.FitScreen(decoder.ReceivedTexture.width, decoder.ReceivedTexture.height);
        }
    }
}
