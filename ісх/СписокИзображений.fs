простір Бібліотека

відкрити System

[<Sealed>]
тип СписокЗображень private () =
   static нехай зображення = Словник<string,byte[]>()
   static member ЗавантажитиЗображення(шлях:string) =
      нехай імя = "СписокЗображень" + Guid.NewGuid().ToString()
      нехай байти =
         якщо шлях.StartsWith("http:") || шлях.StartsWith("https:") 
         тоді Хттп.ЗавантажитиБайти шлях
         інакше Ресурс.ЗавантажитиБайти шлях
      зображення.Add(імя, байти)
      імя
   static member внутрішній СпробуватиОтриматиБайтиЗображення(імя:string) =
      match зображення.TryGetValue(імя) із
      | true, байты -> Some(байты)
      | false, _ -> None



