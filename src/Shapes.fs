namespace Библиотека

open Xwt
open System
open System.Collections.Generic

type internal ShapeInfo = { Shape:Shape; mutable Offset:Point; mutable Opacity:float }

[<Sealed>]
type Shapes private () =
   static let pen () = Pen(ГрафическоеОкно.ЦветПера,ГрафическоеОкно.PenWidth)
   static let brush () = ГрафическоеОкно.ЦветКисти
   static let font () = 
      Font(ГрафическоеОкно.FontSize,ГрафическоеОкно.FontName,ГрафическоеОкно.FontBold, ГрафическоеОкно.FontItalic)
   static let shapes = Dictionary<string,ShapeInfo>()
   static let addShape name shape =
      let info = { Shape=shape; Offset=Point(); Opacity=1.0 }
      shapes.Add(name,info)      
      addDrawing (DrawShape(name,shape))
   static let onShape shapeName action =
      match shapes.TryGetValue(shapeName) with
      | true, info -> action info
      | false, _ -> ()
   static let genName name = name + Guid.NewGuid().ToString()
   static member Удалить(shapeName) =
      Мое.Приложение.Вызвать (fun () -> Мое.Приложение.Холст.RemoveShape(shapeName))
   static member ДобавитьЛинию(x1,y1,x2,y2) =
      let имя = genName "Line"
      LineShape(Line(x1,y1,x2,y2),pen()) |> addShape имя
      имя
   static member ДобавитьЛинию(x1:int,y1:int,x2:int,y2:int) =
      Shapes.ДобавитьЛинию(float x1, float y1, float x2, float y2)
   static member ДобавитьПрямоугольник(ширина,высота) =
      let имя = genName "Rectangle"
      RectShape(Rect(ширина,высота),pen(),brush()) |> addShape имя
      имя
   static member ДобавитьПрямоугольник(ширина:int,высота:int) =
      Shapes.ДобавитьПрямоугольник(float ширина, float высота)
   static member ДобавитьТреугольник(x1,y1,x2,y2,x3,y3) =
      let имя = genName "Triangle"
      TriangleShape(Triangle(x1,y1,x2,y2,x3,y3),pen(),brush()) |> addShape имя
      имя
   static member ДобавитьЭллипс(ширина,высота) =
      let имя = genName "Ellipse"
      EllipseShape(Ellipse(ширина,высота),pen(),brush()) |> addShape имя
      имя
   static member ДобавитьЭллипс(ширина:int,высота:int) =
      Shapes.ДобавитьЭллипс(float ширина,float высота)
   static member ДобавитьИзображение(imageName) =
      let имя = genName "Image"
      match ImageList.TryGetImageBytes(imageName) with
      | Some bytes ->
         let stream = new System.IO.MemoryStream(bytes)
         let image = Xwt.Drawing.Image.FromStream(stream)
         ImageShape(ref image) |> addShape имя
      | None ->
         let imageRef = 
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
               let imageRef = ref null                 
               Мое.Приложение.Вызвать(fun () ->
                  use stream = Resource.GetStream(imageName)
                  imageRef := Xwt.Drawing.Image.FromStream(stream)
               )
               imageRef
         ImageShape(imageRef) |> addShape имя
      имя
   static member ДобавитьТекст(text) =
      let name = genName "Text"
      TextShape(ref text, font(), brush()) |> addShape name
      name
   static member СкрытьФигуру(shapeName) =      
      Мое.Приложение.Вызвать (fun () -> Мое.Приложение.Холст.SetShapeVisibility(shapeName,false))      
   static member ПоказатьФигуру(shapeName) =
      Мое.Приложение.Вызвать (fun () -> Мое.Приложение.Холст.SetShapeVisibility(shapeName,true))      
   static member Переместить(shapeName,x,y) =
      onShape shapeName (fun info ->
         info.Offset <- Point(x,y)
         Мое.Приложение.Вызвать (fun () -> Мое.Приложение.Холст.MoveShape(shapeName,info.Offset))
      )
   static member Переместить(shapeName,x:int,y:int) =
      Shapes.Переместить(shapeName, float x, float y)
   static member GetLeft(shapeName) =      
      match shapes.TryGetValue(shapeName) with
      | true, info -> info.Offset.X
      | false, _ -> 0.0
   static member GetTop(shapeName) =
      match shapes.TryGetValue(shapeName) with
      | true, info -> info.Offset.Y
      | false, _ -> 0.0
   static member SetOpacity(shapeName, opacity) =
      onShape shapeName (fun info ->
         info.Opacity <- opacity
         Мое.Приложение.Вызвать (fun () -> Мое.Приложение.Холст.SetShapeOpacity(shapeName,opacity))
      )
   static member SetOpacity(shapeName, opacity:int) =
      Shapes.SetOpacity(shapeName, float opacity)
   static member GetOpacity(shapeName) =
      match shapes.TryGetValue(shapeName) with
      | true, info -> info.Opacity
      | false, _ -> 1.0
   static member Повернуть(shapeName, angle) =
      match shapes.TryGetValue(shapeName) with
      | true, info ->
         Мое.Приложение.Вызвать (fun () -> Мое.Приложение.Холст.SetShapeRotation(shapeName,angle))
      | false, _ -> ()
   static member Повернуть(shapeName, angle:int) =
      Shapes.Повернуть(shapeName, float angle)
   static member Zoom(shapeName, scaleX, scaleY) =
      match shapes.TryGetValue(shapeName) with
      | true, info ->
         Мое.Приложение.Вызвать (fun () -> Мое.Приложение.Холст.SetShapeScale(shapeName,scaleX,scaleY))
      | false, _ -> ()
   static member УстановитьТекст(shapeName, text) =
      onShape shapeName (fun info ->
         match info.Shape with
         | TextShape(textRef, font, color) ->
            Мое.Приложение.Вызвать (fun () -> textRef := text; Мое.Приложение.Холст.Invalidate())
         | _ -> invalidOp "Expecting text shape"
      )       
   static member Animate(shapeName,x:float,y:float,ms:int) =
      Shapes.Переместить(shapeName, x, y)
   static member Animate(shapeName,x:int,y:int,ms:int) =
      Shapes.Animate(shapeName, float x, float y, ms)

