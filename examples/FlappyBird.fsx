#r "nuget: Avalonia.Desktop, 11.0.0-preview3"
#r "nuget: Avalonia.Themes.Fluent, 11.0.0-preview3"
#r "../src/bin/Debug/net7.0/FunSharp.Library.dll"

open Кітапхана

let bg = Пішіндері.AddImage("http://flappycreator.com/default/bg.png")
let ground = Пішіндері.AddImage("http://flappycreator.com/default/ground.png")
let bird = Пішіндері.AddImage("http://flappycreator.com/default/bird_sing.png")
let tube1 = ImageList.LoadImage("http://flappycreator.com/default/tube1.png")
let tube2 = ImageList.LoadImage("http://flappycreator.com/default/tube2.png")
let t1 = Пішіндері.AddImage(tube1)
let t2 = Пішіндері.AddImage(tube2)

Пішіндері.Жылжытуға(t1, 150.0, 50.0-320.0)
Пішіндері.Жылжытуға(t2, 150.0, 150.0)
Пішіндері.Жылжытуға(ground, 0.0, 340.0)
Пішіндері.Rotate(bird,45.0*4.0)
Пішіндері.Жылжытуға(bird,50.0,100.0)
GraphicsWindow.Show()
GraphicsWindow.Ен <- 288
GraphicsWindow.Биіктік <- 440
