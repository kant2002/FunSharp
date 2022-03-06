﻿open Библиотека

let gw = float ГрафическоеОкно.Ширина
let gh = float ГрафическоеОкно.Высота
let paddle = Фигуры.ДобавитьПрямоугольник(120, 12)
let ball = Фигуры.ДобавитьЭллипс(16, 16)
let mutable score = 0

let OnMouseMove () =
  let paddleX = ГрафическоеОкно.МышьX
  Фигуры.Переместить(paddle, paddleX - 60.0, gh - 12.0)

let PrintScore () =
  // Clear the score first and then draw the real score text
  ГрафическоеОкно.ЦветКисти <- Цвета.White
  ГрафическоеОкно.ЗаполнитьПрямоугольник(10, 10, 200, 20)
  ГрафическоеОкно.ЦветКисти <- Цвета.Black
  ГрафическоеОкно.НарисоватьТекст(10, 10, "Score: " + score.ToString())

ГрафическоеОкно.РазмерШрифта <- 14.0
ГрафическоеОкно.МышьПеремещена <- Callback(OnMouseMove)

PrintScore()
Sound.PlayBellRing (*AndWait*) ()

let mutable x = 0.0
let mutable y = 0.0
let mutable deltaX = 1.0
let mutable deltaY = 2.0

while (y < gh) do
  x <- x + deltaX
  y <- y + deltaY
  
  if (x >= gw - 16.0 || x <= 0.0) then
    deltaX <- -deltaX 
  if (y <= 0.0) then
    deltaY <- -deltaY
 
  let padX = Фигуры.ПолучитьЛево(paddle)
  if (y = gh - 28.0 && x >= padX && x <= padX + 120.0) then
    //Sound.PlayClick()
    score <- score + 10
    PrintScore()
    deltaY <- -deltaY  

  Фигуры.Переместить(ball, x, y)
  Программа.Задержка(15)
  
ГрафическоеОкно.ПоказатьСообщение("Your score is: " + score.ToString(), "Paddle")
