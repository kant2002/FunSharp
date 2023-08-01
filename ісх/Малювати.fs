module internal Бібліотека.Малювати

open Avalonia.Media
open Avalonia

let зробитиТрикутник (Трикутник(x1,y1,x2,y2,x3,y3)) =
    let г = new PathGeometry()
    using (г.Open()) (fun контекстГеометрії -> 
        контекстГеометрії.BeginFigure(Avalonia.Point(x1, y1), true)
        контекстГеометрії.LineTo(Avalonia.Point(x2, y2))
        контекстГеометрії.LineTo(Avalonia.Point(x3, y3))
        контекстГеометрії.EndFigure(true)
    )
    г

let кМакету текст (Шрифт(розмір,родина,жирний,курсив)) колір =
   let макет = new FormattedText(текст, System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, Typeface.Default, 1, new SolidColorBrush(колір, 1.))
   макет.SetFontSize(розмір/2.0)      
   if жирний then макет.SetFontWeight(FontWeight.Bold)
   if курсив then макет.SetFontStyle(FontStyle.Italic)
   if родина <> "" then макет.SetFontFamily(родина)
   макет
 
type ІнфоМалюнка = { 
   Малюнок:Малювання; 
   mutable Зсув:Avalonia.Point; 
   mutable Непрозорість:float option
   mutable Видно:bool 
   mutable Обертання:float option
   mutable Масштаб:(float * float) option
}

let намалюватиЗображення (конт:DrawingContext) (інфо:ІнфоМалюнка) (зображення:IImage) (x:float,y:float) =
   match інфо.Обертання with
   | Some кут ->           
        let ш,в = зображення.Size.Width, зображення.Size.Height
        let джерело = new Rect(new Point(0.0,0.0),зображення.Size)
        use _ = конт.PushTransform (Matrix.CreateTranslation(x+ш/2.0,y+в/2.0))
        use _ = конт.PushTransform (Matrix.CreateRotation(Бібліотека.Математика.ОтриматиРадіани кут))
        use _ = конт.PushTransform (Matrix.CreateTranslation(-ш / 2.0, -в / 2.0))
        match інфо.Масштаб with
        | Some(sx,sy) -> 
            use _ = конт.PushTransform (Matrix.CreateScale(sx,sy))
            конт.DrawImage(зображення, джерело)
        | None -> конт.DrawImage(зображення, джерело)
   | None ->
      match інфо.Масштаб with
      | Some(мx,мy) -> 
         use _ = конт.PushTransform (Matrix.CreateScale(мx,мy))
         конт.DrawImage(зображення,new Rect(x, y, зображення.Size.Width/мx,зображення.Size.Height/мy))
      | None ->
         конт.DrawImage(зображення,new Rect(x, y, зображення.Size.Width, зображення.Size.Height))

let намалювати (конт:DrawingContext) (інфо:ІнфоМалюнка) =
   let x,y = інфо.Зсув.X, інфо.Зсув.Y
   let зНепрозорістю (колір:Color) =
      match інфо.Непрозорість with
      | Some opacity -> Color.FromArgb(byte (opacity * float колір.A), колір.R, колір.G, колір.B)
      | None -> колір
   match інфо.Малюнок with
   | НамалюватиЛінію(Лінія(x1,y1,x2,y2),Перо(колір,ширина)) ->
      let колірАвалонії = доКольораАвалонії колір
      let перо = new Pen(new SolidColorBrush(колірАвалонії, 1.0), ширина)
      конт.DrawLine(перо, Avalonia.Point(x1, y1), Avalonia.Point(x2, y2))
   | НамалюватиПрямокутник(Прямокутник(ш,в),Перо(колір,ширина)) ->
      let колірАвалонії = доКольораАвалонії колір
      let перо = new Pen(new SolidColorBrush(колірАвалонії, 1.0), ширина)
      конт.DrawRectangle(null, перо, Avalonia.Rect(x,y,ш,в))
   | НамалюватиТрикутник(Трикутник(x1,y1,x2,y2,x3,y3),Перо(колір,ширина)) ->
      let колірАвалонії = доКольораАвалонії колір
      let перо = new Pen(new SolidColorBrush(колірАвалонії, 1.0), ширина)
      конт.DrawLine(перо, Avalonia.Point(x1, y1), Avalonia.Point(x2, y2))
      конт.DrawLine(перо, Avalonia.Point(x2, y2), Avalonia.Point(x3, y3))
      конт.DrawLine(перо, Avalonia.Point(x3, y3), Avalonia.Point(x1, y1))
   | НамалюватиЕлліпс(Елліпс(ш,в),Перо(колір,ширина)) ->
      let колірАвалонії = доКольораАвалонії колір
      let перо = new Pen(new SolidColorBrush(колірАвалонії, 1.0), ширина)
      конт.DrawEllipse(null, перо, Avalonia.Point(x,y),ш,в)
   | НамалюватиЗображення(зображення,x',y') ->
      if зображення.Value <> null then         
         намалюватиЗображення конт інфо зображення.Value (x+x',y+y') |> ігнорувати
   | НамалюватиТекст(x,y,текст,шрифт,цвет) ->
      let колірАвалонії = доКольораАвалонії цвет
      let макет = кМакету текст шрифт колірАвалонії
      конт.DrawText(макет, Avalonia.Point(x,y))
   | НамалюватиТекстУРамці(x,y,ширина,текст,шрифт,цвет) ->
      let колірАвалонії = доКольораАвалонії цвет
      let макет = кМакету текст шрифт колірАвалонії
      макет.MaxTextWidth <- ширина      
      конт.DrawText(макет, Avalonia.Point(x,y))
   | ЗаповнитиПрямокутник(Прямокутник(ш,в),колірЗаливки) ->
      let колірЗаливкиАвалонії = доКольораАвалонії колірЗаливки
      let перо = new SolidColorBrush(колірЗаливкиАвалонії, 1.0)
      конт.DrawRectangle(перо, null, Avalonia.Rect(x,y,ш,в))
   | ЗаповнитиТрикутник(трикутник,колірЗаливки) ->
      let колірЗаливкиАвалонії = доКольораАвалонії колірЗаливки
      let перо = new SolidColorBrush(колірЗаливкиАвалонії, 1.0)
      let геометрія = зробитиТрикутник трикутник
      конт.DrawGeometry(перо, null, геометрія)
   | ЗаповнитиЕліпс(Елліпс(ш,в),колірЗаливки) ->
      let колірЗаливкиАвалонії = доКольораАвалонії колірЗаливки
      let пензлик = new SolidColorBrush(колірЗаливкиАвалонії, 1.0)
      конт.DrawEllipse(пензлик, null, Avalonia.Point(x+ш/2.,y+в/2.),ш/2.,в/2.)
   | НамалюватиФігуру(_,ФігураЛінії(Лінія(x1,y1,x2,y2),Перо(колір,ширина))) ->
      let колірАвалонії = доКольораАвалонії колір
      let перо = new Pen(new SolidColorBrush(колірАвалонії, 1.0), ширина)
      конт.DrawLine(перо, Avalonia.Point(x+ x1, y+y1), Avalonia.Point(x+ x2, y+y2))
   | НамалюватиФігуру(_,ФігураПрямокутника(Прямокутник(w,h),Перо(колір,ширина),колірЗаливки)) ->
      use _ = конт.PushTransform (Matrix.CreateTranslation(x,y))
      let колірАвалонії = доКольораАвалонії колір
      let колірЗаливкиАвалонії = доКольораАвалонії колірЗаливки
      let перо = new Pen(new SolidColorBrush(колірАвалонії, 1.0), ширина)
      match інфо.Обертання with
      | Some кут ->
        use _ = конт.PushTransform (Matrix.CreateRotation(кут))
        конт.DrawRectangle(new SolidColorBrush(колірЗаливкиАвалонії, 1.0), перо, Avalonia.Rect(0.,0.,w,h))
      | None ->
        конт.DrawRectangle(new SolidColorBrush(колірЗаливкиАвалонії, 1.0), перо, Avalonia.Rect(0.,0.,w,h))
   | НамалюватиФігуру(_,ФігураТрикутника(трикутник,Перо(колір,ширина),колірЗаливки)) ->
      let пензлик = new SolidColorBrush(зНепрозорістю (доКольораАвалонії колірЗаливки), 1.0)
      let перо = new Pen(new SolidColorBrush(доКольораАвалонії колір, 1.0), ширина)
      let геометрія = зробитиТрикутник трикутник
      конт.DrawGeometry(пензлик, перо, геометрія)
   | НамалюватиФігуру(_,ФігураЕлліпса(Елліпс(ш,в),Перо(колір,ширина),колірЗаливки)) ->
      let перо = new Pen(new SolidColorBrush(доКольораАвалонії колір, 1.0), ширина)
      let пензлик = new SolidColorBrush(зНепрозорістю (доКольораАвалонії колірЗаливки), 1.0)
      конт.DrawEllipse(пензлик, перо, Avalonia.Point(x,y),ш,в)
   | НамалюватиФігуру(_,ФігураТекста(textRef,шрифт,колір)) ->
      let колірАвалонії = доКольораАвалонії колір
      let макет = кМакету textRef.Value шрифт колірАвалонії
      конт.DrawText(макет, Avalonia.Point(x,y))
   | НамалюватиФігуру(_,ФігураЗображения(зображення)) ->
      if зображення.Value <> null then                 
         намалюватиЗображення конт інфо зображення.Value (x,y) |> ігнорувати
