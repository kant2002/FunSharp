﻿namespace Библиотека

open Avalonia
open System
open System.Collections.Generic

type internal ИнфоФигуры = { Фигура:Фигура; mutable Смещение:Point; mutable Opacity:float }

[<Sealed>]
type Фигуры private () =
   static let перо () = Перо(ГрафическоеОкно.ЦветПера,ГрафическоеОкно.ШиринаПера)
   static let кисть () = ГрафическоеОкно.ЦветКисти
   static let шрифт () = 
      Шрифт(ГрафическоеОкно.РазмерШрифта,ГрафическоеОкно.ИмяШрифта,ГрафическоеОкно.ЖирностьШрифта, ГрафическоеОкно.КурсивностьШрифта)
   static let фигуры = Dictionary<string,ИнфоФигуры>()
   static let добавитьФигуру название фигура =
      let инфо = { Фигура=фигура; Смещение=Point(); Opacity=1.0 }
      фигуры.Add(название,инфо)      
      addDrawing (НарисоватьФигуру(название,фигура))
   static let onShape имяФигуры действие =
      match фигуры.TryGetValue(имяФигуры) with
      | true, инфо -> действие инфо
      | false, _ -> ()
   static let генИмя имя = имя + Guid.NewGuid().ToString()
   static member Удалить(имяФигуры) =
      Мое.Приложение.Вызвать (fun () -> Мое.Приложение.Холст.УдалитьФигуру(имяФигуры))
   static member ДобавитьЛинию(x1,y1,x2,y2) =
      let имя = генИмя "Линия"
      ФигураЛинии(Линия(x1,y1,x2,y2),перо()) |> добавитьФигуру имя
      имя
   static member ДобавитьЛинию(x1:int,y1:int,x2:int,y2:int) =
      Фигуры.ДобавитьЛинию(float x1, float y1, float x2, float y2)
   static member ДобавитьПрямоугольник(ширина,высота) =
      let имя = генИмя "Прямоугольник"
      ФигураПрямоугольника(Прямоугольник(ширина,высота),перо(),кисть()) |> добавитьФигуру имя
      имя
   static member ДобавитьПрямоугольник(ширина:int,высота:int) =
      Фигуры.ДобавитьПрямоугольник(float ширина, float высота)
   static member ДобавитьТреугольник(x1,y1,x2,y2,x3,y3) =
      let имя = генИмя "Треугольник"
      ФигураТреугольника(Треугольник(x1,y1,x2,y2,x3,y3),перо(),кисть()) |> добавитьФигуру имя
      имя
   static member ДобавитьЭллипс(ширина,высота) =
      let имя = генИмя "Эллипс"
      ФигураЭллипса(Эллипс(ширина,высота),перо(),кисть()) |> добавитьФигуру имя
      имя
   static member ДобавитьЭллипс(ширина:int,высота:int) =
      Фигуры.ДобавитьЭллипс(float ширина,float высота)
   static member ДобавитьИзображение(имяИзображения) =
      let имя = генИмя "Изображение"
      match СписокИзображений.ПопробоватьПолучитьБайтыИзображения(имяИзображения) with
      | Some байты ->
         let поток = new System.IO.MemoryStream(байты)
         let изображение = new Avalonia.Media.Imaging.Bitmap(поток) :> Avalonia.Media.IImage
         ФигураИзображения(ref изображение) |> добавитьФигуру имя
      | None ->
         let изображениеСсыл = 
            if имяИзображения.StartsWith("http:") || имяИзображения.StartsWith("https:") 
            then
               let изображениеСсыл = ref null
               async {
                  let! изображение = Хттп.ЗагрузитьИзображениеАсинх имяИзображения
                  изображениеСсыл := изображение
                  Мое.Приложение.Вызвать(fun () -> Мое.Приложение.Холст.СделатьНедействительным())
               } |> Async.Start
               изображениеСсыл
            elif имяИзображения.StartsWith("file:") || имяИзображения[1] = ':'
            then
               let изображениеСсыл = ref null
               let изображение = new Avalonia.Media.Imaging.Bitmap(имяИзображения) :> Avalonia.Media.IImage
               изображениеСсыл := изображение
               Мое.Приложение.Вызвать(fun () -> Мое.Приложение.Холст.СделатьНедействительным())
               изображениеСсыл
            else             
               let изображениеСсыл = ref null                 
               Мое.Приложение.Вызвать(fun () ->
                  use поток = Ресурс.ПолучитьПоток(имяИзображения)
                  изображениеСсыл := new Avalonia.Media.Imaging.Bitmap(поток) :> Avalonia.Media.IImage
               )
               изображениеСсыл
         ФигураИзображения(изображениеСсыл) |> добавитьФигуру имя
      имя
   static member ДобавитьТекст(текст) =
      let name = генИмя "Text"
      ФигураТекста(ref текст, шрифт(), кисть()) |> добавитьФигуру name
      name
   static member СкрытьФигуру(имяФигуры) =      
      Мое.Приложение.Вызвать (fun () -> Мое.Приложение.Холст.УстановитьВидимостьФигуры(имяФигуры,false))      
   static member ПоказатьФигуру(имяФигуры) =
      Мое.Приложение.Вызвать (fun () -> Мое.Приложение.Холст.УстановитьВидимостьФигуры(имяФигуры,true))      
   static member Переместить(имяФигуры,x,y) =
      onShape имяФигуры (fun инфо ->
         инфо.Смещение <- Point(x,y)
         Мое.Приложение.Вызвать (fun () -> Мое.Приложение.Холст.ПереместитьФигуру(имяФигуры,инфо.Смещение))
      )
   static member Переместить(имяФигуры,x:int,y:int) =
      Фигуры.Переместить(имяФигуры, float x, float y)
   static member ПолучитьЛево(имяФигуры) =      
      match фигуры.TryGetValue(имяФигуры) with
      | true, info -> info.Смещение.X
      | false, _ -> 0.0
   static member ПолучитьВерх(имяФигуры) =
      match фигуры.TryGetValue(имяФигуры) with
      | true, info -> info.Смещение.Y
      | false, _ -> 0.0
   static member SetOpacity(имяФигуры, opacity) =
      onShape имяФигуры (fun info ->
         info.Opacity <- opacity
         Мое.Приложение.Вызвать (fun () -> Мое.Приложение.Холст.УстановитьНепрозрачностьФигуры(имяФигуры,opacity))
      )
   static member SetOpacity(имяФигуры, opacity:int) =
      Фигуры.SetOpacity(имяФигуры, float opacity)
   static member GetOpacity(имяФигуры) =
      match фигуры.TryGetValue(имяФигуры) with
      | true, info -> info.Opacity
      | false, _ -> 1.0
   static member Повернуть(имяФигуры, угол) =
      match фигуры.TryGetValue(имяФигуры) with
      | true, info ->
         Мое.Приложение.Вызвать (fun () -> Мое.Приложение.Холст.УстановитьВращениеФигуры(имяФигуры,угол))
      | false, _ -> ()
   static member Повернуть(имяФигуры, угол:int) =
      Фигуры.Повернуть(имяФигуры, float угол)
   static member Масштаб(имяФигуры, scaleX, scaleY) =
      match фигуры.TryGetValue(имяФигуры) with
      | true, info ->
         Мое.Приложение.Вызвать (fun () -> Мое.Приложение.Холст.УстановитьМасштабФигуры(имяФигуры,scaleX,scaleY))
      | false, _ -> ()
   static member УстановитьТекст(имяФигуры, текст) =
      onShape имяФигуры (fun info ->
         match info.Фигура with
         | ФигураТекста(textRef, font, color) ->
            Мое.Приложение.Вызвать (fun () -> textRef := текст; Мое.Приложение.Холст.СделатьНедействительным())
         | _ -> invalidOp "Expecting text shape"
      )       
   static member Animate(имяФигуры,x:float,y:float,ms:int) =
      Фигуры.Переместить(имяФигуры, x, y)
   static member Animate(имяФигуры,x:int,y:int,мс:int) =
      Фигуры.Animate(имяФигуры, float x, float y, мс)

