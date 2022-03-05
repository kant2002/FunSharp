namespace Библиотека

open System

[<Sealed>]
type ГрафическоеОкно private () =   
   static let rnd = Random()
   static let mutable фоновыйЦвет = Цвета.White
   static let mutable ширина = 640
   static let mutable высота = 480
   static let перо () = Pen(ГрафическоеОкно.ЦветПера,ГрафическоеОкно.PenWidth)
   static let кисть () = ГрафическоеОкно.ЦветКисти
   static let шрифт () = 
      Шрифт.Font(ГрафическоеОкно.FontSize,ГрафическоеОкно.FontName,ГрафическоеОкно.FontBold, ГрафическоеОкно.FontItalic)
   static let нарисовать drawing = addDrawing drawing      
   static let нарисоватьВ (x,y) drawing = addDrawingAt drawing (x,y)
   static member Заголовок
      with set title =
         Мое.Приложение.Вызвать (fun () -> Мое.Приложение.Окно.Title <- title)
   static member ЦветФона
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
   static member val ЦветПера = Цвета.Black with get, set
   static member val PenWidth = 2.0 with get, set
   static member val ЦветКисти = Цвета.Purple with get,set
   static member val FontSize = 12.0 with get,set
   static member val FontName = "" with get,set
   static member val FontBold = false with get,set
   static member val FontItalic = false with get,set
   static member Очистить () =
      Мое.Приложение.Вызвать (fun () -> Мое.Приложение.Холст.ClearDrawings())
   static member НарисоватьЛинию(x1,y1,x2,y2) =
      DrawLine(Line(x1,y1,x2,y2),перо()) |> нарисовать
   static member НарисоватьЛинию(x1:int,y1:int,x2:int,y2:int) =
      ГрафическоеОкно.НарисоватьЛинию(float x1, float y1, float x2, float y2)
   static member DrawRectangle(x,y,width,height) =
      DrawRect(Rect(width,height),перо()) |> нарисоватьВ (x,y)
   static member DrawRectangle(x:int,y:int,width:int,height:int) =
      ГрафическоеОкно.DrawRectangle(float x, float y, float width, float height)
   static member НарисоватьТреугольник(x1,y1,x2,y2,x3,y3) =
      DrawTriangle(Triangle(x1,y1,x2,y2,x3,y3),перо()) |> нарисовать
   static member DrawEllipse(x,y,width,height) =
      DrawEllipse(Ellipse(width,height),перо()) |> нарисоватьВ (x,y)
   static member DrawEllipse(x:int,y:int,width:int,height:int) =
      ГрафическоеОкно.DrawEllipse(float x, float y, float width, float height)
   static member НарисоватьИзображение(имяИзображения,x,y) =
      let imageRef =
         match ImageList.TryGetImageBytes имяИзображения with
         | Some bytes -> 
            use memoryStream = new System.IO.MemoryStream(bytes)
            ref (Xwt.Drawing.Image.FromStream(memoryStream))           
         | None ->
            if имяИзображения.StartsWith("http:") || имяИзображения.StartsWith("https:") 
            then
                let imageRef = ref null
                async {
                   let! image = Http.LoadImageAsync имяИзображения
                   imageRef := image
                   Мое.Приложение.Вызвать(fun () -> Мое.Приложение.Холст.Invalidate())
                } |> Async.Start
                imageRef
            else
                ref (Xwt.Drawing.Image.FromResource(имяИзображения))
      DrawImage(imageRef,x,y) |> нарисовать
   static member НарисоватьИзображение(imageName,x:int,y:int) =
      ГрафическоеОкно.НарисоватьИзображение(imageName, float x, float y)
   static member НарисоватьТекст(x,y,text) =
      DrawText(x,y,text,шрифт(),кисть()) |> нарисовать
   static member НарисоватьТекст(x:int,y:int,text) =
      ГрафическоеОкно.НарисоватьТекст(float x,float y,text)
   static member DrawBoundText(x,y,width,text) =
      DrawBoundText(x,y,width,text,шрифт(),кисть()) |> нарисовать
   static member ЗаполнитьПрямоугольник(x,y,width,height) =
      FillRect(Rect(width,height),кисть()) |> нарисоватьВ (x,y)
   static member ЗаполнитьПрямоугольник(x:int,y:int,width:int,height:int) =
      ГрафическоеОкно.ЗаполнитьПрямоугольник(float x,float y,float width,float height)
   static member ЗаполнитьТреугольник(x1,y1,x2,y2,x3,y3) =
      FillTriangle(Triangle(x1,y1,x2,y2,x3,y3),кисть()) |> нарисовать
   static member ЗаполнитьЭллипс(x,y,width,height) =
      FillEllipse(Ellipse(width,height),кисть()) |> нарисоватьВ (x,y)
   static member ЗаполнитьЭллипс(x:int,y:int,width:int,height:int) =
      FillEllipse(Ellipse(float width,float height),кисть()) |> нарисоватьВ (float x,float y)
   static member LastKey with get() = Мое.Приложение.LastKey
   static member KeyUp with set callback = Мое.Приложение.KeyUp <- callback
   static member KeyDown with set callback = Мое.Приложение.KeyDown <- callback 
   static member MouseX with get() = Мое.Приложение.MouseX
   static member MouseY with get() = Мое.Приложение.MouseY
   static member MouseDown with set callback = Мое.Приложение.MouseDown <- callback
   static member MouseUp with set callback = Мое.Приложение.MouseUp <- callback
   static member MouseMove with set callback = Мое.Приложение.MouseMove <- callback
   static member GetColorFromRGB(r,g,b) = Цвет(255uy,byte r,byte g,byte b)
   static member ПолучитьСлучайныйЦвет() : Цвет =
      let bytes = [|1uy..3uy|]
      rnd.NextBytes(bytes)
      Цвет(255uy,bytes.[0],bytes.[1],bytes.[2])
   static member Показать() = Мое.Приложение.Показать()
   static member Спрятать() = Мое.Приложение.Спрятать()
   static member ПоказатьСообщение(text:string,title) = 
      Мое.Приложение.Вызвать(fun () -> Мое.Приложение.ПоказатьСообщение(text,title))