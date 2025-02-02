﻿#r "nuget: Avalonia.Desktop, 11.0.0"
#r "nuget: Avalonia.Themes.Fluent, 11.0.0"
#r "../ісх/bin/Debug/net7.0/ВеселШарп.Бібліотека.dll"

відкрити Бібліотека

нехай ГШ = float ГрафичнеВікно.Ширина
нехай ГВ = float ГрафичнеВікно.Висота
нехай Радіус = 200.0
нехай СередX = ГШ/2.0
нехай СередY = ГВ/2.0

нехай инициализуватиВікно () =
   ГрафичнеВікно.Показати()
   ГрафичнеВікно.Заголовок <- "Analog Clock"
   ГрафичнеВікно.КолірФона <- Кольори.Black
   ГрафичнеВікно.КолірПензлика <- Кольори.BurlyWood
   ГрафичнеВікно.НамалюватиЕліпс(СередX-Радіус-15.,СередY-Радіус-5.,Радіус*2.+30.,Радіус*2.+20.)
   ГрафичнеВікно.ЗаповнитиЕліпс(СередX-Радіус-15.,СередY-Радіус-5.,Радіус*2.+30.,Радіус*2.+20.)
   для angle у 1.0..180.0 зробити
     нехай x = СередX+(Радіус+15.)*Математика.Кос(Математика.ОтриматиРадіани(angle))
     нехай y1 = СередY+Радіус*Математика.Син(Математика.ОтриматиРадіани(angle))+15.
     нехай y2 = СередY+(Радіус+15.)*Математика.Син(Математика.ОтриматиРадіани(-angle))+10.
     нехай базоваЯскравість = Математика.ОтриматиВипадковеЧисло(40)+30
     ГрафичнеВікно.ШиринаПера <- Математика.ОтриматиВипадковеЧисло(5) |> float
     нехай колір = 
       ГрафичнеВікно.ОтриматиКолірЗRGB(
         базоваЯскравість+100+Математика.ОтриматиВипадковеЧисло(10),
         базоваЯскравість+60+Математика.ОтриматиВипадковеЧисло(20),
         базоваЯскравість)
     ГрафичнеВікно.КолірПера <- колір
     Фігури.ДодатиЛінію(x,y1,x,y2) |> ignore
   ГрафичнеВікно.КолірПензлика <- Кольори.White   
   нехай ЦифриГодинника = Словник()
   для i у 1. .. 12. зробити
     нехай Радіани = Математика.ОтриматиРадіани(-i * 30. + 90.)
     ЦифриГодинника.[i] <- Фігури.ДодатиТекст(i.ToString())
     Фігури.Перемістити(ЦифриГодинника.[i],СередX-4.+Радіус*Математика.Кос(Радіани),СередY-4.-Радіус*Математика.Син(Радіани))   
   
нехай змінливий ГодиннаСтрілка = "<назва фігури>"
нехай змінливий ХвилиннаСтрілка = "<назва фігури>"
нехай змінливий СекунднаСтрілка = "<назва фігури>"
нехай змінливий Година = 0.
нехай змінливий Хвилина = 0.
нехай змінливий Секунда = 0.
нехай встановитиСтрілки () = 
   якщо (float Годинник.Година + float Годинник.Хвилина/60. + float Годинник.Секунда/3600. <> Година) тоді
     Фігури.Видалити(ГодиннаСтрілка)
     Година <- float Годинник.Година + float Годинник.Хвилина/60. + float Годинник.Секунда/3600.
     ГрафичнеВікно.КолірПера <- Кольори.Black
     ГрафичнеВікно.ШиринаПера <- 3.
     ГодиннаСтрілка <- 
       Фігури.ДодатиЛінію(
         СередX,
         СередY,
         СередX+Радіус/2.*Математика.Кос(Математика.ОтриматиРадіани(Година*30.-90.)),
         СередY+Радіус/2.*Математика.Син(Математика.ОтриматиРадіани(Година*30.-90.)))   
   якщо float Годинник.Хвилина <> Хвилина тоді
     Фігури.Видалити(ХвилиннаСтрілка)
     Хвилина <- float Годинник.Хвилина + float Годинник.Секунда/60.
     ГрафичнеВікно.КолірПера <- Кольори.Blue
     ГрафичнеВікно.ШиринаПера <- 2.
     ХвилиннаСтрілка <- 
       Фігури.ДодатиЛінію(
         СередX,
         СередY,
         СередX+Радіус/1.2*Математика.Кос(Математика.ОтриматиРадіани(Хвилина*6.-90.)),
         СередY+Радіус/1.2*Математика.Син(Математика.ОтриматиРадіани(Хвилина*6.-90.)))   
   якщо float Годинник.Секунда <> Секунда тоді
     Фігури.Видалити(СекунднаСтрілка)
     Секунда <- float Годинник.Секунда
     ГрафичнеВікно.КолірПера <- Кольори.Red
     ГрафичнеВікно.ШиринаПера <- 1.
     СекунднаСтрілка <- 
       Фігури.ДодатиЛінію(
         СередX,
         СередY,
         СередX+Радіус*Математика.Кос(Математика.ОтриматиРадіани(Секунда*6.-90.)),
         СередY+Радіус*Математика.Син(Математика.ОтриматиРадіани(Секунда*6.-90.)))
   
инициализуватиВікно()
доки істина зробити
   встановитиСтрілки()
   //Звук.ИгратьКлик()
   Програма.Затримка(1000)