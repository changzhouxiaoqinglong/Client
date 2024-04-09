
public class FilePathConfig : ConfigBase<FilePathConfig>
{
    public static string FilePath;
    public static void InitConfig()
    {
        ParseConfigByReflection("FilePathConfig.cfg");
    }
}
