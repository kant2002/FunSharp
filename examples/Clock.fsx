﻿//#I "../lib"
#r "nuget: Xwt"
#r "../src/bin/Debug/net48/FunSharp.Library.dll"
//#r "../../../xwt/Xwt.GtkSharp/bin/Debug/netstandard2.0/Xwt.GtkSharp.dll"

open System
open System.Collections.Generic
open Библиотека

let GW = float ГрафическоеОкно.Ширина
let GH = float ГрафическоеОкно.Высота
let Radius = 200.0
let MidX = GW/2.0
let MidY = GW/2.0

let инициализироватьОкно () =
   ГрафическоеОкно.Показать()
   ГрафическоеОкно.Заголовок <- "Analog Clock"
   ГрафическоеОкно.ЦветФона <- Цвета.Black
   ГрафическоеОкно.ЦветКисти <- Цвета.BurlyWood
   ГрафическоеОкно.DrawEllipse(MidX-Radius-15.,MidY-Radius-5.,Radius*2.+30.,Radius*2.+20.)
   ГрафическоеОкно.ЗаполнитьЭллипс(MidX-Radius-15.,MidY-Radius-5.,Radius*2.+30.,Radius*2.+20.)
   for angle in 1.0..180.0 do
     let x = MidX+(Radius+15.)*Math.Cos(Math.GetRadians(angle))
     let y1 = MidY+Radius*Math.Sin(Math.GetRadians(angle))+15.
     let y2 = MidY+(Radius+15.)*Math.Sin(Math.GetRadians(-angle))+10.
     let blue = Math.GetRandomNumber(40)+30
     ГрафическоеОкно.PenWidth <- Math.GetRandomNumber(5) |> float
     let color = 
       ГрафическоеОкно.GetColorFromRGB(
         blue+100+Math.GetRandomNumber(10),
         blue+60+Math.GetRandomNumber(20),
         blue)
     ГрафическоеОкно.ЦветПера <- color
     Фигуры.ДобавитьЛинию(x,y1,x,y2) |> ignore
   ГрафическоеОкно.ЦветКисти <- Цвета.White   
   let ClockNum = Dictionary()
   for i in 1. .. 12. do
     let Radians = Math.GetRadians(-i * 30. + 90.)
     ClockNum.[i] <- Фигуры.ДобавитьТекст(i.ToString())
     Фигуры.Переместить(ClockNum.[i],MidX-4.+Radius*Math.Cos(Radians),MidY-4.-Radius*Math.Sin(Radians))   
   
let mutable HourHand = "<shape name>"
let mutable MinuteHand = "<shape name>"
let mutable SecondHand = "<shape name>"
let mutable Hour = 0.
let mutable Minute = 0.
let mutable Second = 0.
let установитьСтрелки () = 
   if (float Часы.Час + float Часы.Минута/60. + float Часы.Секунда/3600. <> Hour) then
     Фигуры.Удалить(HourHand)
     Hour <- float Часы.Час + float Часы.Минута/60. + float Часы.Секунда/3600.
     ГрафическоеОкно.ЦветПера <- Цвета.Black
     ГрафическоеОкно.PenWidth <- 3.
     HourHand <- 
       Фигуры.ДобавитьЛинию(
         MidX,
         MidY,
         MidX+Radius/2.*Math.Cos(Math.GetRadians(Hour*30.-90.)),
         MidY+Radius/2.*Math.Sin(Math.GetRadians(Hour*30.-90.)))   
   if float Часы.Минута <> Minute then
     Фигуры.Удалить(MinuteHand)
     Minute <- float Часы.Минута + float Часы.Секунда/60.
     ГрафическоеОкно.ЦветПера <- Цвета.Blue
     ГрафическоеОкно.PenWidth <- 2.
     MinuteHand <- 
       Фигуры.ДобавитьЛинию(
         MidX,
         MidY,
         MidX+Radius/1.2*Math.Cos(Math.GetRadians(Minute*6.-90.)),
         MidY+Radius/1.2*Math.Sin(Math.GetRadians(Minute*6.-90.)))   
   if float Часы.Секунда <> Second then
     Фигуры.Удалить(SecondHand)
     Second <- float Часы.Секунда
     ГрафическоеОкно.ЦветПера <- Цвета.Red
     ГрафическоеОкно.PenWidth <- 1.
     SecondHand <- 
       Фигуры.ДобавитьЛинию(
         MidX,
         MidY,
         MidX+Radius*Math.Cos(Math.GetRadians(Second*6.-90.)),
         MidY+Radius*Math.Sin(Math.GetRadians(Second*6.-90.)))
   
инициализироватьОкно()
while true do
   установитьСтрелки()
   //Sound.PlayClick()
   Program.Delay(1000)