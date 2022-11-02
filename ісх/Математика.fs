module Бібліотека.Математика

let private ранд = System.Random()
let ОтриматиРадіани (град:float) = град * System.Math.PI / 180.
let ОтриматиВипадковеЧисло(n) = ранд.Next(n) + 1
let inline Залишок(x,y) = x % y
let Кос(угол) = cos угол
let Син(угол) = sin угол
let АркТан(угол) = atan угол
let inline Модуль(n) = abs n
let inline Floor(n) = floor n |> int
let inline Округляти(n) = round n
let inline КвадратнийКорінь(d) = sqrt d
let inline Ступінь(x,n) = pown x n
