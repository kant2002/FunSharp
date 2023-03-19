open Кітапхана

let gw = float ГрафикалықТерезе.Ен
let gh = float ГрафикалықТерезе.Биіктік
let paddle = Пішіндері.AddRectangle(120, 12)
let ball = Пішіндері.AddEllipse(16, 16)
let mutable score = 0

let OnMouseMove () =
  let paddleX = ГрафикалықТерезе.ТінтуірX
  Пішіндері.Жылжытуға(paddle, paddleX - 60.0, gh - 12.0)

let PrintScore () =
  // Clear the score first and then draw the real score text
  ГрафикалықТерезе.ҚылқаламТүсі <- Түстер.Ақ
  ГрафикалықТерезе.FillRectangle(10, 10, 200, 20)
  ГрафикалықТерезе.ҚылқаламТүсі <- Түстер.Қара
  ГрафикалықТерезе.DrawText(10, 10, "Score: " + score.ToString())

ГрафикалықТерезе.FontSize <- 14.0
ГрафикалықТерезе.MouseMove <- Callback(OnMouseMove)

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
  
ГрафикалықТерезе.ShowMessage("Your score is: " + score.ToString(), "Paddle")
