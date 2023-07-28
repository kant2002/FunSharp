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
   let кXwtЦвету (color:Цвет) = Avalonia.Media.Color.FromArgb(color.A, color.R, color.G, color.B)

type internal Ширина = float
type internal Высота = float
type internal Размер = float
type internal Семейство = string
type internal Жирный = bool
type internal Курсив = bool
type internal X = float
type internal Y = float

type internal Перо = Перо of Цвет * Ширина
type internal Шрифт = Шрифт of Размер * Семейство * Жирный * Курсив
type internal Линия = Линия of X * Y * X * Y
type internal Прямоугольник = Прямоугольник of Ширина * Высота
type internal Треугольник = Треугольник of X * Y * X * Y * X * Y
type internal Эллипс = Эллипс of Ширина * Высота

type internal Фигура =
   | ФигураЛинии of Линия * Перо   
   | ФигураПрямоугольника of Прямоугольник * Перо * Цвет
   | ФигураТреугольника of Треугольник * Перо * Цвет
   | ФигураЭллипса of Эллипс * Перо * Цвет
   | ФигураИзображения of Avalonia.Media.IImage ref
   | ФигураТекста of string ref * Шрифт * Цвет

type internal Рисование =
   | НарисоватьЛинию of Линия * Перо
   | НарисоватьПрямоугольник of Прямоугольник * Перо
   | НарисоватьТреугольник of Треугольник * Перо
   | НарисоватьЭллипс of Эллипс * Перо
   | НарисоватьИзображение of Avalonia.Media.IImage ref * float * float
   | НарисоватьТекст of float * float * string * Шрифт * Цвет
   | НарисоватьТекстВРамке of float * float * float * string * Шрифт * Цвет
   | ЗаполнитьПрямоугольник of Прямоугольник * Цвет
   | ЗаполнитьТреугольник of Треугольник * Цвет
   | ЗаполнитьЭллипс of Эллипс * Цвет
   | НарисоватьФигуру of string * Фигура

