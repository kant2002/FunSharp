namespace Library

open Avalonia
open Avalonia.Controls
open Avalonia.Media.Imaging
open Avalonia.Threading
open Draw

[<AllowNullLiteral>]
type internal DrawingCanvas () =
   inherit Canvas ()
   let turtleImage = new Bitmap(typeof<DrawingCanvas>.Assembly.GetManifestResourceStream("FunSharp.Library.turtle.png"))
   let drawings = ResizeArray<DrawingInfo>()
   let turtle =
      let w,h = turtleImage.Size.Width, turtleImage.Size.Height
      {Drawing=DrawImage(ref turtleImage,-w/2.,-h/2.); Offset=Point(); Opacity=None; IsVisible=false; Rotation=None; Scale=None}
   let onShape shapeName f =
      drawings
      |> Seq.tryPick (function
         | { Drawing=DrawShape(name,_) } as info when name = shapeName -> Some info 
         | _ -> None
      )
      |> Option.iter f   
   member canvas.Turtle = turtle
   member canvas.ClearDrawings() =
      drawings.Clear()
      canvas.InvalidateVisual()
   member canvas.AddDrawing(drawing) =
      { Drawing=drawing; Offset=Point(); Opacity=None; IsVisible=true; Rotation=None; Scale=None }
      |> drawings.Add
      canvas.InvalidateVisual()
   member canvas.AddDrawingAt(drawing, offset:Point) =
      { Drawing=drawing; Offset=offset; Opacity=None; IsVisible=true; Rotation=None; Scale=None }
      |> drawings.Add
      canvas.InvalidateVisual()
   member canvas.MoveShape(shape, offset:Point) =
      onShape shape (fun info -> info.Offset <- offset; canvas.InvalidateVisual())
   member canvas.SetShapeOpacity(shape, opacity) =
      onShape shape (fun info -> info.Opacity <- Some opacity; canvas.InvalidateVisual())
   member canvas.SetShapeVisibility(shape, isVisible) =
      onShape shape (fun info -> info.IsVisible <- isVisible; canvas.InvalidateVisual())
   member canvas.SetShapeRotation(shape, angle) =
      onShape shape (fun info -> info.Rotation <- Some(angle); canvas.InvalidateVisual())
   member canvas.SetShapeScale(shape, scaleX, scaleY) =
      onShape shape (fun info -> info.Scale <- Some(scaleX,scaleY); canvas.InvalidateVisual())
   member canvas.RemoveShape(shape) =
      drawings |> Seq.tryFindIndex (function 
         | { DrawingInfo.Drawing=DrawShape(shapeName,_) } -> shapeName = shape
         | _ -> false 
      )
      |> function Some index -> drawings.RemoveAt(index) | None -> ()
   member canvas.Invalidate() =
      Dispatcher.UIThread.Post (fun () -> canvas.InvalidateVisual())
   member canvas.AddChild(element, x, y) =
      canvas.Children.Add(element)
      Canvas.SetLeft(element, x)
      Canvas.SetTop(element, y)
   member canvas.RemoveChild(element) =
      canvas.Children.Remove(element) |> ignore
   override this.Render(ctx) =
      base.Render(ctx)      
      for drawing in drawings do 
         if drawing.IsVisible then draw ctx drawing
      if turtle.IsVisible then draw ctx turtle