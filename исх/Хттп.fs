module internal Библиотека.Хттп

open System.IO
open System.Net

let ЗагрузитьБайты (url:string) =
   let запрос = HttpWebRequest.Create(url)
   let ответ = запрос.GetResponse()
   use потокОтвета = ответ.GetResponseStream()
   use потокПамяти = new MemoryStream()
   потокОтвета.CopyTo(потокПамяти)
   потокПамяти.GetBuffer()

open Avalonia.Media.Imaging
open Avalonia.Media

let ЗагрузитьИзображениеАсинх (url:string) = async {
   let запрос = HttpWebRequest.Create(url)
   use! ответ = запрос.AsyncGetResponse()
   use поток = ответ.GetResponseStream()
   return new Bitmap(поток) :> IImage
   }

