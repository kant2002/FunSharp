#r "nuget: Avalonia.Desktop, 11.0.0-preview3"
#r "nuget: Avalonia.Themes.Fluent, 11.0.0-preview3"
#r "../src/bin/Debug/net7.0/FunSharp.Library.dll"

open Library

GraphicsWindow.PenColor <- Colors.Purple
Turtle.X <- 150.
Turtle.Y <- 150.
for i in 0..5..200 do
   Turtle.Move(i)
   Turtle.Turn(90)
