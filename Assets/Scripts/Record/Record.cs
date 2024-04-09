using RenderHeads.Media.AVProMovieCapture;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using System.Globalization;

public class Record : MonoSingleTon<Record>
{
    private const string TAG = "[Record]:";

    CaptureBase _movieCapture;

    string FTPHost = "ftp://127.0.0.1/";

    //ftp服务器的账户密码
    string FTPUserName = "admin";
    string FTPPassword = "123456";
    string FilePath = string.Empty;

    public bool isUpLoading = false;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        if (_movieCapture == null)
        {
            _movieCapture = gameObject.AddComponent<CaptureFromScreen>();
        }

        //设置格式
        _movieCapture._codecName = "Media Foundation H.264(MP4)";
        _movieCapture._useMediaFoundationH264 = true;
        _movieCapture._noAudio = true;

        //录屏时存储的视频文件
        FilePath = Application.streamingAssetsPath + "/Record/test.mp4";

        yield return null;
    }

    public void StartCapture()
    {
        if (_movieCapture != null)
        {
            _movieCapture.StartCapture();
        }
    }

    public void StopCapture()
    {
        StartCoroutine(StopCaptureAndUpLoad());
    }

    IEnumerator StopCaptureAndUpLoad()
    {
        if (_movieCapture != null)
        {
            _movieCapture.StopCapture();
        }
        //等待录制文件生成
        yield return new WaitForSeconds(0.5f);

        //Debug.Log("--------------- 开始上传文件");
        isUpLoading = true;

        //获取导控的ip地址
        FTPHost = "ftp://" + NetConfig.GUIDE_IP + "/";
        //上传到服务端的名称 训练id+席位号AppConfig.SeatId
        string saveName = NetVarDataMgr.GetInstance()._NetVarData._TrainStartModel.TrainID + "-" + AppConfig.SEAT_ID + ".mp4";
        UploadFile(saveName);
    }

    void UploadFile(string saveName)
    {
        WebClient client = new System.Net.WebClient();

        Uri uri = new Uri(FTPHost + saveName);

        client.UploadProgressChanged += new UploadProgressChangedEventHandler(OnFileUploadProgressChanged);
        client.UploadFileCompleted += new UploadFileCompletedEventHandler(OnFileUploadCompleted);
        client.Credentials = new System.Net.NetworkCredential(FTPUserName, FTPPassword);

        client.UploadFileAsync(uri, "STOR", FilePath);
    }

    void OnFileUploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
    {
        Logger.LogDebug(TAG + "Uploading Progreess: " + e.ProgressPercentage);
    }

    void OnFileUploadCompleted(object sender, UploadFileCompletedEventArgs e)
    {
        Logger.LogDebug(TAG + "File Uploaded");
        Logger.LogDebug(TAG + "------------- " + e.Error);
        try
        {
            //删除本地文件
            File.Delete(FilePath);
        }
        catch (Exception error)
        {
            Logger.LogError("delete record error:" + error.ToString());
        }


        isUpLoading = false;
        Loom.GetInstance().RunAsync(CheckDelete);
    }

    /// <summary>
    /// 检测文件超期 删除
    /// </summary>
    private void CheckDelete()
    {
        try
        {
            //ftp请求
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(FTPHost);
            request.Credentials = new NetworkCredential(FTPUserName, FTPPassword);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream());
            //ftp目录数据
            string directList = sr.ReadToEnd();
            Logger.LogDebug(TAG + "ftpDirData:" + directList);
            sr.Close();
            response.Close();
            //每个文件数据
            string[] fileLines = directList.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var fileLine in fileLines)
            {
                string[] fileInfos = fileLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                //文件名  20230624111101219-1.mp4 用名字里的时间，因为filezilla创建的ftp 时间没有年份信息
                string fileName = fileInfos[fileInfos.Length - 1];
                string[] fileStrs = fileName.Split('-');
                if (fileStrs.Length > 1 && !string.IsNullOrEmpty(fileStrs[0]))
                {
                    //取前14位时间
                    string dateStr = fileStrs[0].Substring(0, 14);
                    DateTime date;
                    if (DateTime.TryParseExact(dateStr, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                    {
                        TimeSpan ts = DateTime.Now - date;
                        //文件超期了 要删除
                        if (ts.TotalDays >= NetConfig.FTP_SAVE_TIME)
                        {
                            //删除请求
                            FtpWebRequest deleteRequest = (FtpWebRequest)WebRequest.Create(FTPHost + fileName);
                            deleteRequest.Credentials = new NetworkCredential(FTPUserName, FTPPassword);
                            deleteRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                            FtpWebResponse deleteResponse = (FtpWebResponse)deleteRequest.GetResponse();
                            deleteResponse.Close();
                            Logger.LogDebug(TAG + "FtpOutTime delete: " + fileName);
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Logger.LogWarning(TAG + e.ToString());
        }
    }
}
