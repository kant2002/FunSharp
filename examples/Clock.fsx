﻿#r "nuget: Avalonia.Desktop, 11.0.0-preview3"
#r "nuget: Avalonia.Themes.Fluent, 11.0.0-preview3"
#r "../src/bin/Debug/net7.0/FunSharp.Library.dll"

open System
open System.Collections.Generic
open Кітапхана

let GW = float GraphicsWindow.Ен
let GH = float GraphicsWindow.Биіктік
let Радиус = 200.0
let MidX = GW/2.0
let MidY = GW/2.0

let initWindow () =
   GraphicsWindow.Show()
   GraphicsWindow.Title <- "Analog Clock"
   GraphicsWindow.ФонныңТүсі <- Түстер.Қара
   GraphicsWindow.ҚылқаламТүсі <- Түстер.BurlyWood
   GraphicsWindow.DrawEllipse(MidX-Радиус-15.,MidY-Радиус-5.,Радиус*2.+30.,Радиус*2.+20.)
   GraphicsWindow.ТолтыруЭллипс(MidX-Радиус-15.,MidY-Радиус-5.,Радиус*2.+30.,Радиус*2.+20.)
   for angle in 1.0..180.0 do
     let x = MidX+(Радиус+15.)*Математика.Cos(Математика.АлуРадианы(angle))
     let y1 = MidY+Радиус*Математика.Sin(Математика.АлуРадианы(angle))+15.
     let y2 = MidY+(Радиус+15.)*Математика.Sin(Математика.АлуРадианы(-angle))+10.
     let blue = Математика.АлуКездейсоқСаны(40)+30
     GraphicsWindow.PenWidth <- Математика.АлуКездейсоқСаны(5) |> float
     let color = 
       GraphicsWindow.GetColorFromRGB(
         blue+100+Математика.АлуКездейсоқСаны(10),
         blue+60+Математика.АлуКездейсоқСаны(20),
         blue)
     GraphicsWindow.ҚаламТүсі <- color
     Shapes.AddLine(x,y1,x,y2) |> ignore
   GraphicsWindow.ҚылқаламТүсі <- Түстер.Ақ   
   let ClockNum = Dictionary()
   for i in 1. .. 12. do
     let Radians = Математика.АлуРадианы(-i * 30. + 90.)
     ClockNum.[i] <- Shapes.AddText(i.ToString())
     Shapes.Move(ClockNum.[i],MidX-4.+Радиус*Математика.Cos(Radians),MidY-4.-Радиус*Математика.Sin(Radians))   
   
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
     GraphicsWindow.ҚаламТүсі <- Түстер.Қара
     GraphicsWindow.PenWidth <- 3.
     HourHand <- 
       Shapes.AddLine(
         MidX,
         MidY,
         MidX+Радиус/2.*Математика.Cos(Математика.АлуРадианы(Hour*30.-90.)),
         MidY+Радиус/2.*Математика.Sin(Математика.АлуРадианы(Hour*30.-90.)))   
   if float Clock.Minute <> Minute then
     Shapes.Remove(MinuteHand)
     Minute <- float Clock.Minute + float Clock.Second/60.
     GraphicsWindow.ҚаламТүсі <- Түстер.Blue
     GraphicsWindow.PenWidth <- 2.
     MinuteHand <- 
       Shapes.AddLine(
         MidX,
         MidY,
         MidX+Радиус/1.2*Математика.Cos(Математика.АлуРадианы(Minute*6.-90.)),
         MidY+Радиус/1.2*Математика.Sin(Математика.АлуРадианы(Minute*6.-90.)))   
   if float Clock.Second <> Second then
     Shapes.Remove(SecondHand)
     Second <- float Clock.Second
     GraphicsWindow.ҚаламТүсі <- Түстер.Red
     GraphicsWindow.PenWidth <- 1.
     SecondHand <- 
       Shapes.AddLine(
         MidX,
         MidY,
         MidX+Радиус*Математика.Cos(Математика.АлуРадианы(Second*6.-90.)),
         MidY+Радиус*Математика.Sin(Математика.АлуРадианы(Second*6.-90.)))
   
initWindow()
while true do
   setHands()
   //Sound.PlayClick()
   Program.Delay(1000)