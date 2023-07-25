namespace Библиотека

open System
open Avalonia.Media.Imaging
open Avalonia.Media

[<Sealed>]
type ГрафическоеОкно private () =   
   static let рнд = Random()
   static let mutable фоновыйЦвет = Цвета.Белый
   static let mutable ширина = 640
   static let mutable высота = 480
   static let перо () = Перо(ГрафическоеОкно.ЦветПера,ГрафическоеОкно.ШиринаПера)
   static let кисть () = ГрафическоеОкно.ЦветКисти
   static let шрифт () = 
      Шрифт.Шрифт(ГрафическоеОкно.РазмерШрифта,ГрафическоеОкно.ИмяШрифта,ГрафическоеОкно.ЖирностьШрифта, ГрафическоеОкно.КурсивностьШрифта)
   static let нарисовать drawing = addDrawing drawing      
   static let нарисоватьВ (x,y) drawing = addDrawingAt drawing (x,y)
   static member Заголовок
      with set title =
         Мое.Приложение.Вызвать (fun () -> Мое.Приложение.Окно.Title <- title)
   static member ЦветФона
      with get () = фоновыйЦвет
      and set цвет = 
         фоновыйЦвет <- цвет
         Мое.Приложение.Вызвать (fun () -> Мое.Приложение.Холст.Background <- new Avalonia.Media.SolidColorBrush(кXwtЦвету цвет))
   static member Ширина
      with get () = ширина
      and set новаяШирина =
         ширина <- новаяШирина
         Мое.Приложение.Вызвать (fun () -> Мое.Приложение.SetWindowWidth(float новаяШирина))
   static member Высота
      with get () = высота
      and set новаяВысота =
         высота <- новаяВысота
         Мое.Приложение.Вызвать (fun () -> Мое.Приложение.SetWindowHeight(float новаяВысота))
   static member CanResize
      with get () = true
      and set (value:bool) = ()
   static member val ЦветПера = Цвета.Черный with get, set
   static member val ШиринаПера = 2.0 with get, set
   static member val ЦветКисти = Цвета.Purple with get,set
   static member val РазмерШрифта = 12.0 with get,set
   static member val ИмяШрифта = "" with get,set
   static member val ЖирностьШрифта = false with get,set
   static member val КурсивностьШрифта = false with get,set
   static member Очистить () =
      Мое.Приложение.Вызвать (fun () -> Мое.Приложение.Холст.ОчиститьРисунки())
   static member НарисоватьЛинию(x1,y1,x2,y2) =
      НарисоватьЛинию(Линия(x1,y1,x2,y2),перо()) |> нарисовать
   static member НарисоватьЛинию(x1:int,y1:int,x2:int,y2:int) =
      ГрафическоеОкно.НарисоватьЛинию(float x1, float y1, float x2, float y2)
   static member НарисоватьПрямоугольник(x,y,width,height) =
      НарисоватьПрямоугольник(Прямоугольник(width,height),перо()) |> нарисоватьВ (x,y)
   static member НарисоватьПрямоугольник(x:int,y:int,width:int,height:int) =
      ГрафическоеОкно.НарисоватьПрямоугольник(float x, float y, float width, float height)
   static member НарисоватьТреугольник(x1,y1,x2,y2,x3,y3) =
      НарисоватьТреугольник(Треугольник(x1,y1,x2,y2,x3,y3),перо()) |> нарисовать
   static member НарисоватьЭллипс(x,y,width,height) =
      НарисоватьЭллипс(Эллипс(width,height),перо()) |> нарисоватьВ (x,y)
   static member НарисоватьЭллипс(x:int,y:int,width:int,height:int) =
      ГрафическоеОкно.НарисоватьЭллипс(float x, float y, float width, float height)
   static member НарисоватьИзображение(имяИзображения,x,y) =
      let ссылкаИзображение =
         match СписокИзображений.ПопробоватьПолучитьБайтыИзображения имяИзображения with
         | Some байты -> 
            use потокПамяти = new System.IO.MemoryStream(байты)
            ref (new Bitmap(потокПамяти) :> Avalonia.Media.IImage)
         | None ->
            if имяИзображения.StartsWith("http:") || имяИзображения.StartsWith("https:") 
            then
                let ссылкаИзображение = ref null
                async {
                   let! изображение = Хттп.ЗагрузитьИзображениеАсинх имяИзображения
                   ссылкаИзображение := изображение
                   Мое.Приложение.Вызвать(fun () -> Мое.Приложение.Холст.СделатьНедействительным())
                } |> Async.Start
                ссылкаИзображение
            else
                ref (new Bitmap(Reflection.Assembly.GetEntryAssembly().GetManifestResourceStream(имяИзображения)) :> IImage)
      НарисоватьИзображение(ссылкаИзображение,x,y) |> нарисовать
   static member НарисоватьИзображение(имяИзображения,x:int,y:int) =
      ГрафическоеОкно.НарисоватьИзображение(имяИзображения, float x, float y)
   static member НарисоватьТекст(x,y,текст) =
      НарисоватьТекст(x,y,текст,шрифт(),кисть()) |> нарисовать
   static member НарисоватьТекст(x:int,y:int,текст) =
      ГрафическоеОкно.НарисоватьТекст(float x,float y,текст)
   static member DrawBoundText(x,y,ширина,текст) =
      НарисоватьТекстВРамке(x,y,ширина,текст,шрифт(),кисть()) |> нарисовать
   static member ЗаполнитьПрямоугольник(x,y,ширина,высота) =
      ЗаполнитьПрямоугольник(Прямоугольник(ширина,высота),кисть()) |> нарисоватьВ (x,y)
   static member ЗаполнитьПрямоугольник(x:int,y:int,ширина:int,высота:int) =
      ГрафическоеОкно.ЗаполнитьПрямоугольник(float x,float y,float ширина,float высота)
   static member ЗаполнитьТреугольник(x1,y1,x2,y2,x3,y3) =
      ЗаполнитьТреугольник(Треугольник(x1,y1,x2,y2,x3,y3),кисть()) |> нарисовать
   static member ЗаполнитьЭллипс(x,y,ширина,высота) =
      ЗаполнитьЭллипс(Эллипс(ширина,высота),кисть()) |> нарисоватьВ (x,y)
   static member ЗаполнитьЭллипс(x:int,y:int,ширина:int,высота:int) =
      ЗаполнитьЭллипс(Эллипс(float ширина,float высота),кисть()) |> нарисоватьВ (float x,float y)
   static member ПоследняяКнопка with get() = Мое.Приложение.ПоследняяКнопка
   static member КнопкаОтпущена with set callback = Мое.Приложение.KeyUp <- callback
   static member КнопкаНажата with set callback = Мое.Приложение.KeyDown <- callback 
   static member МышьX with get() = Мое.Приложение.МышьX
   static member МышьY with get() = Мое.Приложение.МышьY
   static member МышьНажата with set callback = Мое.Приложение.MouseDown <- callback
   static member МышьОтпущена with set callback = Мое.Приложение.MouseUp <- callback
   static member МышьПеремещена with set callback = Мое.Приложение.MouseMove <- callback
   static member ПолучитьЦветИзRGB(r,g,b) = Цвет(255uy,byte r,byte g,byte b)
   static member ПолучитьСлучайныйЦвет() : Цвет =
      let байты = [|1uy..3uy|]
      рнд.NextBytes(байты)
      Цвет(255uy,байты.[0],байты.[1],байты.[2])
   static member Показать() = Мое.Приложение.Показать()
   static member Спрятать() = Мое.Приложение.Спрятать()
   static member ПоказатьСообщение(text:string,title) = 
      Мое.Приложение.Вызвать(fun () -> Мое.Приложение.ПоказатьСообщение(text,title))