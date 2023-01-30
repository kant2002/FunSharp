#r "nuget: Avalonia.Desktop, 11.0.0-preview3"
#r "nuget: Avalonia.Themes.Fluent, 11.0.0-preview3"
#r "../src/bin/Debug/net7.0/FunSharp.Library.dll"

open Кітапхана

let bg = Shapes.AddImage("http://flappycreator.com/default/bg.png")
let ground = Shapes.AddImage("http://flappycreator.com/default/ground.png")
let bird = Shapes.AddImage("http://flappycreator.com/default/bird_sing.png")
let tube1 = ImageList.LoadImage("http://flappycreator.com/default/tube1.png")
let tube2 = ImageList.LoadImage("http://flappycreator.com/default/tube2.png")
let t1 = Shapes.AddImage(tube1)
let t2 = Shapes.AddImage(tube2)

Shapes.Move(t1, 150.0, 50.0-320.0)
Shapes.Move(t2, 150.0, 150.0)
Shapes.Move(ground, 0.0, 340.0)
Shapes.Rotate(bird,45.0*4.0)
Shapes.Move(bird,50.0,100.0)
GraphicsWindow.Show()
GraphicsWindow.Ен <- 288
GraphicsWindow.Биіктік <- 440
