простір Бібліотека

тип Колір = 
   struct
      val A:byte
      val R:byte
      val G:byte
      val B:byte
   end
   new (a,r,g,b) = { A=a; R=r; G=g; B=b }

[<AutoOpen>]
module internal ПеретворювачКольорів =
   нехай доКольораАвалонії (color:Колір) = Avalonia.Media.Color.FromArgb(color.A, color.R, color.G, color.B)

тип internal Ширина = float
тип internal Висота = float
тип internal Розмір = float
тип internal Родина = string
тип internal Жирний = bool
тип internal Курсив = bool
тип internal X = float
тип internal Y = float

тип internal Перо = Перо of Колір * Ширина
тип internal Шрифт = Шрифт of Розмір * Родина * Жирний * Курсив
тип internal Лінія = Лінія of X * Y * X * Y
тип internal Прямокутник = Прямокутник of Ширина * Висота
тип internal Трикутник = Трикутник of X * Y * X * Y * X * Y
тип internal Елліпс = Елліпс of Ширина * Висота

тип internal Фігура =
   | ФігураЛінії of Лінія * Перо   
   | ФігураПрямокутника of Прямокутник * Перо * Колір
   | ФігураТрикутника of Трикутник * Перо * Колір
   | ФігураЕлліпса of Елліпс * Перо * Колір
   | ФігураЗображения of Avalonia.Media.IImage ref
   | ФігураТекста of string ref * Шрифт * Колір

тип internal Малювання =
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

