namespace Бiблiотека

open Avalonia
open Avalonia.Controls
open Малювати
open Avalonia.Media.Imaging
open Avalonia.Threading

[<AllowNullLiteral>]
type internal ХолстДляРисования () =
   inherit Canvas ()
   let зображенняЧерепахи = new Bitmap(typeof<ХолстДляРисования>.Assembly.GetManifestResourceStream("ВеселШарп.Бiблiотека.черепаха.png"))
   let малюнки = ResizeArray<ИнфоМалюнка>()
   let черепаха =
      let ш,в = зображенняЧерепахи.Size.Width, зображенняЧерепахи.Size.Height
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
      полотно.InvalidateVisual()
   member полотно.ДодатиМалюнок(малюнок) =
      { Малюнок=малюнок; Зсув=Point(); Непрозрачность=None; Видим=true; Врашение=None; Масштаб=None }
      |> малюнки.Add
      полотно.InvalidateVisual()
   member полотно.AddDrawingAt(малюнок, зсув:Point) =
      { Малюнок=малюнок; Зсув=зсув; Непрозрачность=None; Видим=true; Врашение=None; Масштаб=None }
      |> малюнки.Add
      полотно.InvalidateVisual()
   member полотно.ПереместитьФигуру(фигура, зсув:Point) =
      наФигуру фигура (fun инфо -> инфо.Зсув <- зсув; полотно.InvalidateVisual())
   member полотно.УстановитьНепрозрачностьФигуры(фигура, непрозрачность) =
      наФигуру фигура (fun инфо -> инфо.Непрозрачность <- Some непрозрачность; полотно.InvalidateVisual())
   member полотно.УстановитьВидимостьФигуры(фигура, видима) =
      наФигуру фигура (fun инфо -> инфо.Видим <- видима; полотно.InvalidateVisual())
   member полотно.УстановитьВращениеФигуры(фигура, угол) =
      наФигуру фигура (fun инфо -> инфо.Врашение <- Some(угол); полотно.InvalidateVisual())
   member полотно.ВстановитиМасштабФігури(фигура, масштабX, масштабY) =
      наФигуру фигура (fun инфо -> инфо.Масштаб <- Some(масштабX,масштабY); полотно.InvalidateVisual())
   member полотно.ВидалитиФігуру(фигура) =
      малюнки |> Seq.tryFindIndex (function 
         | { ИнфоМалюнка.Малюнок=НамалюватиФігуру(имяФигуры,_) } -> имяФигуры = фигура
         | _ -> false 
      )
      |> function Some індекс -> малюнки.RemoveAt(індекс) | None -> ()
   member полотно.ЗробитиНедійсним() =
      Dispatcher.UIThread.Post (fun () -> полотно.InvalidateVisual())
   member полотно.AddChild(element, x, y) =
      полотно.Children.Add(element)
      Canvas.SetLeft(element, x)
      Canvas.SetTop(element, y)
   member полотно.RemoveChild(element) =
      полотно.Children.Remove(element) |> ignore
   override this.Render(конт) =
      base.Render(конт)
      for малюнок in малюнки do 
         if малюнок.Видим then нарисовать конт малюнок
      if черепаха.Видим then нарисовать конт черепаха