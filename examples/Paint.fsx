#r "nuget: Avalonia.Desktop, 11.0.0-preview3"
#r "nuget: Avalonia.Themes.Fluent, 11.0.0-preview3"
#r "../src/bin/Debug/net7.0/FunSharp.Library.dll"

open Кітапхана

let onKeyDown () =
   match ГрафикалықТерезе.LastKey with
   | "K1" -> ГрафикалықТерезе.ҚаламТүсі <- Түстер.Red
   | "K2" -> ГрафикалықТерезе.ҚаламТүсі <- Түстер.Blue
   | "K3" -> ГрафикалықТерезе.ҚаламТүсі <- Түстер.LightGreen
   | "c" -> ГрафикалықТерезе.Clear()
   | s -> printfn "'%s'" s; System.Diagnostics.Debug.WriteLine(s)

let mutable prevX = 0.0
let mutable prevY = 0.0

let onMouseDown () =
   prevX <- ГрафикалықТерезе.ТінтуірX
   prevY <- ГрафикалықТерезе.ТінтуірY
   
let onMouseMove () =
   let x = ГрафикалықТерезе.ТінтуірX
   let y = ГрафикалықТерезе.ТінтуірY
   if Mouse.IsLeftButtonDown then
      ГрафикалықТерезе.DrawLine(prevX, prevY, x, y)
   prevX <- x
   prevY <- y

ГрафикалықТерезе.ФонныңТүсі <- Түстер.Қара
ГрафикалықТерезе.ҚаламТүсі <- Түстер.Ақ
ГрафикалықТерезе.MouseDown <- Callback(onMouseDown)
ГрафикалықТерезе.MouseMove <- Callback(onMouseMove)
ГрафикалықТерезе.KeyDown <- Callback(onKeyDown)
