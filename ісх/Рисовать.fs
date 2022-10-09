﻿module internal Бiблiотека.Рисовать

open Xwt
open Xwt.Drawing

let намалюватиЕліпс (конт:Context) (x,y,ш,в) =
   let каппа = 0.5522848
   let ox = (ш/2.0) * каппа
   let oy = (в/2.0) * каппа
   let xк = x + ш
   let yк = y + в
   let xс = x + ш / 2.0
   let yс = y + в / 2.0
   конт.MoveTo(x,yс)
   конт.CurveTo(x, yс - oy, xс - ox, y, xс, y)
   конт.CurveTo(xс + ox, y, xк, yс - oy, xк, yс)
   конт.CurveTo(xк, yс + oy, xс + ox, yк, xс, yк)
   конт.CurveTo(xс - ox, yк, x, yс + oy, x, yс)

let намалюватиТрикутник (конт:Context) (Треугольник(x1,y1,x2,y2,x3,y3)) =
   конт.MoveTo(x1,y1)
   конт.LineTo(x2,y2)
   конт.LineTo(x3,y3)
   конт.LineTo(x1,y1)

let росчеркПера (конт:Context) (Перо(цвет,ширина)) =
   конт.SetColor(кXwtЦвету цвет)
   конт.SetLineWidth(ширина)
   конт.Stroke()

let заполнить (конт:Context) (цвет:Color) =
   конт.SetColor(цвет)  
   конт.Fill()

let кМакету текст (Шрифт(размер,семейство,жирный,курсив)) =
   let макет = new TextLayout(Text=текст)
   макет.Font <- макет.Font.WithSize(размер/2.0)      
   if жирный then макет.Font <- макет.Font.WithWeight(FontWeight.Bold)
   if курсив then макет.Font <- макет.Font.WithStyle(FontStyle.Italic)
   if семейство <> "" then макет.Font <- макет.Font.WithFamily(семейство)
   макет
 
type ИнфоРисунка = { 
   Рисунок:Рисование; 
   mutable Смещение:Point; 
   mutable Непрозрачность:float option
   mutable Видим:bool 
   mutable Врашение:float option
   mutable Масштаб:(float * float) option
   }

let нарисоватьИзображение (конт:Context) (инфо:ИнфоРисунка) (изображение:Image) (x,y) =
   match инфо.Врашение with
   | Some угол ->           
      let ш,в = изображение.Width, изображение.Height
      конт.Save()        
      конт.Translate(x+ш/2.0,y+в/2.0)
      конт.Rotate(угол)
      конт.Translate(-ш / 2.0, -в / 2.0)
      match инфо.Масштаб with
      | Some(sx,sy) -> конт.Scale(sx,sy)
      | None -> ()    
      конт.Rectangle(0.0,0.0,ш,в)                
      let узор = new ImagePattern(изображение)            
      конт.Pattern <- узор           
      конт.Fill()
      конт.Restore()
   | None ->
      конт.Save()            
      match инфо.Масштаб with
      | Some(sx,sy) -> 
         конт.Scale(sx,sy)
         конт.DrawImage(изображение,x/sx,y/sy)
      | None ->
         конт.DrawImage(изображение,x,y)
      конт.Restore()

let нарисовать (конт:Context) (инфо:ИнфоРисунка) =
   let x,y = инфо.Смещение.X, инфо.Смещение.Y
   let сНепрозрачностью (цвет:Color) =
      match инфо.Непрозрачность with
      | Some opacity -> цвет.WithAlpha(цвет.Alpha * opacity)
      | None -> цвет
   match инфо.Рисунок with
   | НамалюватиЛінію(Линия(x1,y1,x2,y2),перо) ->
      конт.MoveTo(x1,y1)
      конт.LineTo(x2,y2)
      росчеркПера конт перо
   | НамалюватиПрямокутник(Прямоугольник(ш,в),перо) ->
      конт.Rectangle(x,y,ш,в)
      росчеркПера конт перо
   | НамалюватиТрикутник(треугольник,перо) ->
      намалюватиТрикутник конт треугольник
      росчеркПера конт перо
   | НамалюватиЕліпс(Эллипс(ш,в),перо) ->
      намалюватиЕліпс конт (x,y,ш,в)
      росчеркПера конт перо
   | НамалюватиЗображення(изображение,x',y') ->
      if изображение.Value <> null then         
         нарисоватьИзображение конт инфо изображение.Value (x+x',y+y')
   | НарисоватьТекст(x,y,текст,шрифт,цвет) ->
      let макет = кМакету текст шрифт
      конт.SetColor(кXwtЦвету цвет)
      конт.DrawTextLayout(макет,x,y)
   | НарисоватьТекстВРамке(x,y,ширина,текст,шрифт,цвет) ->
      let макет = кМакету текст шрифт
      макет.Width <- ширина      
      конт.SetColor(кXwtЦвету цвет)
      конт.DrawTextLayout(макет,x,y)
   | ЗаполнитьПрямоугольник(Прямоугольник(ш,в),цветЗаливки) ->
      конт.Rectangle(x,y,ш,в)
      заполнить конт (кXwtЦвету цветЗаливки)
   | ЗаполнитьТреугольник(треугольник,цветЗаливки) ->
      намалюватиТрикутник конт треугольник
      заполнить конт (кXwtЦвету цветЗаливки)
   | ЗаполнитьЭллипс(Эллипс(ш,в),цветЗаливки) ->
      намалюватиЕліпс конт (x,y,ш,в)
      заполнить конт (кXwtЦвету цветЗаливки)
   | НарисоватьФигуру(_,ФигураЛинии(Линия(x1,y1,x2,y2),перо)) ->
      конт.MoveTo(x+x1,y+y1)
      конт.LineTo(x+x2,y+y2)
      росчеркПера конт перо
   | НарисоватьФигуру(_,ФигураПрямоугольника(Прямоугольник(w,h),перо,цветЗаливки)) ->
      конт.Save() 
      конт.Translate(x,y)
      match инфо.Врашение with
      | Some угол -> конт.Rotate(угол)
      | None -> ()            
      конт.Rectangle(0.,0.,w,h)
      заполнить конт (кXwtЦвету цветЗаливки)
      конт.Rectangle(0.,0.,w,h)
      росчеркПера конт перо
      конт.Restore()
   | НарисоватьФигуру(_,ФигураТреугольника(треугольник,перо,цветЗаливки)) ->
      намалюватиТрикутник конт треугольник
      заполнить конт (сНепрозрачностью (кXwtЦвету цветЗаливки))
      намалюватиТрикутник конт треугольник
      росчеркПера конт перо
   | НарисоватьФигуру(_,ФигураЭллипса(Эллипс(ш,в),перо,цветЗаливки)) ->
      намалюватиЕліпс конт (x,y,ш,в)      
      заполнить конт (сНепрозрачностью (кXwtЦвету цветЗаливки))
      намалюватиЕліпс конт (x,y,ш,в)
      росчеркПера конт перо
   | НарисоватьФигуру(_,ФигураТекста(textRef,шрифт,цвет)) ->
      let layout = кМакету textRef.Value шрифт      
      конт.SetColor(сНепрозрачностью (кXwtЦвету цвет))
      конт.DrawTextLayout(layout,x,y)
   | НарисоватьФигуру(_,ФигураИзображения(изображение)) ->
      if изображение.Value <> null then                 
         нарисоватьИзображение конт инфо изображение.Value (x,y)
