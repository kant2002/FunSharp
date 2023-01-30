#r "nuget: Avalonia.Desktop, 11.0.0-preview3"
#r "nuget: Avalonia.Themes.Fluent, 11.0.0-preview3"
#r "../src/bin/Debug/net7.0/FunSharp.Library.dll"

open Library

let саны = 30.0
let дельта = 10.0

let rec салуАғашы қашықтық =
   if қашықтық > 0.0 then
      Тасбақа.Жылжытуға(қашықтық)
      Тасбақа.Бұру(саны)
      салуАғашы(қашықтық - дельта)
      Тасбақа.Бұру(-саны * 2.0)
      салуАғашы(қашықтық - дельта)
      Тасбақа.Бұру(саны)
      Тасбақа.Жылжытуға(-қашықтық)

салуАғашы 60.0