﻿#r "nuget: Avalonia.Desktop, 11.0.0-preview3"
#r "nuget: Avalonia.Themes.Fluent, 11.0.0-preview3"
#r "../ісх/bin/Debug/net7.0/ВеселШарп.Бiблiотека.dll"

open Бiблiотека

ГрафичнеВікно.КолірПера <- Кольори.Purple
Черепаха.X <- 150.
Черепаха.Y <- 150.
for i in 0..5..200 do
   Черепаха.Перемістити(i)
   Черепаха.Повернути(90)

Програма.Затримка(2_000)