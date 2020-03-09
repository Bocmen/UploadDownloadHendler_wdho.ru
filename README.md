# UploadDownloadHendler_wdho.ru
Данное решение необходимо для того чтобы с лёгкостью загружать и скачивать файлы куда нибудь

Для хранения используется сервис [wdho.ru](https://wdho.ru/)

## Пример загрузки:
#### Способ 1
##### Вызываем метод загрузки файла:
* <b>PatchFile</b> - путь к файлу

 `InfoFile FileRes = UploadDownloadWdho.Upload.UploadFile(string PatchFile);`
 
 #### Способ 2
 ##### Вызываем метод загрузки файла:
 * <b>FileByte</b> - содержимое файл а виде массива byte
 * <b>NameFile</b> - название файла вместе с его раширением
 
 `InfoFile FileRes = UploadDownloadWdho.UploadHendler(byte[] FileByte, string NameFile)`
 
В обоих способах вы получаете `InfoFile` содержащий в себе:
 1. <b>name</b> Название файла
 2. <b>size</b> Размер файла
 3. <b>error</b> Ошибки возникшие при загрузке
 4. <b>url</b> Псевдосылка на загрузку файла
 5. <b>delete_url</b> Псевдосылка на удаление файла
 6. <b>info_url</b> Ссылка на страницу с информацией о файле
 7. <b>stats_url</b> Ссылка на статисктику файла

## Пример скачивание файла:
* UploadDownloadWdho.Download.DownloadFile(@"ПутьКудаСохранить\НазваниеФайла.РасширениеФайла", "СсылкаНаФайл"); // Сохранение файла по ссылке
* UploadDownloadWdho.Download.GetBytesFile("СсылкаНаФайл"); // Получение массива byte файла

