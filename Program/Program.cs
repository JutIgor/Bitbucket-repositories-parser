using RepositoriesParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = appDirectory + @"\..\..\..\Bitbucket\{0}.txt";
            var HtmlCssFile = string.Format(filePath, "HtmlCssUsers");
            var JavaScriptFile = string.Format(filePath, "JavaScriptUsers");
            var HtmlCssRepositoriesFile = string.Format(filePath, "HtmlCssRepositories");
            var JavaScriptRepositoriesFile = string.Format(filePath, "JavaScriptRepositories");
            var HtmlCssFolder = appDirectory + @"\..\..\..\Bitbucket\HtmlCss\{0}";
            var JavaScriptFolder = appDirectory + @"\..\..\..\Bitbucket\JavaScript\{0}";

            var newDirectoryPath = string.Format(HtmlCssFolder, string.Empty);
            Directory.CreateDirectory(newDirectoryPath);
            newDirectoryPath = string.Format(JavaScriptFolder, string.Empty);
            Directory.CreateDirectory(newDirectoryPath);

            try
            {
                var HtmlCssusers = GetUsers(HtmlCssFile, Language.HtmlCss);
                var JavaScriptUsers = GetUsers(JavaScriptFile, Language.JavaScript);
                var HtmlCssRepositories = GetRepositories(HtmlCssRepositoriesFile, HtmlCssusers, Language.HtmlCss);
                var JavaScriptRepositories = GetRepositories(JavaScriptRepositoriesFile, JavaScriptUsers, Language.JavaScript);
                
                // TODO: Download all repos
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
