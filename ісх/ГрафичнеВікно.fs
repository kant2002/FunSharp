namespace Бібліотека

open System
open Avalonia.Media.Imaging
open Avalonia.Media

[<Sealed>]
type ГрафичнеВікно private () =   
   static let рнд = Random()
   static let mutable фоновийКолір = Кольори.Білий
   static let mutable ширина = 640
   static let mutable висота = 480
   static let перо () = Перо(ГрафичнеВікно.КолірПера,ГрафичнеВікно.ШиринаПера)
   static let пензлик () = ГрафичнеВікно.КолірПензлика
   static let шрифт () = 
      Шрифт.Шрифт(ГрафичнеВікно.РозмірШрифта,ГрафичнеВікно.ІмяШрифта,ГрафичнеВікно.ЖирністьШрифта, ГрафичнеВікно.КурсивністьШрифта)
   static let намалювати малюнок = додатиМалюнок малюнок      
   static let намалюватиУ (x,y) малюнок = додатиМалюнокУ малюнок (x,y)
   static member Заголовок
      with set заголовок =
         Моя.Апплікація.Викликати (fun () -> Моя.Апплікація.Вікно.Title <- заголовок)
   static member КолірФона
      with get () = фоновийКолір
      and set цвет = 
         фоновийКолір <- цвет
         Моя.Апплікація.Викликати (fun () -> Моя.Апплікація.Полотно.Background <- new Avalonia.Media.SolidColorBrush(доКольораАвалонії цвет))
   static member Ширина
      with get () = ширина
      and set новаШирина =
         ширина <- новаШирина
         Моя.Апплікація.Викликати (fun () -> Моя.Апплікація.ВстановитиШиринуВікна(float новаШирина))
   static member Висота
      with get () = висота
      and set новаВысота =
         висота <- новаВысота
         Моя.Апплікація.Викликати (fun () -> Моя.Апплікація.ВстановитиВисотуВікна(float новаВысота))
   static member МожнаЗмінитиРозмір
      with get () = true
      and set (значення:bool) = ()
   static member val КолірПера = Кольори.Чорний with get, set
   static member val ШиринаПера = 2.0 with get, set
   static member val КолірПензлика = Кольори.Purple with get,set
   static member val РозмірШрифта = 12.0 with get,set
   static member val ІмяШрифта = "" with get,set
   static member val ЖирністьШрифта = false with get,set
   static member val КурсивністьШрифта = false with get,set
   static member Очистити () =
      Моя.Апплікація.Викликати (fun () -> Моя.Апплікація.Полотно.ОчиститиМалюнки())
   static member НамалюватиЛінію(x1,y1,x2,y2) =
      НамалюватиЛінію(Лінія(x1,y1,x2,y2),перо()) |> намалювати
   static member НамалюватиЛінію(x1:int,y1:int,x2:int,y2:int) =
      ГрафичнеВікно.НамалюватиЛінію(float x1, float y1, float x2, float y2)
   static member НамалюватиПрямокутник(x,y,width,height) =
      НамалюватиПрямокутник(Прямокутник(width,height),перо()) |> намалюватиУ (x,y)
   static member НамалюватиПрямокутник(x:int,y:int,width:int,height:int) =
      ГрафичнеВікно.НамалюватиПрямокутник(float x, float y, float width, float height)
   static member НамалюватиТрикутник(x1,y1,x2,y2,x3,y3) =
      НамалюватиТрикутник(Трикутник(x1,y1,x2,y2,x3,y3),перо()) |> намалювати
   static member НамалюватиЕліпс(x,y,width,height) =
      НамалюватиЕлліпс(Елліпс(width,height),перо()) |> намалюватиУ (x,y)
   static member НамалюватиЕліпс(x:int,y:int,width:int,height:int) =
      ГрафичнеВікно.НамалюватиЕліпс(float x, float y, float width, float height)
   static member НамалюватиЗображення(імяЗображення,x,y) =
      let посиланняЗображення =
         match СписокЗображень.СпробуватиОтриматиБайтиЗображення імяЗображення with
         | Some байти -> 
            use потокПамяти = new System.IO.MemoryStream(байти)
            ref (new Bitmap(потокПамяти) :> Avalonia.Media.IImage)           
         | None ->
            if імяЗображення.StartsWith("http:") || імяЗображення.StartsWith("https:") 
            then
                let посиланняЗображення = ref null
                async {
                   let! зображення = Хттп.ЗавантажитиЗображенняАсінх імяЗображення
                   посиланняЗображення := зображення
                   Моя.Апплікація.Викликати(fun () -> Моя.Апплікація.Полотно.ЗробитиНедійсним())
                } |> Async.Start
                посиланняЗображення
            else
                ref (new Bitmap(Reflection.Assembly.GetEntryAssembly().GetManifestResourceStream(імяЗображення)) :> IImage)
      НамалюватиЗображення(посиланняЗображення,x,y) |> намалювати
   static member НамалюватиЗображення(імяЗображення,x:int,y:int) =
      ГрафичнеВікно.НамалюватиЗображення(імяЗображення, float x, float y)
   static member НамалюватиТекст(x,y,текст) =
      НамалюватиТекст(x,y,текст,шрифт(),пензлик()) |> намалювати
   static member НамалюватиТекст(x:int,y:int,текст) =
      ГрафичнеВікно.НамалюватиТекст(float x,float y,текст)
   static member НамалюйтеОбмеженийТекст(x,y,ширина,текст) =
      НамалюватиТекстУРамці(x,y,ширина,текст,шрифт(),пензлик()) |> намалювати
   static member ЗаповнитиПрямокутник(x,y,ширина,высота) =
      ЗаповнитиПрямокутник(Прямокутник(ширина,высота),пензлик()) |> намалюватиУ (x,y)
   static member ЗаповнитиПрямокутник(x:int,y:int,ширина:int,высота:int) =
      ГрафичнеВікно.ЗаповнитиПрямокутник(float x,float y,float ширина,float высота)
   static member ЗаповнитиТрикутник(x1,y1,x2,y2,x3,y3) =
      ЗаповнитиТрикутник(Трикутник(x1,y1,x2,y2,x3,y3),пензлик()) |> намалювати
   static member ЗаповнитиЕліпс(x,y,ширина,высота) =
      ЗаповнитиЕліпс(Елліпс(ширина,высота),пензлик()) |> намалюватиУ (x,y)
   static member ЗаповнитиЕліпс(x:int,y:int,ширина:int,высота:int) =
      ЗаповнитиЕліпс(Елліпс(float ширина,float высота),пензлик()) |> намалюватиУ (float x,float y)
   static member ОстанняКнопка with get() = Моя.Апплікація.ОстанняКнопка
   static member КлавішаДогори with set callback = Моя.Апплікація.KeyUp <- callback
   static member КлавішаНатиснута with set callback = Моя.Апплікація.KeyDown <- callback 
   static member МишаX with get() = Моя.Апплікація.МишаX
   static member МишаY with get() = Моя.Апплікація.МишаY
   static member МишаДонизу with set callback = Моя.Апплікація.MouseDown <- callback
   static member МышьДогори with set callback = Моя.Апплікація.MouseUp <- callback
   static member МишаПереміщена with set callback = Моя.Апплікація.MouseMove <- callback
   static member ОтриматиКолірЗRGB(r,g,b) = Колір(255uy,byte r,byte g,byte b)
   static member ОтриматиВипадковийКолір() : Колір =
      let байты = [|1uy..3uy|]
      рнд.NextBytes(байты)
      Колір(255uy,байты.[0],байты.[1],байты.[2])
   static member Показати() = Моя.Апплікація.Показати()
   static member Сховати() = Моя.Апплікація.Сховати()
   static member ПоказатиПовідомлення(текст:string,заголовок) = 
      Моя.Апплікація.Викликати(fun () -> Моя.Апплікація.ПоказатьСообщение(текст,заголовок))