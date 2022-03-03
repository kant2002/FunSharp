#r "nuget: Xwt"
#r "../src/bin/Debug/net48/FunSharp.Library.dll"

open Библиотека
open System.Threading

let angle = 30.0
let delta = 10.0

let rec drawTree distance =
   if distance > 0.0 then
      Turtle.Move(distance)
      Turtle.Turn(angle)
      drawTree(distance - delta)
      Turtle.Turn(-angle * 2.0)
      drawTree(distance - delta)
      Turtle.Turn(angle)
      Turtle.Move(-distance)

drawTree 60.0
Thread.Sleep 2_000