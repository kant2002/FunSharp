﻿#r "nuget: Avalonia.Desktop, 11.0.0-preview3"
#r "nuget: Avalonia.Themes.Fluent, 11.0.0-preview3"
#r "../исх/bin/Debug/net7.0/ВеселШарп.Библиотека.dll"

open Библиотека

ГрафическоеОкно.ЦветПера <- Цвета.Purple
Черепаха.X <- 150.
Черепаха.Y <- 150.
for i in 0..5..200 do
   Черепаха.Переместить(i)
   Черепаха.Повернуть(90)

Программа.Задержка(2_000)