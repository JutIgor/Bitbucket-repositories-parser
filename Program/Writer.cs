using System;
using System.Collections.Generic;
using System.IO;

namespace Program
{
    public class Writer
    {
        public static bool SaveUsers(string fileName, Dictionary<int, string> userNames)
        {
            try
            {
                using (var writer = new StreamWriter(fileName, false, System.Text.Encoding.Unicode))
                {
                    foreach (var item in userNames)
                    {
                        writer.WriteLine("{0}", item.Value);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool SaveRepositories(string fileName, Dictionary<string,string> repositories)
        {
            try
            {
                using (var writer = new StreamWriter(fileName, false, System.Text.Encoding.Unicode))
                {
                    foreach (var item in repositories)
                    {
                        writer.WriteLine("{0} {1}", item.Value, item.Key);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
