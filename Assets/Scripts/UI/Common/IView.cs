
public interface IView
{
    ViewType ViewType { get; set; }

    /// <summary>
    /// 关闭面板
    /// </summary>
    void Close();

    /// <summary>
    /// 打开了
    /// </summary>
    void OnOpen(IViewParam param);
}
