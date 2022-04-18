namespace Бiблiотека

type Колір = 
   struct
      val A:byte
      val R:byte
      val G:byte
      val B:byte
   end
   new (a,r,g,b) = { A=a; R=r; G=g; B=b }

[<AutoOpen>]
module internal КонвертерЦвета =
   let кXwtЦвету (color:Колір) = Xwt.Drawing.Color.FromBytes(color.R, color.G, color.B, color.A)

type internal Ширина = float
type internal Высота = float
type internal Размер = float
type internal Семейство = string
type internal Жирный = bool
type internal Курсив = bool
type internal X = float
type internal Y = float

type internal Перо = Перо of Колір * Ширина
type internal Шрифт = Шрифт of Размер * Семейство * Жирный * Курсив
type internal Линия = Линия of X * Y * X * Y
type internal Прямоугольник = Прямоугольник of Ширина * Высота
type internal Треугольник = Треугольник of X * Y * X * Y * X * Y
type internal Эллипс = Эллипс of Ширина * Высота

type internal Фигура =
   | ФигураЛинии of Линия * Перо   
   | ФигураПрямоугольника of Прямоугольник * Перо * Колір
   | ФигураТреугольника of Треугольник * Перо * Колір
   | ФигураЭллипса of Эллипс * Перо * Колір
   | ФигураИзображения of Xwt.Drawing.Image ref
   | ФигураТекста of string ref * Шрифт * Колір

type internal Рисование =
   | НарисоватьЛинию of Линия * Перо
   | НарисоватьПрямоугольник of Прямоугольник * Перо
   | НарисоватьТреугольник of Треугольник * Перо
   | НарисоватьЭллипс of Эллипс * Перо
   | НарисоватьИзображение of Xwt.Drawing.Image ref * float * float
   | НарисоватьТекст of float * float * string * Шрифт * Колір
   | НарисоватьТекстВРамке of float * float * float * string * Шрифт * Колір
   | ЗаполнитьПрямоугольник of Прямоугольник * Колір
   | ЗаполнитьТреугольник of Треугольник * Колір
   | ЗаполнитьЭллипс of Эллипс * Колір
   | НарисоватьФигуру of string * Фигура

