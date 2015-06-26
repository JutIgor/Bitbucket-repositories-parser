using RepositoriesParser;
using System;
using System.Collections.Generic;
using System.Net;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = appDirectory + @"\..\..\..\{0}.txt";
            string HtmlCssFile = string.Format(filePath, "HtmlCssUsers");
            string JavaScriptFile = string.Format(filePath, "JavaScriptUsers");
            string HtmlCssRepositoriesFile = string.Format(filePath, "HtmlCssRepositories");
            string JavaScriptRepositoriesFile = string.Format(filePath, "JavaScriptRepositories");

            try
            {
                var HtmlCssusers = GetUsers(HtmlCssFile, Language.HtmlCss);
                var JavaScriptUsers = GetUsers(JavaScriptFile, Language.JavaScript);
                var HtmlCssRepositories = GetRepositories(HtmlCssRepositoriesFile, HtmlCssusers, Language.HtmlCss);
                var JavaScriptRepositories = GetRepositories(JavaScriptRepositoriesFile, JavaScriptUsers, Language.JavaScript);
                // TODO: add 2 folders ofr repo and add code to download it
            }
            catch (WebException ex)
            {
                Console.Write(ex.Message.ToString());
            }
        }

        private static Dictionary<int, string> GetUsers(string fileName, string language)
        {
            var users = new Dictionary<int, string>();
            var parser = new Parser();
            var key = 1;
            foreach (var item in parser.GetUser(language))
            {
                if (!users.ContainsValue(item))
                    users.Add(key++, item);
            }
            Writer.SaveUsers(fileName, users);
            return users;
        }

        private static Dictionary<string, string> GetRepositories(string fileName, Dictionary<int, string> users, string language)
        {
            var repositories = new Dictionary<string, string>();
            var parser = new Parser();
            foreach (var userName in users)
            {
                foreach (var item in parser.GetRepository(userName.Value, language))
                {
                    repositories.Add(item, userName.Value);
                }
            }
            Writer.SaveRepositories(fileName, repositories);
            return repositories;
        }
    }
}
