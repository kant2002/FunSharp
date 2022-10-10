namespace Бiблiотека

[<Sealed>]
type Мышь private () =
   static member ЛіваКнопкаНатиснута = Мое.Приложение.ЛіваКнопкаНатиснута
   static member ПраваКнопкаНатиснута = Мое.Приложение.ПраваКнопкаНатиснута
   static member X = Мое.Приложение.МишаX
   static member Y = Мое.Приложение.МишаY
   static member СкрытьКурсор () =
      Мое.Приложение.Вызвать (fun () -> Мое.Приложение.Полотно.Cursor <- Xwt.CursorType.Invisible)
   static member ПоказатьКурсор () =
      Мое.Приложение.Вызвать (fun () -> Мое.Приложение.Полотно.Cursor <- Xwt.CursorType.Arrow)