модуль внутренний Библиотека.Хттп

открыть System.IO
открыть System.Net

пусть ЗагрузитьБайты (url:string) =
   пусть запрос = HttpWebRequest.Create(url)
   пусть ответ = запрос.GetResponse()
   использовать потокОтвета = ответ.GetResponseStream()
   использовать потокПамяти = новый MemoryStream()
   потокОтвета.CopyTo(потокПамяти)
   потокПамяти.GetBuffer()

открыть Avalonia.Media.Imaging
открыть Avalonia.Media

пусть ЗагрузитьИзображениеАсинх (url:string) = async {
   пусть запрос = HttpWebRequest.Create(url)
   использовать! ответ = запрос.AsyncGetResponse()
   использовать поток = ответ.GetResponseStream()
   return новый Bitmap(поток) :> IImage
   }

