﻿пространство Библиотека

открыть System
открыть System.Collections.Generic
открыть Avalonia.Controls

[<Sealed>]
тип ЭлементыУправления частный () =
   статический пусть изменяемый наКлик = Callback(ignore)
   статический пусть изменяемый последняяНажатаяКнопка = ""
   статический пусть элементыУправления = Dictionary<string, Control>()
   статический пусть позиции = Dictionary<string,int * int>()
   /// Добавляет кнопку к графическому окну
   статический член ДобавитьКнопку(метка:string, x:int, y:int) =
      Мое.Приложение.ВызватьСРезультатом(фун () ->
         пусть имя = "Button" + Guid.NewGuid().ToString()
         пусть кнопка = новый Button(Content = метка)
         кнопка.Click.Add(фун _ -> последняяНажатаяКнопка <- имя; наКлик.Invoke())
         элементыУправления.Add(имя,кнопка)
         позиции.Add(имя,(x,y))
         Мое.Приложение.Холст.AddChild(кнопка,float x,float y)
         имя
      )
   /// Добавляет текстовое поле ввода к графическому окну
   статический член ДобавитьТекстовоеПоле(x,y) =
      Мое.Приложение.ВызватьСРезультатом(фун () ->
         пусть название = "TextBox" + Guid.NewGuid().ToString()
         пусть элементУправления = новый TextBox()
         элементУправления.Text <- "Boo"
         элементыУправления.Add(название,элементУправления)
         позиции.Add(название,(x,y))
         Мое.Приложение.Холст.AddChild(элементУправления,float x,float y)
         название
      )
   /// Устанавливает размер элемента управления
   статический член УстановитьРазмер(названиеЭлемента, ширина:int, высота:int) =
      пусть элементУправления = элементыУправления.[названиеЭлемента]
      пусть x, y = позиции.[названиеЭлемента]
      Мое.Приложение.Вызвать( фун () ->
         Мое.Приложение.Холст.RemoveChild(элементУправления)        
         элементУправления.Width <- float ширина
         элементУправления.Height <- float высота
         элементУправления.MinWidth <- float ширина
         элементУправления.MinHeight <- float высота
         Мое.Приложение.Холст.AddChild(элементУправления,float x,float y)
      )
   /// Устанавливает текст указанного текстового поля
   статический член УстановитьТекстТекстовогоПоля(названиеЭлемента, текст) = 
      пусть элементУправления = элементыУправления.[названиеЭлемента] :?> TextBox
      Мое.Приложение.Вызвать( фун () ->
         элементУправления.Text <- текст
         элементУправления.Focus() |> ignore
      )
   /// Получает текущий текст в указанном текстовом поле
   статический член ПолучитьТекстТекстовогоПоля(названиеЭлемента) =
      пусть элементУправления = элементыУправления.[названиеЭлемента] :?> TextBox
      элементУправления.Text
   статический член КнопкаКликнута с set (callback:Callback) = наКлик <- callback
   статический член ПоследняяКликнутаяКнопка с get() = последняяНажатаяКнопка