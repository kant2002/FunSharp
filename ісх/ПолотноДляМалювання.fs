﻿простір Бібліотека

відкрити Avalonia
відкрити Avalonia.Controls
відкрити Avalonia.Media.Imaging
відкрити Avalonia.Threading
відкрити Малювати

[<AllowNullLiteral>]
тип внутрішній ПолотноДляМалювання () =
   успадкує Canvas ()
   нехай зображенняЧерепахи = новий Bitmap(typeof<ПолотноДляМалювання>.Assembly.GetManifestResourceStream("ВеселШарп.Бібліотека.черепаха.png"))
   нехай малюнки = ResizeArray<ІнфоМалюнка>()
   нехай черепаха =
      нехай ш,в = зображенняЧерепахи.Size.Width, зображенняЧерепахи.Size.Height
      {Малюнок=НамалюватиЗображення(ссил зображенняЧерепахи,-ш/2.,-в/2.); Зсув=Point(); Непрозорість=None; Видно=ложь; Обертання=None; Масштаб=None}
   нехай наФігуру імяФігури ф =
      малюнки
      |> Seq.tryPick (функція
         | { Малюнок=НамалюватиФігуру(назва,_) } як інфо when назва = імяФігури -> Some інфо 
         | _ -> None
      )
      |> Option.iter ф   
   член полотно.Черепаха = черепаха
   член полотно.ОчиститиМалюнки() =
      малюнки.Clear()
      полотно.InvalidateVisual()
   член полотно.ДодатиМалюнок(малюнок) =
      { Малюнок=малюнок; Зсув=Point(); Непрозорість=None; Видно=істина; Обертання=None; Масштаб=None }
      |> малюнки.Add
      полотно.InvalidateVisual()
   член полотно.ДодатиМалюнокУ(малюнок, зсув:Point) =
      { Малюнок=малюнок; Зсув=зсув; Непрозорість=None; Видно=істина; Обертання=None; Масштаб=None }
      |> малюнки.Add
      полотно.InvalidateVisual()
   член полотно.ПереміститиФигуру(фігура, зсув:Point) =
      наФігуру фігура (фун инфо -> инфо.Зсув <- зсув; полотно.InvalidateVisual())
   член полотно.ВстановитиНепрозорістьФігури(фігура, непрозрачность) =
      наФігуру фігура (фун инфо -> инфо.Непрозорість <- Some непрозрачность; полотно.InvalidateVisual())
   член полотно.ВстановитиВидимістьФігури(фігура, видима) =
      наФігуру фігура (фун инфо -> инфо.Видно <- видима; полотно.InvalidateVisual())
   член полотно.ВстановитиОбертанняФігури(фігура, угол) =
      наФігуру фігура (фун инфо -> инфо.Обертання <- Some(угол); полотно.InvalidateVisual())
   член полотно.ВстановитиМасштабФігури(фігура, масштабX, масштабY) =
      наФігуру фігура (фун инфо -> инфо.Масштаб <- Some(масштабX,масштабY); полотно.InvalidateVisual())
   член полотно.ВидалитиФігуру(фигура) =
      малюнки |> Seq.tryFindIndex (функція 
         | { ІнфоМалюнка.Малюнок=НамалюватиФігуру(имяФигуры,_) } -> имяФигуры = фигура
         | _ -> ложь 
      )
      |> функція Some індекс -> малюнки.RemoveAt(індекс) | None -> ()
   член полотно.ЗробитиНедійсним() =
      Dispatcher.UIThread.Post (фун () -> полотно.InvalidateVisual())
   член полотно.ДодатиДітину(елемент, x, y) =
      полотно.Children.Add(елемент)
      Canvas.SetLeft(елемент, x)
      Canvas.SetTop(елемент, y)
   член полотно.ВидалитиДітину(елемент) =
      полотно.Children.Remove(елемент) |> ignore
   перевизначити this.Render(конт) =
      база.Render(конт)
      для малюнок у малюнки зробити 
         якщо малюнок.Видно тоді намалювати конт малюнок
      якщо черепаха.Видно тоді намалювати конт черепаха