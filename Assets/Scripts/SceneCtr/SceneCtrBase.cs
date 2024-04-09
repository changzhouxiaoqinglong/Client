
public class SceneCtrBase : UnityMono
{
    protected override void Start()
    {
        base.Start();
        SceneMgr.GetInstance().SetCurScene(this);
    }
}
