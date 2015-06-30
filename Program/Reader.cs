using System.Collections.Generic;
using System.IO;

namespace Program
{
    public class Reader
    {
        public static IEnumerable<string> ReadFile(string fileName)
        {
            var content = new List<string>(File.ReadAllLines(fileName, System.Text.Encoding.Unicode));
            foreach (var line in content)
            {
                yield return line;
            }
        }
    }
}
