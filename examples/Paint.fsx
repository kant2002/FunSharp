#r "nuget: Xwt"
#r "../src/bin/Debug/net48/FunSharp.Library.dll"

open Library
open System.Threading

let onKeyDown () =
   match ГрафическоеОкно.LastKey with
   | "K1" -> ГрафическоеОкно.PenColor <- Colors.Red
   | "K2" -> ГрафическоеОкно.PenColor <- Colors.Blue
   | "K3" -> ГрафическоеОкно.PenColor <- Colors.LightGreen
   | "c" -> ГрафическоеОкно.Clear()
   | s -> printfn "'%s'" s; System.Diagnostics.Debug.WriteLine(s)

let mutable prevX = 0.0
let mutable prevY = 0.0

let onMouseDown () =
   prevX <- ГрафическоеОкно.MouseX
   prevY <- ГрафическоеОкно.MouseY
   
let onMouseMove () =
   let x = ГрафическоеОкно.MouseX
   let y = ГрафическоеОкно.MouseY
   if Mouse.IsLeftButtonDown then
      ГрафическоеОкно.DrawLine(prevX, prevY, x, y)
   prevX <- x
   prevY <- y

ГрафическоеОкно.BackgroundColor <- Colors.Black
ГрафическоеОкно.PenColor <- Colors.White
ГрафическоеОкно.MouseDown <- Callback(onMouseDown)
ГрафическоеОкно.MouseMove <- Callback(onMouseMove)
ГрафическоеОкно.KeyDown <- Callback(onKeyDown)
Thread.Sleep 2_000