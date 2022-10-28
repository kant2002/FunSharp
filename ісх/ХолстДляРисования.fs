﻿namespace Бiблiотека

open Xwt
open Xwt.Drawing
open Малювати

[<AllowNullLiteral>]
type internal ХолстДляРисования () =
   inherit Canvas ()
   let зображенняЧерепахи = Image.FromResource(typeof<ХолстДляРисования>, "ВеселШарп.Бiблiотека.черепаха.png")
   let малюнки = ResizeArray<ИнфоМалюнка>()
   let черепаха =
      let ш,в = зображенняЧерепахи.Width, зображенняЧерепахи.Height
      {Малюнок=НамалюватиЗображення(ref зображенняЧерепахи,-ш/2.,-в/2.); Зсув=Point(); Непрозрачность=None; Видим=false; Врашение=None; Масштаб=None}
   let наФигуру имяФигуры ф =
      малюнки
      |> Seq.tryPick (function
         | { Малюнок=НамалюватиФігуру(name,_) } as info when name = имяФигуры -> Some info 
         | _ -> None
      )
      |> Option.iter ф   
   member полотно.Черепаха = черепаха
   member полотно.ОчиститиМалюнки() =
      малюнки.Clear()
      полотно.QueueDraw()
   member полотно.ДодатиМалюнок(малюнок) =
      { Малюнок=малюнок; Зсув=Point(); Непрозрачность=None; Видим=true; Врашение=None; Масштаб=None }
      |> малюнки.Add
      полотно.QueueDraw()
   member полотно.AddDrawingAt(малюнок, зсув:Point) =
      { Малюнок=малюнок; Зсув=зсув; Непрозрачность=None; Видим=true; Врашение=None; Масштаб=None }
      |> малюнки.Add
      полотно.QueueDraw()
   member полотно.ПереместитьФигуру(фигура, зсув:Point) =
      наФигуру фигура (fun инфо -> инфо.Зсув <- зсув; полотно.QueueDraw())
   member полотно.УстановитьНепрозрачностьФигуры(фигура, непрозрачность) =
      наФигуру фигура (fun инфо -> инфо.Непрозрачность <- Some непрозрачность; полотно.QueueDraw())
   member полотно.УстановитьВидимостьФигуры(фигура, видима) =
      наФигуру фигура (fun инфо -> инфо.Видим <- видима; полотно.QueueDraw())
   member полотно.УстановитьВращениеФигуры(фигура, угол) =
      наФигуру фигура (fun инфо -> инфо.Врашение <- Some(угол); полотно.QueueDraw())
   member полотно.ВстановитиМасштабФігури(фигура, масштабX, масштабY) =
      наФигуру фигура (fun инфо -> инфо.Масштаб <- Some(масштабX,масштабY); полотно.QueueDraw())
   member полотно.ВидалитиФігуру(фигура) =
      малюнки |> Seq.tryFindIndex (function 
         | { ИнфоМалюнка.Малюнок=НамалюватиФігуру(имяФигуры,_) } -> имяФигуры = фигура
         | _ -> false 
      )
      |> function Some індекс -> малюнки.RemoveAt(індекс) | None -> ()
   member полотно.ЗробитиНедійсним() =
      полотно.QueueDraw()
   override this.OnDraw(конт, прямоугольник) =
      base.OnDraw(конт, прямоугольник)      
      for малюнок in малюнки do 
         if малюнок.Видим then нарисовать конт малюнок
      if черепаха.Видим then нарисовать конт черепаха