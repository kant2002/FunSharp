open Кітапхана

let gw = float GraphicsWindow.Ен
let gh = float GraphicsWindow.Биіктік
let paddle = Пішіндері.AddRectangle(120, 12)
let ball = Пішіндері.AddEllipse(16, 16)
let mutable score = 0

let OnMouseMove () =
  let paddleX = GraphicsWindow.ТінтуірX
  Пішіндері.Жылжытуға(paddle, paddleX - 60.0, gh - 12.0)

let PrintScore () =
  // Clear the score first and then draw the real score text
  GraphicsWindow.ҚылқаламТүсі <- Түстер.Ақ
  GraphicsWindow.FillRectangle(10, 10, 200, 20)
  GraphicsWindow.ҚылқаламТүсі <- Түстер.Қара
  GraphicsWindow.DrawText(10, 10, "Score: " + score.ToString())

GraphicsWindow.FontSize <- 14.0
GraphicsWindow.MouseMove <- Callback(OnMouseMove)

PrintScore()
Дыбыс.ТартуҚоңырауды (*AndWait*) ()

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
 
  let padX = Пішіндері.GetLeft(paddle)
  if (y = gh - 28.0 && x >= padX && x <= padX + 120.0) then
    //Sound.PlayClick()
    score <- score + 10
    PrintScore()
    deltaY <- -deltaY  

  Пішіндері.Жылжытуға(ball, x, y)
  Program.Delay(15)
  
GraphicsWindow.ShowMessage("Your score is: " + score.ToString(), "Paddle")
