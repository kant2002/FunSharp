﻿#r "nuget: Xwt"
#r "nuget: Xwt.Wpf"
#r "../исх/bin/Debug/net48/ВеселШарп.Библиотека.dll"

open Библиотека

ГрафическоеОкно.ЦветФона <- Цвета.Black
for i = 1 to 1200 do
   ГрафическоеОкно.ЦветКисти <- ГрафическоеОкно.ПолучитьСлучайныйЦвет()
   ГрафическоеОкно.ЗаполнитьЭллипс(Математика.ВзятьСлучайноеЧисло(800), Математика.ВзятьСлучайноеЧисло(600), 30, 30)
   Программа.Задержка(50)
