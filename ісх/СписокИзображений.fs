namespace Бібліотека

open System

[<Sealed>]
type СписокЗображень private () =
   static let зображення = Словник<string,byte[]>()
   static member ЗавантажитиЗображення(шлях:string) =
      let імя = "СписокЗображень" + Guid.NewGuid().ToString()
      let байти =
         if шлях.StartsWith("http:") || шлях.StartsWith("https:") 
         then Хттп.ЗавантажитиБайти шлях
         else Ресурс.ЗавантажитиБайти шлях
      зображення.Add(імя, байти)
      імя
   static member internal СпробуватиОтриматиБайтиЗображення(імя:string) =
      match зображення.TryGetValue(імя) with
      | true, байты -> Some(байты)
      | false, _ -> None



