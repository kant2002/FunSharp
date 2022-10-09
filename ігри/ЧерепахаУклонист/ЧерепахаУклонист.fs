﻿#if INTERACTIVE
#r "./bin/debug/Xwt.dll"
#r "./bin/debug/FunSharp.dll"
#endif

open Бiблiотека

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

let заголовок = "Turtle Dodger 0.5b"
ГрафичнеВікно.Заголовок <- заголовок
// Debug variables
let отладка = false
let mutable cross1 = "<shape name>"
let mutable cross2 = "<shape name>"
let mutable поз = "<shape name>"
// Global variables
let гш = 598
let гв = 428
let mutable крутится = false
let mutable движется = false
let mutable обнаруженоСтолкновение = false
let mutable прошло = 0
let mutable последняяКнопка = ""
let mutable последнийпмс = 0
let цвет = dict [1,Кольори.Orange; 2, Кольори.Cyan; 3, Кольори.Lime]
let размер = dict [1,20; 2,16; 3,12]
let mutable счет = "<shape name>"
let mutable iMin = 0
let mutable iMax = 0
/// Falling object
type Объект = { mutable X:int; mutable Y:int; Род:int; НазваниеФигуры:string }
/// Falling objects
let объекты = ResizeArray<Объект>()

let rec Закрытие () =
   Таймер.Пауза()
   Черепаха.Повернути(720)
   ГрафичнеВікно.ЦветКисти <- Кольори.White
   ГрафичнеВікно.ИмяШрифта <- "Trebuchet MS"
   ГрафичнеВікно.РозмірШрифта <- 40.0
   let x = (гш - 217) / 2
   let y = 100
   ГрафичнеВікно.НарисоватьТекст(x, y, "ИГРА ОКОНЧЕНА")
   Програма.Затримка(3000)
and Открытие () =
   let url = "" // "http://www.nonkit.com/smallbasic.files/"
   let большаяЧерепаха = Фигуры.ДобавитьИзображение(url + "turtle.png")
   Фигуры.Перемістити(большаяЧерепаха, 180, 140)
   ГрафичнеВікно.ЦветКисти <- Кольори.White
   ГрафичнеВікно.ИмяШрифта <- "Trebuchet MS"
   ГрафичнеВікно.РозмірШрифта <- 50.0
   let x = (гш - 443) / 2
   let y = 40
   ГрафичнеВікно.НарисоватьТекст(x, y, заголовок)
   Програма.Затримка(3000)
   ГрафичнеВікно.Очистити()
and Готово () =
   ГрафичнеВікно.РозмірШрифта <- 40.0
   let готово = Фигуры.ДобавитьТекст("Готовы?")
   let x = (гш - 130) / 2
   let y = 100
   Фигуры.Перемістити(готово, x, y)
   for opacity in 100 .. -10 .. 0 do
     Фигуры.SetOpacity(готово, opacity)
     Програма.Затримка(200)
   Фигуры.Удалить(готово)
and Игра () =
   Черепаха.Скорость <- 7
   Черепаха.ПоднятьПеро()
   let x = гш / 2
   let y = гв - 40
   ГрафичнеВікно.ЦветКисти <- Кольори.White
   ГрафичнеВікно.РозмірШрифта <- 18.0
   счет <- Фигуры.ДобавитьТекст("0")
   Фигуры.Перемістити(счет, 20, 20)
   if отладка then
     ГрафичнеВікно.ЦветКисти <- Кольори.White
     ГрафичнеВікно.РозмірШрифта <- 12.0
     поз <- Фигуры.ДобавитьТекст("(" + x.ToString() + "," + y.ToString() + ")")
     ГрафичнеВікно.ШиринаПера <- 1.0
     cross1 <- Фигуры.ДобавитьЛинию(0, -8, 0, 8)
     cross2 <- Фигуры.ДобавитьЛинию(-8, 0, 8, 0)
     Фигуры.Перемістити(cross1, x, y)
     Фигуры.Перемістити(cross2, x, y)
     Фигуры.Перемістити(поз, гш - 100, 20)   
   Черепаха.ПереміститиВ(x, y)
   Черепаха.Угол <- 0.0   
   движется <- false
   крутится <- false
   Готово()
   ГрафичнеВікно.КнопкаНажата <- Callback(НаКнопкаНажата)
   let тик = false
   Таймер.Интервал <- 1000 / 24
   Таймер.Тик <- Callback(НаТик)
   последнийпмс <- Часы.ПрошедшиеМиллисекунды
   iMin <- 0
   while not обнаруженоСтолкновение do
     if движется then
       if последняяКнопка = "Left" then
         Черепаха.ПовернутиНалево()
         Черепаха.Перемістити(30)
         Черепаха.ПовернутиНаправо()
       elif последняяКнопка = "Right" then
         Черепаха.ПовернутиНаправо()
         Черепаха.Перемістити(30)
         Черепаха.ПовернутиНалево()      
       движется <- false
     else
       Програма.Затримка(100)      
and Инициализация () =
   ГрафичнеВікно.КолірФона <- Кольори.DodgerBlue
   ГрафичнеВікно.Ширина <- гш
   ГрафичнеВікно.Высота <- гв   
   прошло <- 0
   обнаруженоСтолкновение <- false
and НаТик () =
   if not крутится then
     крутится <- true
     let пмс = Часы.ПрошедшиеМиллисекунды
     if пмс - последнийпмс > 500 then
       ДобавитьОбъект()
       последнийпмс <- пмс    
     КрутитьОбъект()
     крутится <- false
   if отладка then
     let x = Математика.Floor(Черепаха.X)
     let y = Математика.Floor(Черепаха.Y)
     Фигуры.УстановитьТекст(поз, "(" + x.ToString() + "," + y.ToString() + ")")
     Фигуры.Перемістити(cross1, x, y)
     Фигуры.Перемістити(cross2, x, y)
and КрутитьОбъект () =
   for i = iMin to iMax-1 do
     let x = объекты.[i].X
     let y = объекты.[i].Y + 5
     let tx = Математика.Floor(Черепаха.X) |> int
     let ty = Математика.Floor(Черепаха.Y) |> int
     let д = Математика.КвадратнийКорінь(float (Математика.Ступінь(tx - x, 2) + Математика.Ступінь(ty - y, 2))) |> int
     if д < (размер.[объекты.[i].Род] + 16) / 2 then
       обнаруженоСтолкновение <- true   
     if y > гв then
       прошло <- прошло + 1
       Фигуры.УстановитьТекст(счет, string прошло)
       Фигуры.Удалить(объекты.[i].НазваниеФигуры)
       iMin <- i + 1
     else
       Фигуры.Перемістити(объекты.[i].НазваниеФигуры, x, y)
       объекты.[i].X <- x
       объекты.[i].Y <- y   
and ДобавитьОбъект () =   
   iMax <- iMax + 1
   ГрафичнеВікно.ШиринаПера <- 1.0
   let род = Математика.ОтриматиВипадковеЧисло(3)
   ГрафичнеВікно.ЦветКисти <- цвет.[род]
   let рз = размер.[род]
   let названиеФигуры = Фигуры.ДодатиПрямокутник(рз, рз)
   let x = Математика.ОтриматиВипадковеЧисло(гш - 20) + 10
   let y = -20
   объекты.Add({ X = x; Y=y; Род=род; НазваниеФигуры=названиеФигуры})
   Фигуры.Перемістити(названиеФигуры, x, y)
   Фигуры.Повернути(названиеФигуры, Математика.ОтриматиВипадковеЧисло(360))
and НаКнопкаНажата () =
   if not движется then
     движется <- true
     последняяКнопка <- ГрафичнеВікно.ОстанняКнопка   

Инициализация()
Открытие()
Игра()
Закрытие()