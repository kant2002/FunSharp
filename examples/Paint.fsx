﻿#r "nuget: Avalonia.Desktop, 11.0.0-preview3"
#r "nuget: Avalonia.Themes.Fluent, 11.0.0-preview3"
#r "../src/bin/Debug/net7.0/FunSharp.Library.dll"

open Library

let onKeyDown () =
   match GraphicsWindow.LastKey with
   | "K1" -> GraphicsWindow.PenColor <- Colors.Red
   | "K2" -> GraphicsWindow.PenColor <- Colors.Blue
   | "K3" -> GraphicsWindow.PenColor <- Colors.LightGreen
   | "c" -> GraphicsWindow.Clear()
   | s -> printfn "'%s'" s; System.Diagnostics.Debug.WriteLine(s)

let mutable prevX = 0.0
let mutable prevY = 0.0

let onMouseDown () =
   prevX <- GraphicsWindow.MouseX
   prevY <- GraphicsWindow.MouseY
   
let onMouseMove () =
   let x = GraphicsWindow.MouseX
   let y = GraphicsWindow.MouseY
   if Mouse.IsLeftButtonDown then
      GraphicsWindow.DrawLine(prevX, prevY, x, y)
   prevX <- x
   prevY <- y

GraphicsWindow.BackgroundColor <- Colors.Black
GraphicsWindow.PenColor <- Colors.White
GraphicsWindow.MouseDown <- Callback(onMouseDown)
GraphicsWindow.MouseMove <- Callback(onMouseMove)
GraphicsWindow.KeyDown <- Callback(onKeyDown)
