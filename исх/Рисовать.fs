﻿module internal Библиотека.Рисовать

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

let кМакету текст (Шрифт(размер,семейство,жирный,курсив)) цвет =
   let макет = new FormattedText(текст, System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, Typeface.Default, 1, new SolidColorBrush(цвет, 1.))
   макет.SetFontSize(размер/2.0)      
   if жирный then макет.SetFontWeight(FontWeight.Bold)
   if курсив then макет.SetFontStyle(FontStyle.Italic)
   if семейство <> "" then макет.SetFontFamily(семейство)
   макет
 
type ИнфоРисунка = { 
   Рисунок:Рисование; 
   mutable Смещение:Point; 
   mutable Непрозрачность:float option
   mutable Видим:bool 
   mutable Врашение:float option
   mutable Масштаб:(float * float) option
   }

let нарисоватьИзображение (конт:DrawingContext) (инфо:ИнфоРисунка) (изображение:IImage) (x,y) =
   match инфо.Врашение with
   | Some угол ->           
      let ш,в = изображение.Size.Width, изображение.Size.Height
      let currentTransform = конт.CurrentTransform;
      let source = new Rect(new Point(0.0,0.0),изображение.Size)
      конт.PushPreTransform (Matrix.CreateTranslation(x+ш/2.0,y+в/2.0)) |> ignore
      конт.PushPreTransform (Matrix.CreateRotation(Библиотека.Математика.ВзятьРадианы угол)) |> ignore
      конт.PushPreTransform (Matrix.CreateTranslation(-ш / 2.0, -в / 2.0)) |> ignore
      match инфо.Масштаб with
      | Some(sx,sy) -> конт.PushPreTransform (Matrix.CreateScale(sx,sy)) |> ignore
      | None -> ()    
      конт.DrawImage(изображение, source)
      конт.PushSetTransform currentTransform;
   | None ->
      let currentTransform = конт.CurrentTransform;
      match инфо.Масштаб with
      | Some(sx,sy) -> 
         конт.PushPreTransform (Matrix.CreateScale(sx,sy)) |> ignore
         конт.DrawImage(изображение,new Rect(x, y, изображение.Size.Width/sx,изображение.Size.Height/sy))
      | None ->
         конт.DrawImage(изображение,new Rect(x, y, изображение.Size.Width, изображение.Size.Height))
      конт.PushSetTransform currentTransform;

let нарисовать (конт:DrawingContext) (инфо:ИнфоРисунка) =
   let x,y = инфо.Смещение.X, инфо.Смещение.Y
   let сНепрозрачностью (цвет:Color) =
      match инфо.Непрозрачность with
      | Some opacity -> Color.FromArgb(byte (opacity * float цвет.A), цвет.R, цвет.G, цвет.B)
      | None -> цвет
   match инфо.Рисунок with
   | НарисоватьЛинию(Линия(x1,y1,x2,y2),Перо(цвет,ширина)) ->
      let color = кXwtЦвету цвет
      let перо = new Pen(new SolidColorBrush(color, 1.0), ширина)
      конт.DrawLine(перо, Avalonia.Point(x1, y1), Avalonia.Point(x2, y2))
   | НарисоватьПрямоугольник(Прямоугольник(ш,в),Перо(цвет,ширина)) ->
      let color = кXwtЦвету цвет
      let перо = new Pen(new SolidColorBrush(color, 1.0), ширина)
      конт.DrawRectangle(null, перо, Avalonia.Rect(x,y,ш,в))
   | НарисоватьТреугольник(Треугольник(x1,y1,x2,y2,x3,y3),Перо(цвет,ширина)) ->
      let color = кXwtЦвету цвет
      let перо = new Pen(new SolidColorBrush(color, 1.0), ширина)
      конт.DrawLine(перо, Avalonia.Point(x1, y1), Avalonia.Point(x2, y2))
      конт.DrawLine(перо, Avalonia.Point(x2, y2), Avalonia.Point(x3, y3))
      конт.DrawLine(перо, Avalonia.Point(x3, y3), Avalonia.Point(x1, y1))
   | НарисоватьЭллипс(Эллипс(ш,в),Перо(колір,ширина)) ->
      let color = кXwtЦвету колір
      let перо = new Pen(new SolidColorBrush(color, 1.0), ширина)
      конт.DrawEllipse(null, перо, Avalonia.Point(x,y),ш,в)
   | НарисоватьИзображение(изображение,x',y') ->
      if изображение.Value <> null then         
         нарисоватьИзображение конт инфо изображение.Value (x+x',y+y') |> ignore
   | НарисоватьТекст(x,y,текст,шрифт,цвет) ->
      let color = кXwtЦвету цвет
      let макет = кМакету текст шрифт color
      конт.DrawText(макет, Avalonia.Point(x,y))
   | НарисоватьТекстВРамке(x,y,ширина,текст,шрифт,цвет) ->
      let color = кXwtЦвету цвет
      let макет = кМакету текст шрифт color
      макет.MaxTextWidth <- ширина
      конт.DrawText(макет, Avalonia.Point(x,y))
   | ЗаполнитьПрямоугольник(Прямоугольник(ш,в),цветЗаливки) ->
      let color = кXwtЦвету цветЗаливки
      let перо = new SolidColorBrush(color, 1.0)
      конт.DrawRectangle(перо, null, Avalonia.Rect(x,y,ш,в))
   | ЗаполнитьТреугольник(треугольник,цветЗаливки) ->
      let color = кXwtЦвету цветЗаливки
      let перо = new SolidColorBrush(color, 1.0)
      let геометрія = зробитиТрикутник треугольник
      конт.DrawGeometry(перо, null, геометрія)
   | ЗаполнитьЭллипс(Эллипс(ш,в),цветЗаливки) ->
      let color = кXwtЦвету цветЗаливки
      let кисть = new SolidColorBrush(color, 1.0)
      конт.DrawEllipse(кисть, null, Avalonia.Point(x+ш/2.,y+в/2.),ш/2.,в/2.)
   | НарисоватьФигуру(_,ФигураЛинии(Линия(x1,y1,x2,y2),Перо(цвет,ширина))) ->
      let color = кXwtЦвету цвет
      let перо = new Pen(new SolidColorBrush(color, 1.0), ширина)
      конт.DrawLine(перо, Avalonia.Point(x+ x1, y+y1), Avalonia.Point(x+ x2, y+y2))
   | НарисоватьФигуру(_,ФигураПрямоугольника(Прямоугольник(w,h),Перо(цвет,ширина),цветЗаливки)) ->
      let currentTransform = конт.CurrentTransform
      конт.PushPreTransform (Matrix.CreateTranslation(x,y)) |> ignore
      match инфо.Врашение with
      | Some угол -> конт.PushPreTransform (Matrix.CreateRotation(угол)) |> ignore
      | None -> ()            
      let color = кXwtЦвету цвет
      let colorBackground = кXwtЦвету цветЗаливки
      let перо = new Pen(new SolidColorBrush(color, 1.0), ширина)
      конт.DrawRectangle(new SolidColorBrush(colorBackground, 1.0), перо, Avalonia.Rect(0.,0.,w,h))
      конт.PushSetTransform currentTransform |> ignore;
   | НарисоватьФигуру(_,ФигураТреугольника(треугольник,Перо(цвет,ширина),цветЗаливки)) ->
      let пензлик = new SolidColorBrush(сНепрозрачностью (кXwtЦвету цветЗаливки), 1.0)
      let перо = new Pen(new SolidColorBrush(кXwtЦвету цвет, 1.0), ширина)
      let геометрія = зробитиТрикутник треугольник
      конт.DrawGeometry(пензлик, перо, геометрія)
   | НарисоватьФигуру(_,ФигураЭллипса(Эллипс(ш,в),Перо(цвет,ширина),цветЗаливки)) ->
      let перо = new Pen(new SolidColorBrush(кXwtЦвету цвет, 1.0), ширина)
      let пензлик = new SolidColorBrush(сНепрозрачностью (кXwtЦвету цветЗаливки), 1.0)
      конт.DrawEllipse(пензлик, перо, Avalonia.Point(x,y),ш,в)
   | НарисоватьФигуру(_,ФигураТекста(textRef,шрифт,цвет)) ->
      let color = кXwtЦвету цвет
      let макет = кМакету textRef.Value шрифт color
      конт.DrawText(макет, Avalonia.Point(x,y))
   | НарисоватьФигуру(_,ФигураИзображения(изображение)) ->
      if изображение.Value <> null then                 
         нарисоватьИзображение конт инфо изображение.Value (x,y) |> ignore
