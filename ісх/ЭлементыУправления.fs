namespace Бiблiотека

open System
open System.Collections.Generic
open Xwt

[<Sealed>]
type ЭлементыУправления private () =
   static let mutable наКлик = Callback(ignore)
   static let mutable последняяНажатаяКнопка = ""
   static let элементыУправления = Dictionary<string,Widget>()
   static let позиции = Dictionary<string,int * int>()
   /// Добавляет кнопку к графическому окну
   static member ДобавитьКнопку(метка:string, x:int, y:int) =
      let имя = "Button" + Guid.NewGuid().ToString()
      let кнопка = new Button(метка)
      кнопка.Clicked.Add(fun _ -> последняяНажатаяКнопка <- имя; наКлик.Invoke())
      //toXwtColor(ГрафичнеВікно.ЦветКисти)      
      элементыУправления.Add(имя,кнопка)
      позиции.Add(имя,(x,y))
      Моя.Апплікація.Викликати( fun () ->                  
         Моя.Апплікація.Полотно.AddChild(кнопка,float x,float y)
      )
      имя
   /// Добавляет текстовое поле ввода к графическому окну
   static member ДобавитьТекстовоеПоле(x,y) =
      let название = "TextBox" + Guid.NewGuid().ToString()
      let элементУправления = new TextEntry()
      элементУправления.Text <- "Boo"
      // toXwtColor(ГрафичнеВікно.ЦветКисти)
      элементыУправления.Add(название,элементУправления)
      позиции.Add(название,(x,y))
      Моя.Апплікація.Викликати( fun () ->                  
         Моя.Апплікація.Полотно.AddChild(элементУправления,float x,float y)
      )
      название
   /// Устанавливает размер элемента управления
   static member УстановитьРазмер(названиеЭлемента, ширина:int, высота:int) =
      let элементУправления = элементыУправления.[названиеЭлемента]
      let x, y = позиции.[названиеЭлемента]
      Моя.Апплікація.Викликати( fun () ->
         Моя.Апплікація.Полотно.RemoveChild(элементУправления)        
         элементУправления.WidthRequest <- float ширина
         элементУправления.HeightRequest <- float высота
         элементУправления.MinWidth <- float ширина
         элементУправления.MinHeight <- float высота
         Моя.Апплікація.Полотно.AddChild(элементУправления,float x,float y)
      )
   /// Устанавливает текст указанного текстового поля
   static member УстановитьТекстТекстовогоПоля(названиеЭлемента, текст) = 
      let элементУправления = элементыУправления.[названиеЭлемента] :?> Xwt.TextEntry
      элементУправления.Text <- текст
      Моя.Апплікація.Викликати( fun () ->
         элементУправления.SetFocus()
         элементУправления.QueueForReallocate()
      )
   /// Получает текущий текст в указанном текстовом поле
   static member ПолучитьТекстТекстовогоПоля(названиеЭлемента) =
      let элементУправления = элементыУправления.[названиеЭлемента] :?> Xwt.TextEntry
      элементУправления.Text
   static member КнопкаКликнута with set (callback:Callback) = наКлик <- callback
   static member ПоследняяКликнутаяКнопка with get() = последняяНажатаяКнопка