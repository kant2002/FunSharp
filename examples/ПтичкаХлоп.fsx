﻿#r "nuget: Xwt"
#r "../src/bin/Debug/net48/FunSharp.Library.dll"

open Библиотека

let фон = Фигуры.ДобавитьИзображение("http://flappycreator.com/default/bg.png")
let земля = Фигуры.ДобавитьИзображение("http://flappycreator.com/default/ground.png")
let птица = Фигуры.ДобавитьИзображение("http://flappycreator.com/default/bird_sing.png")
let труба1 = ImageList.LoadImage("http://flappycreator.com/default/tube1.png")
let труба2 = ImageList.LoadImage("http://flappycreator.com/default/tube2.png")
let т1 = Фигуры.ДобавитьИзображение(труба1)
let т2 = Фигуры.ДобавитьИзображение(труба2)

Фигуры.Переместить(т1, 150.0, 50.0-320.0)
Фигуры.Переместить(т2, 150.0, 150.0)
Фигуры.Переместить(земля, 0.0, 340.0)
Фигуры.Повернуть(птица,45.0*4.0)
Фигуры.Переместить(птица,50.0,100.0)
ГрафическоеОкно.Показать()
ГрафическоеОкно.Ширина <- 288
ГрафическоеОкно.Высота <- 440
