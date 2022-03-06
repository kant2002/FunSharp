﻿namespace Библиотека

open System
open System.Collections.Generic
open Xwt

[<Sealed>]
type ЭлементыУправления private () =
   static let mutable наКлик = Callback(ignore)
   static let mutable последняяНажатаяКнопка = ""
   static let элементыУправления = Dictionary<string,Widget>()
   static let позиции = Dictionary<string,int * int>()
   /// Adds a button to the graphics window
   static member ДобавитьКнопку(метка:string, x:int, y:int) =
      let имя = "Button" + Guid.NewGuid().ToString()
      let кнопка = new Button(метка)
      кнопка.Clicked.Add(fun _ -> последняяНажатаяКнопка <- имя; наКлик.Invoke())
      //toXwtColor(GraphicsWindow.BrushColor)      
      элементыУправления.Add(имя,кнопка)
      позиции.Add(имя,(x,y))
      Мое.Приложение.Вызвать( fun () ->                  
         Мое.Приложение.Холст.AddChild(кнопка,float x,float y)
      )
      имя
   /// Adds a text input box to the graphics window
   static member ДобавитьТекстовоеПоле(x,y) =
      let название = "TextBox" + Guid.NewGuid().ToString()
      let элементУправления = new TextEntry()
      элементУправления.Text <- "Boo"
      // toXwtColor(GraphicsWindow.BrushColor)
      элементыУправления.Add(название,элементУправления)
      позиции.Add(название,(x,y))
      Мое.Приложение.Вызвать( fun () ->                  
         Мое.Приложение.Холст.AddChild(элементУправления,float x,float y)
      )
      название
   /// Sets the size of the control
   static member УстановитьРазмер(названиеЭлемента, ширина:int, высота:int) =
      let элементУправления = элементыУправления.[названиеЭлемента]
      let x, y = позиции.[названиеЭлемента]
      Мое.Приложение.Вызвать( fun () ->
         Мое.Приложение.Холст.RemoveChild(элементУправления)        
         элементУправления.WidthRequest <- float ширина
         элементУправления.HeightRequest <- float высота
         элементУправления.MinWidth <- float ширина
         элементУправления.MinHeight <- float высота
         Мое.Приложение.Холст.AddChild(элементУправления,float x,float y)
      )
   /// Sets the text of the specified text box
   static member УстановитьТекстТекстовогоПоля(названиеЭлемента, текст) = 
      let элементУправления = элементыУправления.[названиеЭлемента] :?> Xwt.TextEntry
      элементУправления.Text <- текст
      Мое.Приложение.Вызвать( fun () ->
         элементУправления.SetFocus()
         элементУправления.QueueForReallocate()
      )
   /// Gets the current text of the specified text box
   static member ПолучитьТекстТекстовогоПоля(названиеЭлемента) =
      let элементУправления = элементыУправления.[названиеЭлемента] :?> Xwt.TextEntry
      элементУправления.Text
   static member КнопкаКликнута with set (callback:Callback) = наКлик <- callback
   static member ПоследняяКликнутаяКнопка with get() = последняяНажатаяКнопка