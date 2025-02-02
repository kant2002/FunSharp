﻿простір Бібліотека

відкрити System
відкрити Avalonia

[<Sealed>]
тип Черепаха приватний () =
   статичний нехай змінливий прихованоКористувачем = ложь
   статичний нехай змінливий швидкість = 0
   статичний нехай змінливий кут = 0.0
   статичний нехай змінливий _x = float ГрафичнеВікно.Ширина / 2.0
   статичний нехай змінливий _y = float ГрафичнеВікно.Висота / 2.0
   статичний нехай змінливий пероНажато = істина
   статичний нехай показати () =
      якщо не прихованоКористувачем тоді
        Моя.Апплікація.Полотно.Черепаха.Видно <- істина
        Моя.Апплікація.Полотно.ЗробитиНедійсним()
   статичний член Швидкість
      із get () = швидкість
      та set значення = 
         швидкість <- значення
         показати ()
   статичний член Кут
      із get () = кут
      та set значення = 
         кут <- значення
         Моя.Апплікація.Полотно.Черепаха.Обертання <- Some кут
         показати ()
   статичний член X
      із get () = _x
      та set значення = 
         _x <- значення
         показати ()
   статичний член Y
      із get () = _y
      та set значення = 
         _y <- значення
         показати ()
   статичний член Повернути(amount:float) =
      Черепаха.Кут <- кут + amount
   статичний член Повернути(amount:int) =
      Черепаха.Повернути(float amount)      
   статичний член ПовернутиНалево() =
      Черепаха.Повернути(-90.0)
   статичний член ПовернутиНаправо() =
      Черепаха.Повернути(90.0)
   статичний член Перемістити(відстань:float) =
      нехай r = (кут - 90.0) * Math.PI / 180.0
      нехай x' = _x + відстань * cos r
      нехай y' = _y + відстань * sin r
      якщо пероНажато тоді
         ГрафичнеВікно.НамалюватиЛінію(_x,_y,x',y')
      _x <- x'
      _y <- y'
      Моя.Апплікація.Полотно.Черепаха.Зсув <- Point(_x,_y)
      показати ()
   статичний член Перемістити(distance:int) =
      Черепаха.Перемістити (float distance)
   статичний член ПереміститиВ(x:float,y:float) =
      _x <- x; _y <- y
      Моя.Апплікація.Полотно.Черепаха.Зсув <- Point(_x,_y)
      показати()
   статичний член ПереміститиВ(x:int, y:int) = 
      Черепаха.ПереміститиВ(float x, float y)
   статичний член ПіднятиПеро() =
      пероНажато <- ложь
      показати ()
   статичний член ОпуститиПеро() =
      пероНажато <- істина
      показати ()
   статичний член Показати() =
      прихованоКористувачем <- ложь
      показати()
   статичний член Сховати() =
      прихованоКористувачем <- істина
      Моя.Апплікація.Полотно.Черепаха.Видно <- ложь
