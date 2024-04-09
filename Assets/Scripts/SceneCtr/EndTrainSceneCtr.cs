
public class EndTrainSceneCtr : SceneCtrBase
{
    protected override void Start()
    {
        base.Start();
        UIMgr.GetInstance().OpenView(ViewType.ScoreView);
    }
}
