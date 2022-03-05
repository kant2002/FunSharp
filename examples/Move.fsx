#r "nuget: Xwt"
#r "../src/bin/Debug/net48/FunSharp.Library.dll"

open Библиотека
open System.Threading

let мяч = Shapes.ДобавитьПрямоугольник(200.0, 100.0)

let OnMouseDown () =
  let x = ГрафическоеОкно.MouseX
  let y = ГрафическоеОкно.MouseY
  Shapes.Переместить(мяч, x, y)

ГрафическоеОкно.MouseDown <- Callback(OnMouseDown)
Thread.Sleep 2_000
