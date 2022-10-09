namespace Бiблiотека

open System

[<Sealed>]
type Черепаха private () =
   static let mutable скрытоПользователем = false
   static let mutable скорость = 0
   static let mutable угол = 0.0
   static let mutable _x = float ГрафичнеВікно.Ширина / 2.0
   static let mutable _y = float ГрафичнеВікно.Высота / 2.0
   static let mutable пероНажато = true
   static let показать () =
      if not скрытоПользователем then
        Мое.Приложение.Холст.Черепаха.Видим <- true
        Мое.Приложение.Холст.СделатьНедействительным()
   static member Скорость
      with get () = скорость
      and set значение = 
         скорость <- значение
         показать ()
   static member Угол
      with get () = угол
      and set значение = 
         угол <- значение
         Мое.Приложение.Холст.Черепаха.Врашение <- Some угол
         показать ()
   static member X
      with get () = _x
      and set значение = 
         _x <- значение
         показать ()
   static member Y
      with get () = _y
      and set значение = 
         _y <- значение
         показать ()
   static member Повернути(amount:float) =
      Черепаха.Угол <- угол + amount
   static member Повернути(amount:int) =
      Черепаха.Повернути(float amount)      
   static member ПовернутиНалево() =
      Черепаха.Повернути(-90.0)
   static member ПовернутиНаправо() =
      Черепаха.Повернути(90.0)
   static member Перемістити(відстань:float) =
      let r = (угол - 90.0) * Math.PI / 180.0
      let x' = _x + відстань * cos r
      let y' = _y + відстань * sin r
      if пероНажато then
         ГрафичнеВікно.НамалюватиЛінію(_x,_y,x',y')
      _x <- x'
      _y <- y'
      Мое.Приложение.Холст.Черепаха.Смещение <- Xwt.Point(_x,_y)
      показать ()
   static member Перемістити(distance:int) =
      Черепаха.Перемістити (float distance)
   static member ПереміститиВ(x:float,y:float) =
      _x <- x; _y <- y
      Мое.Приложение.Холст.Черепаха.Смещение <- Xwt.Point(_x,_y)
      показать()
   static member ПереміститиВ(x:int, y:int) = 
      Черепаха.ПереміститиВ(float x, float y)
   static member ПоднятьПеро() =
      пероНажато <- false
      показать ()
   static member ОпуститьПеро() =
      пероНажато <- true
      показать ()
   static member Показать() =
      скрытоПользователем <- false
      показать()
   static member Скрыть() =
      скрытоПользователем <- true
      Мое.Приложение.Холст.Черепаха.Видим <- false
