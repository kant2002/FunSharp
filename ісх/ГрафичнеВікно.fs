namespace Бiблiотека

open System

[<Sealed>]
type ГрафичнеВікно private () =   
   static let рнд = Random()
   static let mutable фоновыйЦвет = Кольори.White
   static let mutable ширина = 640
   static let mutable высота = 480
   static let перо () = Перо(ГрафичнеВікно.КолірПера,ГрафичнеВікно.ШиринаПера)
   static let кисть () = ГрафичнеВікно.ЦветКисти
   static let шрифт () = 
      Шрифт.Шрифт(ГрафичнеВікно.РозмірШрифта,ГрафичнеВікно.ИмяШрифта,ГрафичнеВікно.ЖирностьШрифта, ГрафичнеВікно.КурсивностьШрифта)
   static let нарисовать drawing = addDrawing drawing      
   static let нарисоватьВ (x,y) drawing = addDrawingAt drawing (x,y)
   static member Заголовок
      with set title =
         Мое.Приложение.Вызвать (fun () -> Мое.Приложение.Окно.Title <- title)
   static member КолірФона
      with get () = фоновыйЦвет
      and set цвет = 
         фоновыйЦвет <- цвет
         Мое.Приложение.Вызвать (fun () -> Мое.Приложение.Холст.BackgroundColor <- кXwtЦвету цвет)
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
   static member val КолірПера = Кольори.Black with get, set
   static member val ШиринаПера = 2.0 with get, set
   static member val ЦветКисти = Кольори.Purple with get,set
   static member val РозмірШрифта = 12.0 with get,set
   static member val ИмяШрифта = "" with get,set
   static member val ЖирностьШрифта = false with get,set
   static member val КурсивностьШрифта = false with get,set
   static member Очистити () =
      Мое.Приложение.Вызвать (fun () -> Мое.Приложение.Холст.ОчиститиМалюнки())
   static member НамалюватиЛінію(x1,y1,x2,y2) =
      НамалюватиЛінію(Линия(x1,y1,x2,y2),перо()) |> нарисовать
   static member НамалюватиЛінію(x1:int,y1:int,x2:int,y2:int) =
      ГрафичнеВікно.НамалюватиЛінію(float x1, float y1, float x2, float y2)
   static member НамалюватиПрямокутник(x,y,width,height) =
      НамалюватиПрямокутник(Прямоугольник(width,height),перо()) |> нарисоватьВ (x,y)
   static member НамалюватиПрямокутник(x:int,y:int,width:int,height:int) =
      ГрафичнеВікно.НамалюватиПрямокутник(float x, float y, float width, float height)
   static member НамалюватиТрикутник(x1,y1,x2,y2,x3,y3) =
      НамалюватиТрикутник(Треугольник(x1,y1,x2,y2,x3,y3),перо()) |> нарисовать
   static member НамалюватиЕліпс(x,y,width,height) =
      НамалюватиЕліпс(Эллипс(width,height),перо()) |> нарисоватьВ (x,y)
   static member НамалюватиЕліпс(x:int,y:int,width:int,height:int) =
      ГрафичнеВікно.НамалюватиЕліпс(float x, float y, float width, float height)
   static member НамалюватиЗображення(імяЗображення,x,y) =
      let ссылкаИзображение =
         match СписокЗображень.ПопробоватьПолучитьБайтыИзображения імяЗображення with
         | Some байты -> 
            use потокПамяти = new System.IO.MemoryStream(байты)
            ref (Xwt.Drawing.Image.FromStream(потокПамяти))           
         | None ->
            if імяЗображення.StartsWith("http:") || імяЗображення.StartsWith("https:") 
            then
                let ссылкаИзображение = ref null
                async {
                   let! изображение = Хттп.ЗагрузитьИзображениеАсинх імяЗображення
                   ссылкаИзображение := изображение
                   Мое.Приложение.Вызвать(fun () -> Мое.Приложение.Холст.СделатьНедействительным())
                } |> Async.Start
                ссылкаИзображение
            else
                ref (Xwt.Drawing.Image.FromResource(імяЗображення))
      НамалюватиЗображення(ссылкаИзображение,x,y) |> нарисовать
   static member НамалюватиЗображення(імяЗображення,x:int,y:int) =
      ГрафичнеВікно.НамалюватиЗображення(імяЗображення, float x, float y)
   static member НарисоватьТекст(x,y,текст) =
      НарисоватьТекст(x,y,текст,шрифт(),кисть()) |> нарисовать
   static member НарисоватьТекст(x:int,y:int,текст) =
      ГрафичнеВікно.НарисоватьТекст(float x,float y,текст)
   static member DrawBoundText(x,y,ширина,текст) =
      НарисоватьТекстВРамке(x,y,ширина,текст,шрифт(),кисть()) |> нарисовать
   static member ЗаполнитьПрямоугольник(x,y,ширина,высота) =
      ЗаполнитьПрямоугольник(Прямоугольник(ширина,высота),кисть()) |> нарисоватьВ (x,y)
   static member ЗаполнитьПрямоугольник(x:int,y:int,ширина:int,высота:int) =
      ГрафичнеВікно.ЗаполнитьПрямоугольник(float x,float y,float ширина,float высота)
   static member ЗаполнитьТреугольник(x1,y1,x2,y2,x3,y3) =
      ЗаполнитьТреугольник(Треугольник(x1,y1,x2,y2,x3,y3),кисть()) |> нарисовать
   static member ЗаповнитиЕліпс(x,y,ширина,высота) =
      ЗаповнитиЕліпс(Эллипс(ширина,высота),кисть()) |> нарисоватьВ (x,y)
   static member ЗаповнитиЕліпс(x:int,y:int,ширина:int,высота:int) =
      ЗаповнитиЕліпс(Эллипс(float ширина,float высота),кисть()) |> нарисоватьВ (float x,float y)
   static member ОстанняКнопка with get() = Мое.Приложение.ОстанняКнопка
   static member КнопкаОтпущена with set callback = Мое.Приложение.KeyUp <- callback
   static member КнопкаНатиснута with set callback = Мое.Приложение.KeyDown <- callback 
   static member МишаX with get() = Мое.Приложение.МишаX
   static member МишаY with get() = Мое.Приложение.МишаY
   static member МишаНатиснута with set callback = Мое.Приложение.MouseDown <- callback
   static member МышьОтпущена with set callback = Мое.Приложение.MouseUp <- callback
   static member МишаПереміщена with set callback = Мое.Приложение.MouseMove <- callback
   static member ПолучитьЦветИзRGB(r,g,b) = Колір(255uy,byte r,byte g,byte b)
   static member ОтриматиВипадковийКолір() : Колір =
      let байты = [|1uy..3uy|]
      рнд.NextBytes(байты)
      Колір(255uy,байты.[0],байты.[1],байты.[2])
   static member Показать() = Мое.Приложение.Показать()
   static member Сховати() = Мое.Приложение.Сховати()
   static member ПоказатьСообщение(text:string,title) = 
      Мое.Приложение.Вызвать(fun () -> Мое.Приложение.ПоказатьСообщение(text,title))