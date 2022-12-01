# ВеселШарп
Весела кросс-платформенна графична біблиотека, базуючаяся на библиотеці із [Small Basic](http://smallbasic.com/), зроблена спеціально для Ф# і C#.

## Переклад

Ця библиотека являєтся перекладом [FunSharp](https://github.com/ptrelford/FunSharp) на українську мову. Я вважаю що цей підхід буде у першу чергу цікавішим для новачків у програмуванні.

## Розгортування

ВеселШарп запускаєтся на Raspbian, Linux, Windows та MacOS.

## Залежності

ВеселШарп використовує [Avalonia](https://github.com/AvaloniaUI/Avalonia).

Для компіляції ВеселШарп використовує [Ф#](https://github.com/kant2002/fsharp) що э локалізованим діалектом F#.

## Інші мови

Цей проект також переведено на інші мови
- [English](https://github.com/kant2002/funsharp/tree/main)
- [Русский](https://github.com/kant2002/funsharp/tree/%D0%B3%D0%BB%D0%B0%D0%B2%D0%BD%D0%B0%D1%8F)

## Приклад

```fsharp
відкрити Бібліотека

ГрафичнеВікно.КолірПера <- Цвета.Purple
Черепаха.X <- 150.
Черепаха.Y <- 150.
для i у 0..5..200 зробити
   Черепаха.Перемістити(i)
   Черепаха.Повернути(90)
```
![Alt text](http://trelford.com/FunSharp/Turtle_Example.png "Приклад Черепахи")

## Ігри

![Alt text](http://trelford.com/FunSharp/1942.png "1942")

![Alt text](http://trelford.com/FunSharp/Asteroids.png "Астероіди")

![Alt text](http://trelford.com/FunSharp/Tetris.png "Тетріс")

## Співпраця

Сотрудничество вітається, зокрема нові приклади, виправлення помилок та додання API через завдання up-for-grabs.

## Up-for-grabs

- реализувати Фігури.Анімувати(x,y,тривалість)
- реализувати ЭлементиКерування.ДодатиБагатоторядковеТекстовеВікно(ліво,верх)
