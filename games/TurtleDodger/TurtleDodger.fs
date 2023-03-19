#if INTERACTIVE
#r "./bin/debug/Xwt.dll"
#r "./bin/debug/FunSharp.dll"
#endif

open Кітапхана

// Turtle Dodger 0.5b
// Copyright (c) 2014 Nonki Takahashi.
//
// License:
// The MIT License (MIT)
// http://opensource.org/licenses/mit-license.php
//
// History:
// 0.5b 2014-04-17 Changed to detect collision. (QZN342-3)
// 0.4a 2014-04-17 Added opening. (QZN342-2)
// 0.3a 2014-04-02 Avoided to hold while Turtle moving. (QZN342-1)
// 0.2a 2014-04-02 Changed for Silverlight. (QZN342-0)
// 0.1a 2014-04-02 Created. (QZN342)

let title = "Turtle Dodger 0.5b"
ГрафикалықТерезе.Title <- title
// Debug variables
let debug = false
let mutable cross1 = "<shape name>"
let mutable cross2 = "<shape name>"
let mutable pos = "<shape name>"
// Global variables
let gw = 598
let gh = 428
let mutable scrolling = false
let mutable moving = false
let mutable collisionDetected = false
let mutable passed = 0
let mutable lastKey = ""
let mutable lastems = 0
let color = dict [1,Түстер.Orange; 2, Түстер.Cyan; 3, Түстер.Lime]
let size = dict [1,20; 2,16; 3,12]
let mutable score = "<shape name>"
let mutable iMin = 0
let mutable iMax = 0
/// Falling object
type Object = { mutable X:int; mutable Y:int; Kind:int; ShapeName:string }
/// Falling objects
let objects = ResizeArray<Object>()

let rec Closing () =
   Timer.Pause()
   Тасбақа.Бұру(720)
   ГрафикалықТерезе.ҚылқаламТүсі <- Түстер.Ақ
   ГрафикалықТерезе.FontName <- "Trebuchet MS"
   ГрафикалықТерезе.FontSize <- 40.0
   let x = (gw - 217) / 2
   let y = 100
   ГрафикалықТерезе.DrawText(x, y, "GAME OVER")
   Program.Delay(3000)
and Opening () =
   let url = "" // "http://www.nonkit.com/smallbasic.files/"
   let bigTurtle = Пішіндері.AddImage(url + "Turtle.png")
   Пішіндері.Move(bigTurtle, 180, 140)
   ГрафикалықТерезе.ҚылқаламТүсі <- Түстер.Ақ
   ГрафикалықТерезе.FontName <- "Trebuchet MS"
   ГрафикалықТерезе.FontSize <- 50.0
   let x = (gw - 443) / 2
   let y = 40
   ГрафикалықТерезе.DrawText(x, y, title)
   Program.Delay(3000)
   ГрафикалықТерезе.Clear()
and Ready () =
   ГрафикалықТерезе.FontSize <- 40.0
   let ready = Пішіндері.AddText("Ready?")
   let x = (gw - 130) / 2
   let y = 100
   Пішіндері.Move(ready, x, y)
   for opacity in 100 .. -10 .. 0 do
     Пішіндері.SetOpacity(ready, opacity)
     Program.Delay(200)
   Пішіндері.Remove(ready)
and Game () =
   Тасбақа.Speed <- 7
   Тасбақа.PenUp()
   let x = gw / 2
   let y = gh - 40
   ГрафикалықТерезе.ҚылқаламТүсі <- Түстер.Ақ
   ГрафикалықТерезе.FontSize <- 18.0
   score <- Пішіндері.AddText("0")
   Пішіндері.Move(score, 20, 20)
   if debug then
     ГрафикалықТерезе.ҚылқаламТүсі <- Түстер.Ақ
     ГрафикалықТерезе.FontSize <- 12.0
     pos <- Пішіндері.AddText("(" + x.ToString() + "," + y.ToString() + ")")
     ГрафикалықТерезе.PenWidth <- 1.0
     cross1 <- Пішіндері.AddLine(0, -8, 0, 8)
     cross2 <- Пішіндері.AddLine(-8, 0, 8, 0)
     Пішіндері.Move(cross1, x, y)
     Пішіндері.Move(cross2, x, y)
     Пішіндері.Move(pos, gw - 100, 20)   
   Тасбақа.MoveTo(x, y)
   Тасбақа.Angle <- 0.0   
   moving <- false
   scrolling <- false
   Ready()
   ГрафикалықТерезе.KeyDown <- Callback(OnKeyDown)
   let tick = false
   Timer.Interval <- 1000 / 24
   Timer.Tick <- Callback(OnTick)
   lastems <- Clock.ElapsedMilliseconds
   iMin <- 0
   while not collisionDetected do
     if moving then
       if lastKey = "Left" then
         Тасбақа.СолғаБұру()
         Тасбақа.Жылжытуға(30)
         Тасбақа.ОңғаБұру()
       elif lastKey = "Right" then
         Тасбақа.ОңғаБұру()
         Тасбақа.Жылжытуға(30)
         Тасбақа.СолғаБұру()      
       moving <- false
     else
       Program.Delay(100)      
and Init () =
   ГрафикалықТерезе.ФонныңТүсі <- Түстер.DodgerBlue
   ГрафикалықТерезе.Ен <- gw
   ГрафикалықТерезе.Биіктік <- gh   
   passed <- 0
   collisionDetected <- false
and OnTick () =
   if not scrolling then
     scrolling <- true
     let ems = Clock.ElapsedMilliseconds
     if ems - lastems > 500 then
       AddObject()
       lastems <- ems    
     ScrollObject()
     scrolling <- false
   if debug then
     let x = Математика.Floor(Тасбақа.X)
     let y = Математика.Floor(Тасбақа.Y)
     Пішіндері.SetText(pos, "(" + x.ToString() + "," + y.ToString() + ")")
     Пішіндері.Move(cross1, x, y)
     Пішіндері.Move(cross2, x, y)
and ScrollObject () =
   for i = iMin to iMax-1 do
     let x = objects.[i].X
     let y = objects.[i].Y + 5
     let tx = Математика.Floor(Тасбақа.X) |> int
     let ty = Математика.Floor(Тасбақа.Y) |> int
     let d = Математика.SquareRoot(float (Математика.Power(tx - x, 2) + Математика.Power(ty - y, 2))) |> int
     if d < (size.[objects.[i].Kind] + 16) / 2 then
       collisionDetected <- true   
     if y > gh then
       passed <- passed + 1
       Пішіндері.SetText(score, string passed)
       Пішіндері.Remove(objects.[i].ShapeName)
       iMin <- i + 1
     else
       Пішіндері.Move(objects.[i].ShapeName, x, y)
       objects.[i].X <- x
       objects.[i].Y <- y   
and AddObject () =   
   iMax <- iMax + 1
   ГрафикалықТерезе.PenWidth <- 1.0
   let kind = Математика.АлуКездейсоқСаны(3)
   ГрафикалықТерезе.ҚылқаламТүсі <- color.[kind]
   let sz = size.[kind]
   let shapeName = Пішіндері.AddRectangle(sz, sz)
   let x = Математика.АлуКездейсоқСаны(gw - 20) + 10
   let y = -20
   objects.Add({ X = x; Y=y; Kind=kind; ShapeName=shapeName})
   Пішіндері.Move(shapeName, x, y)
   Пішіндері.Rotate(shapeName, Математика.АлуКездейсоқСаны(360))
and OnKeyDown () =
   if not moving then
     moving <- true
     lastKey <- ГрафикалықТерезе.LastKey   

Init()
Opening()
Game()
Closing()