using System;
using System.Collections.Generic;
using RepositoriesParser;
using System.IO;
using System.Net;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new Parser();
            var userList = new Dictionary<int, string>();
            var exceptions = new Dictionary<int, string>();
            var key = 1;
            
            try
            {
                foreach (var item in parser.GetUser())
                {
                    if (!userList.ContainsValue(item))
                        userList.Add(key++, item);
                }
            }
            catch (WebException ex)
            {
                exceptions.Add(key++,ex.ToString());
            }

            using (var writer = new StreamWriter("outputJS.txt", false, System.Text.Encoding.Unicode))
            {
                foreach (var item in userList)
                {
                    writer.WriteLine("{0,-10} - {1}", item.Key, item.Value);
                }
                writer.WriteLine();
                foreach (var item in exceptions)
                {
                    writer.WriteLine("{0,-10} - {1}", item.Key, item.Value);
                }
            }
            Console.ReadKey();
        }
    }
}
