#r "nuget: Avalonia.Desktop, 11.0.0-preview3"
#r "nuget: Avalonia.Themes.Fluent, 11.0.0-preview3"
#r "../src/bin/Debug/net7.0/FunSharp.Library.dll"

open Кітапхана

GraphicsWindow.ҚаламТүсі <- Түстер.Күлгін
Тасбақа.X <- 150.
Тасбақа.Y <- 150.
for i in 0..5..200 do
   Тасбақа.Жылжытуға(i)
   Тасбақа.Бұру(90)
