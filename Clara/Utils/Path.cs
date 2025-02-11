namespace Clara.Utils
{
    public static class Path
    {
        public static void Create(string path, bool isFilePath)
        {
            if (isFilePath)
            {
                if (path.Contains("/") || path.Contains("\\"))
                {
                    string? directory = System.IO.Path.GetDirectoryName(path);

                    if (directory != null && !Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                }

                if (!File.Exists(path))
                {
                    File.Create(path).Close();
                }
            }
            else
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
        }
    }
}
