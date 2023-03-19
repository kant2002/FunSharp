namespace Кітапхана

open System
open System.Collections.Generic
open Avalonia.Media.Imaging

type internal ShapeInfo = { Shape:Пішін; mutable Offset:Avalonia.Point; mutable Opacity:float }

[<Sealed>]
type Пішіндері private () =
   static let pen () = Қауырсын(ГрафикалықТерезе.ҚаламТүсі,ГрафикалықТерезе.PenWidth)
   static let brush () = ГрафикалықТерезе.ҚылқаламТүсі
   static let font () = 
      Қаріп(ГрафикалықТерезе.FontSize,ГрафикалықТерезе.FontName,ГрафикалықТерезе.FontBold, ГрафикалықТерезе.FontItalic)
   static let shapes = Dictionary<string,ShapeInfo>()
   static let addShape name shape =
      let info = { Shape=shape; Offset=Avalonia.Point(); Opacity=1.0 }
      shapes.Add(name,info)      
      addDrawing (DrawShape(name,shape))
   static let onShape shapeName action =
      match shapes.TryGetValue(shapeName) with
      | true, info -> action info
      | false, _ -> ()
   static let genName name = name + Guid.NewGuid().ToString()
   static member Remove(shapeName) =
      My.App.Invoke (fun () -> My.App.Canvas.RemoveShape(shapeName))
   static member AddLine(x1,y1,x2,y2) =
      let name = genName "Line"
      СызықПішіні(Сызық(x1,y1,x2,y2),pen()) |> addShape name
      name
   static member AddLine(x1:int,y1:int,x2:int,y2:int) =
      Пішіндері.AddLine(float x1, float y1, float x2, float y2)
   static member AddRectangle(width,height) =
      let name = genName "Rectangle"
      ТікПішіні(Rect(width,height),pen(),brush()) |> addShape name
      name
   static member AddRectangle(width:int,height:int) =
      Пішіндері.AddRectangle(float width, float height)
   static member AddTriangle(x1,y1,x2,y2,x3,y3) =
      let name = genName "Triangle"
      ҮшбұрышПішіні(Үшбұрыш(x1,y1,x2,y2,x3,y3),pen(),brush()) |> addShape name
      name
   static member AddEllipse(width,height) =
      let name = genName "Ellipse"
      ЭллипсПішіні(Эллипс(width,height),pen(),brush()) |> addShape name
      name
   static member AddEllipse(width:int,height:int) =
      Пішіндері.AddEllipse(float width,float height)
   static member AddImage(imageName) =
      let name = genName "Image"
      match ImageList.TryGetImageBytes(imageName) with
      | Some bytes ->
         let stream = new System.IO.MemoryStream(bytes)
         let image = new Avalonia.Media.Imaging.Bitmap(stream) :> Avalonia.Media.IImage
         КескінПішіні(ref image) |> addShape name
      | None ->
         let imageRef = 
            if imageName.StartsWith("http:") || imageName.StartsWith("https:") 
            then
               let imageRef = ref null
               async {
                  let! image = Http.LoadImageAsync imageName
                  imageRef := image
                  My.App.Invoke(fun () -> My.App.Canvas.Invalidate())
               } |> Async.Start
               imageRef
            else             
               let imageRef = ref null                 
               My.App.Invoke(fun () ->
                  use stream = Resource.GetStream(imageName)
                  imageRef := new Bitmap(stream) :> Avalonia.Media.IImage
               )
               imageRef
         КескінПішіні(imageRef) |> addShape name
      name
   static member AddText(text) =
      let name = genName "Text"
      МәтінПішіні(ref text, font(), brush()) |> addShape name
      name
   static member HideShape(shapeName) =      
      My.App.Invoke (fun () -> My.App.Canvas.SetShapeVisibility(shapeName,false))      
   static member ShowShape(shapeName) =
      My.App.Invoke (fun () -> My.App.Canvas.SetShapeVisibility(shapeName,true))      
   static member Жылжытуға(shapeName,x,y) =
      onShape shapeName (fun info ->
         info.Offset <- Avalonia.Point(x,y)
         My.App.Invoke (fun () -> My.App.Canvas.MoveShape(shapeName,info.Offset))
      )
   static member Move(shapeName,x:int,y:int) =
      Пішіндері.Жылжытуға(shapeName, float x, float y)
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
         My.App.Invoke (fun () -> My.App.Canvas.SetShapeOpacity(shapeName,opacity))
      )
   static member SetOpacity(shapeName, opacity:int) =
      Пішіндері.SetOpacity(shapeName, float opacity)
   static member GetOpacity(shapeName) =
      match shapes.TryGetValue(shapeName) with
      | true, info -> info.Opacity
      | false, _ -> 1.0
   static member Rotate(shapeName, angle) =
      match shapes.TryGetValue(shapeName) with
      | true, info ->
         My.App.Invoke (fun () -> My.App.Canvas.SetShapeRotation(shapeName,angle))
      | false, _ -> ()
   static member Rotate(shapeName, angle:int) =
      Пішіндері.Rotate(shapeName, float angle)
   static member Zoom(shapeName, scaleX, scaleY) =
      match shapes.TryGetValue(shapeName) with
      | true, info ->
         My.App.Invoke (fun () -> My.App.Canvas.SetShapeScale(shapeName,scaleX,scaleY))
      | false, _ -> ()
   static member SetText(shapeName, text) =
      onShape shapeName (fun info ->
         match info.Shape with
         | МәтінПішіні(textRef, font, color) ->
            My.App.Invoke (fun () -> textRef := text; My.App.Canvas.Invalidate())
         | _ -> invalidOp "Expecting text shape"
      )       
   static member Animate(shapeName,x:float,y:float,ms:int) =
      Пішіндері.Жылжытуға(shapeName, x, y)
   static member Animate(shapeName,x:int,y:int,ms:int) =
      Пішіндері.Animate(shapeName, float x, float y, ms)

