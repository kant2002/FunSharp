namespace Library

type Түс = 
   struct
      val A:byte
      val R:byte
      val G:byte
      val B:byte
   end
   new (a,r,g,b) = { A=a; R=r; G=g; B=b }

[<AutoOpen>]
module internal ТүсТүрлендіргіші =
   let toXwtColor (түс:Түс) = Avalonia.Media.Color.FromArgb(түс.R, түс.G, түс.B, түс.A)

type internal Ен = float
type internal Биіктік = float
type internal Size = float
type internal Family = string
type internal IsBold = bool
type internal IsItalic = bool
type internal X = float
type internal Y = float

type internal Pen = Pen of Түс * Ен
type internal Қаріп = Font of Size * Family * IsBold * IsItalic
type internal Line = Line of X * Y * X * Y
type internal Rect = Rect of Ен * Биіктік
type internal Triangle = Triangle of X * Y * X * Y * X * Y
type internal Ellipse = Ellipse of Ен * Биіктік

type internal Shape =
   | LineShape of Line * Pen   
   | RectShape of Rect * Pen * Түс
   | TriangleShape of Triangle * Pen * Түс
   | EllipseShape of Ellipse * Pen * Түс
   | ImageShape of Avalonia.Media.IImage ref
   | TextShape of string ref * Қаріп * Түс

type internal Drawing =
   | DrawLine of Line * Pen
   | DrawRect of Rect * Pen
   | DrawTriangle of Triangle * Pen
   | DrawEllipse of Ellipse * Pen
   | DrawImage of Avalonia.Media.IImage ref * float * float
   | DrawText of float * float * string * Қаріп * Түс
   | DrawBoundText of float * float * float * string * Қаріп * Түс
   | FillRect of Rect * Түс
   | FillTriangle of Triangle * Түс
   | FillEllipse of Ellipse * Түс
   | DrawShape of string * Shape

