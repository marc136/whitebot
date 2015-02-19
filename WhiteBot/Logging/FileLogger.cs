using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging
{
    public interface ILogger
    {
        Task Log(string format, params object[] args);
        Task Log(string message);
    }

    public class FileLogger : ILogger
    {
        private string filename;
        private TextWriter logFile = null;
        private List<List<string>> logMessages;
        private int currentLogIndex;

        public static string LogFolder = ""; //current folder

        public FileLogger(string filename = "")
        {
            if (filename != "")
            {
                this.filename = filename;
            }
            else
            {
                GenerateLogFileName();
            }

            logMessages = new List<List<string>>(2);
            logMessages.Add(new List<string>());
            logMessages.Add(new List<string>());
            currentLogIndex = 0;
        }

        ~FileLogger()
        {
            CloseLogFileIfNeeded();
        }

        private void GenerateLogFileName()
        {
            var folder = @"C:\logs\";
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd");//_H-mm-ss")
            this.filename = folder + timestamp + ".log";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        public async Task Log(string format, params object[] args)
        {
            string s = String.Format(format, args);
            await Log(String.Format(format, args));
        }

        public async Task Log(string message)
        {
            //var messageWithTimestamp = DateTime.Now.ToString("yyyy-MM-dd\tH:mm:ss:fff\t") + message;
            var messageWithTimestamp = DateTime.Now.ToString("H:mm:ss:fff\t") + message;

            lock (logMessages)
            {
                if (logMessages[currentLogIndex].Count > 50)
                {// switch Log
                    currentLogIndex++;
                    if (currentLogIndex >= logMessages.Count) { currentLogIndex = 0; }
                    logMessages[currentLogIndex] = new List<string>();
                }

                logMessages[currentLogIndex].Add(messageWithTimestamp);
            }
            await Write(messageWithTimestamp);
        }

        private async Task Write(string line)
        {
            OpenLogFileIfNeeded();
            await logFile.WriteLineAsync(line);
            await logFile.FlushAsync();
        }


        private void OpenLogFileIfNeeded()
        {
            if (logFile == null)
            {
                try
                {
                    var folder = Path.GetDirectoryName(filename);
                    if (!Directory.Exists(folder)) { Directory.CreateDirectory(folder); };
                    logFile = new StreamWriter(filename, append: true, encoding: System.Text.Encoding.UTF8);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        private void CloseLogFileIfNeeded()
        {
            if (logFile != null)
            {
                try
                {
                    logFile.Close();
                }
                catch (ObjectDisposedException)
                {
                    //ignore, file was already closed
                }
                catch (InvalidOperationException) { }
                finally
                {
                    logFile.Dispose();
                    logFile = null;
                }
            }
        }

    }
}
