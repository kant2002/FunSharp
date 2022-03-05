#r "nuget: Xwt"
#r "../src/bin/Debug/net48/FunSharp.Library.dll"

open Библиотека
open System.Threading

let onKeyDown () =
   match ГрафическоеОкно.LastKey with
   | "K1" -> ГрафическоеОкно.ЦветПера <- Цвета.Red
   | "K2" -> ГрафическоеОкно.ЦветПера <- Цвета.Blue
   | "K3" -> ГрафическоеОкно.ЦветПера <- Цвета.LightGreen
   | "c" -> ГрафическоеОкно.Очистить()
   | s -> printfn "'%s'" s; System.Diagnostics.Debug.WriteLine(s)

let mutable prevX = 0.0
let mutable prevY = 0.0

let onMouseDown () =
   prevX <- ГрафическоеОкно.MouseX
   prevY <- ГрафическоеОкно.MouseY
   
let onMouseMove () =
   let x = ГрафическоеОкно.MouseX
   let y = ГрафическоеОкно.MouseY
   if Mouse.IsLeftButtonDown then
      ГрафическоеОкно.НарисоватьЛинию(prevX, prevY, x, y)
   prevX <- x
   prevY <- y

ГрафическоеОкно.ЦветФона <- Цвета.Black
ГрафическоеОкно.ЦветПера <- Цвета.White
ГрафическоеОкно.MouseDown <- Callback(onMouseDown)
ГрафическоеОкно.MouseMove <- Callback(onMouseMove)
ГрафическоеОкно.KeyDown <- Callback(onKeyDown)
Thread.Sleep 2_000