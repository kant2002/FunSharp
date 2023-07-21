﻿#r "nuget: Avalonia.Desktop, 11.0.0"
#r "nuget: Avalonia.Themes.Fluent, 11.0.0"
#r "../ісх/bin/Debug/net7.0/ВеселШарп.Бібліотека.dll"

open Бібліотека

let кут = 30.0
let дельта = 10.0

let rec намалюватиДерево відстань =
   if відстань > 0.0 then
      Черепаха.Перемістити(відстань)
      Черепаха.Повернути(кут)
      намалюватиДерево(відстань - дельта)
      Черепаха.Повернути(-кут * 2.0)
      намалюватиДерево(відстань - дельта)
      Черепаха.Повернути(кут)
      Черепаха.Перемістити(-відстань)

намалюватиДерево 60.0
Програма.Затримка(2_000)