
using Spore.DataBinding;
using UnityEngine;
/// <summary>
/// 训练界面VM层
/// </summary>
public class TrainViewModel : ViewModelBase
{

    /// <summary>
    /// 下车侦察到的 辐射剂量率 这里初始值不要设置为0  因为只有值改变了 才会触发刷新ui逻辑
    /// </summary>
    [AutoBinding(BindConstant.UpOutCarRadiomRate)]
    public BindableProperty<float> OutCarRadiomRate { get; set; } = new BindableProperty<float>(-1);

    /// <summary>
    /// 任务结束
    /// </summary>
    public void TrainEnd()
    {
        TaskMgr.GetInstance().ResportEndTask();
    }
}
