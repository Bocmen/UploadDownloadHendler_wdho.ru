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
            Console.WriteLine("\r\nФайл успешно загружен, вот некоторая информация о загруженном файле");
            Console.WriteLine(FileRes.ToString());
            Console.WriteLine("\r\nИнформация о файле хранящиеся на сайте");
            Console.WriteLine(UploadDownloadWdho.Function.GetInfoFile(FileRes.url).ToString());
            Console.WriteLine("\r\nВведите путь до директории сохранения");
            UploadDownloadWdho.Download.DownloadFile(System.IO.Path.Combine(Console.ReadLine(),FileRes.name), FileRes.url);
            Console.WriteLine("\r\nПример закончен");
            while (true) Console.ReadKey();
        }
    }
}

