namespace Бiблiотека

open System
open System.Collections.Generic
open Avalonia.Controls

[<Sealed>]
type ЭлементыУправления private () =
   static let mutable наКлик = Callback(ignore)
   static let mutable последняяНажатаяКнопка = ""
   static let элементыУправления = Dictionary<string,Avalonia.Controls.Control>()
   static let позиции = Dictionary<string,int * int>()
   /// Добавляет кнопку к графическому окну
   static member ДобавитьКнопку(метка:string, x:int, y:int) =
      Моя.Апплікація.ВикликатиЗРезультатом(fun () ->
         let имя = "Button" + Guid.NewGuid().ToString()
         let кнопка = new Button(Content = метка)
         кнопка.Click.Add(fun _ -> последняяНажатаяКнопка <- имя; наКлик.Invoke())
         //toXwtColor(ГрафичнеВікно.ЦветКисти)      
         элементыУправления.Add(имя,кнопка)
         позиции.Add(имя,(x,y))
         Моя.Апплікація.Полотно.AddChild(кнопка,float x,float y)
         имя
      )
   /// Добавляет текстовое поле ввода к графическому окну
   static member ДобавитьТекстовоеПоле(x,y) =
      Моя.Апплікація.ВикликатиЗРезультатом(fun () ->
          let название = "TextBox" + Guid.NewGuid().ToString()
          let элементУправления = new Avalonia.Controls.TextBox()
          элементУправления.Text <- "Boo"
          // toXwtColor(ГрафичнеВікно.ЦветКисти)
          элементыУправления.Add(название,элементУправления)
          позиции.Add(название,(x,y))
          Моя.Апплікація.Полотно.AddChild(элементУправления,float x,float y)
          название
      )
   /// Устанавливает размер элемента управления
   static member УстановитьРазмер(названиеЭлемента, ширина:int, высота:int) =
      let элементУправления = элементыУправления.[названиеЭлемента]
      let x, y = позиции.[названиеЭлемента]
      Моя.Апплікація.Викликати( fun () ->
         Моя.Апплікація.Полотно.RemoveChild(элементУправления)        
         элементУправления.Width <- float ширина
         элементУправления.Height <- float высота
         элементУправления.MinWidth <- float ширина
         элементУправления.MinHeight <- float высота
         Моя.Апплікація.Полотно.AddChild(элементУправления,float x,float y)
      )
   /// Устанавливает текст указанного текстового поля
   static member УстановитьТекстТекстовогоПоля(названиеЭлемента, текст) = 
      let элементУправления = элементыУправления.[названиеЭлемента] :?> TextBox
      Моя.Апплікація.Викликати( fun () ->
         printf "%s" текст
         элементУправления.Text <- текст
         элементУправления.Focus()
         //элементУправления.QueueForReallocate()
      )
   /// Получает текущий текст в указанном текстовом поле
   static member ПолучитьТекстТекстовогоПоля(названиеЭлемента) =
      let элементУправления = элементыУправления.[названиеЭлемента] :?> TextBox
      элементУправления.Text
   static member КнопкаКликнута with set (callback:Callback) = наКлик <- callback
   static member ПоследняяКликнутаяКнопка with get() = последняяНажатаяКнопка