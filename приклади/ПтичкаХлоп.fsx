﻿#r "nuget: Avalonia.Desktop, 11.0.0-preview3"
#r "nuget: Avalonia.Themes.Fluent, 11.0.0-preview3"
#r "../ісх/bin/Debug/net7.0/ВеселШарп.Бiблiотека.dll"

open Бiблiотека

let фон = Фігури.ДодатиЗображення("http://flappycreator.com/default/bg.png")
let земля = Фігури.ДодатиЗображення("http://flappycreator.com/default/ground.png")
let птица = Фігури.ДодатиЗображення("http://flappycreator.com/default/bird_sing.png")
let труба1 = СписокЗображень.ЗавантажитиЗображення("http://flappycreator.com/default/tube1.png")
let труба2 = СписокЗображень.ЗавантажитиЗображення("http://flappycreator.com/default/tube2.png")
let т1 = Фігури.ДодатиЗображення(труба1)
let т2 = Фігури.ДодатиЗображення(труба2)

Фігури.Перемістити(т1, 150.0, 50.0-320.0)
Фігури.Перемістити(т2, 150.0, 150.0)
Фігури.Перемістити(земля, 0.0, 340.0)
Фігури.Повернути(птица,45.0*4.0)
Фігури.Перемістити(птица,50.0,100.0)
ГрафичнеВікно.Показать()
ГрафичнеВікно.Ширина <- 288
ГрафичнеВікно.Висота <- 440
Програма.Затримка(20_000)
