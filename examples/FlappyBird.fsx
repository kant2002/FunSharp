#r "nuget: Xwt"
#r "../src/bin/Debug/net48/FunSharp.Library.dll"

open Библиотека

let bg = Фигуры.ДобавитьИзображение("http://flappycreator.com/default/bg.png")
let ground = Фигуры.ДобавитьИзображение("http://flappycreator.com/default/ground.png")
let bird = Фигуры.ДобавитьИзображение("http://flappycreator.com/default/bird_sing.png")
let tube1 = ImageList.LoadImage("http://flappycreator.com/default/tube1.png")
let tube2 = ImageList.LoadImage("http://flappycreator.com/default/tube2.png")
let t1 = Фигуры.ДобавитьИзображение(tube1)
let t2 = Фигуры.ДобавитьИзображение(tube2)

Фигуры.Переместить(t1, 150.0, 50.0-320.0)
Фигуры.Переместить(t2, 150.0, 150.0)
Фигуры.Переместить(ground, 0.0, 340.0)
Фигуры.Повернуть(bird,45.0*4.0)
Фигуры.Переместить(bird,50.0,100.0)
ГрафическоеОкно.Показать()
ГрафическоеОкно.Ширина <- 288
ГрафическоеОкно.Высота <- 440
