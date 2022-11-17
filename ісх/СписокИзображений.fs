простір Бібліотека

відкрити System

[<Sealed>]
тип СписокЗображень private () =
   static нехай зображення = Словник<string,byte[]>()
   static member ЗавантажитиЗображення(шлях:string) =
      нехай імя = "СписокЗображень" + Guid.NewGuid().ToString()
      нехай байти =
         if шлях.StartsWith("http:") || шлях.StartsWith("https:") 
         then Хттп.ЗавантажитиБайти шлях
         else Ресурс.ЗавантажитиБайти шлях
      зображення.Add(імя, байти)
      імя
   static member internal СпробуватиОтриматиБайтиЗображення(імя:string) =
      match зображення.TryGetValue(імя) із
      | true, байты -> Some(байты)
      | false, _ -> None



