module internal Бібліотека.Малювати

відкрити Avalonia.Media
відкрити Avalonia

нехай зробитиТрикутник (Трикутник(x1,y1,x2,y2,x3,y3)) =
    нехай г = new PathGeometry()
    using (г.Open()) (fun контекстГеометрії -> 
        контекстГеометрії.BeginFigure(Avalonia.Point(x1, y1), true)
        контекстГеометрії.LineTo(Avalonia.Point(x2, y2))
        контекстГеометрії.LineTo(Avalonia.Point(x3, y3))
        контекстГеометрії.EndFigure(true)
    )
    г

нехай кМакету текст (Шрифт(розмір,родина,жирний,курсив)) колір =
   нехай макет = new FormattedText(текст, System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, Typeface.Default, 1, new SolidColorBrush(колір, 1.))
   макет.SetFontSize(розмір/2.0)      
   if жирний then макет.SetFontWeight(FontWeight.Bold)
   if курсив then макет.SetFontStyle(FontStyle.Italic)
   if родина <> "" then макет.SetFontFamily(родина)
   макет
 
тип ІнфоМалюнка = { 
   Малюнок:Малювання; 
   змінливий Зсув:Avalonia.Point; 
   змінливий Непрозорість:float option
   змінливий Видно:bool 
   змінливий Обертання:float option
   змінливий Масштаб:(float * float) option
   }

нехай намалюватиЗображення (конт:DrawingContext) (інфо:ІнфоМалюнка) (зображення:IImage) (x,y) =
   match інфо.Обертання із
   | Some кут ->           
      нехай ш,в = зображення.Size.Width, зображення.Size.Height
      нехай поточнеПеретворення = конт.CurrentTransform;
      нехай джерело = new Rect(new Point(0.0,0.0),зображення.Size)
      конт.PushPreTransform (Matrix.CreateTranslation(x+ш/2.0,y+в/2.0)) |> ignore
      конт.PushPreTransform (Matrix.CreateRotation(Бібліотека.Математика.ОтриматиРадіани кут)) |> ignore
      конт.PushPreTransform (Matrix.CreateTranslation(-ш / 2.0, -в / 2.0)) |> ignore
      match інфо.Масштаб із
      | Some(sx,sy) -> конт.PushPreTransform (Matrix.CreateScale(sx,sy)) |> ignore
      | None -> ()    
      конт.DrawImage(зображення, джерело)
      конт.PushSetTransform поточнеПеретворення;
   | None ->
      нехай поточнеПеретворення = конт.CurrentTransform;
      match інфо.Масштаб із
      | Some(мx,мy) -> 
         конт.PushPreTransform (Matrix.CreateScale(мx,мy)) |> ignore
         конт.DrawImage(зображення,new Rect(x, y, зображення.Size.Width/мx,зображення.Size.Height/мy))
      | None ->
         конт.DrawImage(зображення,new Rect(x, y, зображення.Size.Width, зображення.Size.Height))
      конт.PushSetTransform поточнеПеретворення;

нехай намалювати (конт:DrawingContext) (інфо:ІнфоМалюнка) =
   нехай x,y = інфо.Зсув.X, інфо.Зсув.Y
   нехай зНепрозорістю (колір:Color) =
      match інфо.Непрозорість із
      | Some opacity -> Color.FromArgb(byte (opacity * float колір.A), колір.R, колір.G, колір.B)
      | None -> колір
   match інфо.Малюнок із
   | НамалюватиЛінію(Лінія(x1,y1,x2,y2),Перо(колір,ширина)) ->
      нехай колірАвалонії = доКольораАвалонії колір
      нехай перо = new Pen(new SolidColorBrush(колірАвалонії, 1.0), ширина)
      конт.DrawLine(перо, Avalonia.Point(x1, y1), Avalonia.Point(x2, y2))
   | НамалюватиПрямокутник(Прямокутник(ш,в),Перо(колір,ширина)) ->
      нехай колірАвалонії = доКольораАвалонії колір
      нехай перо = new Pen(new SolidColorBrush(колірАвалонії, 1.0), ширина)
      конт.DrawRectangle(null, перо, Avalonia.Rect(x,y,ш,в))
   | НамалюватиТрикутник(Трикутник(x1,y1,x2,y2,x3,y3),Перо(колір,ширина)) ->
      нехай колірАвалонії = доКольораАвалонії колір
      нехай перо = new Pen(new SolidColorBrush(колірАвалонії, 1.0), ширина)
      конт.DrawLine(перо, Avalonia.Point(x1, y1), Avalonia.Point(x2, y2))
      конт.DrawLine(перо, Avalonia.Point(x2, y2), Avalonia.Point(x3, y3))
      конт.DrawLine(перо, Avalonia.Point(x3, y3), Avalonia.Point(x1, y1))
   | НамалюватиЕлліпс(Елліпс(ш,в),Перо(колір,ширина)) ->
      нехай колірАвалонії = доКольораАвалонії колір
      нехай перо = new Pen(new SolidColorBrush(колірАвалонії, 1.0), ширина)
      конт.DrawEllipse(null, перо, Avalonia.Point(x,y),ш,в)
   | НамалюватиЗображення(зображення,x',y') ->
      if зображення.Value <> null then         
         намалюватиЗображення конт інфо зображення.Value (x+x',y+y') |> ignore
   | НамалюватиТекст(x,y,текст,шрифт,цвет) ->
      нехай колірАвалонії = доКольораАвалонії цвет
      нехай макет = кМакету текст шрифт колірАвалонії
      конт.DrawText(макет, Avalonia.Point(x,y))
   | НамалюватиТекстУРамці(x,y,ширина,текст,шрифт,цвет) ->
      нехай колірАвалонії = доКольораАвалонії цвет
      нехай макет = кМакету текст шрифт колірАвалонії
      макет.MaxTextWidth <- ширина      
      конт.DrawText(макет, Avalonia.Point(x,y))
   | ЗаповнитиПрямокутник(Прямокутник(ш,в),колірЗаливки) ->
      нехай колірЗаливкиАвалонії = доКольораАвалонії колірЗаливки
      нехай перо = new SolidColorBrush(колірЗаливкиАвалонії, 1.0)
      конт.DrawRectangle(перо, null, Avalonia.Rect(x,y,ш,в))
   | ЗаповнитиТрикутник(трикутник,колірЗаливки) ->
      нехай колірЗаливкиАвалонії = доКольораАвалонії колірЗаливки
      нехай перо = new SolidColorBrush(колірЗаливкиАвалонії, 1.0)
      нехай геометрія = зробитиТрикутник трикутник
      конт.DrawGeometry(перо, null, геометрія)
   | ЗаповнитиЕліпс(Елліпс(ш,в),колірЗаливки) ->
      нехай колірЗаливкиАвалонії = доКольораАвалонії колірЗаливки
      нехай пензлик = new SolidColorBrush(колірЗаливкиАвалонії, 1.0)
      конт.DrawEllipse(пензлик, null, Avalonia.Point(x+ш/2.,y+в/2.),ш/2.,в/2.)
   | НамалюватиФігуру(_,ФігураЛінії(Лінія(x1,y1,x2,y2),Перо(колір,ширина))) ->
      нехай колірАвалонії = доКольораАвалонії колір
      нехай перо = new Pen(new SolidColorBrush(колірАвалонії, 1.0), ширина)
      конт.DrawLine(перо, Avalonia.Point(x+ x1, y+y1), Avalonia.Point(x+ x2, y+y2))
   | НамалюватиФігуру(_,ФігураПрямокутника(Прямокутник(w,h),Перо(колір,ширина),колірЗаливки)) ->
      нехай поточнеПеретворення = конт.CurrentTransform;
      конт.PushPreTransform (Matrix.CreateTranslation(x,y)) |> ignore
      match інфо.Обертання із
      | Some кут -> конт.PushPreTransform (Matrix.CreateRotation(кут)) |> ignore
      | None -> ()            
      нехай колірАвалонії = доКольораАвалонії колір
      нехай колірЗаливкиАвалонії = доКольораАвалонії колірЗаливки
      нехай перо = new Pen(new SolidColorBrush(колірАвалонії, 1.0), ширина)
      конт.DrawRectangle(new SolidColorBrush(колірЗаливкиАвалонії, 1.0), перо, Avalonia.Rect(0.,0.,w,h))
      конт.PushSetTransform поточнеПеретворення |> ignore;
   | НамалюватиФігуру(_,ФігураТрикутника(трикутник,Перо(колір,ширина),колірЗаливки)) ->
      нехай пензлик = new SolidColorBrush(зНепрозорістю (доКольораАвалонії колірЗаливки), 1.0)
      нехай перо = new Pen(new SolidColorBrush(доКольораАвалонії колір, 1.0), ширина)
      нехай геометрія = зробитиТрикутник трикутник
      конт.DrawGeometry(пензлик, перо, геометрія)
   | НамалюватиФігуру(_,ФігураЕлліпса(Елліпс(ш,в),Перо(колір,ширина),колірЗаливки)) ->
      нехай перо = new Pen(new SolidColorBrush(доКольораАвалонії колір, 1.0), ширина)
      нехай пензлик = new SolidColorBrush(зНепрозорістю (доКольораАвалонії колірЗаливки), 1.0)
      конт.DrawEllipse(пензлик, перо, Avalonia.Point(x,y),ш,в)
   | НамалюватиФігуру(_,ФігураТекста(textRef,шрифт,колір)) ->
      нехай колірАвалонії = доКольораАвалонії колір
      нехай макет = кМакету textRef.Value шрифт колірАвалонії
      конт.DrawText(макет, Avalonia.Point(x,y))
   | НамалюватиФігуру(_,ФігураЗображения(зображення)) ->
      if зображення.Value <> null then                 
         намалюватиЗображення конт інфо зображення.Value (x,y) |> ignore
