# UploadDownloadHendler_wdho.ru
 Для чего вам нужен этот класс. хмммммммммммм 

Наверное для того чтобы с лёгкостью загружать и скачивать файлы куда нибудь

Для хранения используется сервис [wdho.ru](https://wdho.ru/)

## Пример загрузки:




 var FileRes = UploadDownloadWdho.Upload.UploadFile(@"ПутьКудаСохранить\НазваниеФайла.РасширениеФайла);
 В результате вы получите InfoFile содержащий в себе:
 * Название файла
 * Размер файла
 * Ошибки возникшие при загрузке
 * Псевдосылка на загрузку файла
 * Псевдосылка на удаление файла
 * Ссылка на страницу с информацией о файле
 * Ссылка на статисктику файла

## Пример скачивание файла:
* UploadDownloadWdho.Download.DownloadFile(@"ПутьКудаСохранить\НазваниеФайла.РасширениеФайла", "СсылкаНаФайл"); // Сохранение файла по ссылке
* UploadDownloadWdho.Download.GetBytesFile("СсылкаНаФайл"); // Получение массива byte файла

