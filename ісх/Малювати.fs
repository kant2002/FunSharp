﻿модуль внутрішній Бібліотека.Малювати

відкрити Avalonia.Media
відкрити Avalonia

нехай зробитиТрикутник (Трикутник(x1,y1,x2,y2,x3,y3)) =
    нехай г = новий PathGeometry()
    using (г.Open()) (фун контекстГеометрії -> 
        контекстГеометрії.BeginFigure(Avalonia.Point(x1, y1), істина)
        контекстГеометрії.LineTo(Avalonia.Point(x2, y2))
        контекстГеометрії.LineTo(Avalonia.Point(x3, y3))
        контекстГеометрії.EndFigure(істина)
    )
    г

нехай кМакету текст (Шрифт(розмір,родина,жирний,курсив)) колір =
   нехай макет = новий FormattedText(текст, System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, Typeface.Default, 1, новий SolidColorBrush(колір, 1.))
   макет.SetFontSize(розмір/2.0)      
   якщо жирний тоді макет.SetFontWeight(FontWeight.Bold)
   якщо курсив тоді макет.SetFontStyle(FontStyle.Italic)
   якщо родина <> "" тоді макет.SetFontFamily(родина)
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
   відповідає інфо.Обертання із
   | Some кут ->           
        нехай ш,в = зображення.Size.Width, зображення.Size.Height
        нехай джерело = новий Rect(новий Point(0.0,0.0),зображення.Size)
        вживати _ = конт.PushTransform (Matrix.CreateTranslation(x+ш/2.0,y+в/2.0))
        вживати _ = конт.PushTransform (Matrix.CreateRotation(Бібліотека.Математика.ОтриматиРадіани кут))
        вживати _ = конт.PushTransform (Matrix.CreateTranslation(-ш / 2.0, -в / 2.0))
        відповідає інфо.Масштаб із
        | Some(sx,sy) -> 
            вживати _ = конт.PushTransform (Matrix.CreateScale(sx,sy))
            конт.DrawImage(зображення, джерело)
        | None -> конт.DrawImage(зображення, джерело)
   | None ->
      відповідає інфо.Масштаб із
      | Some(мx,мy) -> 
         вживати _ = конт.PushTransform (Matrix.CreateScale(мx,мy))
         конт.DrawImage(зображення,новий Rect(x, y, зображення.Size.Width/мx,зображення.Size.Height/мy))
      | None ->
         конт.DrawImage(зображення,new Rect(x, y, зображення.Size.Width, зображення.Size.Height))

нехай намалювати (конт:DrawingContext) (інфо:ІнфоМалюнка) =
   нехай x,y = інфо.Зсув.X, інфо.Зсув.Y
   нехай зНепрозорістю (колір:Color) =
      відповідає інфо.Непрозорість із
      | Some opacity -> Color.FromArgb(byte (opacity * float колір.A), колір.R, колір.G, колір.B)
      | None -> колір
   відповідає інфо.Малюнок із
   | НамалюватиЛінію(Лінія(x1,y1,x2,y2),Перо(колір,ширина)) ->
      нехай колірАвалонії = доКольораАвалонії колір
      нехай перо = новий Pen(новий SolidColorBrush(колірАвалонії, 1.0), ширина)
      конт.DrawLine(перо, Avalonia.Point(x1, y1), Avalonia.Point(x2, y2))
   | НамалюватиПрямокутник(Прямокутник(ш,в),Перо(колір,ширина)) ->
      нехай колірАвалонії = доКольораАвалонії колір
      нехай перо = новий Pen(новий SolidColorBrush(колірАвалонії, 1.0), ширина)
      конт.DrawRectangle(нуль, перо, Avalonia.Rect(x,y,ш,в))
   | НамалюватиТрикутник(Трикутник(x1,y1,x2,y2,x3,y3),Перо(колір,ширина)) ->
      нехай колірАвалонії = доКольораАвалонії колір
      нехай перо = новий Pen(новий SolidColorBrush(колірАвалонії, 1.0), ширина)
      конт.DrawLine(перо, Avalonia.Point(x1, y1), Avalonia.Point(x2, y2))
      конт.DrawLine(перо, Avalonia.Point(x2, y2), Avalonia.Point(x3, y3))
      конт.DrawLine(перо, Avalonia.Point(x3, y3), Avalonia.Point(x1, y1))
   | НамалюватиЕлліпс(Елліпс(ш,в),Перо(колір,ширина)) ->
      нехай колірАвалонії = доКольораАвалонії колір
      нехай перо = новий Pen(новий SolidColorBrush(колірАвалонії, 1.0), ширина)
      конт.DrawEllipse(нуль, перо, Avalonia.Point(x,y),ш,в)
   | НамалюватиЗображення(зображення,x',y') ->
      якщо зображення.Value <> нуль тоді         
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
      нехай перо = новий SolidColorBrush(колірЗаливкиАвалонії, 1.0)
      конт.DrawRectangle(перо, нуль, Avalonia.Rect(x,y,ш,в))
   | ЗаповнитиТрикутник(трикутник,колірЗаливки) ->
      нехай колірЗаливкиАвалонії = доКольораАвалонії колірЗаливки
      нехай перо = новий SolidColorBrush(колірЗаливкиАвалонії, 1.0)
      нехай геометрія = зробитиТрикутник трикутник
      конт.DrawGeometry(перо, нуль, геометрія)
   | ЗаповнитиЕліпс(Елліпс(ш,в),колірЗаливки) ->
      нехай колірЗаливкиАвалонії = доКольораАвалонії колірЗаливки
      нехай пензлик = новий SolidColorBrush(колірЗаливкиАвалонії, 1.0)
      конт.DrawEllipse(пензлик, нуль, Avalonia.Point(x+ш/2.,y+в/2.),ш/2.,в/2.)
   | НамалюватиФігуру(_,ФігураЛінії(Лінія(x1,y1,x2,y2),Перо(колір,ширина))) ->
      нехай колірАвалонії = доКольораАвалонії колір
      нехай перо = новий Pen(новий SolidColorBrush(колірАвалонії, 1.0), ширина)
      конт.DrawLine(перо, Avalonia.Point(x+ x1, y+y1), Avalonia.Point(x+ x2, y+y2))
   | НамалюватиФігуру(_,ФігураПрямокутника(Прямокутник(w,h),Перо(колір,ширина),колірЗаливки)) ->
      вживати _ = конт.PushTransform (Matrix.CreateTranslation(x,y))
      нехай колірАвалонії = доКольораАвалонії колір
      нехай колірЗаливкиАвалонії = доКольораАвалонії колірЗаливки
      нехай перо = новий Pen(новий SolidColorBrush(колірАвалонії, 1.0), ширина)
      відповідає інфо.Обертання із
      | Some кут ->
        вживати _ = конт.PushTransform (Matrix.CreateRotation(кут))
        конт.DrawRectangle(новий SolidColorBrush(колірЗаливкиАвалонії, 1.0), перо, Avalonia.Rect(0.,0.,w,h))
      | None ->
        конт.DrawRectangle(новий SolidColorBrush(колірЗаливкиАвалонії, 1.0), перо, Avalonia.Rect(0.,0.,w,h))
   | НамалюватиФігуру(_,ФігураТрикутника(трикутник,Перо(колір,ширина),колірЗаливки)) ->
      нехай пензлик = новий SolidColorBrush(зНепрозорістю (доКольораАвалонії колірЗаливки), 1.0)
      нехай перо = новий Pen(новий SolidColorBrush(доКольораАвалонії колір, 1.0), ширина)
      нехай геометрія = зробитиТрикутник трикутник
      конт.DrawGeometry(пензлик, перо, геометрія)
   | НамалюватиФігуру(_,ФігураЕлліпса(Елліпс(ш,в),Перо(колір,ширина),колірЗаливки)) ->
      нехай перо = новий Pen(новий SolidColorBrush(доКольораАвалонії колір, 1.0), ширина)
      нехай пензлик = новий SolidColorBrush(зНепрозорістю (доКольораАвалонії колірЗаливки), 1.0)
      конт.DrawEllipse(пензлик, перо, Avalonia.Point(x,y),ш,в)
   | НамалюватиФігуру(_,ФігураТекста(textRef,шрифт,колір)) ->
      нехай колірАвалонії = доКольораАвалонії колір
      нехай макет = кМакету textRef.Value шрифт колірАвалонії
      конт.DrawText(макет, Avalonia.Point(x,y))
   | НамалюватиФігуру(_,ФігураЗображения(зображення)) ->
      якщо зображення.Value <> нуль тоді                 
         намалюватиЗображення конт інфо зображення.Value (x,y) |> ignore
