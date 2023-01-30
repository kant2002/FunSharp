module Library.Math

let private кезд = System.Random()
let АлуРадианы (deg:float) = deg * System.Math.PI / 180.
let АлуКездейсоқСаны(n) = кезд.Next(n) + 1
let inline Remainder(x,y) = x % y
let Cos(angle) = cos angle
let Sin(angle) = sin angle
let ArcTan(angle) = atan angle
let inline Abs(n) = abs n
let inline Floor(n) = floor n |> int
let inline Round(n) = round n
let inline SquareRoot(d) = sqrt d
let inline Power(x,n) = pown x n

