
using System.Collections.Generic;

/// <summary>
/// 表数据管理基类
/// </summary>
/// <typeparam name="Tmgr">子类</typeparam>
/// <typeparam name="Tdata">管理的数据类型</typeparam>
public class ExDataMgrBase<Tmgr, Tdata> : SingleTonBase<Tmgr>
    where Tmgr : ExDataMgrBase<Tmgr, Tdata>, new()
    where Tdata : ExDataBase, new()
{
    /// <summary>
    /// 表名
    /// </summary>
    protected virtual string FILE_NAME
    {
        get; set;
    }

    //数据集
    public Dictionary<int, Tdata> dataDic = new Dictionary<int, Tdata>();

    //数据list
    public List<Tdata> dataList = new List<Tdata>();

    public ExDataMgrBase()
    {
        InitData();
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    private void InitData()
    {
      //  Logger.LogWarning("FILE_NAME: " + FILE_NAME);
        if(FILE_NAME== "QuestionData")
        dataDic.Clear();
        dataList = ExcelSerializer.Deserialize<Tdata>(FILE_NAME);
        
        foreach (var item in dataList)
        {
            dataDic.Add(item.Id, item);
        }
    }

    public Tdata GetDataById(int id)
    {
        return dataDic[id];
    }
}
