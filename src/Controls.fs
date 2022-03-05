namespace Библиотека

open System
open Xwt

[<Sealed>]
type Controls private () =
   static let mutable наКлик = Callback(ignore)
   static let mutable последняяНажатаяКнопка = ""
   static let controls = System.Collections.Generic.Dictionary<string,Widget>()
   static let позиции = System.Collections.Generic.Dictionary<string,int * int>()
   /// Adds a button to the graphics window
   static member ДобавитьКнопку(label:string, x:int, y:int) =
      let name = "Button" + Guid.NewGuid().ToString()
      let button = new Button(label)
      button.Clicked.Add(fun _ -> последняяНажатаяКнопка <- name; наКлик.Invoke())
      //toXwtColor(GraphicsWindow.BrushColor)      
      controls.Add(name,button)
      позиции.Add(name,(x,y))
      Мое.Приложение.Вызвать( fun () ->                  
         Мое.Приложение.Холст.AddChild(button,float x,float y)
      )
      name
   /// Adds a text input box to the graphics window
   static member ДобавитьТекстовоеПоле(x,y) =
      let название = "TextBox" + Guid.NewGuid().ToString()
      let control = new TextEntry()
      control.Text <- "Boo"
      // toXwtColor(GraphicsWindow.BrushColor)
      controls.Add(название,control)
      позиции.Add(название,(x,y))
      Мое.Приложение.Вызвать( fun () ->                  
         Мое.Приложение.Холст.AddChild(control,float x,float y)
      )
      название
   /// Sets the size of the control
   static member УстановитьРазмер(controlName, ширина:int, высота:int) =
      let control = controls.[controlName]
      let x, y = позиции.[controlName]
      Мое.Приложение.Вызвать( fun () ->
         Мое.Приложение.Холст.RemoveChild(control)        
         control.WidthRequest <- float ширина
         control.HeightRequest <- float высота
         control.MinWidth <- float ширина
         control.MinHeight <- float высота
         Мое.Приложение.Холст.AddChild(control,float x,float y)
      )
   /// Sets the text of the specified text box
   static member УстановитьТекстТекстовогоПоля(controlName, текст) = 
      let control = controls.[controlName] :?> Xwt.TextEntry
      control.Text <- текст
      Мое.Приложение.Вызвать( fun () ->
         control.SetFocus()
         control.QueueForReallocate()
      )
   /// Gets the current text of the specified text box
   static member ПолучитьТекстТекстовогоПоля(controlName) =
      let control = controls.[controlName] :?> Xwt.TextEntry
      control.Text
   static member КнопкаКликнута with set (callback:Callback) = наКлик <- callback
   static member ПоследняяКликнутаяКнопка with get() = последняяНажатаяКнопка