﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kts.Gis.Data
{
    public class Log
    {
        private static object sync = new object();

        public static void Write(String message)
        {
            try
            {
                // Путь .\\Log
                string pathToLog = "c:\\1";//Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
                if (!Directory.Exists(pathToLog))
                    Directory.CreateDirectory(pathToLog); // Создаем директорию, если нужно
                string filename = "debug.log";//Path.Combine(pathToLog, string.Format("{0}_{1:dd.MM.yyy}.log",
                //AppDomain.CurrentDomain.FriendlyName, DateTime.Now);
                string fullText = string.Format("[{0:dd.MM.yyy HH:mm:ss.fff}] [{1}()] \r\n",
                DateTime.Now, message);
                lock (sync)
                {
                    File.AppendAllText(filename, fullText, Encoding.GetEncoding("Windows-1251"));
                }
            }
            catch
            {
                // Перехватываем все и ничего не делаем
            }
        }

        public static void Write(Exception ex)
        {
            try
            {
                // Путь .\\Log
                string pathToLog = "c:\\1";//Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
                if (!Directory.Exists(pathToLog))
                    Directory.CreateDirectory(pathToLog); // Создаем директорию, если нужно
                string filename = "debug.log";//Path.Combine(pathToLog, string.Format("{0}_{1:dd.MM.yyy}.log",
                //AppDomain.CurrentDomain.FriendlyName, DateTime.Now);
                string fullText = string.Format("[{0:dd.MM.yyy HH:mm:ss.fff}] [{1}.{2}()] {3}\r\n",
                DateTime.Now, ex.TargetSite.DeclaringType, ex.TargetSite.Name, ex.Message);
                lock (sync)
                {
                    File.AppendAllText(filename, fullText, Encoding.GetEncoding("Windows-1251"));
                }
            }
            catch
            {
                // Перехватываем все и ничего не делаем
            }
        }
    }
}
