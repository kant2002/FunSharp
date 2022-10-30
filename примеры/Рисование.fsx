﻿#r "nuget: Avalonia.Desktop, 11.0.0-preview3"
#r "nuget: Avalonia.Themes.Fluent, 11.0.0-preview3"
#r "../исх/bin/Debug/net7.0/ВеселШарп.Библиотека.dll"

open Библиотека

let наКнопкаНажата () =
   match ГрафическоеОкно.ПоследняяКнопка with
   | "F1" -> ГрафическоеОкно.ЦветПера <- Цвета.Red
   | "F2" -> ГрафическоеОкно.ЦветПера <- Цвета.Blue
   | "F3" -> ГрафическоеОкно.ЦветПера <- Цвета.LightGreen
   | "C" -> ГрафическоеОкно.Очистить()
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

ГрафическоеОкно.ЦветПера <- Цвета.Black
ГрафическоеОкно.ЦветФона <- Цвета.White
ГрафическоеОкно.МышьНажата <- Callback(наМышьНажата)
ГрафическоеОкно.МышьПеремещена <- Callback(наМышьПеремещена)
ГрафическоеОкно.КнопкаНажата <- Callback(наКнопкаНажата)
Программа.Задержка(20_000)