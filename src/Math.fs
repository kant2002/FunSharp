module Библиотека.Math

let private ранд = System.Random()
let GetRadians (град:float) = град * System.Math.PI / 180.
let GetRandomNumber(n) = ранд.Next(n) + 1
let inline Remainder(x,y) = x % y
let Cos(угол) = cos угол
let Sin(угол) = sin угол
let ArcTan(угол) = atan угол
let inline Abs(n) = abs n
let inline Floor(n) = floor n |> int
let inline Round(n) = round n
let inline SquareRoot(d) = sqrt d
let inline Power(x,n) = pown x n

