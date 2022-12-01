﻿#r "nuget: Avalonia.Desktop, 11.0.0-preview3"
#r "nuget: Avalonia.Themes.Fluent, 11.0.0-preview3"
#r "../исх/bin/Debug/net7.0/ВеселШарп.Библиотека.dll"

открыть Библиотека

пусть гш = float ГрафическоеОкно.Ширина
пусть гв = float ГрафическоеОкно.Высота
пусть ракетка = Фигуры.ДобавитьПрямоугольник(120, 12)
пусть мяч = Фигуры.ДобавитьЭллипс(16, 16)
пусть изменяемый счет = 0

пусть НаМышьПеремещена () =
  пусть ракеткаX = ГрафическоеОкно.МышьX
  Фигуры.Переместить(ракетка, ракеткаX - 60.0, гв - 12.0)

пусть НапечататьСчет () =
  // Сперва очистим счет и затем нарисуем текст настоящего счета
  ГрафическоеОкно.ЦветКисти <- Цвета.White
  ГрафическоеОкно.ЗаполнитьПрямоугольник(10, 10, 200, 20)
  ГрафическоеОкно.ЦветКисти <- Цвета.Black
  ГрафическоеОкно.НарисоватьТекст(10, 10, "Счет: " + счет.ToString())

ГрафическоеОкно.РазмерШрифта <- 14.0
ГрафическоеОкно.МышьПеремещена <- Callback(НаМышьПеремещена)

НапечататьСчет()
//Звук.ИгратьЗвонок()

пусть изменяемый x = 0.0
пусть изменяемый y = 0.0
пусть изменяемый дельтаX = 1.0
пусть изменяемый дельтаY = 2.0

пока (y < гв) сделать
  x <- x + дельтаX
  y <- y + дельтаY
  
  если (x >= гш - 16.0 || x <= 0.0) тогда
    дельтаX <- -дельтаX 
  если (y <= 0.0) тогда
    дельтаY <- -дельтаY
 
  пусть padX = Фигуры.ПолучитьЛево(ракетка)
  если (y = гв - 28.0 && x >= padX && x <= padX + 120.0) тогда
    //Звук.ИгратьКлик()
    счет <- счет + 10
    НапечататьСчет()
    дельтаY <- -дельтаY  

  Фигуры.Переместить(мяч, x, y)
  Программа.Задержка(15)
  
ГрафическоеОкно.ПоказатьСообщение("Ваш счет: " + счет.ToString(), "Ракетка")
Программа.Задержка(1_500)