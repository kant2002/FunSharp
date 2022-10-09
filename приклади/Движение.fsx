﻿#r "nuget: Xwt"
#r "nuget: Xwt.GtkSharp"
#r "../ісх/bin/Debug/net48/ВеселШарп.Бiблiотека.dll"

open Бiблiотека

let мяч = Фигуры.ДодатиПрямокутник(200.0, 100.0)

let НаМышьНажата () =
  let x = ГрафичнеВікно.МишаX
  let y = ГрафичнеВікно.МишаY
  Фигуры.Перемістити(мяч, x, y)

ГрафичнеВікно.МышьНажата <- Callback(НаМышьНажата)
Програма.Затримка(20_000)
