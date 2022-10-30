﻿namespace Бiблiотека

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
   let кXwtЦвету (color:Колір) = Avalonia.Media.Color.FromArgb(color.A, color.R, color.G, color.B)

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
type internal Прямоугольник = Прямокутник of Ширина * Высота
type internal Треугольник = Треугольник of X * Y * X * Y * X * Y
type internal Эллипс = Эллипс of Ширина * Высота

type internal Фигура =
   | ФигураЛинии of Линия * Перо   
   | ФигураПрямоугольника of Прямоугольник * Перо * Колір
   | ФигураТреугольника of Треугольник * Перо * Колір
   | ФигураЭллипса of Эллипс * Перо * Колір
   | ФигураИзображения of Avalonia.Media.IImage ref
   | ФигураТекста of string ref * Шрифт * Колір

type internal Рисование =
   | НамалюватиЛінію of Линия * Перо
   | НамалюватиПрямокутник of Прямоугольник * Перо
   | НамалюватиТрикутник of Треугольник * Перо
   | НамалюватиЕліпс of Эллипс * Перо
   | НамалюватиЗображення of Avalonia.Media.IImage ref * float * float
   | НамалюватиТекст of float * float * string * Шрифт * Колір
   | НамалюватиТекстУРамці of float * float * float * string * Шрифт * Колір
   | ЗаполнитьПрямоугольник of Прямоугольник * Колір
   | ЗаполнитьТреугольник of Треугольник * Колір
   | ЗаповнитиЕліпс of Эллипс * Колір
   | НамалюватиФігуру of string * Фигура

