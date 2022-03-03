namespace Library

open System
open Xwt

[<Sealed>]
type Controls private () =
   static let mutable onClick = Callback(ignore)
   static let mutable lastClickedButton = ""
   static let controls = System.Collections.Generic.Dictionary<string,Widget>()
   static let positions = System.Collections.Generic.Dictionary<string,int * int>()
   /// Adds a button to the graphics window
   static member ДобавитьКнопку(label:string, x:int, y:int) =
      let name = "Button" + Guid.NewGuid().ToString()
      let button = new Button(label)
      button.Clicked.Add(fun _ -> lastClickedButton <- name; onClick.Invoke())
      //toXwtColor(GraphicsWindow.BrushColor)      
      controls.Add(name,button)
      positions.Add(name,(x,y))
      Мое.Приложение.Вызвать( fun () ->                  
         Мое.Приложение.Холст.AddChild(button,float x,float y)
      )
      name
   /// Adds a text input box to the graphics window
   static member AddTextBox(x,y) =
      let name = "TextBox" + Guid.NewGuid().ToString()
      let control = new TextEntry()
      control.Text <- "Boo"
      // toXwtColor(GraphicsWindow.BrushColor)
      controls.Add(name,control)
      positions.Add(name,(x,y))
      Мое.Приложение.Вызвать( fun () ->                  
         Мое.Приложение.Холст.AddChild(control,float x,float y)
      )
      name
   /// Sets the size of the control
   static member SetSize(controlName, width:int, height:int) =
      let control = controls.[controlName]
      let x, y = positions.[controlName]
      Мое.Приложение.Вызвать( fun () ->
         Мое.Приложение.Холст.RemoveChild(control)        
         control.WidthRequest <- float width
         control.HeightRequest <- float height
         control.MinWidth <- float width
         control.MinHeight <- float height
         Мое.Приложение.Холст.AddChild(control,float x,float y)
      )
   /// Sets the text of the specified text box
   static member SetTextBoxText(controlName, text) = 
      let control = controls.[controlName] :?> Xwt.TextEntry
      control.Text <- text
      Мое.Приложение.Вызвать( fun () ->
         control.SetFocus()
         control.QueueForReallocate()
      )
   /// Gets the current text of the specified text box
   static member GetTextBoxText(controlName) =
      let control = controls.[controlName] :?> Xwt.TextEntry
      control.Text
   static member ButtonClicked with set (callback:Callback) = onClick <- callback
   static member LastClickedButton with get() = lastClickedButton