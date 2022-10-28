namespace Бiблiотека

[<Sealed>]
type Мышь private () =
   static member ЛіваКнопкаНатиснута = Моя.Апплікація.ЛіваКнопкаНатиснута
   static member ПраваКнопкаНатиснута = Моя.Апплікація.ПраваКнопкаНатиснута
   static member X = Моя.Апплікація.МишаX
   static member Y = Моя.Апплікація.МишаY
   static member СкрытьКурсор () =
      Моя.Апплікація.Викликати (fun () -> Моя.Апплікація.Полотно.Cursor <- Xwt.CursorType.Invisible)
   static member ПоказатьКурсор () =
      Моя.Апплікація.Викликати (fun () -> Моя.Апплікація.Полотно.Cursor <- Xwt.CursorType.Arrow)