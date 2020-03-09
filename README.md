# UploadDownloadHendler_wdho.ru
Данное решение необходимо для того чтобы с лёгкостью загружать и скачивать файлы куда нибудь

Для хранения используется сервис [wdho.ru](https://wdho.ru/)

## Пример загрузки:
#### Способ 1
##### Вызываем метод загрузки (Upload) файла:
* <b>PatchFile</b> - путь к файлу

 `InfoFile FileRes = UploadDownloadWdho.Upload.UploadFile(string PatchFile);`
 
 #### Способ 2
 ##### Вызываем метод загрузки файла:
 * <b>FileByte</b> - содержимое файл а виде массива byte
 * <b>NameFile</b> - название файла вместе с его раширением
 
 `InfoFile FileRes = UploadDownloadWdho.UploadHendler(byte[] FileByte, string NameFile);`
 
В обоих способах вы получаете `InfoFile` содержащий в себе:
 * <b>name</b> - название файла
 * <b>size</b> - размер файла
 * <b>error</b> - ошибки возникшие при загрузке
 * <b>url</b> - псевдосылка на загрузку файла
 * <b>delete_url</b> - псевдосылка на удаление файла
 * <b>info_url</b> - ссылка на страницу с информацией о файле
 * <b>stats_url</b> - ссылка на статисктику файла

## Пример скачивание файла:
#### Способ 1
##### Вызываем метод скачивания (Download) файла:
* <b>PatchSave</b> - в какую папку сохранить
* <b>Url</b> - ссылка на файл

`UploadDownloadWdho.Download.DownloadFile(string PatchSave, string Url);`

#### Способ 2
##### Вызываем метод скачивания (Download) файла:
* <b>Url</b> - ссылка на файл

`byte[] file = UploadDownloadWdho.Download.GetBytesFile(string Url);`

Данный метод получает содержимое файла в виде массива byte

## Дополнительные функции:
#### Получение ссылки на страницу со статистикой
* <b>UrlFile</b> - ссылка на файл
`string PageInfo = GetStatiUrl(string UrlFile);`

#### Получение информации о файле
* <b>Url</b> - ссылка на файл

`GetedInfoFile Info = GetedInfoFile GetInfoFile(string Url);`

Класс `GetedInfoFile` в себе содержит такую информацию как:
* Данные
  * <b>NameFile</b> - Название файла
  * <b>Size</b> - Размер файла в MB
  * <b>UploadData</b> - Когда был загружен файл
  * <b>CountDownload</b> - Количесттво скачиваний
  * <b>stats_url</b> - Ссылка на страницу со статистикой
* Методы
  * `ToString();` Данный метод возвращает значение всех переменных в виде строки
