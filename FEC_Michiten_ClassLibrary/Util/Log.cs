using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FEC_Michiten_ClassLibrary.Util
{
	public class Log
	{
        private static readonly string PATH_LOG = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            Define.LOG_DIR,
            "viewerlog.log");

        private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static string _userName = string.Empty;

        public static void Setup()
        {
            var Appender = new log4net.Appender.RollingFileAppender()
            {
                File = PATH_LOG,
                AppendToFile = true,
                RollingStyle = log4net.Appender.RollingFileAppender.RollingMode.Size,
                MaximumFileSize = "10MB",
                MaxSizeRollBackups = 0,
                Encoding = Encoding.UTF8,
                Layout = new log4net.Layout.PatternLayout(@"%d [%t] [%-5p] %m%n"),
            };
            Appender.ActivateOptions();
            var log = (log4net.Repository.Hierarchy.Logger)_logger.Logger;
            log.AddAppender(Appender);
            log.Hierarchy.Configured = true;

            _userName = Environment.UserName;
        }

        public static void Error(object msg)
        {
            Debug.WriteLine($"[Error] [{_userName}] {msg}");
            _logger.Error($"[{_userName}] {msg}");
        }

        public static void Error(object msg, Exception e)
        {
            Debug.WriteLine($"[Error] [{_userName}] {msg.ToString()}, {e.ToString()}");
            _logger.Error($"[{_userName}] {msg}", e);
        }

        public static void Warning(object msg)
        {
            Debug.WriteLine($"[Warn ] [{_userName}] {msg}");
            _logger.Warn($"[{_userName}] {msg}");
        }

        public static void Warning(object msg, Exception e)
        {
            Debug.WriteLine($"[Warn ] [{_userName}] {msg.ToString()}, {e.ToString()}");
            _logger.Warn($"[{_userName}] {msg}", e);
        }

        public static void Info(object msg)
        {
            Debug.WriteLine($"[Info ] [{_userName}] {msg}");
            _logger.Info($"[{_userName}] {msg}");
        }
    }
}
