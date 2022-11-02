namespace Бібліотека

open System
open Avalonia

[<Sealed>]
type Черепаха private () =
   static let mutable прихованоКористувачем = false
   static let mutable швидкість = 0
   static let mutable кут = 0.0
   static let mutable _x = float ГрафичнеВікно.Ширина / 2.0
   static let mutable _y = float ГрафичнеВікно.Висота / 2.0
   static let mutable пероНажато = true
   static let показати () =
      if not прихованоКористувачем then
        Моя.Апплікація.Полотно.Черепаха.Видно <- true
        Моя.Апплікація.Полотно.ЗробитиНедійсним()
   static member Швидкість
      with get () = швидкість
      and set значення = 
         швидкість <- значення
         показати ()
   static member Кут
      with get () = кут
      and set значення = 
         кут <- значення
         Моя.Апплікація.Полотно.Черепаха.Обертання <- Some кут
         показати ()
   static member X
      with get () = _x
      and set значення = 
         _x <- значення
         показати ()
   static member Y
      with get () = _y
      and set значення = 
         _y <- значення
         показати ()
   static member Повернути(amount:float) =
      Черепаха.Кут <- кут + amount
   static member Повернути(amount:int) =
      Черепаха.Повернути(float amount)      
   static member ПовернутиНалево() =
      Черепаха.Повернути(-90.0)
   static member ПовернутиНаправо() =
      Черепаха.Повернути(90.0)
   static member Перемістити(відстань:float) =
      let r = (кут - 90.0) * Math.PI / 180.0
      let x' = _x + відстань * cos r
      let y' = _y + відстань * sin r
      if пероНажато then
         ГрафичнеВікно.НамалюватиЛінію(_x,_y,x',y')
      _x <- x'
      _y <- y'
      Моя.Апплікація.Полотно.Черепаха.Зсув <- Point(_x,_y)
      показати ()
   static member Перемістити(distance:int) =
      Черепаха.Перемістити (float distance)
   static member ПереміститиВ(x:float,y:float) =
      _x <- x; _y <- y
      Моя.Апплікація.Полотно.Черепаха.Зсув <- Point(_x,_y)
      показати()
   static member ПереміститиВ(x:int, y:int) = 
      Черепаха.ПереміститиВ(float x, float y)
   static member ПіднятиПеро() =
      пероНажато <- false
      показати ()
   static member ОпуститиПеро() =
      пероНажато <- true
      показати ()
   static member Показати() =
      прихованоКористувачем <- false
      показати()
   static member Сховати() =
      прихованоКористувачем <- true
      Моя.Апплікація.Полотно.Черепаха.Видно <- false
