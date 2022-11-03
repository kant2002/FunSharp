#if INTERACTIVE
#r "nuget: Avalonia.Desktop, 11.0.0-preview3"
#r "nuget: Avalonia.Themes.Fluent, 11.0.0-preview3"
#r "../../ісх/bin/Debug/net7.0/ВеселШарп.Бібліотека.dll"
#endif

open Бібліотека

let mutable КВАДРАТИ = 4      // number of boxes per piece
let mutable ШИРИНАК = 25    // box width in pixels
let mutable ЗМІЩЕННЯX = 40   // Screen X offset in pixels of where the board starts
let mutable ЗМІЩЕННЯY = 40   // Screen Y offset in pixels of where the board starts
let mutable ШИРИНАХ = 10    // Canvas Width, in number of boxes
let mutable ВИСОТАХ = 20   // Canvas Height, in number of boxes.
let mutable ПОЧАТКОВАЗАТРИМКА = 800
let mutable КІНЦЕВАЗАТРИМКА = 175
let mutable ПЕРЕДГЛЯД_xпоз = 13
let mutable ПЕРЕДГЛЯД_yпоз = 2

let mutable шаблон = ""
let mutable базовийшаблон = ""
let mutable обертання = ""
let mutable h = ""
let mutable наступнийШматок = ""
let mutable кількістьh = 0
let mutable xпоз = 0
let mutable yпоз = 0
let mutable зроблено = 0
let mutable напрямокРуху = 0
let mutable неправильнийРух = 0
let mutable затримка = 0
let mutable рахунок = 0

type Шаблон = {
   Значення : int[]
   mutable Колір : Колір
   mutable Розм : int
   mutable ViewX : int
   mutable ViewY : int
   }

/// Named templates
let шаблони = Словник<string,Шаблон>()
/// Array of box shape names
type Квадрати () as this=
   inherit ResizeArray<string>()
   do for i = 0 to КВАДРАТИ-1 do this.Add("")
/// Piece name to boxes
let шматки = Словник<string,Квадрати>()
/// Piece name to template name
let шматкиДоШаблонів = Словник<string,string>()
/// Spots on the grid as shape names
let плями = Array.create (ШИРИНАХ*(ВИСОТАХ+1)) ""

let rec ГоловнийЦикл () =
  шаблон <- Текст.Додати("template", Математика.ОтриматиВипадковеЧисло(7))

  СтворитиШматок() // in: template ret: h
  наступнийШматок <- h

  let mutable кінець = 0
  let mutable затримкаСесії = ПОЧАТКОВАЗАТРИМКА
  while кінець = 0 do
    if затримкаСесії > КІНЦЕВАЗАТРИМКА then
      затримкаСесії <- затримкаСесії - 1    

    затримка <- затримкаСесії
    let цейШматок = наступнийШматок
    шаблон <- Текст.Додати("template", Математика.ОтриматиВипадковеЧисло(7))

    СтворитиШматок() // in: template ret: h
    наступнийШматок <- h
    НарисоватьПредпросмотрКуска()

    h <- цейШматок

    yпоз <- 0
    зроблено <- 0
    xпоз <- 3 // always drop from column 3
    ПеревіритиЗупинку() // in: ypos, xpos, h ret: done
    if зроблено = 1 then
      yпоз <- yпоз - 1
      ПереміститиШматок()  // in: ypos, xpos, h
      кінець <- 1    

    let mutable yпоздельта = 0
    while зроблено = 0 || yпоздельта > 0 do
      ПереміститиШматок()   // in: ypos, xpos, h

      // Delay, but break if the delay get set to 0 if the piece gets dropped
      let mutable індексЗатримки = затримка
      while індексЗатримки > 0 && затримка > 0 do
        Програма.Затримка(10)
        індексЗатримки <- індексЗатримки - 10      

      if yпоздельта > 0 then
        yпоздельта <- yпоздельта - 1  // used to create freespin, when the piece is rotated
      else
        yпоз <- yпоз + 1            // otherwise, move the piece down.      

      // Check if the piece should stop.
      ПеревіритиЗупинку() // in: ypos, xpos, h ret: done    

and ОбробитиКнопки () =
  // Зупинити гру
  if ГрафичнеВікно.ОстанняКнопка = "Escape" then
    Програма.Закінчити()  

  // Перемістити фигуру вліво
  if ГрафичнеВікно.ОстанняКнопка = "Left" then
    напрямокРуху <- -1
    ПідтвердитиРух()  // in: ypos, xpos, h, moveDirection ret: invalidMove = 1 or -1 or 2 if move is invalid, otherwise 0
    if неправильнийРух = 0 then
      xпоз <- xпоз + напрямокРуху   
    ПереміститиШматок()   // in: ypos, xpos, h 

  // Перемістити фигуру вправо
  if ГрафичнеВікно.ОстанняКнопка = "Right" then
    напрямокРуху <- 1
    ПідтвердитиРух()  // in: ypos, xpos, h, moveDirection ret: invalidMove = 1 or -1 or 2 if move is invalid, otherwise 0
    if неправильнийРух = 0 then
      xпоз <- xпоз + напрямокРуху    
    ПереміститиШматок()  // in: ypos, xpos, h  

  // Перемістити фигуру вниз
  if ГрафичнеВікно.ОстанняКнопка = "Down" || ГрафичнеВікно.ОстанняКнопка = "Space" then
    затримка <- 0
 
  // Повернути фигуру
  if ГрафичнеВікно.ОстанняКнопка = "Up" then
    базовийшаблон <- шматкиДоШаблонів.[h]
    шаблон <- "temptemplate"
    обертання <- "CW"
    СкопіюватиШматок()  // in basetemplate, template, rotation

    шматкиДоШаблонів.[h] <- шаблон
    напрямокРуху <- 0
    ПідтвердитиРух()  // in: ypos, xpos, h, moveDirection ret: invalidMove = 1 or -1 or 2 if move is invalid, otherwise 0

    // See if it can be moved so that it will rotate.
    let xпозсхр = xпоз
    let mutable yпоздельта = 0
    while yпоздельта = 0 && Математика.Модуль(xпозсхр - xпоз) < 3 do // move up to 3 times only
      // if the rotation move worked, copy the temp to "rotatedtemplate" and use that from now on
      if неправильнийРух = 0 then
        базовийшаблон <- шаблон
        шаблон <- "rotatedtemplate"
        шматкиДоШаблонів.[h] <- шаблон
        обертання <- "COPY"
        СкопіюватиШматок()  // in basetemplate, template, rotation
        yпоздельта <- 1 // Don't move down if we rotate
        ПереміститиШматок()  // in: ypos, xpos, h
      elif неправильнийРух = 2 then
        // Don't support shifting piece when hitting another piece to the right or left.
        xпоз <- 99 // exit the loop
      else
        // if the rotated piece can't be placed, move it left or right and try again.
        xпоз <- xпоз - неправильнийРух
        ПідтвердитиРух()  // in: ypos, xpos, h, moveDirection ret: invalidMove = 1 or -1 or 2 if move is invalid, otherwise 0

    if неправильнийРух <> 0 then
      xпоз <- xпозсхр
      шматкиДоШаблонів.[h] <- базовийшаблон
      шаблон <- ""

and НарисоватьПредпросмотрКуска () =
  xпоз <- ПЕРЕДГЛЯД_xпоз
  yпоз <- ПЕРЕДГЛЯД_yпоз
  h <- наступнийШматок

  let ЗМІЩЕННЯXСОХР = ЗМІЩЕННЯX
  let ЗМІЩЕННЯYСОХР = ЗМІЩЕННЯY
  ЗМІЩЕННЯX <- ЗМІЩЕННЯX - 20 + шаблони.[шматкиДоШаблонів.[h]].ViewX
  ЗМІЩЕННЯY <- ЗМІЩЕННЯY + шаблони.[шматкиДоШаблонів.[h]].ViewY
  ПереміститиШматок()  // in: ypos, xpos, h

  ЗМІЩЕННЯX <- ЗМІЩЕННЯXСОХР
  ЗМІЩЕННЯY <- ЗМІЩЕННЯYСОХР

// creates template that's a rotated basetemplate
and СкопіюватиШматок () = // in basetemplate, template, rotation 
  let L = шаблони.[базовийшаблон].Розм

  if not (шаблони.ContainsKey шаблон) then
      шаблони.[шаблон] <-
         { Значення=[|0;0;0;0|]; Колір=Кольори.Black; Розм=0; ViewX=0; ViewY=0 }        

  if обертання = "CW" then
    for i = 0 to КВАДРАТИ - 1 do // x' = y y' = L - 1 - x
      let v = шаблони.[базовийшаблон].Значення.[i]

      //x = Math.Floor(v/10)
      //y = Math.Remainder(v, 10)

      // new x and y
      let x = (Математика.Залишок(v, 10))
      let y = (L - 1 - Математика.Floor(float v/10.0))
      шаблони.[шаблон].Значення.[i] <- x * 10 + y
    
  // Против часовой стрелки сейчас не используется
  elif обертання = "CCW" then
    for i = 0 to КВАДРАТИ - 1 do // x' = L - 1 - y y' = x
      let v = шаблони.[базовийшаблон].Значення.[i]
      //x = Math.Floor(v/10)
      //y = Math.Remainder(v, 10)

      // new x and y
      let x = (L - 1 - Математика.Залишок(v, 10))
      let y = Математика.Floor(float v / 10.0)
      шаблони.[шаблон].Значення.[i] <- x * 10 + y
    
  elif обертання = "COPY" then
    for i = 0 to КВАДРАТИ - 1 do
      шаблони.[шаблон].Значення.[i] <- шаблони.[базовийшаблон].Значення.[i]
  else
    ГрафичнеВікно.ПоказатиПовідомлення("invalid parameter", "Error")
    Програма.Закінчити() 

  // Copy the remain properties from basetemplate to template.
  шаблони.[шаблон].Колір <- шаблони.[базовийшаблон].Колір
  шаблони.[шаблон].Розм <- шаблони.[базовийшаблон].Розм
  шаблони.[шаблон].ViewX <- шаблони.[базовийшаблон].ViewX
  шаблони.[шаблон].ViewY <- шаблони.[базовийшаблон].ViewY

and СтворитиШматок () = // in: template ret: h
  // Create a new handle, representing an arrayName, that will represent the piece
  кількістьh <- кількістьh + 1
  h <- Текст.Додати("piece", кількістьh)

  шматкиДоШаблонів.[h] <- шаблон

  ГрафичнеВікно.ШиринаПера <- 1.0
  ГрафичнеВікно.КолірПера <- Кольори.Black
  ГрафичнеВікно.КолірПензлика <- шаблони.[шаблон].Колір

  шматки.[h] <- Квадрати()
  for i = 0 to КВАДРАТИ - 1 do
    let s = Фігури.ДодатиПрямокутник(ШИРИНАК, ШИРИНАК)
    Фігури.Перемістити(s, -ШИРИНАК, -ШИРИНАК) // move off screen
    шматки.[h].[i] <- s    

and ПереміститиШматок () = // in: ypos, xpos, h. ypos/xpos is 0-19, representing the top/left box coordinate of the piece on the canvas. h returned by CreatePiece
  for i = 0 to КВАДРАТИ - 1 do
    let v = шаблони.[шматкиДоШаблонів.[h]].Значення.[i]
    let x = Математика.Floor(float v / 10.0)
    let y = Математика.Залишок(v, 10)

    // Array.GetValue(h, i) = box for piece h.
    // xpos/ypos = are topleft of shape. x/y is the box offset within the shape.
    Фігури.Перемістити(шматки.[h].[i], ЗМІЩЕННЯX + xпоз * ШИРИНАК + x * ШИРИНАК, ЗМІЩЕННЯY + yпоз * ШИРИНАК + y * ШИРИНАК)  

and ПідтвердитиРух () = // in: ypos, xpos, h, moveDirection ret: invalidMove = 1 or -1 or 2 if move is invalid, otherwise 0
  let mutable i = 0
  неправильнийРух <- 0
  while i < КВАДРАТИ do
    let v = шаблони.[шматкиДоШаблонів.[h]].Значення.[i]

    // x/y is the box offset within the shape.
    let x = Математика.Floor(float v / 10.0)
    let y = Математика.Залишок(v, 10)

    if (x + xпоз + напрямокРуху) < 0 then
      неправильнийРух <- -1
      i <- КВАДРАТИ // force getting out of the loop    

    if (x + xпоз + напрямокРуху) >= ШИРИНАХ then
      неправильнийРух <- 1
      i <- КВАДРАТИ // force getting out of the loop   

    if плями.[(x + xпоз + напрямокРуху) + (y + yпоз) * ШИРИНАХ] <> "." then
      неправильнийРух <- 2
      i <- КВАДРАТИ // force getting out of the loop    

    i <- i + 1 

and ПеревіритиЗупинку () = // in: ypos, xpos, h ret: done
  зроблено <- 0
  let mutable i = 0
  while i < КВАДРАТИ do
    let v = шаблони.[шматкиДоШаблонів.[h]].Значення.[i]

    // x/y is the box offset within the shape.
    let x = Математика.Floor(float v / 10.0)
    let y = Математика.Залишок(v, 10)

    if y + yпоз > ВИСОТАХ || плями.[(x + xпоз) + (y + yпоз) * ШИРИНАХ] <> "." then
      зроблено <- 1
      i <- КВАДРАТИ // force getting out of the loop   

    i <- i + 1 

  // If we need to stop the piece, move the box handles to the canvas
  if зроблено = 1 then
    for i = 0 to КВАДРАТИ - 1 do
      let v = шаблони.[шматкиДоШаблонів.[h]].Значення.[i]
      //x = Math.Floor(v/10)
      //y = Math.Remainder(v, 10)
      let x = (Математика.Floor(float v / 10.0) + xпоз)
      let y = (Математика.Залишок(v, 10) + yпоз - 1)
      if y >= 0 then
         плями.[x + y * ШИРИНАХ] <- шматки.[h].[i]

    // 1 points for every piece successfully dropped
    рахунок <- рахунок + 1
    НадрукуватиРахунок()

    // Delete cleared lines
    ВидалитиЛінії()

and ВидалитиЛінії () =
  let mutable лінійОчищено = 0

  // Iterate over each row, starting from the bottom
  for y = ВИСОТАХ - 1 downto 0 do

    // Check to see if the whole row is filled
    let mutable x = ШИРИНАХ
    while x = ШИРИНАХ do
      x <- 0
      while x < ШИРИНАХ do
        let piece = плями.[x + y * ШИРИНАХ]
        if piece = "." then
          x <- ШИРИНАХ        
        x <- x + 1    

      // if non of them were empty (i.e "."), then remove the line.
      if x = ШИРИНАХ then

        // Delete the line
        for x1 = 0 to ШИРИНАХ - 1 do
          Фігури.Видалити(плями.[x1 + y * ШИРИНАХ])
        лінійОчищено <- лінійОчищено + 1

        // Move everything else down one.
        for y1 = y downto 1 do
          for x1 = 0 to ШИРИНАХ - 1 do
            let piece = плями.[x1 + (y1 - 1) * ШИРИНАХ]
            плями.[x1 + y1 * ШИРИНАХ] <- piece
            Фігури.Перемістити(piece, Фігури.ОтриматиЛіво(piece), Фігури.ОтриматиВерх(piece) + float ШИРИНАК)

  if лінійОчищено > 0 then
    рахунок <- рахунок + 100 * int (Математика.Округляти(float лінійОчищено * 2.15 - 1.0))
    НадрукуватиРахунок()

and НалаштуватиПолотно () =
// GraphicsWindow.DrawResizedImage( Flickr.GetRandomPicture( "bricks" ), 0, 0, GraphicsWindow.Width, GraphicsWindow.Height)

  ГрафичнеВікно.КолірПензлика <- ГрафичнеВікно.КолірФона
  ГрафичнеВікно.ЗаповнитиПрямокутник(ЗМІЩЕННЯX, ЗМІЩЕННЯY, ШИРИНАХ*ШИРИНАК, ВИСОТАХ*ШИРИНАК)

  Програма.Затримка(200)
  ГрафичнеВікно.ШиринаПера <- 1.0
  ГрафичнеВікно.КолірПера <- Кольори.Pink
  for x = 0 to ШИРИНАХ-1 do
    for y = 0 to ВИСОТАХ-1 do
      плями.[x + y * ШИРИНАХ] <- "." // "." indicates spot is free
      ГрафичнеВікно.НамалюватиПрямокутник(ЗМІЩЕННЯX + x * ШИРИНАК, ЗМІЩЕННЯY + y * ШИРИНАК, ШИРИНАК, ШИРИНАК)

  ГрафичнеВікно.ШиринаПера <- 4.0
  ГрафичнеВікно.КолірПера <- Кольори.Black
  ГрафичнеВікно.НамалюватиЛінію(ЗМІЩЕННЯX, ЗМІЩЕННЯY, ЗМІЩЕННЯX, ЗМІЩЕННЯY + ВИСОТАХ*ШИРИНАК)
  ГрафичнеВікно.НамалюватиЛінію(ЗМІЩЕННЯX + ШИРИНАХ*ШИРИНАК, ЗМІЩЕННЯY, ЗМІЩЕННЯX + ШИРИНАХ*ШИРИНАК, ЗМІЩЕННЯY + ВИСОТАХ*ШИРИНАК)
  ГрафичнеВікно.НамалюватиЛінію(ЗМІЩЕННЯX, ЗМІЩЕННЯY + ВИСОТАХ*ШИРИНАК, ЗМІЩЕННЯX + ШИРИНАХ*ШИРИНАК, ЗМІЩЕННЯY + ВИСОТАХ*ШИРИНАК)

  ГрафичнеВікно.КолірПера <- Кольори.Lime
  ГрафичнеВікно.НамалюватиЛінію(ЗМІЩЕННЯX - 4, ЗМІЩЕННЯY, ЗМІЩЕННЯX - 4, ЗМІЩЕННЯY + ВИСОТАХ*ШИРИНАК + 6)
  ГрафичнеВікно.НамалюватиЛінію(ЗМІЩЕННЯX + ШИРИНАХ*ШИРИНАК + 4, ЗМІЩЕННЯY, ЗМІЩЕННЯX + ШИРИНАХ*ШИРИНАК + 4, ЗМІЩЕННЯY + ВИСОТАХ*ШИРИНАК + 6)
  ГрафичнеВікно.НамалюватиЛінію(ЗМІЩЕННЯX - 4, ЗМІЩЕННЯY + ВИСОТАХ*ШИРИНАК + 4, ЗМІЩЕННЯX + ШИРИНАХ*ШИРИНАК + 4, ЗМІЩЕННЯY + ВИСОТАХ*ШИРИНАК + 4)

  ГрафичнеВікно.КолірПера <- Кольори.Black
  ГрафичнеВікно.КолірПензлика <- Кольори.Pink
  let x = ЗМІЩЕННЯX + ПЕРЕДГЛЯД_xпоз * ШИРИНАК - ШИРИНАК
  let y = ЗМІЩЕННЯY + ПЕРЕДГЛЯД_yпоз * ШИРИНАК - ШИРИНАК
  ГрафичнеВікно.ЗаповнитиПрямокутник(x - 20, y, ШИРИНАК * 5, ШИРИНАК * 6)
  ГрафичнеВікно.НамалюватиПрямокутник(x - 20, y, ШИРИНАК * 5, ШИРИНАК * 6)

  ГрафичнеВікно.ЗаповнитиПрямокутник(x - 20, y + 190, 310, 170)
  ГрафичнеВікно.НамалюватиПрямокутник(x - 20, y + 190, 310, 170)

  ГрафичнеВікно.КолірПензлика <- Кольори.Black
  ГрафичнеВікно.КурсивністьШрифта <- false
  ГрафичнеВікно.ІмяШрифта <- "Comic Sans MS"
  ГрафичнеВікно.РозмірШрифта <- 16.0
  ГрафичнеВікно.НамалюватиТекст(x, y + 200, "Game control keys:")
  ГрафичнеВікно.НамалюватиТекст(x + 25, y + 220, "Left Arrow = Move piece left")
  ГрафичнеВікно.НамалюватиТекст(x + 25, y + 240, "Right Arrow = Move piece right")
  ГрафичнеВікно.НамалюватиТекст(x + 25, y + 260, "Up Arrow = Rotate piece")
  ГрафичнеВікно.НамалюватиТекст(x + 25, y + 280, "Down Arrow = Drop piece")
  ГрафичнеВікно.НамалюватиТекст(x, y + 320, "Press to stop game")

  Програма.Затримка(200) // without this delay, the above text will use the fontsize of the score 

  ГрафичнеВікно.КолірПензлика <- Кольори.Black
  ГрафичнеВікно.ІмяШрифта <- "Georgia"
  ГрафичнеВікно.КурсивністьШрифта <- true
  ГрафичнеВікно.РозмірШрифта <- 36.0
  ГрафичнеВікно.НамалюватиТекст(x - 20, y + 400, "Small Basic Tetris")
  Програма.Затримка(200) // without this delay, the above text will use the fontsize of the score 
  ГрафичнеВікно.РозмірШрифта <- 16.0
  ГрафичнеВікно.НамалюватиТекст(x - 20, y + 440, "ver.0.1")

  Програма.Затримка(200) // without this delay, the above text will use the fontsize of the score 
  рахунок <- 0
  НадрукуватиРахунок()

and НадрукуватиРахунок () =
  ГрафичнеВікно.ШиринаПера <- 4.0
  ГрафичнеВікно.КолірПензлика <- Кольори.Pink
  ГрафичнеВікно.ЗаповнитиПрямокутник(480, 65, 150, 50)
  ГрафичнеВікно.КолірПензлика <- Кольори.Black
  ГрафичнеВікно.НамалюватиПрямокутник(480, 65, 150, 50)
  ГрафичнеВікно.КурсивністьШрифта <- false
  ГрафичнеВікно.РозмірШрифта <- 32.0
  ГрафичнеВікно.ІмяШрифта <- "Impact"
  ГрафичнеВікно.КолірПензлика <- Кольори.Black
  ГрафичнеВікно.НамалюватиТекст(485, 70, Текст.Додати(Текст.ОтриматиЧастинуТекста( "00000000", 0, 8 - Текст.ОтриматиДовжину( string рахунок ) ), рахунок))

and НалаштуватиШаблони () =
  // each piece has 4 boxes.
  // the index of each entry within a piece represents the box number (1-4)
  // the value of each entry represents to box zero-based box coordinate within the piece: tens place is x, ones place y

  //_X_
  //_X_
  //_XX
  шаблони.["template1"] <- { Значення=[|10;11;12;22|]; Колір=Кольори.Yellow; Розм=3; ViewX = -12; ViewY = 12 }

  //_X_
  //_X_
  //XX_
  шаблони.["template2"] <- { Значення=[|10;11;12;02|]; Колір=Кольори.Magenta; Розм=3; ViewX=12; ViewY=12 }

  //_X_
  //XXX
  //_
  шаблони.["template3"] <- { Значення=[|10;01;11;21|]; Колір=Кольори.Gray; Розм=3; ViewX=0; ViewY=25}

  //XX_
  //XX_
  //_
  шаблони.["template4"] <- { Значення=[|00;10;01;11|]; Колір=Кольори.Cyan; Розм=2; ViewX=12; ViewY=25 }

  //XX_
  //_XX
  //_
  шаблони.["template5"] <- { Значення=[|00;10;11;21|]; Колір=Кольори.Green; Розм=3; ViewX=0; ViewY=25 }

  //_XX
  //XX_
  //_
  шаблони.["template6"] <- { Значення=[|10;20;01;11|]; Колір=Кольори.Blue; Розм=3; ViewX=0; ViewY=25}

  //_X
  //_X
  //_X
  //_X
  шаблони.["template7"] <- { Значення=[|10;11;12;13|]; Колір=Кольори.Red; Розм=4; ViewX=0; ViewY=0}

ГрафичнеВікно.Висота <- 580
ГрафичнеВікно.Ширина <- 700
ГрафичнеВікно.КлавішаНатиснута <- Callback(ОбробитиКнопки)
ГрафичнеВікно.КолірФона <- ГрафичнеВікно.ОтриматиКолірЗRGB( 253, 252, 251 )

while true do
  КВАДРАТИ <- 4      // number of boxes per piece
  ШИРИНАК <- 25    // box width in pixels
  ЗМІЩЕННЯX <- 40   // Screen X offset in pixels of where the board starts
  ЗМІЩЕННЯY <- 40   // Screen Y offset in pixels of where the board starts
  ШИРИНАХ <- 10    // Ширина Полотноа в количестве квадратов
  ВИСОТАХ <- 20   // Высота Полотноа в количестве квадратов.
  ПОЧАТКОВАЗАТРИМКА <- 800
  КІНЦЕВАЗАТРИМКА <- 175
  ПЕРЕДГЛЯД_xпоз <- 13
  ПЕРЕДГЛЯД_yпоз <- 2

  ГрафичнеВікно.Очистити()
  ГрафичнеВікно.Заголовок <- "Small Basic Tetris"

  ГрафичнеВікно.Показати()

  НалаштуватиШаблони()
  НалаштуватиПолотно()
  ГоловнийЦикл()

  ГрафичнеВікно.ПоказатиПовідомлення( "Гра завершена", "Small Basic Tetris" )


