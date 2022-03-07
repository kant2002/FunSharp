#r "nuget: Xwt"
#r "../src/bin/Debug/net48/FunSharp.Library.dll"

open Библиотека

let мяч = Фигуры.ДобавитьПрямоугольник(200.0, 100.0)

let OnMouseDown () =
  let x = ГрафическоеОкно.MouseX
  let y = ГрафическоеОкно.MouseY
  Фигуры.Переместить(мяч, x, y)

ГрафическоеОкно.MouseDown <- Callback(OnMouseDown)
Программа.Задержка(2_000)
