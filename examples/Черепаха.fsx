#r "nuget: Xwt"
#r "../src/bin/Debug/net48/FunSharp.Library.dll"

open Библиотека
open System.Threading

ГрафическоеОкно.ЦветПера <- Цвета.Purple
Черепаха.X <- 150.
Черепаха.Y <- 150.
for i in 0..5..200 do
   Черепаха.Переместить(i)
   Черепаха.Повернуть(90)

Thread.Sleep 2_000