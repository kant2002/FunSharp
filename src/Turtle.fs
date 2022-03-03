namespace Library

open System

[<Sealed>]
type Turtle private () =
   static let mutable userHidden = false
   static let mutable скорость = 0
   static let mutable угол = 0.0
   static let mutable _x = float ГрафическоеОкно.Ширина / 2.0
   static let mutable _y = float ГрафическоеОкно.Высота / 2.0
   static let mutable isPenDown = true
   static let показать () =
      if not userHidden then
        Мое.Приложение.Холст.Turtle.IsVisible <- true
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
         Мое.Приложение.Холст.Turtle.Rotation <- Some угол
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
      Turtle.Angle <- угол + amount
   static member Повернуть(amount:int) =
      Turtle.Повернуть(float amount)      
   static member ПовернутьНалево() =
      Turtle.Повернуть(-90.0)
   static member ПовернутьНаправо() =
      Turtle.Повернуть(90.0)
   static member Move(дистанция:float) =
      let r = (угол - 90.0) * Math.PI / 180.0
      let x' = _x + дистанция * cos r
      let y' = _y + дистанция * sin r
      if isPenDown then
         ГрафическоеОкно.НарисоватьЛинию(_x,_y,x',y')
      _x <- x'
      _y <- y'
      Мое.Приложение.Холст.Turtle.Offset <- Xwt.Point(_x,_y)
      показать ()
   static member Move(distance:int) =
      Turtle.Move (float distance)
   static member MoveTo(x:float,y:float) =
      _x <- x; _y <- y
      Мое.Приложение.Холст.Turtle.Offset <- Xwt.Point(_x,_y)
      показать()
   static member MoveTo(x:int, y:int) = 
      Turtle.MoveTo(float x, float y)
   static member PenUp() =
      isPenDown <- false
      показать ()
   static member PenDown() =
      isPenDown <- true
      показать ()
   static member Show() =
      userHidden <- false
      показать()
   static member Hide() =
      userHidden <- true
      Мое.Приложение.Холст.Turtle.IsVisible <- false
