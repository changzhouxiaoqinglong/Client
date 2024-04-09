
using UnityEngine;
/// <summary>
/// FZC02B车型
/// </summary>
public class Car02b : CarBase
{
    /// <summary>
    /// 左后视镜
    /// </summary>
    public MeshRenderer LeftRearMirroRender;

    /// <summary>
    /// 左后视镜 非相机渲染材质（同步别的车不用相机渲染）
    /// </summary>
    public Material LeftRearMirrorNoneMaterial;

    protected override void InitRearMirror()
    {
        base.InitRearMirror();
        if (!IsSelfCar())
        {
            LeftRearMirroRender.material = LeftRearMirrorNoneMaterial;
        }
    }
}
