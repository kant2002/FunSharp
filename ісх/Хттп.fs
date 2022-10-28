module internal Бiблiотека.Хттп

open System.IO
open System.Net

let ЗавантажитиБайти (url:string) =
   let запит = HttpWebRequest.Create(url)
   let відповідь = запит.GetResponse()
   use струмВідповіді = відповідь.GetResponseStream()
   use струмПамяті = new MemoryStream()
   струмВідповіді.CopyTo(струмПамяті)
   струмПамяті.GetBuffer()

open Avalonia.Media.Imaging
open Avalonia.Media

let ЗавантажитиЗображенняАсінх (url:string) = async {
   let запит = HttpWebRequest.Create(url)
   use! відповідь = запит.AsyncGetResponse()
   use струм = відповідь.GetResponseStream()
   return new Bitmap(струм) :> IImage
   }

