﻿#r "nuget: Avalonia.Desktop, 11.0.0-preview4"
#r "nuget: Avalonia.Themes.Fluent, 11.0.0-preview4"
#r "../ісх/bin/Debug/net7.0/ВеселШарп.Бібліотека.dll"

відкрити Бібліотека

нехай кут = 30.0
нехай дельта = 10.0

нехай рек намалюватиДерево відстань =
   якщо відстань > 0.0 тоді
      Черепаха.Перемістити(відстань)
      Черепаха.Повернути(кут)
      намалюватиДерево(відстань - дельта)
      Черепаха.Повернути(-кут * 2.0)
      намалюватиДерево(відстань - дельта)
      Черепаха.Повернути(кут)
      Черепаха.Перемістити(-відстань)

намалюватиДерево 60.0
Програма.Затримка(2_000)