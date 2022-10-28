﻿module internal Бiблiотека.Малювати

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
 
type ИнфоМалюнка = { 
   Малюнок:Рисование; 
   mutable Зсув:Point; 
   mutable Непрозрачность:float option
   mutable Видим:bool 
   mutable Врашение:float option
   mutable Масштаб:(float * float) option
   }

let нарисоватьИзображение (конт:Context) (инфо:ИнфоМалюнка) (зображення:Image) (x,y) =
   match инфо.Врашение with
   | Some угол ->           
      let ш,в = зображення.Width, зображення.Height
      конт.Save()        
      конт.Translate(x+ш/2.0,y+в/2.0)
      конт.Rotate(угол)
      конт.Translate(-ш / 2.0, -в / 2.0)
      match инфо.Масштаб with
      | Some(sx,sy) -> конт.Scale(sx,sy)
      | None -> ()    
      конт.Rectangle(0.0,0.0,ш,в)                
      let узор = new ImagePattern(зображення)            
      конт.Pattern <- узор           
      конт.Fill()
      конт.Restore()
   | None ->
      конт.Save()            
      match инфо.Масштаб with
      | Some(sx,sy) -> 
         конт.Scale(sx,sy)
         конт.DrawImage(зображення,x/sx,y/sy)
      | None ->
         конт.DrawImage(зображення,x,y)
      конт.Restore()

let нарисовать (конт:Context) (инфо:ИнфоМалюнка) =
   let x,y = инфо.Зсув.X, инфо.Зсув.Y
   let сНепрозрачностью (цвет:Color) =
      match инфо.Непрозрачность with
      | Some opacity -> цвет.WithAlpha(цвет.Alpha * opacity)
      | None -> цвет
   match инфо.Малюнок with
   | НамалюватиЛінію(Линия(x1,y1,x2,y2),перо) ->
      конт.MoveTo(x1,y1)
      конт.LineTo(x2,y2)
      росчеркПера конт перо
   | НамалюватиПрямокутник(Прямокутник(ш,в),перо) ->
      конт.Rectangle(x,y,ш,в)
      росчеркПера конт перо
   | НамалюватиТрикутник(треугольник,перо) ->
      намалюватиТрикутник конт треугольник
      росчеркПера конт перо
   | НамалюватиЕліпс(Эллипс(ш,в),перо) ->
      намалюватиЕліпс конт (x,y,ш,в)
      росчеркПера конт перо
   | НамалюватиЗображення(зображення,x',y') ->
      if зображення.Value <> null then         
         нарисоватьИзображение конт инфо зображення.Value (x+x',y+y')
   | НамалюватиТекст(x,y,текст,шрифт,цвет) ->
      let макет = кМакету текст шрифт
      конт.SetColor(кXwtЦвету цвет)
      конт.DrawTextLayout(макет,x,y)
   | НамалюватиТекстУРамці(x,y,ширина,текст,шрифт,цвет) ->
      let макет = кМакету текст шрифт
      макет.Width <- ширина      
      конт.SetColor(кXwtЦвету цвет)
      конт.DrawTextLayout(макет,x,y)
   | ЗаполнитьПрямоугольник(Прямокутник(ш,в),цветЗаливки) ->
      конт.Rectangle(x,y,ш,в)
      заполнить конт (кXwtЦвету цветЗаливки)
   | ЗаполнитьТреугольник(треугольник,цветЗаливки) ->
      намалюватиТрикутник конт треугольник
      заполнить конт (кXwtЦвету цветЗаливки)
   | ЗаповнитиЕліпс(Эллипс(ш,в),цветЗаливки) ->
      намалюватиЕліпс конт (x,y,ш,в)
      заполнить конт (кXwtЦвету цветЗаливки)
   | НамалюватиФігуру(_,ФигураЛинии(Линия(x1,y1,x2,y2),перо)) ->
      конт.MoveTo(x+x1,y+y1)
      конт.LineTo(x+x2,y+y2)
      росчеркПера конт перо
   | НамалюватиФігуру(_,ФигураПрямоугольника(Прямокутник(w,h),перо,цветЗаливки)) ->
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
   | НамалюватиФігуру(_,ФигураТреугольника(треугольник,перо,цветЗаливки)) ->
      намалюватиТрикутник конт треугольник
      заполнить конт (сНепрозрачностью (кXwtЦвету цветЗаливки))
      намалюватиТрикутник конт треугольник
      росчеркПера конт перо
   | НамалюватиФігуру(_,ФигураЭллипса(Эллипс(ш,в),перо,цветЗаливки)) ->
      намалюватиЕліпс конт (x,y,ш,в)      
      заполнить конт (сНепрозрачностью (кXwtЦвету цветЗаливки))
      намалюватиЕліпс конт (x,y,ш,в)
      росчеркПера конт перо
   | НамалюватиФігуру(_,ФигураТекста(textRef,шрифт,цвет)) ->
      let layout = кМакету textRef.Value шрифт      
      конт.SetColor(сНепрозрачностью (кXwtЦвету цвет))
      конт.DrawTextLayout(layout,x,y)
   | НамалюватиФігуру(_,ФигураИзображения(зображення)) ->
      if зображення.Value <> null then                 
         нарисоватьИзображение конт инфо зображення.Value (x,y)
