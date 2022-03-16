using System.Data;
using System.IO;

namespace MailService.Controller
{
    class Log
    {
        public static void AppendLog(string data)
        {
            var filepath = $"src{Path.DirectorySeparatorChar}Data{Path.DirectorySeparatorChar}log.txt";
            using(var fs = File.AppendText(filepath))
            {
                fs.Write(Environment.NewLine);
                fs.Write(Environment.NewLine);
                fs.Write(DateTime.Now);
                fs.Write(Environment.NewLine);
                fs.Write(data);
            }
        }

        public static bool IsFailureLog()
        {
            var filepath = $"src{Path.DirectorySeparatorChar}Data{Path.DirectorySeparatorChar}log.txt";
            var logText = File.ReadAllText(filepath);
            if(logText.Contains("failure") || logText.Contains("fail") || logText.Contains("error"))
            {
                return true;
            }else
            {
                return false;
            }
        }
    }
}