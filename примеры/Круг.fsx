﻿#r "nuget: Xwt"
#r "../src/bin/Debug/net48/FunSharp.Library.dll"

open Библиотека

ГрафическоеОкно.ЦветФона <- Цвета.Black
for i = 1 to 1200 do
   ГрафическоеОкно.ЦветКисти <- ГрафическоеОкно.ПолучитьСлучайныйЦвет()
   ГрафическоеОкно.ЗаполнитьЭллипс(Math.ВзятьСлучайноеЧисло(800), Math.ВзятьСлучайноеЧисло(600), 30, 30)
   Программа.Задержка(50)
