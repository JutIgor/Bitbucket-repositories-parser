using System.Collections.Generic;
using System.IO;

namespace DownloadService
{
    public class RepositoriesReader
    {
        public static List<string> ReadRepositoreis(string fileName)
        {
            return new List<string>(File.ReadAllLines(fileName, System.Text.Encoding.Unicode));
        }
    }
}