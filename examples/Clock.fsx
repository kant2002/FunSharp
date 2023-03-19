#r "nuget: Avalonia.Desktop, 11.0.0-preview3"
#r "nuget: Avalonia.Themes.Fluent, 11.0.0-preview3"
#r "../src/bin/Debug/net7.0/FunSharp.Library.dll"

open System
open System.Collections.Generic
open Кітапхана

let GW = float ГрафикалықТерезе.Ен
let GH = float ГрафикалықТерезе.Биіктік
let Радиус = 200.0
let MidX = GW/2.0
let MidY = GW/2.0

let initWindow () =
   ГрафикалықТерезе.Show()
   ГрафикалықТерезе.Title <- "Analog Clock"
   ГрафикалықТерезе.ФонныңТүсі <- Түстер.Қара
   ГрафикалықТерезе.ҚылқаламТүсі <- Түстер.BurlyWood
   ГрафикалықТерезе.DrawEllipse(MidX-Радиус-15.,MidY-Радиус-5.,Радиус*2.+30.,Радиус*2.+20.)
   ГрафикалықТерезе.ТолтыруЭллипс(MidX-Радиус-15.,MidY-Радиус-5.,Радиус*2.+30.,Радиус*2.+20.)
   for angle in 1.0..180.0 do
     let x = MidX+(Радиус+15.)*Математика.Cos(Математика.АлуРадианы(angle))
     let y1 = MidY+Радиус*Математика.Sin(Математика.АлуРадианы(angle))+15.
     let y2 = MidY+(Радиус+15.)*Математика.Sin(Математика.АлуРадианы(-angle))+10.
     let blue = Математика.АлуКездейсоқСаны(40)+30
     ГрафикалықТерезе.PenWidth <- Математика.АлуКездейсоқСаны(5) |> float
     let color = 
       ГрафикалықТерезе.GetColorFromRGB(
         blue+100+Математика.АлуКездейсоқСаны(10),
         blue+60+Математика.АлуКездейсоқСаны(20),
         blue)
     ГрафикалықТерезе.ҚаламТүсі <- color
     Пішіндері.AddLine(x,y1,x,y2) |> ignore
   ГрафикалықТерезе.ҚылқаламТүсі <- Түстер.Ақ   
   let ClockNum = Dictionary()
   for i in 1. .. 12. do
     let Radians = Математика.АлуРадианы(-i * 30. + 90.)
     ClockNum.[i] <- Пішіндері.AddText(i.ToString())
     Пішіндері.Жылжытуға(ClockNum.[i],MidX-4.+Радиус*Математика.Cos(Radians),MidY-4.-Радиус*Математика.Sin(Radians))   
   
let mutable HourHand = "<shape name>"
let mutable MinuteHand = "<shape name>"
let mutable SecondHand = "<shape name>"
let mutable Hour = 0.
let mutable Minute = 0.
let mutable Second = 0.
let setHands () = 
   if (float Clock.Hour + float Clock.Minute/60. + float Clock.Second/3600. <> Hour) then
     Пішіндері.Remove(HourHand)
     Hour <- float Clock.Hour + float Clock.Minute/60. + float Clock.Second/3600.
     ГрафикалықТерезе.ҚаламТүсі <- Түстер.Қара
     ГрафикалықТерезе.PenWidth <- 3.
     HourHand <- 
       Пішіндері.AddLine(
         MidX,
         MidY,
         MidX+Радиус/2.*Математика.Cos(Математика.АлуРадианы(Hour*30.-90.)),
         MidY+Радиус/2.*Математика.Sin(Математика.АлуРадианы(Hour*30.-90.)))   
   if float Clock.Minute <> Minute then
     Пішіндері.Remove(MinuteHand)
     Minute <- float Clock.Minute + float Clock.Second/60.
     ГрафикалықТерезе.ҚаламТүсі <- Түстер.Blue
     ГрафикалықТерезе.PenWidth <- 2.
     MinuteHand <- 
       Пішіндері.AddLine(
         MidX,
         MidY,
         MidX+Радиус/1.2*Математика.Cos(Математика.АлуРадианы(Minute*6.-90.)),
         MidY+Радиус/1.2*Математика.Sin(Математика.АлуРадианы(Minute*6.-90.)))   
   if float Clock.Second <> Second then
     Пішіндері.Remove(SecondHand)
     Second <- float Clock.Second
     ГрафикалықТерезе.ҚаламТүсі <- Түстер.Red
     ГрафикалықТерезе.PenWidth <- 1.
     SecondHand <- 
       Пішіндері.AddLine(
         MidX,
         MidY,
         MidX+Радиус*Математика.Cos(Математика.АлуРадианы(Second*6.-90.)),
         MidY+Радиус*Математика.Sin(Математика.АлуРадианы(Second*6.-90.)))
   
initWindow()
while true do
   setHands()
   //Sound.PlayClick()
   Program.Delay(1000)