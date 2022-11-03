﻿#r "nuget: Avalonia.Desktop, 11.0.0-preview3"
#r "nuget: Avalonia.Themes.Fluent, 11.0.0-preview3"
#r "../src/bin/Debug/net7.0/FunSharp.Library.dll"

open Library

let ball = Shapes.AddRectangle(200.0, 100.0)

let OnMouseDown () =
  let x = GraphicsWindow.MouseX
  let y = GraphicsWindow.MouseY
  Shapes.Move(ball, x, y)

GraphicsWindow.MouseDown <- Callback(OnMouseDown)
