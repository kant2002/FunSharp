module Библиотека.Математика

let private ранд = System.Random()
let ВзятьРадианы (град:float) = град * System.Math.PI / 180.
let ВзятьСлучайноеЧисло(n) = ранд.Next(n) + 1
let inline Остаток(x,y) = x % y
let Кос(угол) = cos угол
let Син(угол) = sin угол
let АркТан(угол) = atan угол
let inline Модуль(n) = abs n
let inline Floor(n) = floor n |> int
let inline Округлить(n) = round n
let inline КвадратныйКорень(d) = sqrt d
let inline Степень(x,n) = pown x n
