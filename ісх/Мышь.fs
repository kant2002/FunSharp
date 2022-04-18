namespace Бiблiотека

[<Sealed>]
type Мышь private () =
   static member ЛеваяКнопкаНажата = Мое.Приложение.ЛеваяКнопкаНажата
   static member ПраваяКнопкаНажата = Мое.Приложение.ПраваяКнопкаНажата
   static member X = Мое.Приложение.МышьX
   static member Y = Мое.Приложение.МышьY
   static member СкрытьКурсор () =
      Мое.Приложение.Вызвать (fun () -> Мое.Приложение.Холст.Cursor <- Xwt.CursorType.Invisible)
   static member ПоказатьКурсор () =
      Мое.Приложение.Вызвать (fun () -> Мое.Приложение.Холст.Cursor <- Xwt.CursorType.Arrow)