#r "nuget: Avalonia.Desktop, 11.0.0-preview3"
#r "nuget: Avalonia.Themes.Fluent, 11.0.0-preview3"
#r "../src/bin/Debug/net7.0/FunSharp.Library.dll"

open Кітапхана

let доп = Пішіндері.AddRectangle(200.0, 100.0)

let OnMouseDown () =
  let x = ГрафикалықТерезе.ТінтуірX
  let y = ГрафикалықТерезе.ТінтуірY
  Пішіндері.Жылжытуға(доп, x, y)

ГрафикалықТерезе.MouseDown <- Callback(OnMouseDown)
