using System;

namespace Program
{
    public class Paths
    {
        public static string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
        public static string filePath = appDirectory + @"\..\..\..\Bitbucket\{0}.txt";
        public static string HtmlCssUsersFile = string.Format(filePath, "HtmlCssUsers");
        public static string JavaScriptUsersFile = string.Format(filePath, "JavaScriptUsers");
        public static string HtmlCssRepositoriesFile = string.Format(filePath, "HtmlCssRepositories");
        public static string JavaScriptRepositoriesFile = string.Format(filePath, "JavaScriptRepositories");
        public static string HtmlCssFolder = appDirectory + @"\..\..\..\Bitbucket\HtmlCss\{0}";
        public static string JavaScriptFolder = appDirectory + @"\..\..\..\Bitbucket\JavaScript\{0}";
    }
}
