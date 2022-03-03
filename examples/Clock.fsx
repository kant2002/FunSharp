//#I "../lib"
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

let initWindow () =
   ГрафическоеОкно.Show()
   ГрафическоеОкно.Заголовок <- "Analog Clock"
   ГрафическоеОкно.ФоновыйЦвет <- Цвета.Black
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
     ГрафическоеОкно.PenColor <- color
     Shapes.AddLine(x,y1,x,y2) |> ignore
   ГрафическоеОкно.ЦветКисти <- Цвета.White   
   let ClockNum = Dictionary()
   for i in 1. .. 12. do
     let Radians = Math.GetRadians(-i * 30. + 90.)
     ClockNum.[i] <- Shapes.AddText(i.ToString())
     Shapes.Move(ClockNum.[i],MidX-4.+Radius*Math.Cos(Radians),MidY-4.-Radius*Math.Sin(Radians))   
   
let mutable HourHand = "<shape name>"
let mutable MinuteHand = "<shape name>"
let mutable SecondHand = "<shape name>"
let mutable Hour = 0.
let mutable Minute = 0.
let mutable Second = 0.
let setHands () = 
   if (float Clock.Hour + float Clock.Minute/60. + float Clock.Second/3600. <> Hour) then
     Shapes.Remove(HourHand)
     Hour <- float Clock.Hour + float Clock.Minute/60. + float Clock.Second/3600.
     ГрафическоеОкно.PenColor <- Цвета.Black
     ГрафическоеОкно.PenWidth <- 3.
     HourHand <- 
       Shapes.AddLine(
         MidX,
         MidY,
         MidX+Radius/2.*Math.Cos(Math.GetRadians(Hour*30.-90.)),
         MidY+Radius/2.*Math.Sin(Math.GetRadians(Hour*30.-90.)))   
   if float Clock.Minute <> Minute then
     Shapes.Remove(MinuteHand)
     Minute <- float Clock.Minute + float Clock.Second/60.
     ГрафическоеОкно.PenColor <- Цвета.Blue
     ГрафическоеОкно.PenWidth <- 2.
     MinuteHand <- 
       Shapes.AddLine(
         MidX,
         MidY,
         MidX+Radius/1.2*Math.Cos(Math.GetRadians(Minute*6.-90.)),
         MidY+Radius/1.2*Math.Sin(Math.GetRadians(Minute*6.-90.)))   
   if float Clock.Second <> Second then
     Shapes.Remove(SecondHand)
     Second <- float Clock.Second
     ГрафическоеОкно.PenColor <- Цвета.Red
     ГрафическоеОкно.PenWidth <- 1.
     SecondHand <- 
       Shapes.AddLine(
         MidX,
         MidY,
         MidX+Radius*Math.Cos(Math.GetRadians(Second*6.-90.)),
         MidY+Radius*Math.Sin(Math.GetRadians(Second*6.-90.)))
   
initWindow()
while true do
   setHands()
   //Sound.PlayClick()
   Program.Delay(1000)