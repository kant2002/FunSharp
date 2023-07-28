﻿#if INTERACTIVE
#r "./bin/debug/Xwt.dll"
#r "./bin/debug/FunSharp.dll"
#endif

open Бібліотека

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

let заголовок = "Черепаха Ухиляка 0.5b"
ГрафичнеВікно.Заголовок <- заголовок
// Debug variables
let відлагоджування = false
let mutable хрест1 = "<shape name>"
let mutable хрест2 = "<shape name>"
let mutable поз = "<shape name>"
// Global variables
let гш = 598
let гв = 428
let mutable крутиться = false
let mutable рухається = false
let mutable виявленоЗіткнення = false
let mutable минуло = 0
let mutable останняКнопка = ""
let mutable останнійпмс = 0
let колір = dict [1,Кольори.Orange; 2, Кольори.Cyan; 3, Кольори.Lime]
let розмір = dict [1,20; 2,16; 3,12]
let mutable рахунок = "<shape name>"
let mutable iMin = 0
let mutable iMax = 0
/// Falling object
type Об'єкт = { mutable X:int; mutable Y:int; Род:int; НазваФігури:string }
/// Falling objects
let об'єкти = ResizeArray<Об'єкт>()

let rec Закриття () =
   Таймер.Пауза()
   Черепаха.Повернути(720)
   ГрафичнеВікно.КолірПензлика <- Кольори.Білий
   ГрафичнеВікно.ІмяШрифта <- "Trebuchet MS"
   ГрафичнеВікно.РозмірШрифта <- 40.0
   let x = (гш - 217) / 2
   let y = 100
   ГрафичнеВікно.НамалюватиТекст(x, y, "ГРА ЗАКІНЧИЛАСЬ")
   Програма.Затримка(3000)
and Відкриття () =
   let url = "" // "http://www.nonkit.com/smallbasic.files/"
   let большаяЧерепаха = Фігури.ДодатиЗображення(url + "turtle.png")
   Фігури.Перемістити(большаяЧерепаха, 180, 140)
   ГрафичнеВікно.КолірПензлика <- Кольори.Білий
   ГрафичнеВікно.ІмяШрифта <- "Trebuchet MS"
   ГрафичнеВікно.РозмірШрифта <- 50.0
   let x = (гш - 443) / 2
   let y = 40
   ГрафичнеВікно.НамалюватиТекст(x, y, заголовок)
   Програма.Затримка(3000)
   ГрафичнеВікно.Очистити()
and Готово () =
   ГрафичнеВікно.РозмірШрифта <- 40.0
   let готово = Фігури.ДодатиТекст("Готовы?")
   let x = (гш - 130) / 2
   let y = 100
   Фігури.Перемістити(готово, x, y)
   for opacity in 100 .. -10 .. 0 do
     Фігури.ВстановитиНепрозорість(готово, opacity)
     Програма.Затримка(200)
   Фігури.Видалити(готово)
and Гра () =
   Черепаха.Швидкість <- 7
   Черепаха.ПіднятиПеро()
   let x = гш / 2
   let y = гв - 40
   ГрафичнеВікно.КолірПензлика <- Кольори.Білий
   ГрафичнеВікно.РозмірШрифта <- 18.0
   рахунок <- Фігури.ДодатиТекст("0")
   Фігури.Перемістити(рахунок, 20, 20)
   if відлагоджування then
     ГрафичнеВікно.КолірПензлика <- Кольори.Білий
     ГрафичнеВікно.РозмірШрифта <- 12.0
     поз <- Фігури.ДодатиТекст("(" + x.ToString() + "," + y.ToString() + ")")
     ГрафичнеВікно.ШиринаПера <- 1.0
     хрест1 <- Фігури.ДодатиЛінію(0, -8, 0, 8)
     хрест2 <- Фігури.ДодатиЛінію(-8, 0, 8, 0)
     Фігури.Перемістити(хрест1, x, y)
     Фігури.Перемістити(хрест2, x, y)
     Фігури.Перемістити(поз, гш - 100, 20)   
   Черепаха.ПереміститиВ(x, y)
   Черепаха.Кут <- 0.0   
   рухається <- false
   крутиться <- false
   Готово()
   ГрафичнеВікно.КлавішаНатиснута <- Callback(НаКнопкаНатиснута)
   let тик = false
   Таймер.Интервал <- 1000 / 24
   Таймер.Тик <- Callback(НаТик)
   останнійпмс <- Годинник.МинуліМілісекунди
   iMin <- 0
   while not виявленоЗіткнення do
     if рухається then
       if останняКнопка = "Left" then
         Черепаха.ПовернутиНалево()
         Черепаха.Перемістити(30)
         Черепаха.ПовернутиНаправо()
       elif останняКнопка = "Right" then
         Черепаха.ПовернутиНаправо()
         Черепаха.Перемістити(30)
         Черепаха.ПовернутиНалево()      
       рухається <- false
     else
       Програма.Затримка(100)      
and Ініціалізація () =
   ГрафичнеВікно.КолірФона <- Кольори.DodgerBlue
   ГрафичнеВікно.Ширина <- гш
   ГрафичнеВікно.Висота <- гв   
   минуло <- 0
   виявленоЗіткнення <- false
and НаТик () =
   if not крутиться then
     крутиться <- true
     let пмс = Годинник.МинуліМілісекунди
     if пмс - останнійпмс > 500 then
       ДодатиОб'єкт()
       останнійпмс <- пмс    
     КрутитиОб'єкт()
     крутиться <- false
   if відлагоджування then
     let x = Математика.Floor(Черепаха.X)
     let y = Математика.Floor(Черепаха.Y)
     Фігури.УстановитьТекст(поз, "(" + x.ToString() + "," + y.ToString() + ")")
     Фігури.Перемістити(хрест1, x, y)
     Фігури.Перемістити(хрест2, x, y)
and КрутитиОб'єкт () =
   for i = iMin to iMax-1 do
     let x = об'єкти.[i].X
     let y = об'єкти.[i].Y + 5
     let tx = Математика.Floor(Черепаха.X) |> int
     let ty = Математика.Floor(Черепаха.Y) |> int
     let д = Математика.КвадратнийКорінь(float (Математика.Ступінь(tx - x, 2) + Математика.Ступінь(ty - y, 2))) |> int
     if д < (розмір.[об'єкти.[i].Род] + 16) / 2 then
       виявленоЗіткнення <- true   
     if y > гв then
       минуло <- минуло + 1
       Фігури.УстановитьТекст(рахунок, string минуло)
       Фігури.Видалити(об'єкти.[i].НазваФігури)
       iMin <- i + 1
     else
       Фігури.Перемістити(об'єкти.[i].НазваФігури, x, y)
       об'єкти.[i].X <- x
       об'єкти.[i].Y <- y   
and ДодатиОб'єкт () =   
   iMax <- iMax + 1
   ГрафичнеВікно.ШиринаПера <- 1.0
   let род = Математика.ОтриматиВипадковеЧисло(3)
   ГрафичнеВікно.КолірПензлика <- колір.[род]
   let рз = розмір.[род]
   let назваФігури = Фігури.ДодатиПрямокутник(рз, рз)
   let x = Математика.ОтриматиВипадковеЧисло(гш - 20) + 10
   let y = -20
   об'єкти.Add({ X = x; Y=y; Род=род; НазваФігури=назваФігури})
   Фігури.Перемістити(назваФігури, x, y)
   Фігури.Повернути(назваФігури, Математика.ОтриматиВипадковеЧисло(360))
and НаКнопкаНатиснута () =
   if not рухається then
     рухається <- true
     останняКнопка <- ГрафичнеВікно.ОстанняКнопка   

Ініціалізація()
Відкриття()
Гра()
Закриття()