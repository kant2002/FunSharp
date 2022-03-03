#r "nuget: Xwt"
#r "../src/bin/Debug/net48/FunSharp.Library.dll"

open Library

let gw = float ГрафическоеОкно.Ширина
let gh = float ГрафическоеОкно.Высота
let paddle = Shapes.AddRectangle(120, 12)
let ball = Shapes.AddEllipse(16, 16)
let mutable score = 0

let OnMouseMove () =
  let paddleX = ГрафическоеОкно.MouseX
  Shapes.Move(paddle, paddleX - 60.0, gh - 12.0)

let PrintScore () =
  // Clear the score first and then draw the real score text
  ГрафическоеОкно.ЦветКисти <- Colors.White
  ГрафическоеОкно.ЗаполнитьПрямоугольник(10, 10, 200, 20)
  ГрафическоеОкно.ЦветКисти <- Colors.Black
  ГрафическоеОкно.НарисоватьТекст(10, 10, "Score: " + score.ToString())

ГрафическоеОкно.FontSize <- 14.0
ГрафическоеОкно.MouseMove <- Callback(OnMouseMove)

PrintScore()
//Sound.PlayBellRingAndWait()

let mutable x = 0.0
let mutable y = 0.0
let mutable deltaX = 1.0
let mutable deltaY = 2.0

while (y < gh) do
  x <- x + deltaX
  y <- y + deltaY
  
  if (x >= gw - 16.0 || x <= 0.0) then
    deltaX <- -deltaX 
  if (y <= 0.0) then
    deltaY <- -deltaY
 
  let padX = Shapes.GetLeft(paddle)
  if (y = gh - 28.0 && x >= padX && x <= padX + 120.0) then
    //Sound.PlayClick()
    score <- score + 10
    PrintScore()
    deltaY <- -deltaY  

  Shapes.Move(ball, x, y)
  Program.Delay(15)
  
ГрафическоеОкно.ShowMessage("Your score is: " + score.ToString(), "Paddle")
Program.Delay(1_500)