﻿простір Бібліотека

відкрити Avalonia
відкрити Avalonia.Controls
відкрити Avalonia.Media.Imaging
відкрити Avalonia.Threading
відкрити Малювати

[<AllowNullLiteral>]
тип internal ПолотноДляМалювання () =
   inherit Canvas ()
   нехай зображенняЧерепахи = new Bitmap(typeof<ПолотноДляМалювання>.Assembly.GetManifestResourceStream("ВеселШарп.Бібліотека.черепаха.png"))
   нехай малюнки = ResizeArray<ІнфоМалюнка>()
   нехай черепаха =
      нехай ш,в = зображенняЧерепахи.Size.Width, зображенняЧерепахи.Size.Height
      {Малюнок=НамалюватиЗображення(ref зображенняЧерепахи,-ш/2.,-в/2.); Зсув=Point(); Непрозорість=None; Видно=false; Обертання=None; Масштаб=None}
   нехай наФігуру імяФігури ф =
      малюнки
      |> Seq.tryPick (function
         | { Малюнок=НамалюватиФігуру(name,_) } as info when name = імяФігури -> Some info 
         | _ -> None
      )
      |> Option.iter ф   
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
      полотно.Children.Remove(елемент) |> ignore
   override this.Render(конт) =
      base.Render(конт)
      for малюнок in малюнки зробити 
         if малюнок.Видно then намалювати конт малюнок
      if черепаха.Видно then намалювати конт черепаха