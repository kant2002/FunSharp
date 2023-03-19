namespace Кітапхана

open System

[<Sealed>]
type Тасбақа private () =
   static let mutable userHidden = false
   static let mutable speed = 0
   static let mutable angle = 0.0
   static let mutable _x = float ГрафикалықТерезе.Ен / 2.0
   static let mutable _y = float ГрафикалықТерезе.Биіктік / 2.0
   static let mutable isPenDown = true
   static let show () =
      if not userHidden then
        My.App.Canvas.Тасбақа.IsVisible <- true
        My.App.Canvas.Invalidate()
   static member Speed
      with get () = speed
      and set value = 
         speed <- value
         show ()
   static member Angle
      with get () = angle
      and set мәні = 
         angle <- мәні
         My.App.Canvas.Тасбақа.Rotation <- Some angle
         show ()
   static member X
      with get () = _x
      and set мәні = 
         _x <- мәні
         show ()
   static member Y
      with get () = _y
      and set мәні = 
         _y <- мәні
         show ()
   static member Бұру(саны:float) =
      Тасбақа.Angle <- angle + саны
   static member Бұру(саны:int) =
      Тасбақа.Бұру(float саны)      
   static member СолғаБұру() =
      Тасбақа.Бұру(-90.0)
   static member ОңғаБұру() =
      Тасбақа.Бұру(90.0)
   static member Жылжытуға(қашықтық:float) =
      let r = (angle - 90.0) * Math.PI / 180.0
      let x' = _x + қашықтық * cos r
      let y' = _y + қашықтық * sin r
      if isPenDown then
         ГрафикалықТерезе.DrawLine(_x,_y,x',y')
      _x <- x'
      _y <- y'
      My.App.Canvas.Тасбақа.Offset <- Avalonia.Point(_x,_y)
      show ()
   static member Жылжытуға(қашықтық:int) =
      Тасбақа.Жылжытуға (float қашықтық)
   static member MoveTo(x:float,y:float) =
      _x <- x; _y <- y
      My.App.Canvas.Тасбақа.Offset <- Avalonia.Point(_x,_y)
      show()
   static member MoveTo(x:int, y:int) = 
      Тасбақа.MoveTo(float x, float y)
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
      My.App.Canvas.Тасбақа.IsVisible <- false
