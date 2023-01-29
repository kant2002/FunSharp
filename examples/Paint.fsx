﻿#r "nuget: Avalonia.Desktop, 11.0.0-preview3"
#r "nuget: Avalonia.Themes.Fluent, 11.0.0-preview3"
#r "../src/bin/Debug/net7.0/FunSharp.Library.dll"

open Library

let onKeyDown () =
   match GraphicsWindow.LastKey with
   | "K1" -> GraphicsWindow.PenColor <- Түстер.Red
   | "K2" -> GraphicsWindow.PenColor <- Түстер.Blue
   | "K3" -> GraphicsWindow.PenColor <- Түстер.LightGreen
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

GraphicsWindow.АяТүсі <- Түстер.Black
GraphicsWindow.PenColor <- Түстер.White
GraphicsWindow.MouseDown <- Callback(onMouseDown)
GraphicsWindow.MouseMove <- Callback(onMouseMove)
GraphicsWindow.KeyDown <- Callback(onKeyDown)