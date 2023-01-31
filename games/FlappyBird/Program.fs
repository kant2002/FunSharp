open Кітапхана

GraphicsWindow.Ен <- 288
GraphicsWindow.Биіктік <- 440

let bg = Пішіндері.AddImage("bg.png")
let ground = Пішіндері.AddImage("ground.png")
let bird_sing = Пішіндері.AddImage("bird_sing.png")
let tube1 = ImageList.LoadImage("tube1.png")
let tube2 = ImageList.LoadImage("tube2.png")
Пішіндері.Move(ground, 0, 340)

/// Bird type
type Bird = { X:float; Y:float; VY:float; IsAlive:bool }
/// Respond to flap command
let flap (bird:Bird) = { bird with VY = - System.Math.PI }
/// Applies gravity to bird
let gravity (bird:Bird) = { bird with VY = bird.VY + 0.1 }
/// Applies physics to bird
let physics (bird:Bird) = { bird with Y = bird.Y + bird.VY }
/// Updates bird with gravity & physics
let update = gravity >> physics
 
/// Generates the level's tube positions
let generateLevel n =
   let rand = System.Random()
   [for i in 1..n -> 50+(i*150), 32+rand.Next(160)]

let level = generateLevel 10

let tubes =
   [for (x,y) in level ->
      let tube1 = Пішіндері.AddImage(tube1)
      let tube2 = Пішіндері.AddImage(tube2)
      Пішіндері.Move(tube1,x,y-320)
      Пішіндері.Move(tube2,x,y+100)
      (x,y), tube1, tube2]

let scroll = ref 0
let flappy = ref { X = 30.0; Y = 150.0; VY = 0.0; IsAlive=true }
let flapme () = if (!flappy).IsAlive then flappy := flap !flappy

GraphicsWindow.KeyDown <-    
    fun () -> if GraphicsWindow.LastKey = "Space" then flapme ()
GraphicsWindow.MouseDown <-
    fun () -> flapme ()

while true do
   flappy := update !flappy
   let bird = !flappy
   Пішіндері.Жылжытуға(bird_sing, bird.X, bird.Y)
   for ((x,y),tube1,tube2) in tubes do
      Пішіндері.Жылжытуға(tube1, float (x + !scroll),float (y-320))
      Пішіндері.Жылжытуға(tube2, float (x + !scroll),float (y+100))
   Пішіндері.Move(ground, !scroll % 48, 340)
   decr scroll  
   Program.Delay(20)
