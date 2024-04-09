
public class PauseView : ViewBase<PauseViewModel>
{
    protected override void OnEnable()
    {
        base.OnEnable();
        //暂停
        TimeTool.Pause();
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.GUIDE_PROCESS_CTR, OnGetGuideProcessMsg);
    }

    private void OnGetGuideProcessMsg(IEventParam param)
    {
        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            GuideProcessCtrModel model = JsonTool.ToObject<GuideProcessCtrModel>(tcpReceiveEvParam.netData.Msg);
            switch (model.Operate)
            {
                case GuideProcessType.Continue:
                case GuideProcessType.End:
                    CloseThis();
                    break;
            }
        }
    }


    protected override void OnDisable()
    {
        base.OnDisable();
        //解除暂停
        TimeTool.UnPause();
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.GUIDE_PROCESS_CTR, OnGetGuideProcessMsg);
    }
}
