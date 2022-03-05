namespace Библиотека

open System

[<Sealed>]
type Черепаха private () =
   static let mutable userHidden = false
   static let mutable скорость = 0
   static let mutable угол = 0.0
   static let mutable _x = float ГрафическоеОкно.Ширина / 2.0
   static let mutable _y = float ГрафическоеОкно.Высота / 2.0
   static let mutable isPenDown = true
   static let показать () =
      if not userHidden then
        Мое.Приложение.Холст.Черепаха.IsVisible <- true
        Мое.Приложение.Холст.Invalidate()
   static member Скорость
      with get () = скорость
      and set значение = 
         скорость <- значение
         показать ()
   static member Angle
      with get () = угол
      and set значение = 
         угол <- значение
         Мое.Приложение.Холст.Черепаха.Rotation <- Some угол
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
   static member Повернуть(amount:float) =
      Черепаха.Angle <- угол + amount
   static member Повернуть(amount:int) =
      Черепаха.Повернуть(float amount)      
   static member ПовернутьНалево() =
      Черепаха.Повернуть(-90.0)
   static member ПовернутьНаправо() =
      Черепаха.Повернуть(90.0)
   static member Переместить(дистанция:float) =
      let r = (угол - 90.0) * Math.PI / 180.0
      let x' = _x + дистанция * cos r
      let y' = _y + дистанция * sin r
      if isPenDown then
         ГрафическоеОкно.НарисоватьЛинию(_x,_y,x',y')
      _x <- x'
      _y <- y'
      Мое.Приложение.Холст.Черепаха.Offset <- Xwt.Point(_x,_y)
      показать ()
   static member Переместить(distance:int) =
      Черепаха.Переместить (float distance)
   static member ПереместитьВ(x:float,y:float) =
      _x <- x; _y <- y
      Мое.Приложение.Холст.Черепаха.Offset <- Xwt.Point(_x,_y)
      показать()
   static member ПереместитьВ(x:int, y:int) = 
      Черепаха.ПереместитьВ(float x, float y)
   static member PenUp() =
      isPenDown <- false
      показать ()
   static member PenDown() =
      isPenDown <- true
      показать ()
   static member Показать() =
      userHidden <- false
      показать()
   static member Скрыть() =
      userHidden <- true
      Мое.Приложение.Холст.Черепаха.IsVisible <- false
