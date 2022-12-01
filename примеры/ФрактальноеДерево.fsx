﻿#r "nuget: Avalonia.Desktop, 11.0.0-preview3"
#r "nuget: Avalonia.Themes.Fluent, 11.0.0-preview3"
#r "../исх/bin/Debug/net7.0/ВеселШарп.Библиотека.dll"

открыть Библиотека

пусть угол = 30.0
пусть дельта = 10.0

пусть рек нарисоватьДерево дистанция =
   если дистанция > 0.0 тогда
      Черепаха.Переместить(дистанция)
      Черепаха.Повернуть(угол)
      нарисоватьДерево(дистанция - дельта)
      Черепаха.Повернуть(-угол * 2.0)
      нарисоватьДерево(дистанция - дельта)
      Черепаха.Повернуть(угол)
      Черепаха.Переместить(-дистанция)

нарисоватьДерево 60.0
Программа.Задержка(2_000)