module internal Бібліотека.Хттп

відкрити System.IO
відкрити System.Net

нехай ЗавантажитиБайти (url:string) =
   нехай запит = HttpWebRequest.Create(url)
   нехай відповідь = запит.GetResponse()
   use струмВідповіді = відповідь.GetResponseStream()
   use струмПамяті = new MemoryStream()
   струмВідповіді.CopyTo(струмПамяті)
   струмПамяті.GetBuffer()

відкрити Avalonia.Media.Imaging
відкрити Avalonia.Media

нехай ЗавантажитиЗображенняАсінх (url:string) = async {
   нехай запит = HttpWebRequest.Create(url)
   use! відповідь = запит.AsyncGetResponse()
   use струм = відповідь.GetResponseStream()
   return new Bitmap(струм) :> IImage
   }

