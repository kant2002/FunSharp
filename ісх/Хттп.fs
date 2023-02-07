модуль внутрішній Бібліотека.Хттп

відкрити System.IO
відкрити System.Net

нехай ЗавантажитиБайти (url:string) =
   нехай запит = HttpWebRequest.Create(url)
   нехай відповідь = запит.GetResponse()
   вживати струмВідповіді = відповідь.GetResponseStream()
   вживати струмПамяті = новий MemoryStream()
   струмВідповіді.CopyTo(струмПамяті)
   струмПамяті.GetBuffer()

відкрити Avalonia.Media.Imaging
відкрити Avalonia.Media

нехай ЗавантажитиЗображенняАсінх (url:string) = асинх {
   нехай запит = HttpWebRequest.Create(url)
   вживати! відповідь = запит.AsyncGetResponse()
   вживати струм = відповідь.GetResponseStream()
   повернути новий Bitmap(струм) :> IImage
   }

