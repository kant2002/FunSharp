﻿#r "nuget: Xwt"
#r "../src/bin/Debug/net48/FunSharp.Library.dll"

open Библиотека
open System.Threading

let угол = 30.0
let дельта = 10.0

let rec нарисоватьДерево дистанция =
   if дистанция > 0.0 then
      Черепаха.Переместить(дистанция)
      Черепаха.Повернуть(угол)
      нарисоватьДерево(дистанция - дельта)
      Черепаха.Повернуть(-угол * 2.0)
      нарисоватьДерево(дистанция - дельта)
      Черепаха.Повернуть(угол)
      Черепаха.Переместить(-дистанция)

нарисоватьДерево 60.0
Thread.Sleep 2_000