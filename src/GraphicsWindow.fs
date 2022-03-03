namespace Library

open System

[<Sealed>]
type ГрафическоеОкно private () =   
   static let rnd = Random()
   static let mutable backgroundColor = Colors.White
   static let mutable width = 640
   static let mutable height = 480
   static let pen () = Pen(ГрафическоеОкно.PenColor,ГрафическоеОкно.PenWidth)
   static let brush () = ГрафическоеОкно.BrushColor
   static let font () = 
      Font.Font(ГрафическоеОкно.FontSize,ГрафическоеОкно.FontName,ГрафическоеОкно.FontBold, ГрафическоеОкно.FontItalic)
   static let draw drawing = addDrawing drawing      
   static let drawAt (x,y) drawing = addDrawingAt drawing (x,y)
   static member Title
      with set title =
         Мое.Приложение.Вызвать (fun () -> Мое.Приложение.Окно.Title <- title)
   static member BackgroundColor
      with get () = backgroundColor
      and set color = 
         backgroundColor <- color
         Мое.Приложение.Вызвать (fun () -> Мое.Приложение.Холст.BackgroundColor <- toXwtColor color)
   static member Width
      with get () = width
      and set newWidth =
         width <- newWidth
         Мое.Приложение.Вызвать (fun () -> Мое.Приложение.SetWindowWidth(float newWidth))
   static member Height
      with get () = height
      and set newHeight =
         height <- newHeight
         Мое.Приложение.Вызвать (fun () -> Мое.Приложение.SetWindowHeight(float newHeight))
   static member CanResize
      with get () = true
      and set (value:bool) = ()
   static member val PenColor = Colors.Black with get, set
   static member val PenWidth = 2.0 with get, set
   static member val BrushColor = Colors.Purple with get,set
   static member val FontSize = 12.0 with get,set
   static member val FontName = "" with get,set
   static member val FontBold = false with get,set
   static member val FontItalic = false with get,set
   static member Clear () =
      Мое.Приложение.Вызвать (fun () -> Мое.Приложение.Холст.ClearDrawings())
   static member DrawLine(x1,y1,x2,y2) =
      DrawLine(Line(x1,y1,x2,y2),pen()) |> draw
   static member DrawLine(x1:int,y1:int,x2:int,y2:int) =
      ГрафическоеОкно.DrawLine(float x1, float y1, float x2, float y2)
   static member DrawRectangle(x,y,width,height) =
      DrawRect(Rect(width,height),pen()) |> drawAt (x,y)
   static member DrawRectangle(x:int,y:int,width:int,height:int) =
      ГрафическоеОкно.DrawRectangle(float x, float y, float width, float height)
   static member DrawTriangle(x1,y1,x2,y2,x3,y3) =
      DrawTriangle(Triangle(x1,y1,x2,y2,x3,y3),pen()) |> draw
   static member DrawEllipse(x,y,width,height) =
      DrawEllipse(Ellipse(width,height),pen()) |> drawAt (x,y)
   static member DrawEllipse(x:int,y:int,width:int,height:int) =
      ГрафическоеОкно.DrawEllipse(float x, float y, float width, float height)
   static member DrawImage(imageName,x,y) =
      let imageRef =
         match ImageList.TryGetImageBytes imageName with
         | Some bytes -> 
            use memoryStream = new System.IO.MemoryStream(bytes)
            ref (Xwt.Drawing.Image.FromStream(memoryStream))           
         | None ->
            if imageName.StartsWith("http:") || imageName.StartsWith("https:") 
            then
                let imageRef = ref null
                async {
                   let! image = Http.LoadImageAsync imageName
                   imageRef := image
                   Мое.Приложение.Вызвать(fun () -> Мое.Приложение.Холст.Invalidate())
                } |> Async.Start
                imageRef
            else
                ref (Xwt.Drawing.Image.FromResource(imageName))
      DrawImage(imageRef,x,y) |> draw
   static member DrawImage(imageName,x:int,y:int) =
      ГрафическоеОкно.DrawImage(imageName, float x, float y)
   static member DrawText(x,y,text) =
      DrawText(x,y,text,font(),brush()) |> draw
   static member DrawText(x:int,y:int,text) =
      ГрафическоеОкно.DrawText(float x,float y,text)
   static member DrawBoundText(x,y,width,text) =
      DrawBoundText(x,y,width,text,font(),brush()) |> draw
   static member FillRectangle(x,y,width,height) =
      FillRect(Rect(width,height),brush()) |> drawAt (x,y)
   static member FillRectangle(x:int,y:int,width:int,height:int) =
      ГрафическоеОкно.FillRectangle(float x,float y,float width,float height)
   static member FillTriangle(x1,y1,x2,y2,x3,y3) =
      FillTriangle(Triangle(x1,y1,x2,y2,x3,y3),brush()) |> draw
   static member FillEllipse(x,y,width,height) =
      FillEllipse(Ellipse(width,height),brush()) |> drawAt (x,y)
   static member FillEllipse(x:int,y:int,width:int,height:int) =
      FillEllipse(Ellipse(float width,float height),brush()) |> drawAt (float x,float y)
   static member LastKey with get() = Мое.Приложение.LastKey
   static member KeyUp with set callback = Мое.Приложение.KeyUp <- callback
   static member KeyDown with set callback = Мое.Приложение.KeyDown <- callback 
   static member MouseX with get() = Мое.Приложение.MouseX
   static member MouseY with get() = Мое.Приложение.MouseY
   static member MouseDown with set callback = Мое.Приложение.MouseDown <- callback
   static member MouseUp with set callback = Мое.Приложение.MouseUp <- callback
   static member MouseMove with set callback = Мое.Приложение.MouseMove <- callback
   static member GetColorFromRGB(r,g,b) = Color(255uy,byte r,byte g,byte b)
   static member GetRandomColor() : Color =
      let bytes = [|1uy..3uy|]
      rnd.NextBytes(bytes)
      Color(255uy,bytes.[0],bytes.[1],bytes.[2])
   static member Show() = Мое.Приложение.Показать()
   static member Hide() = Мое.Приложение.Спрятать()
   static member ShowMessage(text:string,title) = 
      Мое.Приложение.Вызвать(fun () -> Мое.Приложение.ПоказатьСообщение(text,title))