﻿namespace Бiблiотека

open Avalonia
open System
open System.Collections.Generic

type internal ИнфоФигуры = { Фигура:Фигура; mutable Зсув:Point; mutable Opacity:float }

[<Sealed>]
type Фігури private () =
   static let перо () = Перо(ГрафичнеВікно.КолірПера,ГрафичнеВікно.ШиринаПера)
   static let кисть () = ГрафичнеВікно.КолірПензлика
   static let шрифт () = 
      Шрифт(ГрафичнеВікно.РозмірШрифта,ГрафичнеВікно.ИмяШрифта,ГрафичнеВікно.ЖирностьШрифта, ГрафичнеВікно.КурсивностьШрифта)
   static let фигуры = Dictionary<string,ИнфоФигуры>()
   static let добавитьФигуру название фигура =
      let инфо = { Фигура=фигура; Зсув=Point(); Opacity=1.0 }
      фигуры.Add(название,инфо)      
      додатиМалюнок (НамалюватиФігуру(название,фигура))
   static let onShape имяФигуры действие =
      match фигуры.TryGetValue(имяФигуры) with
      | true, инфо -> действие инфо
      | false, _ -> ()
   static let генИмя имя = имя + Guid.NewGuid().ToString()
   static member Видалити(имяФигуры) =
      Моя.Апплікація.Викликати (fun () -> Моя.Апплікація.Полотно.ВидалитиФігуру(имяФигуры))
   static member ДобавитьЛинию(x1,y1,x2,y2) =
      let имя = генИмя "Линия"
      ФигураЛинии(Линия(x1,y1,x2,y2),перо()) |> добавитьФигуру имя
      имя
   static member ДобавитьЛинию(x1:int,y1:int,x2:int,y2:int) =
      Фігури.ДобавитьЛинию(float x1, float y1, float x2, float y2)
   static member ДодатиПрямокутник(ширина,высота) =
      let имя = генИмя "Прямоугольник"
      ФигураПрямоугольника(Прямокутник(ширина,высота),перо(),кисть()) |> добавитьФигуру имя
      имя
   static member ДодатиПрямокутник(ширина:int,высота:int) =
      Фігури.ДодатиПрямокутник(float ширина, float высота)
   static member ДодатиТрикутник(x1,y1,x2,y2,x3,y3) =
      let имя = генИмя "Треугольник"
      ФигураТреугольника(Треугольник(x1,y1,x2,y2,x3,y3),перо(),кисть()) |> добавитьФигуру имя
      имя
   static member ДобавитьЭллипс(ширина,высота) =
      let имя = генИмя "Эллипс"
      ФигураЭллипса(Эллипс(ширина,высота),перо(),кисть()) |> добавитьФигуру имя
      имя
   static member ДобавитьЭллипс(ширина:int,высота:int) =
      Фігури.ДобавитьЭллипс(float ширина,float высота)
   static member ДодатиЗображення(імяЗображення) =
      let имя = генИмя "Изображение"
      match СписокЗображень.ПопробоватьПолучитьБайтыИзображения(імяЗображення) with
      | Some байты ->
         let струм = new System.IO.MemoryStream(байты)
         let зображення = new Avalonia.Media.Imaging.Bitmap(струм) :> Avalonia.Media.IImage
         ФигураИзображения(ref зображення) |> добавитьФигуру имя
      | None ->
         let зображенняПосил = 
            if імяЗображення.StartsWith("http:") || імяЗображення.StartsWith("https:") 
            then
               let зображенняПосил = ref null
               async {
                  let! зображення = Хттп.ЗавантажитиЗображенняАсінх імяЗображення
                  зображенняПосил := зображення
                  Моя.Апплікація.Викликати(fun () -> Моя.Апплікація.Полотно.ЗробитиНедійсним())
               } |> Async.Start
               зображенняПосил
            else             
               let зображенняПосил = ref null                 
               Моя.Апплікація.Викликати(fun () ->
                  use струм = Ресурс.ОтриматиСтрум(імяЗображення)
                  зображенняПосил := new Avalonia.Media.Imaging.Bitmap(струм) :> Avalonia.Media.IImage
               )
               зображенняПосил
         ФигураИзображения(зображенняПосил) |> добавитьФигуру имя
      имя
   static member ДобавитьТекст(текст) =
      let name = генИмя "Text"
      ФигураТекста(ref текст, шрифт(), кисть()) |> добавитьФигуру name
      name
   static member СкрытьФигуру(имяФигуры) =      
      Моя.Апплікація.Викликати (fun () -> Моя.Апплікація.Полотно.УстановитьВидимостьФигуры(имяФигуры,false))      
   static member ПоказатьФигуру(имяФигуры) =
      Моя.Апплікація.Викликати (fun () -> Моя.Апплікація.Полотно.УстановитьВидимостьФигуры(имяФигуры,true))      
   static member Перемістити(имяФигуры,x,y) =
      onShape имяФигуры (fun инфо ->
         инфо.Зсув <- Point(x,y)
         Моя.Апплікація.Викликати (fun () -> Моя.Апплікація.Полотно.ПереместитьФигуру(имяФигуры,инфо.Зсув))
      )
   static member Перемістити(имяФигуры,x:int,y:int) =
      Фігури.Перемістити(имяФигуры, float x, float y)
   static member ПолучитьЛево(имяФигуры) =      
      match фигуры.TryGetValue(имяФигуры) with
      | true, info -> info.Зсув.X
      | false, _ -> 0.0
   static member ПолучитьВерх(имяФигуры) =
      match фигуры.TryGetValue(имяФигуры) with
      | true, info -> info.Зсув.Y
      | false, _ -> 0.0
   static member SetOpacity(имяФигуры, opacity) =
      onShape имяФигуры (fun info ->
         info.Opacity <- opacity
         Моя.Апплікація.Викликати (fun () -> Моя.Апплікація.Полотно.УстановитьНепрозрачностьФигуры(имяФигуры,opacity))
      )
   static member SetOpacity(имяФигуры, opacity:int) =
      Фігури.SetOpacity(имяФигуры, float opacity)
   static member GetOpacity(имяФигуры) =
      match фигуры.TryGetValue(имяФигуры) with
      | true, info -> info.Opacity
      | false, _ -> 1.0
   static member Повернути(имяФигуры, угол) =
      match фигуры.TryGetValue(имяФигуры) with
      | true, info ->
         Моя.Апплікація.Викликати (fun () -> Моя.Апплікація.Полотно.УстановитьВращениеФигуры(имяФигуры,угол))
      | false, _ -> ()
   static member Повернути(имяФигуры, угол:int) =
      Фігури.Повернути(имяФигуры, float угол)
   static member Zoom(имяФигуры, scaleX, scaleY) =
      match фигуры.TryGetValue(имяФигуры) with
      | true, info ->
         Моя.Апплікація.Викликати (fun () -> Моя.Апплікація.Полотно.ВстановитиМасштабФігури(имяФигуры,scaleX,scaleY))
      | false, _ -> ()
   static member УстановитьТекст(имяФигуры, текст) =
      onShape имяФигуры (fun info ->
         match info.Фигура with
         | ФигураТекста(textRef, font, color) ->
            Моя.Апплікація.Викликати (fun () -> textRef := текст; Моя.Апплікація.Полотно.ЗробитиНедійсним())
         | _ -> invalidOp "Expecting text shape"
      )       
   static member Animate(имяФигуры,x:float,y:float,ms:int) =
      Фігури.Перемістити(имяФигуры, x, y)
   static member Animate(имяФигуры,x:int,y:int,мс:int) =
      Фігури.Animate(имяФигуры, float x, float y, мс)

