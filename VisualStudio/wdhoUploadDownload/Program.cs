using System;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

namespace wdhoUploadDownload
{
    class Program
    {
        static void Main(string[] args)
        {
            var FileRes = UploadDownloadWdho.Upload.UploadFile(@"D:\Вн в кармане.rar");
            UploadDownloadWdho.Download.DownloadFile(@"D:\Test\20200126_181732.rar", FileRes.url);
            while (true) Console.ReadKey();
        }
    }
}
namespace UploadDownloadWdho
{
    public static class Upload
    {
        /// <summary>
        /// Загрузка файла
        /// </summary>
        /// <param name="PatchFile">Путь к файллу на диске</param>
        /// <returns>Информация о загруженном файле</returns>
        public static InfoFile UploadFile(string PatchFile)
        {
            return UploadHendler(File.ReadAllBytes(PatchFile), PatchFile.Remove(0, PatchFile.Replace('\\', '/').LastIndexOf('/')));
        }
        /// <summary>
        /// Загрузчик файлов
        /// </summary>
        /// <param name="FileByte">Массив byte файла</param>
        /// <param name="NameFile">Название файла</param>
        /// <returns>Информация о загруженном файле</returns>
        public static InfoFile UploadHendler(byte[] FileByte, string NameFile)
        {
            DatKeys keys = GetKeysData();
            InfoFile infoFile = new InfoFile();
            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>(0);
            keyValuePairs.Add(Setting.Search__sessionid, keys._sessionid);
            keyValuePairs.Add(Setting.Search_cTracker, keys.cTracke);
            keyValuePairs.Add(Setting.Search_maxChunkSize, keys.maxChunkSize);
            keyValuePairs.Add(Setting.Search_fileFolder, keys.fileFolder);
            keyValuePairs.Add("files[]", new FormUpload.FileParameter(FileByte, NameFile+".txt", "application/octet-stream"));
            var Resul = FormUpload.MultipartFormDataPost(keys.Url, "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:73.0) Gecko/20100101 Firefox/73.0", keyValuePairs);
            string StrRes;
            using (StreamReader stream = new StreamReader(Resul.GetResponseStream(), Encoding.GetEncoding(Resul.CharacterSet)))
            {
                StrRes = System.Text.RegularExpressions.Regex.Unescape(stream.ReadToEnd());
            }
            infoFile.delete_url = ParsJson(StrRes, "delete_url");
            infoFile.error = ParsJson(StrRes, "error");
            infoFile.info_url = ParsJson(StrRes, "info_url");
            infoFile.name = ParsJson(StrRes, "name");
            if(infoFile.name.Contains('.')) infoFile.name = infoFile.name != null ? infoFile.name.Substring(0, infoFile.name.LastIndexOf('.')) : infoFile.name;
            infoFile.size = ParsJson(StrRes, "size");
            infoFile.stats_url = ParsJson(StrRes, "stats_url");
            infoFile.url = ParsJson(StrRes, "url");
            return infoFile;
        }
        /// <summary>
        /// В ответе у них Json содержит ошибки и не получается десериализовать поэтому было принято решения написать парсер значений
        /// </summary>
        /// <param name="Json">Json txt</param>
        /// <param name="Name">Название искомой переменной</param>
        /// <returns>значение переменной</returns>
        private static string ParsJson(string Json, string Name)
        {

            string resul = null;
            try
            {
                resul = Json.Remove(0, Json.IndexOf(Name));
                resul = resul.Remove(0, resul.IndexOf(":") + 1);
                resul = resul.Substring(0, resul.IndexOf(','));
                resul = resul.Remove(0, resul.IndexOf("\"") + 1);
                resul = resul.Substring(0, resul.IndexOf("\""));
            }
            catch { }
            return resul;
        }
        /// <summary>
        /// Получение одноразовых ключей для загрузки файла
        /// </summary>
        /// <returns>Набор ключей</returns>
        private static DatKeys GetKeysData()
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument htmlDocument = web.Load(Setting.UrlHomePage);
            HtmlNodeCollection nodes = htmlDocument.DocumentNode.SelectNodes("//script");

            // Поиск нужного скрипта
            foreach (var Elem in nodes)
            {
                if (Elem.InnerText.Contains(Setting.SearchScript))
                {
                    DatKeys datKeys = new DatKeys();
                    // Поиск переменных
                    // Поиск ссылки
                    string Resul = Elem.InnerText.Remove(0, Elem.InnerText.IndexOf(Setting.SearchScript));
                    Resul = Resul.Remove(Resul.IndexOf('\''), Resul.Length - Resul.IndexOf('\''));
                    datKeys.Url = Resul;
                    // Поиск название папки (обычно он null)
                    Resul = Elem.InnerText.Remove(0, Elem.InnerText.IndexOf(Setting.Search_fileFolder));
                    datKeys.fileFolder = Resul.Substring(Resul.IndexOf('\'') + 1, Resul.Remove(0, Resul.IndexOf('\'') + 1).IndexOf('\''));
                    // Поиск maxChunkSize (хз для чего он)
                    Resul = Elem.InnerText.Remove(0, Elem.InnerText.IndexOf(Setting.Search_browserXHR2Support));
                    Resul = Resul.Remove(0, Resul.IndexOf(Setting.Search_maxChunkSize));
                    Resul = Resul.Substring(Resul.IndexOf('='), Resul.IndexOf(';'));
                    datKeys.maxChunkSize = Convert.ToUInt32(Resul.Replace('=', ' ').Replace(';', ' '));
                    // Поиск _sessionid (какой то личный индификатор "сессии")
                    Resul = Elem.InnerText.Remove(0, Elem.InnerText.IndexOf(Setting.Search__sessionid));
                    datKeys._sessionid = Resul.Substring(Resul.IndexOf('\'') + 1, Resul.Remove(0, Resul.IndexOf('\'') + 1).IndexOf('\''));
                    // Поиск cTracke (какой то ещё один ключ)
                    Resul = Elem.InnerText.Remove(0, Elem.InnerText.IndexOf(Setting.Search_cTracker));
                    datKeys.cTracke = Resul.Substring(Resul.IndexOf('\'') + 1, Resul.Remove(0, Resul.IndexOf('\'') + 1).IndexOf('\''));
                    return datKeys;
                }
            }
            return new DatKeys();
        }
        /// <summary>
        /// Структура для ключей
        /// </summary>
        private struct DatKeys
        {
            public string Url;
            public string _sessionid;
            public string cTracke;
            public uint maxChunkSize;
            public string fileFolder;
        }
        /// <summary>
        /// Информация о загруженном файле
        /// </summary>
        [Serializable]
        public struct InfoFile
        {
            /// <summary>
            /// Название файла
            /// </summary>
            public string name;
            /// <summary>
            /// Размер файла
            /// </summary>
            public string size;
            /// <summary>
            /// Ошибки возникшие при загрузке
            /// </summary>
            public string error;
            /// <summary>
            /// Псевдоссылка на загрузку файла
            /// </summary>
            public string url;
            /// <summary>
            /// Псевдоссылка на удаление файла
            /// </summary>
            public string delete_url;
            /// <summary>
            /// Ссылка на страницу с информацией о файле
            /// </summary>
            public string info_url;
            /// <summary>
            /// Ссылка на статисктику файла
            /// </summary>
            public string stats_url;
        }
    }
    public static class Download
    {
        /// <summary>
        /// Загрузка файла по псевдоссылке
        /// </summary>
        /// <param name="PatchSave">Куда сохранить</param>
        /// <param name="Url">Псевдоссылка на файл</param>
        public static void DownloadFile(string PatchSave, string Url)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFile(GetUrlFinal(Url), PatchSave);
        }
        /// <summary>
        /// Получить массив byte файла
        /// </summary>
        /// <param name="Url">Псевдоссылка на файл</param>
        /// <returns>массив byte файла</returns>
        public static byte[] GetBytesFile(string Url)
        {
            WebClient webClient = new WebClient();
            return webClient.DownloadData(GetUrlFinal(Url));
        }
        /// <summary>
        /// Получение ссылки на загрузку файла
        /// </summary>
        /// <param name="Url">Псевдоссылка на файл</param>
        /// <returns>ссылка на файл</returns>
        public static string GetUrlFinal(string Url)
        {
            string GetCookie = null;
            string Urli = GetUrlOne(Url, Url, ref GetCookie);
            string Resul = getHTML(Urli, ref GetCookie);
            Resul = Resul.Remove(0, Resul.IndexOf(Setting.Search_UrlFinal));
            Resul = Resul.Substring(0, Resul.IndexOf("\""));
            Resul = Url + "?" + Resul;
            return Resul;
        }
        /// <summary>
        /// Получения ссылки со страницей на которой есть ссылка на загрузку
        /// </summary>
        /// <param name="Url">Ссылка на страницу со сылкой</param>
        /// <param name="UrlAppend">Псевдоссылка на файл</param>
        /// <param name="Cookie">Записать Cookie которые будут обнаружены</param>
        /// <returns>Ссылка на сайт с финальной ссылкой загрузки</returns>
        private static string GetUrlOne(string Url, string UrlAppend, ref string Cookie)
        {
            string Resul = getHTML(Url, ref Cookie);
            Resul = Resul.Remove(0, Resul.IndexOf(String.Format(Setting.Search_UrlOne, UrlAppend)));
            Resul = Resul.Substring(0, Resul.IndexOf('\''));
            return Resul;
        }
        /// <summary>
        /// Специальный метод получения Html кода поддерживающий запись предыдущей страницы посещения 
        /// и Cookie которые автоматически записываются при их наличии
        /// </summary>
        /// <param name="url">Ссылка получения Html</param>
        /// <param name="Cookie">Cookie которые записываются или принимаются для использования</param>
        /// <param name="Refer">Предыдущая страница</param>
        /// <returns>HTML код страницы</returns>
        private static string getHTML(string url, ref string Cookie, string Refer = null)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.AllowAutoRedirect = false;//Запрещаем автоматический редирект
                httpWebRequest.Method = "GET"; //Можно не указывать, по умолчанию используется GET.

                httpWebRequest.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:73.0) Gecko/20100101 Firefox/73.0");
                httpWebRequest.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                httpWebRequest.Headers.Add("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.5,en;q=0.3");
                httpWebRequest.Headers.Add("Upgrade-Insecure-Requests", "1");
                httpWebRequest.Headers.Add("Host", "wdho.ru");
                httpWebRequest.Headers.Add("Connection", "keep-alive");
                if (Cookie != null) httpWebRequest.Headers.Add("Cookie", Cookie);

                if (Refer != null) httpWebRequest.Referer = Refer;

                using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (var stream = httpWebResponse.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream, Encoding.GetEncoding(httpWebResponse.CharacterSet)))
                        {
                            Cookie = httpWebResponse.Headers["Set-Cookie"];
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch
            {
                return null;
            }
        }
    }
    public static class Setting
    {
        // Upload
        public const string UrlHomePage = @"https://wdho.ru/index.html?upload=1";
        public const string SearchScript = @"https://wdho.ru/core/page/ajax/file_upload_handler.ajax.php?r=wdho.ru";
        public const string Search__sessionid = @"_sessionid:";
        public const string Search_fileFolder = @"fileFolder";
        public const string Search_maxChunkSize = @"maxChunkSize";
        public const string Search_browserXHR2Support = @"browserXHR2Support";
        public const string Search_cTracker = @"cTracker";
        // Dovnload
        public const string Search_UrlOne = @"{0}?pt=";
        public const string Search_UrlFinal = @"download_token";
    }
}
