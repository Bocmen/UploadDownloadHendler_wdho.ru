using System;

namespace wdhoUploadDownload
{
    class Program
    {
        /// <summary>
        /// Пример
        /// </summary>
        static void Main(string[] args)
        {
            Console.WriteLine("Введите путь к файлу который вы хотите сохранить");
            var FileRes = UploadDownloadWdho.Upload.UploadFile(Console.ReadLine());
            Console.WriteLine("Введите куда сохранить файл (вместе с названием и расширением файла)");
            UploadDownloadWdho.Download.DownloadFile(Console.ReadLine(), FileRes.url);
            Console.WriteLine("Пример закончен");
            while (true) Console.ReadKey();
        }
    }
}

