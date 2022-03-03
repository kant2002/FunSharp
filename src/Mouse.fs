namespace Library

[<Sealed>]
type Mouse private () =
   static member IsLeftButtonDown = Мое.Приложение.IsLeftButtonDown
   static member IsRightButtonDown = Мое.Приложение.IsRightButtonDown
   static member X = Мое.Приложение.MouseX
   static member Y = Мое.Приложение.MouseY
   static member HideCursor () =
      Мое.Приложение.Вызвать (fun () -> Мое.Приложение.Холст.Cursor <- Xwt.CursorType.Invisible)
   static member ShowCursor () =
      Мое.Приложение.Вызвать (fun () -> Мое.Приложение.Холст.Cursor <- Xwt.CursorType.Arrow)