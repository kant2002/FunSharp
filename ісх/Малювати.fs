module internal Бiблiотека.Малювати

open Avalonia.Media
open Avalonia

let зробитиТрикутник (Треугольник(x1,y1,x2,y2,x3,y3)) =
    let g = new PathGeometry()
    using (g.Open()) (fun streaming -> 
        streaming.BeginFigure(Avalonia.Point(x1, y1), true)
        streaming.LineTo(Avalonia.Point(x2, y2))
        streaming.LineTo(Avalonia.Point(x3, y3))
        streaming.EndFigure(true)
    )
    g

let кМакету текст (Шрифт(размер,семейство,жирный,курсив)) колір =
   let макет = new FormattedText(текст, System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, Typeface.Default, 1, new SolidColorBrush(колір, 1.))
   макет.SetFontSize(размер/2.0)      
   if жирный then макет.SetFontWeight(FontWeight.Bold)
   if курсив then макет.SetFontStyle(FontStyle.Italic)
   if семейство <> "" then макет.SetFontFamily(семейство)
   макет
 
type ИнфоМалюнка = { 
   Малюнок:Рисование; 
   mutable Зсув:Avalonia.Point; 
   mutable Непрозрачность:float option
   mutable Видим:bool 
   mutable Врашение:float option
   mutable Масштаб:(float * float) option
   }

let нарисоватьИзображение (конт:DrawingContext) (инфо:ИнфоМалюнка) (зображення:IImage) (x,y) =
   match инфо.Врашение with
   | Some угол ->           
      let ш,в = зображення.Size.Width, зображення.Size.Height
      //конт.PushClip
      let currentTransform = конт.CurrentTransform;
      let source = new Rect(new Point(0.0,0.0),зображення.Size)
      конт.PushPreTransform (Matrix.CreateTranslation(x+ш/2.0,y+в/2.0)) |> ignore
      конт.PushPreTransform (Matrix.CreateRotation(Бiблiотека.Математика.ОтриматиРадіани угол)) |> ignore
      конт.PushPreTransform (Matrix.CreateTranslation(-ш / 2.0, -в / 2.0)) |> ignore
      match инфо.Масштаб with
      | Some(sx,sy) -> конт.PushPreTransform (Matrix.CreateScale(sx,sy)) |> ignore
      | None -> ()    
      конт.DrawImage(зображення, source)
      //let узор = new ImagePattern(зображення)            
      //конт.Pattern <- узор           
      //конт.Fill()
      //конт.Restore()
      конт.PushSetTransform currentTransform;
   | None ->
      let currentTransform = конт.CurrentTransform;
      //конт.Save()            
      match инфо.Масштаб with
      | Some(sx,sy) -> 
         //конт.Scale(sx,sy)
         конт.PushPreTransform (Matrix.CreateScale(sx,sy)) |> ignore
         конт.DrawImage(зображення,new Rect(x, y, зображення.Size.Width/sx,зображення.Size.Height/sy))
      | None ->
         конт.DrawImage(зображення,new Rect(x, y, зображення.Size.Width, зображення.Size.Height))
      //конт.Restore()
      конт.PushSetTransform currentTransform;

let нарисовать (конт:DrawingContext) (інфо:ИнфоМалюнка) =
   let x,y = інфо.Зсув.X, інфо.Зсув.Y
   let сНепрозрачностью (колір:Color) =
      match інфо.Непрозрачность with
      | Some opacity -> Color.FromArgb(byte (opacity * float колір.A), колір.R, колір.G, колір.B)
      | None -> колір
   match інфо.Малюнок with
   | НамалюватиЛінію(Линия(x1,y1,x2,y2),Перо(колір,ширина)) ->
      let color = кXwtЦвету колір
      let перо = new Pen(new SolidColorBrush(color, 1.0), ширина)
      конт.DrawLine(перо, Avalonia.Point(x1, y1), Avalonia.Point(x2, y2))
   | НамалюватиПрямокутник(Прямокутник(ш,в),Перо(колір,ширина)) ->
      let color = кXwtЦвету колір
      let перо = new Pen(new SolidColorBrush(color, 1.0), ширина)
      конт.DrawRectangle(null, перо, Avalonia.Rect(x,y,ш,в))
   | НамалюватиТрикутник(Треугольник(x1,y1,x2,y2,x3,y3),Перо(колір,ширина)) ->
      let color = кXwtЦвету колір
      let перо = new Pen(new SolidColorBrush(color, 1.0), ширина)
      конт.DrawLine(перо, Avalonia.Point(x1, y1), Avalonia.Point(x2, y2))
      конт.DrawLine(перо, Avalonia.Point(x2, y2), Avalonia.Point(x3, y3))
      конт.DrawLine(перо, Avalonia.Point(x3, y3), Avalonia.Point(x1, y1))
   | НамалюватиЕліпс(Эллипс(ш,в),Перо(колір,ширина)) ->
      let color = кXwtЦвету колір
      let перо = new Pen(new SolidColorBrush(color, 1.0), ширина)
      конт.DrawEllipse(null, перо, Avalonia.Point(x,y),ш,в)
   | НамалюватиЗображення(зображення,x',y') ->
      if зображення.Value <> null then         
         нарисоватьИзображение конт інфо зображення.Value (x+x',y+y') |> ignore
   | НамалюватиТекст(x,y,текст,шрифт,цвет) ->
      let color = кXwtЦвету цвет
      let макет = кМакету текст шрифт color
      конт.DrawText(макет, Avalonia.Point(x,y))
   | НамалюватиТекстУРамці(x,y,ширина,текст,шрифт,цвет) ->
      let color = кXwtЦвету цвет
      let макет = кМакету текст шрифт color
      макет.MaxTextWidth <- ширина      
      конт.DrawText(макет, Avalonia.Point(x,y))
   | ЗаполнитьПрямоугольник(Прямокутник(ш,в),цветЗаливки) ->
      let color = кXwtЦвету цветЗаливки
      let перо = new SolidColorBrush(color, 1.0)
      конт.DrawRectangle(перо, null, Avalonia.Rect(x,y,ш,в))
   | ЗаполнитьТреугольник(трикутник,цветЗаливки) ->
      let color = кXwtЦвету цветЗаливки
      let перо = new SolidColorBrush(color, 1.0)
      let геометрія = зробитиТрикутник трикутник
      конт.DrawGeometry(перо, null, геометрія)
   | ЗаповнитиЕліпс(Эллипс(ш,в),цветЗаливки) ->
      let color = кXwtЦвету цветЗаливки
      let пензлик = new SolidColorBrush(color, 1.0)
      конт.DrawEllipse(пензлик, null, Avalonia.Point(x+ш/2.,y+в/2.),ш/2.,в/2.)
   | НамалюватиФігуру(_,ФигураЛинии(Линия(x1,y1,x2,y2),Перо(колір,ширина))) ->
      let color = кXwtЦвету колір
      let перо = new Pen(new SolidColorBrush(color, 1.0), ширина)
      конт.DrawLine(перо, Avalonia.Point(x+ x1, y+y1), Avalonia.Point(x+ x2, y+y2))
   | НамалюватиФігуру(_,ФигураПрямоугольника(Прямокутник(w,h),Перо(колір,ширина),цветЗаливки)) ->
      let currentTransform = конт.CurrentTransform;
      //конт.Save() 
      конт.PushPreTransform (Matrix.CreateTranslation(x,y)) |> ignore
      //конт.PushPreTransform (Matrix.CreateRotation(угол)) |> ignore
      //конт.PushPreTransform (Matrix.CreateTranslation(-ш / 2.0, -в / 2.0)) |> ignore
      //конт.Translate(x,y)
      match інфо.Врашение with
      | Some угол -> конт.PushPreTransform (Matrix.CreateRotation(угол)) |> ignore
      | None -> ()            
      let color = кXwtЦвету колір
      let colorBackground = кXwtЦвету цветЗаливки
      let перо = new Pen(new SolidColorBrush(color, 1.0), ширина)
      конт.DrawRectangle(new SolidColorBrush(colorBackground, 1.0), перо, Avalonia.Rect(0.,0.,w,h))
      //конт.Restore()
      конт.PushSetTransform currentTransform |> ignore;
   | НамалюватиФігуру(_,ФигураТреугольника(трикутник,Перо(колір,ширина),цветЗаливки)) ->
      let пензлик = new SolidColorBrush(сНепрозрачностью (кXwtЦвету цветЗаливки), 1.0)
      let перо = new Pen(new SolidColorBrush(кXwtЦвету колір, 1.0), ширина)
      let геометрія = зробитиТрикутник трикутник
      конт.DrawGeometry(пензлик, перо, геометрія)
   | НамалюватиФігуру(_,ФигураЭллипса(Эллипс(ш,в),Перо(колір,ширина),цветЗаливки)) ->
      let перо = new Pen(new SolidColorBrush(кXwtЦвету колір, 1.0), ширина)
      let пензлик = new SolidColorBrush(сНепрозрачностью (кXwtЦвету цветЗаливки), 1.0)
      конт.DrawEllipse(пензлик, перо, Avalonia.Point(x,y),ш,в)
   | НамалюватиФігуру(_,ФигураТекста(textRef,шрифт,цвет)) ->
      let color = кXwtЦвету цвет
      let макет = кМакету textRef.Value шрифт color
      конт.DrawText(макет, Avalonia.Point(x,y))
   | НамалюватиФігуру(_,ФигураИзображения(зображення)) ->
      if зображення.Value <> null then                 
         нарисоватьИзображение конт інфо зображення.Value (x,y) |> ignore
