#r "nuget: Xwt"
#r "../src/bin/Debug/net48/FunSharp.Library.dll"

open Библиотека
open System.Threading

let наКнопкаНажата () =
   match ГрафическоеОкно.LastKey with
   | "K1" -> ГрафическоеОкно.ЦветПера <- Цвета.Red
   | "K2" -> ГрафическоеОкно.ЦветПера <- Цвета.Blue
   | "K3" -> ГрафическоеОкно.ЦветПера <- Цвета.LightGreen
   | "c" -> ГрафическоеОкно.Очистить()
   | s -> printfn "'%s'" s; System.Diagnostics.Debug.WriteLine(s)

let mutable прошлX = 0.0
let mutable прошлY = 0.0

let наМышьНажата () =
   прошлX <- ГрафическоеОкно.МышьX
   прошлY <- ГрафическоеОкно.МышьY
   
let наМышьПеремещена () =
   let x = ГрафическоеОкно.МышьX
   let y = ГрафическоеОкно.МышьY
   if Мышь.ЛеваяКнопкаНажата then
      ГрафическоеОкно.НарисоватьЛинию(прошлX, прошлY, x, y)
   прошлX <- x
   прошлY <- y

ГрафическоеОкно.ЦветФона <- Цвета.Black
ГрафическоеОкно.ЦветПера <- Цвета.White
ГрафическоеОкно.MouseDown <- Callback(наМышьНажата)
ГрафическоеОкно.MouseMove <- Callback(наМышьПеремещена)
ГрафическоеОкно.KeyDown <- Callback(наКнопкаНажата)
Thread.Sleep 20_000