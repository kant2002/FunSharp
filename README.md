# ВеселШарп
Веселая кросс-платформенная графическая библиотека, основанная на библиотеке из [Small Basic](http://smallbasic.com/), сделанная специально для F# и C#.

## Перевод

Данная библиотека является переводом [FunSharp](https://github.com/ptrelford/FunSharp) на русский язык. Мне кажется такой подход будет в первую очередь интересен тем для кого программирование в новинку.

## Развертывание

ВеселШарп запускается на Raspbian, Linux, Windows и MacOS.

## Зависимости

ВеселШарп использует [Avalonia](https://github.com/AvaloniaUI/Avalonia).

## Другие языки

Этот прооект также переведен на другие языки
- [Українська](https://github.com/kant2002/funsharp)
- [English](https://github.com/kant2002/funsharp/tree/main)

## Сборка

* Appveyor: [![Build status](https://ci.appveyor.com/api/projects/status/94dkcwcrkwhj06vj?svg=true)](https://ci.appveyor.com/project/ptrelford/funsharp)
* Travis: [![Build Status](https://travis-ci.org/ptrelford/FunSharp.png?branch=master)](https://travis-ci.org/ptrelford/FunSharp/)

## Пример

```F#
open Библиотека

ГрафическоеОкно.ЦветПера <- Цвета.Purple
Черепаха.X <- 150.
Черепаха.Y <- 150.
for i in 0..5..200 do
   Черепаха.Переместить(i)
   Черепаха.Повернуть(90)
```
![Alt text](http://trelford.com/FunSharp/Turtle_Example.png "Пример Черепахи")

## Игры

![Alt text](http://trelford.com/FunSharp/1942.png "1942")

![Alt text](http://trelford.com/FunSharp/Asteroids.png "Астероиды")

![Alt text](http://trelford.com/FunSharp/Tetris.png "Тетрис")

## Сотрудничество

Сотрудничество приветствуется, в частности новые пример, исправления ошибок и пополнение API через задачи up-for-grabs.

## Up-for-grabs

- реализовать Фигуры.Анимировать(x,y,длительность)
- реализовать ЭлементыУправления.ДобавитьМногострочноеТекстовоеПоле(лево,верх)
