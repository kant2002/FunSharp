namespace Бібліотека

type Колір = 
   struct
      val A:byte
      val R:byte
      val G:byte
      val B:byte
   end
   new (a,r,g,b) = { A=a; R=r; G=g; B=b }

[<AutoOpen>]
module internal ПеретворювачКольорів =
   let доКольораАвалонії (color:Колір) = Avalonia.Media.Color.FromArgb(color.A, color.R, color.G, color.B)

type internal Ширина = float
type internal Висота = float
type internal Розмір = float
type internal Родина = string
type internal Жирний = bool
type internal Курсив = bool
type internal X = float
type internal Y = float

type internal Перо = Перо of Колір * Ширина
type internal Шрифт = Шрифт of Розмір * Родина * Жирний * Курсив
type internal Лінія = Лінія of X * Y * X * Y
type internal Прямокутник = Прямокутник of Ширина * Висота
type internal Трикутник = Трикутник of X * Y * X * Y * X * Y
type internal Елліпс = Елліпс of Ширина * Висота

type internal Фігура =
   | ФігураЛінії of Лінія * Перо   
   | ФігураПрямокутника of Прямокутник * Перо * Колір
   | ФігураТрикутника of Трикутник * Перо * Колір
   | ФігураЕлліпса of Елліпс * Перо * Колір
   | ФігураЗображения of Avalonia.Media.IImage ref
   | ФігураТекста of string ref * Шрифт * Колір

type internal Малювання =
   | НамалюватиЛінію of Лінія * Перо
   | НамалюватиПрямокутник of Прямокутник * Перо
   | НамалюватиТрикутник of Трикутник * Перо
   | НамалюватиЕлліпс of Елліпс * Перо
   | НамалюватиЗображення of Avalonia.Media.IImage ref * float * float
   | НамалюватиТекст of float * float * string * Шрифт * Колір
   | НамалюватиТекстУРамці of float * float * float * string * Шрифт * Колір
   | ЗаповнитиПрямокутник of Прямокутник * Колір
   | ЗаповнитиТрикутник of Трикутник * Колір
   | ЗаповнитиЕліпс of Елліпс * Колір
   | НамалюватиФігуру of string * Фігура

