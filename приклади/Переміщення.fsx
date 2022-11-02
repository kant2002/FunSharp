﻿#r "nuget: Avalonia.Desktop, 11.0.0-preview3"
#r "nuget: Avalonia.Themes.Fluent, 11.0.0-preview3"
#r "../ісх/bin/Debug/net7.0/ВеселШарп.Бібліотека.dll"

open Бібліотека

let мяч = Фігури.ДодатиПрямокутник(200.0, 100.0)

let ПоКлацаннюМишою () =
  let x = ГрафичнеВікно.МишаX
  let y = ГрафичнеВікно.МишаY
  Фігури.Перемістити(мяч, x, y)

ГрафичнеВікно.МишаНатиснута <- Callback(ПоКлацаннюМишою)
Програма.Затримка(20_000)
