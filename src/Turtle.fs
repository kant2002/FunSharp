﻿namespace Library

open System

[<Sealed>]
type Turtle private () =
   static let mutable userHidden = false
   static let mutable speed = 0
   static let mutable angle = 0.0
   static let mutable _x = float ГрафическоеОкно.Width / 2.0
   static let mutable _y = float ГрафическоеОкно.Height / 2.0
   static let mutable isPenDown = true
   static let show () =
      if not userHidden then
        Мое.Приложение.Холст.Turtle.IsVisible <- true
        Мое.Приложение.Холст.Invalidate()
   static member Speed
      with get () = speed
      and set value = 
         speed <- value
         show ()
   static member Angle
      with get () = angle
      and set value = 
         angle <- value
         Мое.Приложение.Холст.Turtle.Rotation <- Some angle
         show ()
   static member X
      with get () = _x
      and set value = 
         _x <- value
         show ()
   static member Y
      with get () = _y
      and set value = 
         _y <- value
         show ()
   static member Turn(amount:float) =
      Turtle.Angle <- angle + amount
   static member Turn(amount:int) =
      Turtle.Turn(float amount)      
   static member TurnLeft() =
      Turtle.Turn(-90.0)
   static member TurnRight() =
      Turtle.Turn(90.0)
   static member Move(distance:float) =
      let r = (angle - 90.0) * Math.PI / 180.0
      let x' = _x + distance * cos r
      let y' = _y + distance * sin r
      if isPenDown then
         ГрафическоеОкно.DrawLine(_x,_y,x',y')
      _x <- x'
      _y <- y'
      Мое.Приложение.Холст.Turtle.Offset <- Xwt.Point(_x,_y)
      show ()
   static member Move(distance:int) =
      Turtle.Move (float distance)
   static member MoveTo(x:float,y:float) =
      _x <- x; _y <- y
      Мое.Приложение.Холст.Turtle.Offset <- Xwt.Point(_x,_y)
      show()
   static member MoveTo(x:int, y:int) = 
      Turtle.MoveTo(float x, float y)
   static member PenUp() =
      isPenDown <- false
      show ()
   static member PenDown() =
      isPenDown <- true
      show ()
   static member Show() =
      userHidden <- false
      show()
   static member Hide() =
      userHidden <- true
      Мое.Приложение.Холст.Turtle.IsVisible <- false
