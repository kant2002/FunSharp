namespace Бiблiотека

open System

[<Sealed>]
type СписокЗображень private () =
   static let зображення = Словник<string,byte[]>()
   static member ЗавантажитиЗображення(путь:string) =
      let имя = "ImageList" + Guid.NewGuid().ToString()
      let байты =
         if путь.StartsWith("http:") || путь.StartsWith("https:") 
         then Хттп.ЗавантажитиБайти путь
         else Ресурс.ЗавантажитиБайти путь
      зображення.Add(имя, байты)
      имя
   static member internal ПопробоватьПолучитьБайтыИзображения(имя:string) =
      match зображення.TryGetValue(имя) with
      | true, байты -> Some(байты)
      | false, _ -> None



