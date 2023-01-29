namespace Library

open System
open Avalonia.Controls

[<Sealed>]
type Controls private () =
   static let mutable onClick = Callback(ignore)
   static let mutable lastClickedButton = ""
   static let controls = System.Collections.Generic.Dictionary<string, Control>()
   static let positions = System.Collections.Generic.Dictionary<string,int * int>()
   /// Adds a button to the graphics window
   static member AddButton(label:string, x:int, y:int) =
      My.App.InvokeWithResult( fun () ->      
          let name = "Button" + Guid.NewGuid().ToString()
          let button = new Button(Content = label)
          button.Click.Add(fun _ -> lastClickedButton <- name; onClick.Invoke())
          //toXwtColor(GraphicsWindow.ҚылқаламТүсі)      
          controls.Add(name,button)
          positions.Add(name,(x,y))            
          My.App.Canvas.AddChild(button,float x,float y)
          name
      )
   /// Adds a text input box to the graphics window
   static member AddTextBox(x,y) =
      My.App.InvokeWithResult( fun () ->      
        let name = "TextBox" + Guid.NewGuid().ToString()
        let control = new TextBox()
        control.Text <- "Boo"
        // toXwtColor(GraphicsWindow.ҚылқаламТүсі)
        controls.Add(name,control)
        positions.Add(name,(x,y))
        My.App.Canvas.AddChild(control,float x,float y)
        name
      )
   /// Sets the size of the control
   static member SetSize(controlName, width:int, height:int) =
      let control = controls.[controlName]
      let x, y = positions.[controlName]
      My.App.Invoke( fun () ->
         My.App.Canvas.RemoveChild(control)        
         control.Width <- float width
         control.Height <- float height
         control.MinWidth <- float width
         control.MinHeight <- float height
         My.App.Canvas.AddChild(control,float x,float y)
      )
   /// Sets the text of the specified text box
   static member SetTextBoxText(controlName, text) = 
      let control = controls.[controlName] :?> TextBox
      My.App.Invoke( fun () ->
         control.Text <- text
         control.Focus()
      )
   /// Gets the current text of the specified text box
   static member GetTextBoxText(controlName) =
      let control = controls.[controlName] :?> TextBox
      control.Text
   static member ButtonClicked with set (callback:Callback) = onClick <- callback
   static member LastClickedButton with get() = lastClickedButton