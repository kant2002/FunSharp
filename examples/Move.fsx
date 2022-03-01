#r "nuget: Xwt"
#r "../src/bin/Debug/net48/FunSharp.Library.dll"

open Library
open System.Threading

let ball = Shapes.AddRectangle(200.0, 100.0)

let OnMouseDown () =
  let x = GraphicsWindow.MouseX
  let y = GraphicsWindow.MouseY
  Shapes.Move(ball, x, y)

GraphicsWindow.MouseDown <- Callback(OnMouseDown)
Thread.Sleep 2_000
