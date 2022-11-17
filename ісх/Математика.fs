module Бібліотека.Математика

нехай private ранд = System.Random()
нехай ОтриматиРадіани (град:float) = град * System.Math.PI / 180.
нехай ОтриматиВипадковеЧисло(n) = ранд.Next(n) + 1
нехай inline Залишок(x,y) = x % y
нехай Кос(угол) = cos угол
нехай Син(угол) = sin угол
нехай АркТан(угол) = atan угол
нехай inline Модуль(n) = abs n
нехай inline Floor(n) = floor n |> int
нехай inline Округляти(n) = round n
нехай inline КвадратнийКорінь(d) = sqrt d
нехай inline Ступінь(x,n) = pown x n
