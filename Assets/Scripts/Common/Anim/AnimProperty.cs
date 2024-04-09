using UnityEngine;

/// <summary>
/// 动画属性封装
/// </summary>
public class AnimProperty
{
    /// <summary>
    /// 动画属性
    /// </summary>
    private string animProperty;

    /// <summary>
    /// 动画属性哈希值
    /// </summary>
    private int animHash = int.MinValue;

    public int Value
    {
        get
        {
            if (animHash == int.MinValue)
            {
                animHash = Animator.StringToHash(animProperty);
            }
            return animHash;
        }
    }
    public AnimProperty(string property)
    {
        animProperty = property;
    }
}
