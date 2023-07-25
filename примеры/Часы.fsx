﻿#r "nuget: Avalonia.Desktop, 11.0.0"
#r "nuget: Avalonia.Themes.Fluent, 11.0.0"
#r "../исх/bin/Debug/net7.0/ВеселШарп.Библиотека.dll"

открыть Библиотека

пусть ГШ = float ГрафическоеОкно.Ширина
пусть ГВ = float ГрафическоеОкно.Высота
пусть Радиус = 200.0
пусть СредX = ГШ/2.0
пусть СредY = ГВ/2.0

пусть инициализироватьОкно () =
   ГрафическоеОкно.Показать()
   ГрафическоеОкно.Заголовок <- "Analog Clock"
   ГрафическоеОкно.ЦветФона <- Цвета.Black
   ГрафическоеОкно.ЦветКисти <- Цвета.BurlyWood
   ГрафическоеОкно.НарисоватьЭллипс(СредX-Радиус-15.,СредY-Радиус-5.,Радиус*2.+30.,Радиус*2.+20.)
   ГрафическоеОкно.ЗаполнитьЭллипс(СредX-Радиус-15.,СредY-Радиус-5.,Радиус*2.+30.,Радиус*2.+20.)
   для angle в 1.0..180.0 сделать
     пусть x = СредX+(Радиус+15.)*Математика.Кос(Математика.ВзятьРадианы(angle))
     пусть y1 = СредY+Радиус*Математика.Син(Математика.ВзятьРадианы(angle))+15.
     пусть y2 = СредY+(Радиус+15.)*Математика.Син(Математика.ВзятьРадианы(-angle))+10.
     пусть базоваяЯркость = Математика.ВзятьСлучайноеЧисло(40)+30
     ГрафическоеОкно.ШиринаПера <- Математика.ВзятьСлучайноеЧисло(5) |> float
     пусть цвет = 
       ГрафическоеОкно.ПолучитьЦветИзRGB(
         базоваяЯркость+100+Математика.ВзятьСлучайноеЧисло(10),
         базоваяЯркость+60+Математика.ВзятьСлучайноеЧисло(20),
         базоваяЯркость)
     ГрафическоеОкно.ЦветПера <- цвет
     Фигуры.ДобавитьЛинию(x,y1,x,y2) |> ignore
   ГрафическоеОкно.ЦветКисти <- Цвета.White   
   пусть ЦифрыЧасов = Словарь()
   для i в 1. .. 12. сделать
     пусть Радианы = Математика.ВзятьРадианы(-i * 30. + 90.)
     ЦифрыЧасов.[i] <- Фигуры.ДобавитьТекст(i.ToString())
     Фигуры.Переместить(ЦифрыЧасов.[i],СредX-4.+Радиус*Математика.Кос(Радианы),СредY-4.-Радиус*Математика.Син(Радианы))   
   
пусть изменяемый ЧасоваяСтрелка = "<shape name>"
пусть изменяемый МинутнаяСтрелка = "<shape name>"
пусть изменяемый СекунднаяСтрелка = "<shape name>"
пусть изменяемый Час = 0.
пусть изменяемый Минута = 0.
пусть изменяемый Секунда = 0.
пусть установитьСтрелки () = 
   если (float Часы.Час + float Часы.Минута/60. + float Часы.Секунда/3600. <> Час) тогда
     Фигуры.Удалить(ЧасоваяСтрелка)
     Час <- float Часы.Час + float Часы.Минута/60. + float Часы.Секунда/3600.
     ГрафическоеОкно.ЦветПера <- Цвета.Black
     ГрафическоеОкно.ШиринаПера <- 3.
     ЧасоваяСтрелка <- 
       Фигуры.ДобавитьЛинию(
         СредX,
         СредY,
         СредX+Радиус/2.*Математика.Кос(Математика.ВзятьРадианы(Час*30.-90.)),
         СредY+Радиус/2.*Математика.Син(Математика.ВзятьРадианы(Час*30.-90.)))   
   если float Часы.Минута <> Минута тогда
     Фигуры.Удалить(МинутнаяСтрелка)
     Минута <- float Часы.Минута + float Часы.Секунда/60.
     ГрафическоеОкно.ЦветПера <- Цвета.Blue
     ГрафическоеОкно.ШиринаПера <- 2.
     МинутнаяСтрелка <- 
       Фигуры.ДобавитьЛинию(
         СредX,
         СредY,
         СредX+Радиус/1.2*Математика.Кос(Математика.ВзятьРадианы(Минута*6.-90.)),
         СредY+Радиус/1.2*Математика.Син(Математика.ВзятьРадианы(Минута*6.-90.)))   
   если float Часы.Секунда <> Секунда тогда
     Фигуры.Удалить(СекунднаяСтрелка)
     Секунда <- float Часы.Секунда
     ГрафическоеОкно.ЦветПера <- Цвета.Red
     ГрафическоеОкно.ШиринаПера <- 1.
     СекунднаяСтрелка <- 
       Фигуры.ДобавитьЛинию(
         СредX,
         СредY,
         СредX+Радиус*Математика.Кос(Математика.ВзятьРадианы(Секунда*6.-90.)),
         СредY+Радиус*Математика.Син(Математика.ВзятьРадианы(Секунда*6.-90.)))
   
инициализироватьОкно()
пока истина сделать
   установитьСтрелки()
   //Звук.ИгратьКлик()
   Программа.Задержка(1000)