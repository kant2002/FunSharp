﻿namespace Библиотека

open System

[<Sealed>]
type СписокИзображений private () =
   static let изображения = Словарь<string,byte[]>()
   static member ЗагрузитьИзображение(путь:string) =
      let имя = "ImageList" + Guid.NewGuid().ToString()
      let байты =
         if путь.StartsWith("http:") || путь.StartsWith("https:") 
         then Хттп.ЗагрузитьБайты путь
         else Ресурс.ЗагрузитьБайты путь
      изображения.Add(имя, байты)
      имя
   static member internal ПопробоватьПолучитьБайтыИзображения(имя:string) =
      match изображения.TryGetValue(имя) with
      | true, байты -> Some(байты)
      | false, _ -> None



