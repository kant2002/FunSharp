﻿#r "nuget: Avalonia.Desktop, 11.0.0-preview3"
#r "nuget: Avalonia.Themes.Fluent, 11.0.0-preview3"
#r "../ісх/bin/Debug/net7.0/ВеселШарп.Бібліотека.dll"

відкрити Бібліотека

ГрафичнеВікно.КолірФона <- Кольори.Black
для i = 1 до 1200 зробити
   ГрафичнеВікно.КолірПензлика <- ГрафичнеВікно.ОтриматиВипадковийКолір()
   ГрафичнеВікно.ЗаповнитиЕліпс(Математика.ОтриматиВипадковеЧисло(800), Математика.ОтриматиВипадковеЧисло(600), 30, 30)
   Програма.Затримка(50)
