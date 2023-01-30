module internal Кітапхана.Draw

open Avalonia.Media
open Avalonia

let makeTriangle (Triangle(x1,y1,x2,y2,x3,y3)) =
    let g = new PathGeometry()
    using (g.Open()) (fun streaming -> 
        streaming.BeginFigure(Avalonia.Point(x1, y1), true)
        streaming.LineTo(Avalonia.Point(x2, y2))
        streaming.LineTo(Avalonia.Point(x3, y3))
        streaming.EndFigure(true)
    )
    g

let toLayout text (Font(size,family,isBold,isItalic)) color =
   let layout = new FormattedText(text, System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, Typeface.Default, 1, new SolidColorBrush(color, 1.))
   layout.SetFontSize(size/2.0)      
   if isBold then layout.SetFontWeight(FontWeight.Bold)
   if isItalic then layout.SetFontStyle(FontStyle.Italic)
   if family <> "" then layout.SetFontFamily(family)
   layout
 
type DrawingInfo = { 
   Drawing:Кітапхана.Drawing; 
   mutable Offset:Point; 
   mutable Opacity:float option
   mutable IsVisible:bool 
   mutable Rotation:float option
   mutable Scale:(float * float) option
   }

let drawImage (ctx:DrawingContext) (info:DrawingInfo) (image:IImage) (x,y) =
   match info.Rotation with
   | Some angle ->           
      let w,h = image.Size.Width, image.Size.Height
      let currentTransform = ctx.CurrentTransform;
      let source = new Rect(new Point(0.0,0.0),image.Size)
      ctx.PushPreTransform (Matrix.CreateTranslation(x+w/2.0,y+h/2.0)) |> ignore
      ctx.PushPreTransform (Matrix.CreateRotation(Кітапхана.Математика.АлуРадианы angle)) |> ignore
      ctx.PushPreTransform (Matrix.CreateTranslation(-w / 2.0, -h / 2.0)) |> ignore
      match info.Scale with
      | Some(sx,sy) -> ctx.PushPreTransform (Matrix.CreateScale(sx,sy)) |> ignore
      | None -> ()    
      ctx.DrawImage(image, source)
      ctx.PushSetTransform currentTransform;
   | None ->
      let currentTransform = ctx.CurrentTransform;
      match info.Scale with
      | Some(sx,sy) -> 
         ctx.PushPreTransform (Matrix.CreateScale(sx,sy)) |> ignore
         ctx.DrawImage(image,new Rect(x, y, image.Size.Width/sx,image.Size.Height/sy))
      | None ->
         ctx.DrawImage(image,new Rect(x, y, image.Size.Width, image.Size.Height))
      ctx.PushSetTransform currentTransform;

let draw (ctx:DrawingContext) (info:DrawingInfo) =
   let x,y = info.Offset.X, info.Offset.Y
   let withOpacity (color:Color) =
      match info.Opacity with
      | Some opacity -> Color.FromArgb(byte (opacity * float color.A), color.R, color.G, color.B)
      | None -> color
   match info.Drawing with
   | DrawLine(Line(x1,y1,x2,y2),Pen(color,width)) ->
      let color = toXwtColor color
      let pen = new Pen(new SolidColorBrush(color, 1.0), width)
      ctx.DrawLine(pen, Avalonia.Point(x1, y1), Avalonia.Point(x2, y2))
   | DrawRect(Rect(w,h),Pen(color,width)) ->
      let color = toXwtColor color
      let pen = new Pen(new SolidColorBrush(color, 1.0), width)
      ctx.DrawRectangle(null, pen, Avalonia.Rect(x,y,w,h))
   | DrawTriangle(Triangle(x1,y1,x2,y2,x3,y3),Pen(color,width)) ->
      let color = toXwtColor color
      let pen = new Pen(new SolidColorBrush(color, 1.0), width)
      ctx.DrawLine(pen, Avalonia.Point(x1, y1), Avalonia.Point(x2, y2))
      ctx.DrawLine(pen, Avalonia.Point(x2, y2), Avalonia.Point(x3, y3))
      ctx.DrawLine(pen, Avalonia.Point(x3, y3), Avalonia.Point(x1, y1))
   | DrawEllipse(Ellipse(w,h),Pen(color,width)) ->
      let color = toXwtColor color
      let pen = new Pen(new SolidColorBrush(color, 1.0), width)
      ctx.DrawEllipse(null, pen, Avalonia.Point(x,y),w,h)
   | DrawImage(image,x',y') ->
      if !image <> null then         
         drawImage ctx info !image (x+x',y+y') |> ignore
   | DrawText(x,y,text,font,color) ->
      let color = toXwtColor color
      let layout = toLayout text font color
      ctx.DrawText(layout, Avalonia.Point(x,y))
   | DrawBoundText(x,y,width,text,font,color) ->
      let color = toXwtColor color
      let layout = toLayout text font color
      layout.MaxTextWidth <- width      
      ctx.DrawText(layout, Avalonia.Point(x,y))
   | FillRect(Rect(w,h),fillColor) ->
      let color = toXwtColor fillColor
      let brush = new SolidColorBrush(color, 1.0)
      ctx.DrawRectangle(brush, null, Avalonia.Rect(x,y,w,h))
   | FillTriangle(triangle,fillColor) ->
      let color = toXwtColor fillColor
      let brush = new SolidColorBrush(color, 1.0)
      let geometry = makeTriangle triangle
      ctx.DrawGeometry(brush, null, geometry)
   | FillEllipse(Ellipse(w,h),fillColor) ->
      let color = toXwtColor fillColor
      let brush = new SolidColorBrush(color, 1.0)
      ctx.DrawEllipse(brush, null, Avalonia.Point(x+w/2.,y+h/2.),w/2.,h/2.)
   | DrawShape(_,LineShape(Line(x1,y1,x2,y2),Pen(color,width))) ->
      let color = toXwtColor color
      let pen = new Pen(new SolidColorBrush(color, 1.0), width)
      ctx.DrawLine(pen, Avalonia.Point(x+ x1, y+y1), Avalonia.Point(x+ x2, y+y2))
   | DrawShape(_,RectShape(Rect(w,h),Pen(color,width),fillColor)) ->
      let currentTransform = ctx.CurrentTransform
      ctx.PushPreTransform (Matrix.CreateTranslation(x,y)) |> ignore
      match info.Rotation with
      | Some angle -> ctx.PushPreTransform (Matrix.CreateRotation(angle)) |> ignore
      | None -> ()            
      let color = toXwtColor color
      let colorBackground = toXwtColor fillColor
      let pen = new Pen(new SolidColorBrush(color, 1.0), width)
      ctx.DrawRectangle(new SolidColorBrush(colorBackground, 1.0), pen, Avalonia.Rect(0.,0.,w,h))
      ctx.PushSetTransform currentTransform |> ignore;
   | DrawShape(_,TriangleShape(triangle,Pen(color,width),fillColor)) ->
      let brush = new SolidColorBrush(withOpacity (toXwtColor fillColor), 1.0)
      let pen = new Pen(new SolidColorBrush(toXwtColor color, 1.0), width)
      let geometry = makeTriangle triangle
      ctx.DrawGeometry(brush, pen, geometry)
   | DrawShape(_,EllipseShape(Ellipse(w,h),Pen(color,width),fillColor)) ->
      let pen = new Pen(new SolidColorBrush(toXwtColor color, 1.0), width)
      let brush = new SolidColorBrush(withOpacity (toXwtColor fillColor), 1.0)
      ctx.DrawEllipse(brush, pen, Avalonia.Point(x,y),w,h)
   | DrawShape(_,TextShape(textRef,font,color)) ->
      let color = toXwtColor color
      let layout = toLayout !textRef font color
      ctx.DrawText(layout, Avalonia.Point(x,y))
   | DrawShape(_,ImageShape(image)) ->
      if !image <> null then                 
         drawImage ctx info !image (x,y) |> ignore
