namespace Библиотека

type Цвет = 
   struct
      val A:byte
      val R:byte
      val G:byte
      val B:byte
   end
   new (a,r,g,b) = { A=a; R=r; G=g; B=b }

[<AutoOpen>]
module internal КонвертерЦвета =
   let кXwtЦвету (color:Цвет) = Xwt.Drawing.Color.FromBytes(color.R, color.G, color.B, color.A)

type internal Ширина = float
type internal Height = float
type internal Размер = float
type internal Family = string
type internal IsBold = bool
type internal IsItalic = bool
type internal X = float
type internal Y = float

type internal Pen = Pen of Цвет * Ширина
type internal Шрифт = Font of Размер * Family * IsBold * IsItalic
type internal Линия = Line of X * Y * X * Y
type internal Прямоугольник = Rect of Ширина * Height
type internal Треугольник = Triangle of X * Y * X * Y * X * Y
type internal Эллипс = Ellipse of Ширина * Height

type internal Shape =
   | LineShape of Линия * Pen   
   | RectShape of Прямоугольник * Pen * Цвет
   | TriangleShape of Треугольник * Pen * Цвет
   | EllipseShape of Эллипс * Pen * Цвет
   | ImageShape of Xwt.Drawing.Image ref
   | TextShape of string ref * Шрифт * Цвет

type internal Drawing =
   | DrawLine of Линия * Pen
   | DrawRect of Прямоугольник * Pen
   | DrawTriangle of Треугольник * Pen
   | DrawEllipse of Эллипс * Pen
   | DrawImage of Xwt.Drawing.Image ref * float * float
   | DrawText of float * float * string * Шрифт * Цвет
   | DrawBoundText of float * float * float * string * Шрифт * Цвет
   | FillRect of Прямоугольник * Цвет
   | FillTriangle of Треугольник * Цвет
   | FillEllipse of Эллипс * Цвет
   | DrawShape of string * Shape

