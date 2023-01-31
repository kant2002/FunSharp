namespace Кітапхана

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
type internal Көлем = float
type internal Топ = string
type internal IsBold = bool
type internal IsItalic = bool
type internal X = float
type internal Y = float

type internal Қауырсын = Қауырсын of Түс * Ен
type internal Қаріп = Font of Көлем * Топ * IsBold * IsItalic
type internal Сызық = Сызық of X * Y * X * Y
type internal Rect = Rect of Ен * Биіктік
type internal Үшбұрыш = Үшбұрыш of X * Y * X * Y * X * Y
type internal Эллипс = Эллипс of Ен * Биіктік

type internal Shape =
   | LineShape of Сызық * Қауырсын   
   | RectShape of Rect * Қауырсын * Түс
   | TriangleShape of Үшбұрыш * Қауырсын * Түс
   | EllipseShape of Эллипс * Қауырсын * Түс
   | ImageShape of Avalonia.Media.IImage ref
   | TextShape of string ref * Қаріп * Түс

type internal Drawing =
   | DrawLine of Сызық * Қауырсын
   | DrawRect of Rect * Қауырсын
   | DrawTriangle of Үшбұрыш * Қауырсын
   | DrawEllipse of Эллипс * Қауырсын
   | DrawImage of Avalonia.Media.IImage ref * float * float
   | DrawText of float * float * string * Қаріп * Түс
   | DrawBoundText of float * float * float * string * Қаріп * Түс
   | FillRect of Rect * Түс
   | FillTriangle of Үшбұрыш * Түс
   | FillEllipse of Эллипс * Түс
   | DrawShape of string * Shape

