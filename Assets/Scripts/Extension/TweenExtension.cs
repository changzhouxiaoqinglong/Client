using DG.Tweening;
/// <summary>
/// tween扩展
/// </summary>
public static class TweenExtension
{
    /// <summary>
    /// 动画队列连接
    /// </summary>
    public static Sequence Append(this Tweener self, Tween tween)
    {
        return DOTween.Sequence().Append(self).Append(tween);
    }

    /// <summary>
    /// 动画队列 加入延迟
    /// </summary>
    public static Sequence AppendInternal(this Tweener self, float internalTime)
    {
        return DOTween.Sequence().Append(self).AppendInterval(internalTime);
    }
}
