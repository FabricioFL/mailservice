namespace MailService.Controller
{
    class Log
    {
        public static void AppendLog(string data)
        {
            var filepath = $"src{Path.DirectorySeparatorChar}Data{Path.DirectorySeparatorChar}errors.log";
            using(var file = File.AppendText(filepath))
            {
                file.Write(Environment.NewLine);
                file.Write(Environment.NewLine);
                file.Write(DateTime.Now);
                file.Write(Environment.NewLine);
                file.Write(data);
            }
        }

        public static bool IsFailureLog()
        {
            var filepath = $"src{Path.DirectorySeparatorChar}Data{Path.DirectorySeparatorChar}errors.log";
            var logText = File.ReadAllText(filepath);
            if(logText.Contains("failure") || logText.Contains("fail") || logText.Contains("error"))
            {
                return true;
            }
            return false;
        }
    }
}