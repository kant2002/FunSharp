пространство Библиотека

открыть Avalonia
открыть Avalonia.Controls
открыть Avalonia.LogicalTree
открыть Avalonia.Media
открыть Avalonia.Media.Imaging
открыть Avalonia.Metadata
открыть Avalonia.Threading
открыть Рисовать

[<AllowNullLiteral>]
тип внутренний ХолстДляРисования () =
   наследует Control ()
   пусть изображениеЧерепахи = новый Bitmap(typeof<ХолстДляРисования>.Assembly.GetManifestResourceStream("ВеселШарп.Библиотека.черепаха.png"))
   пусть рисунки = ResizeArray<ИнфоРисунка>()
   пусть черепаха =
      пусть ш,``в`` = изображениеЧерепахи.Size.Width, изображениеЧерепахи.Size.Height
      {Рисунок=НарисоватьИзображение(ref изображениеЧерепахи,-ш/2.,-``в``/2.); Смещение=Point(); Непрозрачность=None; Видим=ложь; Врашение=None; Масштаб=None}
   пусть наФигуру имяФигуры ф =
      рисунки
      |> Seq.tryPick (функция
         | { Рисунок=НарисоватьФигуру(name,_) } как info when name = имяФигуры -> Some info 
         | _ -> None
      )
      |> Option.iter ф   
   пусть childIndexChanged = Event<System.EventHandler<ChildIndexChangedEventArgs>, ChildIndexChangedEventArgs>()
      
   член знач Background : IBrush = null with get, set

   /// <summary>
   /// Получает детей для <see cref="ПолотноДляМалювання"/>.
   /// </summary>
   [<Content>]
   член полотно.Children = новый Avalonia.Controls.Controls()

   член холст.Черепаха = черепаха
   член холст.ОчиститьРисунки() =
      рисунки.Clear()
      холст.InvalidateVisual()
   член холст.ДобавитьРисунок(рисунок) =
      { Рисунок=рисунок; Смещение=Point(); Непрозрачность=None; Видим=истина; Врашение=None; Масштаб=None }
      |> рисунки.Add
      холст.InvalidateVisual()
   член холст.AddDrawingAt(рисунок, смещение:Point) =
      { Рисунок=рисунок; Смещение=смещение; Непрозрачность=None; Видим=истина; Врашение=None; Масштаб=None }
      |> рисунки.Add
      холст.InvalidateVisual()
   член холст.ПереместитьФигуру(фигура, смещение:Point) =
      наФигуру фигура (фун инфо -> инфо.Смещение <- смещение; холст.InvalidateVisual())
   член холст.УстановитьНепрозрачностьФигуры(фигура, непрозрачность) =
      наФигуру фигура (фун инфо -> инфо.Непрозрачность <- Some непрозрачность; холст.InvalidateVisual())
   член холст.УстановитьВидимостьФигуры(фигура, видима) =
      наФигуру фигура (фун инфо -> инфо.Видим <- видима; холст.InvalidateVisual())
   член холст.УстановитьВращениеФигуры(фигура, угол) =
      наФигуру фигура (фун инфо -> инфо.Врашение <- Some(угол); холст.InvalidateVisual())
   член холст.УстановитьМасштабФигуры(фигура, масштабX, масштабY) =
      наФигуру фигура (фун инфо -> инфо.Масштаб <- Some(масштабX,масштабY); холст.InvalidateVisual())
   член холст.УдалитьФигуру(фигура) =
      рисунки |> Seq.tryFindIndex (функция 
         | { ИнфоРисунка.Рисунок=НарисоватьФигуру(имяФигуры,_) } -> имяФигуры = фигура
         | _ -> ложь 
      )
      |> функция Some индекс -> рисунки.RemoveAt(индекс) | None -> ()
   член холст.СделатьНедействительным() =
      Dispatcher.UIThread.Post (фун () -> холст.InvalidateVisual())
   член полотно.AddChild(element, x, y) =
      полотно.Children.Add(element)
      Canvas.SetLeft(element, x)
      Canvas.SetTop(element, y)
   член полотно.RemoveChild(element) =
      полотно.Children.Remove(element) |> ignore
   переопределить this.Render(конт) =
      пусть фон = this.Background;
      если (not (isNull фон)) тогда
        пусть отрисовываемыйРазмер = this.Bounds.Size
        конт.FillRectangle(фон, new Rect(отрисовываемыйРазмер));
      base.Render(конт)
      для рисунок в рисунки сделать 
         если рисунок.Видим тогда нарисовать конт рисунок
      если черепаха.Видим тогда нарисовать конт черепаха
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