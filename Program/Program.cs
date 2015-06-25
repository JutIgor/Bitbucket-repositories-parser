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
            var key = 1;
            
            try
            {
                foreach (var item in parser.GetUser(Language.HtmlCss))
                {
                    if (!userList.ContainsValue(item))
                        userList.Add(key++, item);
                }
            }
            catch (WebException ex)
            {
                Console.Write(ex.ToString());
            }

            using (var writer = new StreamWriter("outputJS.txt", false, System.Text.Encoding.Unicode))  // outputJS
            {
                foreach (var item in userList)
                {
                    writer.WriteLine("{0,-10} - {1}", item.Key, item.Value);
                }
            }
            Console.ReadKey();
        }
    }
}
