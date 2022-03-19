namespace Библиотека

open Xwt
open Xwt.Drawing
open Рисовать

[<AllowNullLiteral>]
type internal DrawingCanvas () =
   inherit Canvas ()
   let turtleImage = Xwt.Drawing.Image.FromResource(typeof<DrawingCanvas>, "FunSharp.Library.черепаха.png")
   let drawings = ResizeArray<ИнфоРисунка>()
   let turtle =
      let w,h = turtleImage.Width, turtleImage.Height
      {Рисунок=НарисоватьИзображение(ref turtleImage,-w/2.,-h/2.); Смещение=Point(); Непрозрачность=None; Видим=false; Врашение=None; Масштаб=None}
   let onShape shapeName f =
      drawings
      |> Seq.tryPick (function
         | { Рисунок=НарисоватьФигуру(name,_) } as info when name = shapeName -> Some info 
         | _ -> None
      )
      |> Option.iter f   
   member canvas.Черепаха = turtle
   member canvas.ClearDrawings() =
      drawings.Clear()
      canvas.QueueDraw()
   member canvas.AddDrawing(drawing) =
      { Рисунок=drawing; Смещение=Point(); Непрозрачность=None; Видим=true; Врашение=None; Масштаб=None }
      |> drawings.Add
      canvas.QueueDraw()
   member canvas.AddDrawingAt(drawing, offset:Point) =
      { Рисунок=drawing; Смещение=offset; Непрозрачность=None; Видим=true; Врашение=None; Масштаб=None }
      |> drawings.Add
      canvas.QueueDraw()
   member canvas.MoveShape(shape, offset:Point) =
      onShape shape (fun info -> info.Смещение <- offset; canvas.QueueDraw())
   member canvas.SetShapeOpacity(shape, opacity) =
      onShape shape (fun info -> info.Непрозрачность <- Some opacity; canvas.QueueDraw())
   member canvas.SetShapeVisibility(shape, isVisible) =
      onShape shape (fun info -> info.Видим <- isVisible; canvas.QueueDraw())
   member canvas.SetShapeRotation(shape, angle) =
      onShape shape (fun info -> info.Врашение <- Some(angle); canvas.QueueDraw())
   member canvas.SetShapeScale(shape, scaleX, scaleY) =
      onShape shape (fun info -> info.Масштаб <- Some(scaleX,scaleY); canvas.QueueDraw())
   member canvas.RemoveShape(shape) =
      drawings |> Seq.tryFindIndex (function 
         | { ИнфоРисунка.Рисунок=НарисоватьФигуру(shapeName,_) } -> shapeName = shape
         | _ -> false 
      )
      |> function Some index -> drawings.RemoveAt(index) | None -> ()
   member canvas.Invalidate() =
      canvas.QueueDraw()
   override this.OnDraw(ctx, rect) =
      base.OnDraw(ctx, rect)      
      for drawing in drawings do 
         if drawing.Видим then нарисовать ctx drawing
      if turtle.Видим then нарисовать ctx turtle