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
   let авалонияТүске (түс:Түс) = Avalonia.Media.Color.FromArgb(түс.R, түс.G, түс.B, түс.A)

type internal Ен = float
type internal Биіктік = float
type internal Көлем = float
type internal Топ = string
type internal ҚалыңБар = bool
type internal КурсивБар = bool
type internal X = float
type internal Y = float

type internal Қауырсын = Қауырсын of Түс * Ен
type internal Қаріп = Қаріп of Көлем * Топ * ҚалыңБар * КурсивБар
type internal Сызық = Сызық of X * Y * X * Y
type internal Тік = Rect of Ен * Биіктік // тікбұрыш
type internal Үшбұрыш = Үшбұрыш of X * Y * X * Y * X * Y
type internal Эллипс = Эллипс of Ен * Биіктік

type internal Пішін =
   | СызықПішіні of Сызық * Қауырсын   
   | ТікПішіні of Тік * Қауырсын * Түс
   | ҮшбұрышПішіні of Үшбұрыш * Қауырсын * Түс
   | ЭллипсПішіні of Эллипс * Қауырсын * Түс
   | КескінПішіні of Avalonia.Media.IImage ref
   | МәтінПішіні of string ref * Қаріп * Түс

type internal Drawing =
   | DrawLine of Сызық * Қауырсын
   | DrawRect of Тік * Қауырсын
   | DrawTriangle of Үшбұрыш * Қауырсын
   | DrawEllipse of Эллипс * Қауырсын
   | DrawImage of Avalonia.Media.IImage ref * float * float
   | DrawText of float * float * string * Қаріп * Түс
   | DrawBoundText of float * float * float * string * Қаріп * Түс
   | FillRect of Тік * Түс
   | FillTriangle of Үшбұрыш * Түс
   | FillEllipse of Эллипс * Түс
   | DrawShape of string * Пішін

