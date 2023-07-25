модуль внутренний Библиотека.Рисовать

открыть Avalonia.Media
открыть Avalonia

пусть зробитиТрикутник (Треугольник(x1,y1,x2,y2,x3,y3)) =
    пусть g = новый PathGeometry()
    using (g.Open()) (фун streaming -> 
        streaming.BeginFigure(Avalonia.Point(x1, y1), истина)
        streaming.LineTo(Avalonia.Point(x2, y2))
        streaming.LineTo(Avalonia.Point(x3, y3))
        streaming.EndFigure(истина)
    )
    g

пусть кМакету текст (Шрифт(размер,семейство,жирный,курсив)) цвет =
   пусть макет = новый FormattedText(текст, System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, Typeface.Default, 1, новый SolidColorBrush(цвет, 1.))
   макет.SetFontSize(размер/2.0)      
   если жирный тогда макет.SetFontWeight(FontWeight.Bold)
   если курсив тогда макет.SetFontStyle(FontStyle.Italic)
   если семейство <> "" тогда макет.SetFontFamily(семейство)
   макет
 
тип ИнфоРисунка = { 
   Рисунок:Рисование; 
   изменяемый Смещение:Point; 
   изменяемый Непрозрачность:float option
   изменяемый Видим:bool 
   изменяемый Врашение:float option
   изменяемый Масштаб:(float * float) option
   }

пусть нарисоватьИзображение (конт:DrawingContext) (инфо:ИнфоРисунка) (изображение:IImage) (x,y) =
   сопоставить инфо.Врашение с
   | Some угол ->           
      пусть ш,``в`` = изображение.Size.Width, изображение.Size.Height
      пусть source = новый Rect(новый Point(0.0,0.0),изображение.Size)
      использовать _ = конт.PushPreTransform (Matrix.CreateTranslation(x+ш/2.0,y+``в``/2.0))
      использовать _ = конт.PushPreTransform (Matrix.CreateRotation(Библиотека.Математика.ВзятьРадианы угол))
      использовать _ = конт.PushPreTransform (Matrix.CreateTranslation(-ш / 2.0, -``в`` / 2.0))
      сопоставить инфо.Масштаб с
      | Some(sx,sy) ->
        использовать _ = конт.PushPreTransform (Matrix.CreateScale(sx,sy))
        конт.DrawImage(изображение, source)
      | None ->
        конт.DrawImage(изображение, source)
   | None ->
      сопоставить инфо.Масштаб с
      | Some(sx,sy) -> 
         использовать _ = конт.PushPreTransform (Matrix.CreateScale(sx,sy))
         конт.DrawImage(изображение,новый Rect(x, y, изображение.Size.Width/sx,изображение.Size.Height/sy))
      | None ->
         конт.DrawImage(изображение,новый Rect(x, y, изображение.Size.Width, изображение.Size.Height))

пусть нарисовать (конт:DrawingContext) (инфо:ИнфоРисунка) =
   пусть x,y = инфо.Смещение.X, инфо.Смещение.Y
   пусть сНепрозрачностью (цвет:Color) =
      сопоставить инфо.Непрозрачность с
      | Some opacity -> Color.FromArgb(byte (opacity * float цвет.A), цвет.R, цвет.G, цвет.B)
      | None -> цвет
   сопоставить инфо.Рисунок с
   | НарисоватьЛинию(Линия(x1,y1,x2,y2),Перо(цвет,ширина)) ->
      пусть color = кXwtЦвету цвет
      пусть перо = новый Pen(новый SolidColorBrush(color, 1.0), ширина)
      конт.DrawLine(перо, Avalonia.Point(x1, y1), Avalonia.Point(x2, y2))
   | НарисоватьПрямоугольник(Прямоугольник(ш,``в``),Перо(цвет,ширина)) ->
      пусть color = кXwtЦвету цвет
      пусть перо = новый Pen(новый SolidColorBrush(color, 1.0), ширина)
      конт.DrawRectangle(нуль, перо, Avalonia.Rect(x,y,ш,``в``))
   | НарисоватьТреугольник(Треугольник(x1,y1,x2,y2,x3,y3),Перо(цвет,ширина)) ->
      пусть color = кXwtЦвету цвет
      пусть перо = новый Pen(новый SolidColorBrush(color, 1.0), ширина)
      конт.DrawLine(перо, Avalonia.Point(x1, y1), Avalonia.Point(x2, y2))
      конт.DrawLine(перо, Avalonia.Point(x2, y2), Avalonia.Point(x3, y3))
      конт.DrawLine(перо, Avalonia.Point(x3, y3), Avalonia.Point(x1, y1))
   | НарисоватьЭллипс(Эллипс(ш,``в``),Перо(колір,ширина)) ->
      пусть color = кXwtЦвету колір
      пусть перо = новый Pen(новый SolidColorBrush(color, 1.0), ширина)
      конт.DrawEllipse(нуль, перо, Avalonia.Point(x,y),ш,``в``)
   | НарисоватьИзображение(изображение,x',y') ->
      если изображение.Value <> нуль тогда         
         нарисоватьИзображение конт инфо изображение.Value (x+x',y+y') |> ignore
   | НарисоватьТекст(x,y,текст,шрифт,цвет) ->
      пусть color = кXwtЦвету цвет
      пусть макет = кМакету текст шрифт color
      конт.DrawText(макет, Avalonia.Point(x,y))
   | НарисоватьТекстВРамке(x,y,ширина,текст,шрифт,цвет) ->
      пусть color = кXwtЦвету цвет
      пусть макет = кМакету текст шрифт color
      макет.MaxTextWidth <- ширина
      конт.DrawText(макет, Avalonia.Point(x,y))
   | ЗаполнитьПрямоугольник(Прямоугольник(ш,``в``),цветЗаливки) ->
      пусть color = кXwtЦвету цветЗаливки
      пусть перо = новый SolidColorBrush(color, 1.0)
      конт.DrawRectangle(перо, нуль, Avalonia.Rect(x,y,ш,``в``))
   | ЗаполнитьТреугольник(треугольник,цветЗаливки) ->
      пусть color = кXwtЦвету цветЗаливки
      пусть перо = новый SolidColorBrush(color, 1.0)
      пусть геометрія = зробитиТрикутник треугольник
      конт.DrawGeometry(перо, нуль, геометрія)
   | ЗаполнитьЭллипс(Эллипс(ш,``в``),цветЗаливки) ->
      пусть color = кXwtЦвету цветЗаливки
      пусть кисть = новый SolidColorBrush(color, 1.0)
      конт.DrawEllipse(кисть, нуль, Avalonia.Point(x+ш/2.,y+``в``/2.),ш/2.,``в``/2.)
   | НарисоватьФигуру(_,ФигураЛинии(Линия(x1,y1,x2,y2),Перо(цвет,ширина))) ->
      пусть color = кXwtЦвету цвет
      пусть перо = новый Pen(новый SolidColorBrush(color, 1.0), ширина)
      конт.DrawLine(перо, Avalonia.Point(x+ x1, y+y1), Avalonia.Point(x+ x2, y+y2))
   | НарисоватьФигуру(_,ФигураПрямоугольника(Прямоугольник(w,h),Перо(цвет,ширина),цветЗаливки)) ->
      пусть color = кXwtЦвету цвет
      пусть colorBackground = кXwtЦвету цветЗаливки
      пусть перо = новый Pen(новый SolidColorBrush(color, 1.0), ширина)
      использовать _ = конт.PushPreTransform (Matrix.CreateTranslation(x,y))
      сопоставить инфо.Врашение с
      | Some угол ->
        использовать _ = конт.PushPreTransform (Matrix.CreateRotation(угол))
        конт.DrawRectangle(новый SolidColorBrush(colorBackground, 1.0), перо, Avalonia.Rect(0.,0.,w,h))
      | None ->
        конт.DrawRectangle(новый SolidColorBrush(colorBackground, 1.0), перо, Avalonia.Rect(0.,0.,w,h))
   | НарисоватьФигуру(_,ФигураТреугольника(треугольник,Перо(цвет,ширина),цветЗаливки)) ->
      пусть пензлик = новый SolidColorBrush(сНепрозрачностью (кXwtЦвету цветЗаливки), 1.0)
      пусть перо = новый Pen(новый SolidColorBrush(кXwtЦвету цвет, 1.0), ширина)
      пусть геометрія = зробитиТрикутник треугольник
      конт.DrawGeometry(пензлик, перо, геометрія)
   | НарисоватьФигуру(_,ФигураЭллипса(Эллипс(ш,``в``),Перо(цвет,ширина),цветЗаливки)) ->
      пусть перо = новый Pen(новый SolidColorBrush(кXwtЦвету цвет, 1.0), ширина)
      пусть пензлик = новый SolidColorBrush(сНепрозрачностью (кXwtЦвету цветЗаливки), 1.0)
      конт.DrawEllipse(пензлик, перо, Avalonia.Point(x,y),ш,``в``)
   | НарисоватьФигуру(_,ФигураТекста(textRef,шрифт,цвет)) ->
      пусть color = кXwtЦвету цвет
      пусть макет = кМакету textRef.Value шрифт color
      конт.DrawText(макет, Avalonia.Point(x,y))
   | НарисоватьФигуру(_,ФигураИзображения(изображение)) ->
      если изображение.Value <> нуль тогда                 
         нарисоватьИзображение конт инфо изображение.Value (x,y) |> ignore
