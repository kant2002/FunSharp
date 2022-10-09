# ВеселШарп
Весела кросс-платформенна графична біблиотека, базуючаяся на библиотеці із [Small Basic](http://smallbasic.com/), зроблена спеціально для F# і C#.

## Переклад

Ця библиотека являєтся перекладом [FunSharp](https://github.com/ptrelford/FunSharp) на українську мову. Я вважаю що цей підхід буде у першу чергу цікавішим для новачків у програмуванні.

## Розгортування

ВеселШарп запускаєтся на Raspbian, Linux та Windows.

## Зависимости

ВеселШарп використовує Mono [Xwt](https://github.com/mono/xwt) та [Gtk#](http://www.mono-project.com/docs/gui/gtksharp/).

## Сборка

* Appveyor: [![Build status](https://ci.appveyor.com/api/projects/status/94dkcwcrkwhj06vj?svg=true)](https://ci.appveyor.com/project/ptrelford/funsharp)
* Travis: [![Build Status](https://travis-ci.org/ptrelford/FunSharp.png?branch=master)](https://travis-ci.org/ptrelford/FunSharp/)

## Пример

```F#
open Бібліотека

ГрафичнеВікно.КолірПера <- Цвета.Purple
Черепаха.X <- 150.
Черепаха.Y <- 150.
for i in 0..5..200 do
   Черепаха.Перемістити(i)
   Черепаха.Повернути(90)
```
![Alt text](http://trelford.com/FunSharp/Turtle_Example.png "Приклад Черепахи")

## Игры

![Alt text](http://trelford.com/FunSharp/1942.png "1942")

![Alt text](http://trelford.com/FunSharp/Asteroids.png "Астероиды")

![Alt text](http://trelford.com/FunSharp/Tetris.png "Тетрис")

## Співпраця

Сотрудничество вітається, зокрема нові приклади, виправлення помилок та додання API через завдання up-for-grabs.

## Up-for-grabs

- реализовать Фигуры.Анимировать(x,y,длительность)
- реализовать ЭлементыУправления.ДобавитьМногострочноеТекстовоеПоле(лево,верх)
