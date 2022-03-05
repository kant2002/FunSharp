﻿#if INTERACTIVE
#r "./bin/debug/Xwt.dll"
#r "./bin/debug/FunSharp.dll"
#endif

open Библиотека

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
ГрафическоеОкно.Заголовок <- title
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
let color = dict [1,Цвета.Orange; 2, Цвета.Cyan; 3, Цвета.Lime]
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
   Черепаха.Повернуть(720)
   ГрафическоеОкно.ЦветКисти <- Цвета.White
   ГрафическоеОкно.FontName <- "Trebuchet MS"
   ГрафическоеОкно.FontSize <- 40.0
   let x = (gw - 217) / 2
   let y = 100
   ГрафическоеОкно.НарисоватьТекст(x, y, "GAME OVER")
   Program.Delay(3000)
and Opening () =
   let url = "" // "http://www.nonkit.com/smallbasic.files/"
   let bigTurtle = Фигуры.ДобавитьИзображение(url + "turtle.png")
   Фигуры.Переместить(bigTurtle, 180, 140)
   ГрафическоеОкно.ЦветКисти <- Цвета.White
   ГрафическоеОкно.FontName <- "Trebuchet MS"
   ГрафическоеОкно.FontSize <- 50.0
   let x = (gw - 443) / 2
   let y = 40
   ГрафическоеОкно.НарисоватьТекст(x, y, title)
   Program.Delay(3000)
   ГрафическоеОкно.Очистить()
and Ready () =
   ГрафическоеОкно.FontSize <- 40.0
   let ready = Фигуры.ДобавитьТекст("Ready?")
   let x = (gw - 130) / 2
   let y = 100
   Фигуры.Переместить(ready, x, y)
   for opacity in 100 .. -10 .. 0 do
     Фигуры.SetOpacity(ready, opacity)
     Program.Delay(200)
   Фигуры.Удалить(ready)
and Game () =
   Черепаха.Скорость <- 7
   Черепаха.PenUp()
   let x = gw / 2
   let y = gh - 40
   ГрафическоеОкно.ЦветКисти <- Цвета.White
   ГрафическоеОкно.FontSize <- 18.0
   score <- Фигуры.ДобавитьТекст("0")
   Фигуры.Переместить(score, 20, 20)
   if debug then
     ГрафическоеОкно.ЦветКисти <- Цвета.White
     ГрафическоеОкно.FontSize <- 12.0
     pos <- Фигуры.ДобавитьТекст("(" + x.ToString() + "," + y.ToString() + ")")
     ГрафическоеОкно.PenWidth <- 1.0
     cross1 <- Фигуры.ДобавитьЛинию(0, -8, 0, 8)
     cross2 <- Фигуры.ДобавитьЛинию(-8, 0, 8, 0)
     Фигуры.Переместить(cross1, x, y)
     Фигуры.Переместить(cross2, x, y)
     Фигуры.Переместить(pos, gw - 100, 20)   
   Черепаха.ПереместитьВ(x, y)
   Черепаха.Angle <- 0.0   
   moving <- false
   scrolling <- false
   Ready()
   ГрафическоеОкно.КнопкаНажата <- Callback(OnKeyDown)
   let tick = false
   Timer.Interval <- 1000 / 24
   Timer.Tick <- Callback(OnTick)
   lastems <- Часы.ПрошедшиеМиллисекунды
   iMin <- 0
   while not collisionDetected do
     if moving then
       if lastKey = "Left" then
         Черепаха.ПовернутьНалево()
         Черепаха.Переместить(30)
         Черепаха.ПовернутьНаправо()
       elif lastKey = "Right" then
         Черепаха.ПовернутьНаправо()
         Черепаха.Переместить(30)
         Черепаха.ПовернутьНалево()      
       moving <- false
     else
       Program.Delay(100)      
and Init () =
   ГрафическоеОкно.ЦветФона <- Цвета.DodgerBlue
   ГрафическоеОкно.Ширина <- gw
   ГрафическоеОкно.Высота <- gh   
   passed <- 0
   collisionDetected <- false
and OnTick () =
   if not scrolling then
     scrolling <- true
     let ems = Часы.ПрошедшиеМиллисекунды
     if ems - lastems > 500 then
       AddObject()
       lastems <- ems    
     ScrollObject()
     scrolling <- false
   if debug then
     let x = Math.Floor(Черепаха.X)
     let y = Math.Floor(Черепаха.Y)
     Фигуры.УстановитьТекст(pos, "(" + x.ToString() + "," + y.ToString() + ")")
     Фигуры.Переместить(cross1, x, y)
     Фигуры.Переместить(cross2, x, y)
and ScrollObject () =
   for i = iMin to iMax-1 do
     let x = objects.[i].X
     let y = objects.[i].Y + 5
     let tx = Math.Floor(Черепаха.X) |> int
     let ty = Math.Floor(Черепаха.Y) |> int
     let d = Math.SquareRoot(float (Math.Power(tx - x, 2) + Math.Power(ty - y, 2))) |> int
     if d < (size.[objects.[i].Kind] + 16) / 2 then
       collisionDetected <- true   
     if y > gh then
       passed <- passed + 1
       Фигуры.УстановитьТекст(score, string passed)
       Фигуры.Удалить(objects.[i].ShapeName)
       iMin <- i + 1
     else
       Фигуры.Переместить(objects.[i].ShapeName, x, y)
       objects.[i].X <- x
       objects.[i].Y <- y   
and AddObject () =   
   iMax <- iMax + 1
   ГрафическоеОкно.PenWidth <- 1.0
   let kind = Math.GetRandomNumber(3)
   ГрафическоеОкно.ЦветКисти <- color.[kind]
   let sz = size.[kind]
   let shapeName = Фигуры.ДобавитьПрямоугольник(sz, sz)
   let x = Math.GetRandomNumber(gw - 20) + 10
   let y = -20
   objects.Add({ X = x; Y=y; Kind=kind; ShapeName=shapeName})
   Фигуры.Переместить(shapeName, x, y)
   Фигуры.Повернуть(shapeName, Math.GetRandomNumber(360))
and OnKeyDown () =
   if not moving then
     moving <- true
     lastKey <- ГрафическоеОкно.ПоследняяКнопка   

Init()
Opening()
Game()
Closing()