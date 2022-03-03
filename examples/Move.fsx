#r "nuget: Xwt"
#r "../src/bin/Debug/net48/FunSharp.Library.dll"

open Библиотека
open System.Threading

let ball = Shapes.AddRectangle(200.0, 100.0)

let OnMouseDown () =
  let x = ГрафическоеОкно.MouseX
  let y = ГрафическоеОкно.MouseY
  Shapes.Move(ball, x, y)

ГрафическоеОкно.MouseDown <- Callback(OnMouseDown)
Thread.Sleep 2_000
