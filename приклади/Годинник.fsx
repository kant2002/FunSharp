﻿#r "nuget: Avalonia.Desktop, 11.0.0"
#r "nuget: Avalonia.Themes.Fluent, 11.0.0"
#r "nuget: FSharp.Core.Ukrainian"
#r "../ісх/bin/Debug/net472/ВеселШарп.Бібліотека.dll"

open Бібліотека

let ГШ = float ГрафичнеВікно.Ширина
let ГВ = float ГрафичнеВікно.Висота
let Радіус = 200.0
let СередX = ГШ/2.0
let СередY = ГВ/2.0

let инициализуватиВікно () =
   ГрафичнеВікно.Показати()
   ГрафичнеВікно.Заголовок <- "Analog Clock"
   ГрафичнеВікно.КолірФона <- Кольори.Чорний
   ГрафичнеВікно.КолірПензлика <- Кольори.BurlyWood
   ГрафичнеВікно.НамалюватиЕліпс(СередX-Радіус-15.,СередY-Радіус-5.,Радіус*2.+30.,Радіус*2.+20.)
   ГрафичнеВікно.ЗаповнитиЕліпс(СередX-Радіус-15.,СередY-Радіус-5.,Радіус*2.+30.,Радіус*2.+20.)
   for angle in 1.0..180.0 do
     let x = СередX+(Радіус+15.)*Математика.Кос(Математика.ОтриматиРадіани(angle))
     let y1 = СередY+Радіус*Математика.Син(Математика.ОтриматиРадіани(angle))+15.
     let y2 = СередY+(Радіус+15.)*Математика.Син(Математика.ОтриматиРадіани(-angle))+10.
     let базоваЯскравість = Математика.ОтриматиВипадковеЧисло(40)+30
     ГрафичнеВікно.ШиринаПера <- Математика.ОтриматиВипадковеЧисло(5) |> float
     let колір = 
       ГрафичнеВікно.ОтриматиКолірЗRGB(
         базоваЯскравість+100+Математика.ОтриматиВипадковеЧисло(10),
         базоваЯскравість+60+Математика.ОтриматиВипадковеЧисло(20),
         базоваЯскравість)
     ГрафичнеВікно.КолірПера <- колір
     Фігури.ДодатиЛінію(x,y1,x,y2) |> ігнорувати
   ГрафичнеВікно.КолірПензлика <- Кольори.Білий   
   let ЦифриГодинника = Словник()
   for i in 1. .. 12. do
     let Радіани = Математика.ОтриматиРадіани(-i * 30. + 90.)
     ЦифриГодинника.[i] <- Фігури.ДодатиТекст(i.ToString())
     Фігури.Перемістити(ЦифриГодинника.[i],СередX-4.+Радіус*Математика.Кос(Радіани),СередY-4.-Радіус*Математика.Син(Радіани))   
   
let mutable ГодиннаСтрілка = "<назва фігури>"
let mutable ХвилиннаСтрілка = "<назва фігури>"
let mutable СекунднаСтрілка = "<назва фігури>"
let mutable Година = 0.
let mutable Хвилина = 0.
let mutable Секунда = 0.
let встановитиСтрілки () = 
   if (float Годинник.Година + float Годинник.Хвилина/60. + float Годинник.Секунда/3600. <> Година) then
     Фігури.Видалити(ГодиннаСтрілка)
     Година <- float Годинник.Година + float Годинник.Хвилина/60. + float Годинник.Секунда/3600.
     ГрафичнеВікно.КолірПера <- Кольори.Чорний
     ГрафичнеВікно.ШиринаПера <- 3.
     ГодиннаСтрілка <- 
       Фігури.ДодатиЛінію(
         СередX,
         СередY,
         СередX+Радіус/2.*Математика.Кос(Математика.ОтриматиРадіани(Година*30.-90.)),
         СередY+Радіус/2.*Математика.Син(Математика.ОтриматиРадіани(Година*30.-90.)))   
   if float Годинник.Хвилина <> Хвилина then
     Фігури.Видалити(ХвилиннаСтрілка)
     Хвилина <- float Годинник.Хвилина + float Годинник.Секунда/60.
     ГрафичнеВікно.КолірПера <- Кольори.Синій
     ГрафичнеВікно.ШиринаПера <- 2.
     ХвилиннаСтрілка <- 
       Фігури.ДодатиЛінію(
         СередX,
         СередY,
         СередX+Радіус/1.2*Математика.Кос(Математика.ОтриматиРадіани(Хвилина*6.-90.)),
         СередY+Радіус/1.2*Математика.Син(Математика.ОтриматиРадіани(Хвилина*6.-90.)))   
   if float Годинник.Секунда <> Секунда then
     Фігури.Видалити(СекунднаСтрілка)
     Секунда <- float Годинник.Секунда
     ГрафичнеВікно.КолірПера <- Кольори.Червоний
     ГрафичнеВікно.ШиринаПера <- 1.
     СекунднаСтрілка <- 
       Фігури.ДодатиЛінію(
         СередX,
         СередY,
         СередX+Радіус*Математика.Кос(Математика.ОтриматиРадіани(Секунда*6.-90.)),
         СередY+Радіус*Математика.Син(Математика.ОтриматиРадіани(Секунда*6.-90.)))
   
инициализуватиВікно()
while true do
   встановитиСтрілки()
   Звук.ГратиКлацання()
   Програма.Затримка(1000)