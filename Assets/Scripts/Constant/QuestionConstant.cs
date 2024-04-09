using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionConstant 
{
    /// <summary>
    /// 选项A
    /// </summary>
    public const int OPTIONA = 1;

    /// <summary>
    /// 选项B
    /// </summary>
    public const int OPTIONB = 2;

    /// <summary>
    /// 选项C
    /// </summary>
    public const int OPTIONC = 3;

    /// <summary>
    /// 选项D
    /// </summary>
    public const int OPTIOND = 4;

    /// <summary>
    /// 地面管子
    /// </summary>
    public const int TUBESCENE = 1;

    /// <summary>
    /// 空气管子
    /// </summary>
    public const int AIRSCENE = 2;

    /// <summary>
    /// 重新选管侦检题目ID
    /// </summary>
    public const int SELECTTUBE = 2;

    /// <summary>
    /// 设置抽气时间前最后一题ID
    /// </summary>
    public const int EXAMINEID = 4;

    /// <summary>
    /// 侦毒管显色题目ID
    /// </summary>
    public const int JUDGEID = 5;

    /// <summary>
    /// 结论题目ID
    /// </summary>
    public const int CONCLUSIONID = 6;

    /// <summary>
    /// 回答错误
    /// </summary>
    public const int ERRORAUDIO = 0;

    /// <summary>
    /// 回答正确
    /// </summary>
    public const int CORRECTAUDIO = 1;


}


public class BleedTimeConstant
{
    /// <summary>
    /// 最长时间180秒
    /// </summary>
    public const int MAXTIME = 180;

    /// <summary>
    /// 最长时间为0秒
    /// </summary>
    public const int MINTIME = 0;

    /// <summary>
    /// 每秒分针转动角度
    /// </summary>
    public const float SECONDANGLE = 1.5f;

    /// <summary>
    /// 时针转动速度
    /// </summary>
    public const float MOVETIME = 1.0f;

    /// <summary>
    /// 抽气时间正确最长时间为65秒
    /// </summary>
    public const int CORRECTMAXTIME = 65;

    /// <summary>
    /// 抽气时间正确最短时间为55秒
    /// </summary>
    public const int CORRECTMINTIME = 55;


}


