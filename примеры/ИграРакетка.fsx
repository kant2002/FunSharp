﻿#r "nuget: Xwt"
#r "../src/bin/Debug/net48/ВеселШарп.Библиотека.dll"

open Библиотека

let гш = float ГрафическоеОкно.Ширина
let гв = float ГрафическоеОкно.Высота
let ракетка = Фигуры.ДобавитьПрямоугольник(120, 12)
let мяч = Фигуры.ДобавитьЭллипс(16, 16)
let mutable счет = 0

let НаМышьПеремещена () =
  let ракеткаX = ГрафическоеОкно.МышьX
  Фигуры.Переместить(ракетка, ракеткаX - 60.0, гв - 12.0)

let НапечататьСчет () =
  // Сперва очистим счет и затем нарисуем текст настоящего счета
  ГрафическоеОкно.ЦветКисти <- Цвета.White
  ГрафическоеОкно.ЗаполнитьПрямоугольник(10, 10, 200, 20)
  ГрафическоеОкно.ЦветКисти <- Цвета.Black
  ГрафическоеОкно.НарисоватьТекст(10, 10, "Счет: " + счет.ToString())

ГрафическоеОкно.РазмерШрифта <- 14.0
ГрафическоеОкно.МышьПеремещена <- Callback(НаМышьПеремещена)

НапечататьСчет()
//Звук.ИгратьЗвонок()

let mutable x = 0.0
let mutable y = 0.0
let mutable дельтаX = 1.0
let mutable дельтаY = 2.0

while (y < гв) do
  x <- x + дельтаX
  y <- y + дельтаY
  
  if (x >= гш - 16.0 || x <= 0.0) then
    дельтаX <- -дельтаX 
  if (y <= 0.0) then
    дельтаY <- -дельтаY
 
  let padX = Фигуры.ПолучитьЛево(ракетка)
  if (y = гв - 28.0 && x >= padX && x <= padX + 120.0) then
    //Звук.ИгратьКлик()
    счет <- счет + 10
    НапечататьСчет()
    дельтаY <- -дельтаY  

  Фигуры.Переместить(мяч, x, y)
  Программа.Задержка(15)
  
ГрафическоеОкно.ПоказатьСообщение("Ваш счет: " + счет.ToString(), "Ракетка")
Программа.Задержка(1_500)