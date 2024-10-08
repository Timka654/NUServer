namespace NUServer.Utils
{
    public static class DirectoryUtils
    {
        public static string CreateFileDirectoryIfNoExists(this string path)
        { 
            var dir = Path.GetDirectoryName(path);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return path;
        }
    }
}
