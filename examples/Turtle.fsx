﻿#r "nuget: Xwt"
#r "../src/bin/Debug/net48/FunSharp.Library.dll"

open Библиотека
open System.Threading

ГрафическоеОкно.PenColor <- Цвета.Purple
Turtle.X <- 150.
Turtle.Y <- 150.
for i in 0..5..200 do
   Turtle.Move(i)
   Turtle.Turn(90)

Thread.Sleep 2_000