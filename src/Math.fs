module Библиотека.Math

let private ранд = System.Random()
let ВзятьРадианы (град:float) = град * System.Math.PI / 180.
let ВзятьСлучайноеЧисло(n) = ранд.Next(n) + 1
let inline Остаток(x,y) = x % y
let Cos(угол) = cos угол
let Sin(угол) = sin угол
let ArcTan(угол) = atan угол
let inline Abs(n) = abs n
let inline Floor(n) = floor n |> int
let inline Округлить(n) = round n
let inline КвадратныйКорень(d) = sqrt d
let inline Степень(x,n) = pown x n
