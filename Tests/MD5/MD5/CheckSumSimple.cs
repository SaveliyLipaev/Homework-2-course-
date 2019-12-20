using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Diagnostics;

namespace MD5CheckSum
{
    /// <summary>
    /// Класс для вычесления хэш значения
    /// </summary>
    public static class CheckSumSimple
    {
        /// <summary>
        /// Check-сумма
        /// </summary>
        public static double Sum { get; private set; } = 0;
        /// <summary>
        /// Время работы алгоритма
        /// </summary>
        public static long TimeWork { get; private set; }

        /// <summary>
        /// Запуск алгоритма
        /// </summary>
        public static void Run(string path)
        {
            var watch = Stopwatch.StartNew();
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (directoryInfo.Exists)
            {
                RunForDirectorys(directoryInfo);
            }
            else
            {
                CountCheckSum(directoryInfo.FullName);
            }
            watch.Stop();
            TimeWork = watch.ElapsedMilliseconds;
        }

        /// <summary>
        /// Запускается для директории
        /// </summary>
        private static void RunForDirectorys(DirectoryInfo directory)
        {
            var files = directory.GetFiles();
            var directorys = directory.GetDirectories();
            CountCheckSumFileInfo(files);
            foreach (var dir in directorys)
            {
                RunForDirectorys(dir);
            }
        }

        /// <summary>
        /// Считает хэш значение одного файла, принимая FileInfo
        /// </summary>
        private static void CountCheckSumFileInfo(FileInfo[] filesName)
        {
            foreach (var file in filesName)
            {
                CountCheckSum(file.FullName);
            }
        }

        /// <summary>
        /// Считает хэш значение одного файла, принимая путь к файлу
        /// </summary>
        private static void CountCheckSum(string path)
        {
            using (var md5Hash = MD5Cng.Create())
            {
                var file = File.ReadAllBytes(path);
                var data = md5Hash.ComputeHash(file);
                var hashSum = BitConverter.ToDouble(data, 0);

                Sum += hashSum;
            }
        }
    }
}
