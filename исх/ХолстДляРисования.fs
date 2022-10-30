namespace Библиотека

open Avalonia
open Avalonia.Controls
open Avalonia.Media.Imaging
open Avalonia.Threading
open Рисовать

[<AllowNullLiteral>]
type internal ХолстДляРисования () =
   inherit Canvas ()
   let изображениеЧерепахи = new Bitmap(typeof<ХолстДляРисования>.Assembly.GetManifestResourceStream("ВеселШарп.Библиотека.черепаха.png"))
   let рисунки = ResizeArray<ИнфоРисунка>()
   let черепаха =
      let ш,в = изображениеЧерепахи.Size.Width, изображениеЧерепахи.Size.Height
      {Рисунок=НарисоватьИзображение(ref изображениеЧерепахи,-ш/2.,-в/2.); Смещение=Point(); Непрозрачность=None; Видим=false; Врашение=None; Масштаб=None}
   let наФигуру имяФигуры ф =
      рисунки
      |> Seq.tryPick (function
         | { Рисунок=НарисоватьФигуру(name,_) } as info when name = имяФигуры -> Some info 
         | _ -> None
      )
      |> Option.iter ф   
   member холст.Черепаха = черепаха
   member холст.ОчиститьРисунки() =
      рисунки.Clear()
      холст.InvalidateVisual()
   member холст.ДобавитьРисунок(рисунок) =
      { Рисунок=рисунок; Смещение=Point(); Непрозрачность=None; Видим=true; Врашение=None; Масштаб=None }
      |> рисунки.Add
      холст.InvalidateVisual()
   member холст.AddDrawingAt(рисунок, смещение:Point) =
      { Рисунок=рисунок; Смещение=смещение; Непрозрачность=None; Видим=true; Врашение=None; Масштаб=None }
      |> рисунки.Add
      холст.InvalidateVisual()
   member холст.ПереместитьФигуру(фигура, смещение:Point) =
      наФигуру фигура (fun инфо -> инфо.Смещение <- смещение; холст.InvalidateVisual())
   member холст.УстановитьНепрозрачностьФигуры(фигура, непрозрачность) =
      наФигуру фигура (fun инфо -> инфо.Непрозрачность <- Some непрозрачность; холст.InvalidateVisual())
   member холст.УстановитьВидимостьФигуры(фигура, видима) =
      наФигуру фигура (fun инфо -> инфо.Видим <- видима; холст.InvalidateVisual())
   member холст.УстановитьВращениеФигуры(фигура, угол) =
      наФигуру фигура (fun инфо -> инфо.Врашение <- Some(угол); холст.InvalidateVisual())
   member холст.УстановитьМасштабФигуры(фигура, масштабX, масштабY) =
      наФигуру фигура (fun инфо -> инфо.Масштаб <- Some(масштабX,масштабY); холст.InvalidateVisual())
   member холст.УдалитьФигуру(фигура) =
      рисунки |> Seq.tryFindIndex (function 
         | { ИнфоРисунка.Рисунок=НарисоватьФигуру(имяФигуры,_) } -> имяФигуры = фигура
         | _ -> false 
      )
      |> function Some индекс -> рисунки.RemoveAt(индекс) | None -> ()
   member холст.СделатьНедействительным() =
      Dispatcher.UIThread.Post (fun () -> холст.InvalidateVisual())
   member полотно.AddChild(element, x, y) =
      полотно.Children.Add(element)
      Canvas.SetLeft(element, x)
      Canvas.SetTop(element, y)
   member полотно.RemoveChild(element) =
      полотно.Children.Remove(element) |> ignore
   override this.Render(конт) =
      base.Render(конт)      
      for рисунок in рисунки do 
         if рисунок.Видим then нарисовать конт рисунок
      if черепаха.Видим then нарисовать конт черепаха