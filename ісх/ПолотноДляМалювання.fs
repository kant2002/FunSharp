namespace Бібліотека

open Avalonia
open Avalonia.Controls
open Avalonia.LogicalTree
open Avalonia.Media
open Avalonia.Media.Imaging
open Avalonia.Metadata
open Avalonia.Threading
open Малювати

[<AllowNullLiteral>]
type internal ПолотноДляМалювання () =
   inherit Control ()

   let зображенняЧерепахи = new Bitmap(typeof<ПолотноДляМалювання>.Assembly.GetManifestResourceStream("ВеселШарп.Бібліотека.черепаха.png"))
   let малюнки = ResizeArray<ІнфоМалюнка>()
   let черепаха =
      let ш,в = зображенняЧерепахи.Size.Width, зображенняЧерепахи.Size.Height
      {Малюнок=НамалюватиЗображення(ref зображенняЧерепахи,-ш/2.,-в/2.); Зсув=Point(); Непрозорість=None; Видно=false; Обертання=None; Масштаб=None}
   let наФігуру імяФігури ф =
      малюнки
      |> Seq.tryPick (function
         | { Малюнок=НамалюватиФігуру(name,_) } as info when name = імяФігури -> Some info 
         | _ -> None
      )
      |> Option.iter ф   
   let childIndexChanged = Event<System.EventHandler<ChildIndexChangedEventArgs>, ChildIndexChangedEventArgs>()
      
   member val Background : IBrush = null with get, set

   /// <summary>
   /// Получает детей для <see cref="ПолотноДляМалювання"/>.
   /// </summary>
   [<Content>]
   member полотно.Children = new Avalonia.Controls.Controls()

   member полотно.Черепаха = черепаха
   member полотно.ОчиститиМалюнки() =
      малюнки.Clear()
      полотно.InvalidateVisual()
   member полотно.ДодатиМалюнок(малюнок) =
      { Малюнок=малюнок; Зсув=Point(); Непрозорість=None; Видно=true; Обертання=None; Масштаб=None }
      |> малюнки.Add
      полотно.InvalidateVisual()
   member полотно.ДодатиМалюнокУ(малюнок, зсув:Point) =
      { Малюнок=малюнок; Зсув=зсув; Непрозорість=None; Видно=true; Обертання=None; Масштаб=None }
      |> малюнки.Add
      полотно.InvalidateVisual()
   member полотно.ПереміститиФигуру(фігура, зсув:Point) =
      наФігуру фігура (fun инфо -> инфо.Зсув <- зсув; полотно.InvalidateVisual())
   member полотно.ВстановитиНепрозорістьФігури(фігура, непрозрачность) =
      наФігуру фігура (fun инфо -> инфо.Непрозорість <- Some непрозрачность; полотно.InvalidateVisual())
   member полотно.ВстановитиВидимістьФігури(фігура, видима) =
      наФігуру фігура (fun инфо -> инфо.Видно <- видима; полотно.InvalidateVisual())
   member полотно.ВстановитиОбертанняФігури(фігура, угол) =
      наФігуру фігура (fun инфо -> инфо.Обертання <- Some(угол); полотно.InvalidateVisual())
   member полотно.ВстановитиМасштабФігури(фігура, масштабX, масштабY) =
      наФігуру фігура (fun инфо -> инфо.Масштаб <- Some(масштабX,масштабY); полотно.InvalidateVisual())
   member полотно.ВидалитиФігуру(фигура) =
      малюнки |> Seq.tryFindIndex (function 
         | { ІнфоМалюнка.Малюнок=НамалюватиФігуру(имяФигуры,_) } -> имяФигуры = фигура
         | _ -> false 
      )
      |> function Some індекс -> малюнки.RemoveAt(індекс) | None -> ()
   member полотно.ЗробитиНедійсним() =
      Dispatcher.UIThread.Post (fun () -> полотно.InvalidateVisual())
   member полотно.ДодатиДітину(елемент, x, y) =
      полотно.Children.Add(елемент)
      Canvas.SetLeft(елемент, x)
      Canvas.SetTop(елемент, y)
   member полотно.ВидалитиДітину(елемент) =
      полотно.Children.Remove(елемент) |> ігнорувати
   override this.Render(конт) =
      let фон = this.Background;
      if (not (isNull фон)) then
        let отрисовываемыйРазмер = this.Bounds.Size
        конт.FillRectangle(фон, new Rect(отрисовываемыйРазмер));
      base.Render(конт)
      for малюнок in малюнки do 
         if малюнок.Видно then намалювати конт малюнок
      if черепаха.Видно then намалювати конт черепаха
   interface IChildIndexProvider with
      member this.GetChildIndex child =
        match child with
        | :? Control -> this.Children.IndexOf(child :?> Control)
        | _ -> -1
      member this.TryGetTotalCount (count: byref<int>) =
        count <- this.Children.Count
        true

      [<CLIEvent>]
      member this.ChildIndexChanged = childIndexChanged.Publish