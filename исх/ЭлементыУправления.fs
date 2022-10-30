﻿namespace Библиотека

open System
open System.Collections.Generic
open Avalonia.Controls

[<Sealed>]
type ЭлементыУправления private () =
   static let mutable наКлик = Callback(ignore)
   static let mutable последняяНажатаяКнопка = ""
   static let элементыУправления = Dictionary<string, Control>()
   static let позиции = Dictionary<string,int * int>()
   /// Добавляет кнопку к графическому окну
   static member ДобавитьКнопку(метка:string, x:int, y:int) =
      Мое.Приложение.ВызватьСРезультатом(fun () ->
         let имя = "Button" + Guid.NewGuid().ToString()
         let кнопка = new Button(Content = метка)
         кнопка.Click.Add(fun _ -> последняяНажатаяКнопка <- имя; наКлик.Invoke())
         элементыУправления.Add(имя,кнопка)
         позиции.Add(имя,(x,y))
         Мое.Приложение.Холст.AddChild(кнопка,float x,float y)
         имя
      )
   /// Добавляет текстовое поле ввода к графическому окну
   static member ДобавитьТекстовоеПоле(x,y) =
      Мое.Приложение.ВызватьСРезультатом(fun () ->
         let название = "TextBox" + Guid.NewGuid().ToString()
         let элементУправления = new TextBox()
         элементУправления.Text <- "Boo"
         элементыУправления.Add(название,элементУправления)
         позиции.Add(название,(x,y))
         Мое.Приложение.Холст.AddChild(элементУправления,float x,float y)
         название
      )
   /// Устанавливает размер элемента управления
   static member УстановитьРазмер(названиеЭлемента, ширина:int, высота:int) =
      let элементУправления = элементыУправления.[названиеЭлемента]
      let x, y = позиции.[названиеЭлемента]
      Мое.Приложение.Вызвать( fun () ->
         Мое.Приложение.Холст.RemoveChild(элементУправления)        
         элементУправления.Width <- float ширина
         элементУправления.Height <- float высота
         элементУправления.MinWidth <- float ширина
         элементУправления.MinHeight <- float высота
         Мое.Приложение.Холст.AddChild(элементУправления,float x,float y)
      )
   /// Устанавливает текст указанного текстового поля
   static member УстановитьТекстТекстовогоПоля(названиеЭлемента, текст) = 
      let элементУправления = элементыУправления.[названиеЭлемента] :?> TextBox
      Мое.Приложение.Вызвать( fun () ->
         элементУправления.Text <- текст
         элементУправления.Focus()
      )
   /// Получает текущий текст в указанном текстовом поле
   static member ПолучитьТекстТекстовогоПоля(названиеЭлемента) =
      let элементУправления = элементыУправления.[названиеЭлемента] :?> TextBox
      элементУправления.Text
   static member КнопкаКликнута with set (callback:Callback) = наКлик <- callback
   static member ПоследняяКликнутаяКнопка with get() = последняяНажатаяКнопка