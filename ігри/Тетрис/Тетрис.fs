﻿#if INTERACTIVE
#r "./bin/debug/Xwt.dll"
#r "./bin/debug/FunSharp.dll"
#endif

open Бiблiотека

let mutable КВАДРАТЫ = 4      // number of boxes per piece
let mutable ШИРИНАК = 25    // box width in pixels
let mutable СМЕЩЩЕНИЕX = 40   // Screen X offset in pixels of where the board starts
let mutable СМЕЩЩЕНИЕY = 40   // Screen Y offset in pixels of where the board starts
let mutable ШИРИНАХ = 10    // Canvas Width, in number of boxes
let mutable ВЫСОТАХ = 20   // Canvas Height, in number of boxes.
let mutable НАЧАЛЬНАЯЗАДЕРЖКА = 800
let mutable КОНЕЧНАЯЗАДЕРЖКА = 175
let mutable ПРЕДПРОСМОТР_xпоз = 13
let mutable ПРЕДПРОСМОТР_yпоз = 2

let mutable шаблон = ""
let mutable базовыйшаблон = ""
let mutable вращение = ""
let mutable h = ""
let mutable следующийКусок = ""
let mutable количествоh = 0
let mutable xпоз = 0
let mutable yпоз = 0
let mutable ``готово`` = 0
let mutable направлениеДвижения = 0
let mutable неверноеДвижение = 0
let mutable задержка = 0
let mutable счет = 0

type Шаблон = {
   Значения : int[]
   mutable Цвет : Колір
   mutable Разм : int
   mutable ViewX : int
   mutable ViewY : int
   }

/// Named templates
let шаблоны = Словник<string,Шаблон>()
/// Array of box shape names
type Квадраты () as this=
   inherit ResizeArray<string>()
   do for i = 0 to КВАДРАТЫ-1 do this.Add("")
/// Piece name to boxes
let куски = System.Collections.Generic.Dictionary<string,Квадраты>()
/// Piece name to template name
let кускиКШаблонам = System.Collections.Generic.Dictionary<string,string>()
/// Spots on the grid as shape names
let spots = Array.create (ШИРИНАХ*(ВЫСОТАХ+1)) ""

let rec ГлавныйЦикл () =
  шаблон <- Текст.Добавить("template", Математика.ОтриматиВипадковеЧисло(7))

  СоздатьКусок() // in: template ret: h
  следующийКусок <- h

  let mutable ``конец`` = 0
  let mutable задержкаСессии = НАЧАЛЬНАЯЗАДЕРЖКА
  while ``конец`` = 0 do
    if задержкаСессии > КОНЕЧНАЯЗАДЕРЖКА then
      задержкаСессии <- задержкаСессии - 1    

    задержка <- задержкаСессии
    let этотКусок = следующийКусок
    шаблон <- Текст.Добавить("template", Математика.ОтриматиВипадковеЧисло(7))

    СоздатьКусок() // in: template ret: h
    следующийКусок <- h
    НарисоватьПредпросмотрКуска()

    h <- этотКусок

    yпоз <- 0
    ``готово`` <- 0
    xпоз <- 3 // always drop from column 3
    ПроверитьОстановку() // in: ypos, xpos, h ret: done
    if ``готово`` = 1 then
      yпоз <- yпоз - 1
      ПереміститиКусок()  // in: ypos, xpos, h
      ``конец`` <- 1    

    let mutable yпоздельта = 0
    while ``готово`` = 0 || yпоздельта > 0 do
      ПереміститиКусок()   // in: ypos, xpos, h

      // Delay, but break if the delay get set to 0 if the piece gets dropped
      let mutable індексЗатримки = задержка
      while індексЗатримки > 0 && задержка > 0 do
        Програма.Затримка(10)
        індексЗатримки <- індексЗатримки - 10      

      if yпоздельта > 0 then
        yпоздельта <- yпоздельта - 1  // used to create freespin, when the piece is rotated
      else
        yпоз <- yпоз + 1            // otherwise, move the piece down.      

      // Check if the piece should stop.
      ПроверитьОстановку() // in: ypos, xpos, h ret: done    

and ОбработатьКнопки () =
  // Остановить игру
  if ГрафичнеВікно.ОстанняКнопка = "Escape" then
    Програма.Закончить()  

  // Перемістити фигуру влево
  if ГрафичнеВікно.ОстанняКнопка = "Left" then
    направлениеДвижения <- -1
    ВалидироватьДвижение()  // in: ypos, xpos, h, moveDirection ret: invalidMove = 1 or -1 or 2 if move is invalid, otherwise 0
    if неверноеДвижение = 0 then
      xпоз <- xпоз + направлениеДвижения   
    ПереміститиКусок()   // in: ypos, xpos, h 

  // Перемістити фигуру вправо
  if ГрафичнеВікно.ОстанняКнопка = "Right" then
    направлениеДвижения <- 1
    ВалидироватьДвижение()  // in: ypos, xpos, h, moveDirection ret: invalidMove = 1 or -1 or 2 if move is invalid, otherwise 0
    if неверноеДвижение = 0 then
      xпоз <- xпоз + направлениеДвижения    
    ПереміститиКусок()  // in: ypos, xpos, h  

  // Перемістити фигуру вниз
  if ГрафичнеВікно.ОстанняКнопка = "Down" || ГрафичнеВікно.ОстанняКнопка = "Space" then
    задержка <- 0
 
  // Повернути фигуру
  if ГрафичнеВікно.ОстанняКнопка = "Up" then
    базовыйшаблон <- кускиКШаблонам.[h]
    шаблон <- "temptemplate"
    вращение <- "CW"
    СкопироватьКусок()  // in basetemplate, template, rotation

    кускиКШаблонам.[h] <- шаблон
    направлениеДвижения <- 0
    ВалидироватьДвижение()  // in: ypos, xpos, h, moveDirection ret: invalidMove = 1 or -1 or 2 if move is invalid, otherwise 0

    // See if it can be moved so that it will rotate.
    let xпозсхр = xпоз
    let mutable yпоздельта = 0
    while yпоздельта = 0 && Математика.Модуль(xпозсхр - xпоз) < 3 do // move up to 3 times only
      // if the rotation move worked, copy the temp to "rotatedtemplate" and use that from now on
      if неверноеДвижение = 0 then
        базовыйшаблон <- шаблон
        шаблон <- "rotatedtemplate"
        кускиКШаблонам.[h] <- шаблон
        вращение <- "COPY"
        СкопироватьКусок()  // in basetemplate, template, rotation
        yпоздельта <- 1 // Don't move down if we rotate
        ПереміститиКусок()  // in: ypos, xpos, h
      elif неверноеДвижение = 2 then
        // Don't support shifting piece when hitting another piece to the right or left.
        xпоз <- 99 // exit the loop
      else
        // if the rotated piece can't be placed, move it left or right and try again.
        xпоз <- xпоз - неверноеДвижение
        ВалидироватьДвижение()  // in: ypos, xpos, h, moveDirection ret: invalidMove = 1 or -1 or 2 if move is invalid, otherwise 0

    if неверноеДвижение <> 0 then
      xпоз <- xпозсхр
      кускиКШаблонам.[h] <- базовыйшаблон
      шаблон <- ""

and НарисоватьПредпросмотрКуска () =
  xпоз <- ПРЕДПРОСМОТР_xпоз
  yпоз <- ПРЕДПРОСМОТР_yпоз
  h <- следующийКусок

  let СМЕЩЩЕНИЕXСОХР = СМЕЩЩЕНИЕX
  let СМЕЩЩЕНИЕYСОХР = СМЕЩЩЕНИЕY
  СМЕЩЩЕНИЕX <- СМЕЩЩЕНИЕX - 20 + шаблоны.[кускиКШаблонам.[h]].ViewX
  СМЕЩЩЕНИЕY <- СМЕЩЩЕНИЕY + шаблоны.[кускиКШаблонам.[h]].ViewY
  ПереміститиКусок()  // in: ypos, xpos, h

  СМЕЩЩЕНИЕX <- СМЕЩЩЕНИЕXСОХР
  СМЕЩЩЕНИЕY <- СМЕЩЩЕНИЕYСОХР

// creates template that's a rotated basetemplate
and СкопироватьКусок () = // in basetemplate, template, rotation 
  let L = шаблоны.[базовыйшаблон].Разм

  if not (шаблоны.ContainsKey шаблон) then
      шаблоны.[шаблон] <-
         { Значения=[|0;0;0;0|]; Цвет=Кольори.Black; Разм=0; ViewX=0; ViewY=0 }        

  if вращение = "CW" then
    for i = 0 to КВАДРАТЫ - 1 do // x' = y y' = L - 1 - x
      let v = шаблоны.[базовыйшаблон].Значения.[i]

      //x = Math.Floor(v/10)
      //y = Math.Remainder(v, 10)

      // new x and y
      let x = (Математика.Залишок(v, 10))
      let y = (L - 1 - Математика.Floor(float v/10.0))
      шаблоны.[шаблон].Значения.[i] <- x * 10 + y
    
  // Против часовой стрелки сейчас не используется
  elif вращение = "CCW" then
    for i = 0 to КВАДРАТЫ - 1 do // x' = L - 1 - y y' = x
      let v = шаблоны.[базовыйшаблон].Значения.[i]
      //x = Math.Floor(v/10)
      //y = Math.Remainder(v, 10)

      // new x and y
      let x = (L - 1 - Математика.Залишок(v, 10))
      let y = Математика.Floor(float v / 10.0)
      шаблоны.[шаблон].Значения.[i] <- x * 10 + y
    
  elif вращение = "COPY" then
    for i = 0 to КВАДРАТЫ - 1 do
      шаблоны.[шаблон].Значения.[i] <- шаблоны.[базовыйшаблон].Значения.[i]
  else
    ГрафичнеВікно.ПоказатьСообщение("invalid parameter", "Error")
    Програма.Закончить() 

  // Copy the remain properties from basetemplate to template.
  шаблоны.[шаблон].Цвет <- шаблоны.[базовыйшаблон].Цвет
  шаблоны.[шаблон].Разм <- шаблоны.[базовыйшаблон].Разм
  шаблоны.[шаблон].ViewX <- шаблоны.[базовыйшаблон].ViewX
  шаблоны.[шаблон].ViewY <- шаблоны.[базовыйшаблон].ViewY

and СоздатьКусок () = // in: template ret: h
  // Create a new handle, representing an arrayName, that will represent the piece
  количествоh <- количествоh + 1
  h <- Текст.Добавить("piece", количествоh)

  кускиКШаблонам.[h] <- шаблон

  ГрафичнеВікно.ШиринаПера <- 1.0
  ГрафичнеВікно.КолірПера <- Кольори.Black
  ГрафичнеВікно.ЦветКисти <- шаблоны.[шаблон].Цвет

  куски.[h] <- Квадраты()
  for i = 0 to КВАДРАТЫ - 1 do
    let s = Фігури.ДодатиПрямокутник(ШИРИНАК, ШИРИНАК)
    Фігури.Перемістити(s, -ШИРИНАК, -ШИРИНАК) // move off screen
    куски.[h].[i] <- s    

and ПереміститиКусок () = // in: ypos, xpos, h. ypos/xpos is 0-19, representing the top/left box coordinate of the piece on the canvas. h returned by CreatePiece
  for i = 0 to КВАДРАТЫ - 1 do
    let v = шаблоны.[кускиКШаблонам.[h]].Значения.[i]
    let x = Математика.Floor(float v / 10.0)
    let y = Математика.Залишок(v, 10)

    // Array.GetValue(h, i) = box for piece h.
    // xpos/ypos = are topleft of shape. x/y is the box offset within the shape.
    Фігури.Перемістити(куски.[h].[i], СМЕЩЩЕНИЕX + xпоз * ШИРИНАК + x * ШИРИНАК, СМЕЩЩЕНИЕY + yпоз * ШИРИНАК + y * ШИРИНАК)  

and ВалидироватьДвижение () = // in: ypos, xpos, h, moveDirection ret: invalidMove = 1 or -1 or 2 if move is invalid, otherwise 0
  let mutable i = 0
  неверноеДвижение <- 0
  while i < КВАДРАТЫ do
    let v = шаблоны.[кускиКШаблонам.[h]].Значения.[i]

    // x/y is the box offset within the shape.
    let x = Математика.Floor(float v / 10.0)
    let y = Математика.Залишок(v, 10)

    if (x + xпоз + направлениеДвижения) < 0 then
      неверноеДвижение <- -1
      i <- КВАДРАТЫ // force getting out of the loop    

    if (x + xпоз + направлениеДвижения) >= ШИРИНАХ then
      неверноеДвижение <- 1
      i <- КВАДРАТЫ // force getting out of the loop   

    if spots.[(x + xпоз + направлениеДвижения) + (y + yпоз) * ШИРИНАХ] <> "." then
      неверноеДвижение <- 2
      i <- КВАДРАТЫ // force getting out of the loop    

    i <- i + 1 

and ПроверитьОстановку () = // in: ypos, xpos, h ret: done
  ``готово`` <- 0
  let mutable i = 0
  while i < КВАДРАТЫ do
    let v = шаблоны.[кускиКШаблонам.[h]].Значения.[i]

    // x/y is the box offset within the shape.
    let x = Математика.Floor(float v / 10.0)
    let y = Математика.Залишок(v, 10)

    if y + yпоз > ВЫСОТАХ || spots.[(x + xпоз) + (y + yпоз) * ШИРИНАХ] <> "." then
      ``готово`` <- 1
      i <- КВАДРАТЫ // force getting out of the loop   

    i <- i + 1 

  // If we need to stop the piece, move the box handles to the canvas
  if ``готово`` = 1 then
    for i = 0 to КВАДРАТЫ - 1 do
      let v = шаблоны.[кускиКШаблонам.[h]].Значения.[i]
      //x = Math.Floor(v/10)
      //y = Math.Remainder(v, 10)
      let x = (Математика.Floor(float v / 10.0) + xпоз)
      let y = (Математика.Залишок(v, 10) + yпоз - 1)
      if y >= 0 then
         spots.[x + y * ШИРИНАХ] <- куски.[h].[i]

    // 1 points for every piece successfully dropped
    счет <- счет + 1
    НапечататьСчет()

    // Delete cleared lines
    DeleteLines()

and DeleteLines () =
  let mutable linesCleared = 0

  // Iterate over each row, starting from the bottom
  for y = ВЫСОТАХ - 1 downto 0 do

    // Check to see if the whole row is filled
    let mutable x = ШИРИНАХ
    while x = ШИРИНАХ do
      x <- 0
      while x < ШИРИНАХ do
        let piece = spots.[x + y * ШИРИНАХ]
        if piece = "." then
          x <- ШИРИНАХ        
        x <- x + 1    

      // if non of them were empty (i.e "."), then remove the line.
      if x = ШИРИНАХ then

        // Delete the line
        for x1 = 0 to ШИРИНАХ - 1 do
          Фігури.Видалити(spots.[x1 + y * ШИРИНАХ])
        linesCleared <- linesCleared + 1

        // Move everything else down one.
        for y1 = y downto 1 do
          for x1 = 0 to ШИРИНАХ - 1 do
            let piece = spots.[x1 + (y1 - 1) * ШИРИНАХ]
            spots.[x1 + y1 * ШИРИНАХ] <- piece
            Фігури.Перемістити(piece, Фігури.ПолучитьЛево(piece), Фігури.ПолучитьВерх(piece) + float ШИРИНАК)

  if linesCleared > 0 then
    счет <- счет + 100 * int (Математика.Округляти(float linesCleared * 2.15 - 1.0))
    НапечататьСчет()

and НастроитьПолотно () =
// GraphicsWindow.DrawResizedImage( Flickr.GetRandomPicture( "bricks" ), 0, 0, GraphicsWindow.Width, GraphicsWindow.Height)

  ГрафичнеВікно.ЦветКисти <- ГрафичнеВікно.КолірФона
  ГрафичнеВікно.ЗаполнитьПрямоугольник(СМЕЩЩЕНИЕX, СМЕЩЩЕНИЕY, ШИРИНАХ*ШИРИНАК, ВЫСОТАХ*ШИРИНАК)

  Програма.Затримка(200)
  ГрафичнеВікно.ШиринаПера <- 1.0
  ГрафичнеВікно.КолірПера <- Кольори.Pink
  for x = 0 to ШИРИНАХ-1 do
    for y = 0 to ВЫСОТАХ-1 do
      spots.[x + y * ШИРИНАХ] <- "." // "." indicates spot is free
      ГрафичнеВікно.НамалюватиПрямокутник(СМЕЩЩЕНИЕX + x * ШИРИНАК, СМЕЩЩЕНИЕY + y * ШИРИНАК, ШИРИНАК, ШИРИНАК)

  ГрафичнеВікно.ШиринаПера <- 4.0
  ГрафичнеВікно.КолірПера <- Кольори.Black
  ГрафичнеВікно.НамалюватиЛінію(СМЕЩЩЕНИЕX, СМЕЩЩЕНИЕY, СМЕЩЩЕНИЕX, СМЕЩЩЕНИЕY + ВЫСОТАХ*ШИРИНАК)
  ГрафичнеВікно.НамалюватиЛінію(СМЕЩЩЕНИЕX + ШИРИНАХ*ШИРИНАК, СМЕЩЩЕНИЕY, СМЕЩЩЕНИЕX + ШИРИНАХ*ШИРИНАК, СМЕЩЩЕНИЕY + ВЫСОТАХ*ШИРИНАК)
  ГрафичнеВікно.НамалюватиЛінію(СМЕЩЩЕНИЕX, СМЕЩЩЕНИЕY + ВЫСОТАХ*ШИРИНАК, СМЕЩЩЕНИЕX + ШИРИНАХ*ШИРИНАК, СМЕЩЩЕНИЕY + ВЫСОТАХ*ШИРИНАК)

  ГрафичнеВікно.КолірПера <- Кольори.Lime
  ГрафичнеВікно.НамалюватиЛінію(СМЕЩЩЕНИЕX - 4, СМЕЩЩЕНИЕY, СМЕЩЩЕНИЕX - 4, СМЕЩЩЕНИЕY + ВЫСОТАХ*ШИРИНАК + 6)
  ГрафичнеВікно.НамалюватиЛінію(СМЕЩЩЕНИЕX + ШИРИНАХ*ШИРИНАК + 4, СМЕЩЩЕНИЕY, СМЕЩЩЕНИЕX + ШИРИНАХ*ШИРИНАК + 4, СМЕЩЩЕНИЕY + ВЫСОТАХ*ШИРИНАК + 6)
  ГрафичнеВікно.НамалюватиЛінію(СМЕЩЩЕНИЕX - 4, СМЕЩЩЕНИЕY + ВЫСОТАХ*ШИРИНАК + 4, СМЕЩЩЕНИЕX + ШИРИНАХ*ШИРИНАК + 4, СМЕЩЩЕНИЕY + ВЫСОТАХ*ШИРИНАК + 4)

  ГрафичнеВікно.КолірПера <- Кольори.Black
  ГрафичнеВікно.ЦветКисти <- Кольори.Pink
  let x = СМЕЩЩЕНИЕX + ПРЕДПРОСМОТР_xпоз * ШИРИНАК - ШИРИНАК
  let y = СМЕЩЩЕНИЕY + ПРЕДПРОСМОТР_yпоз * ШИРИНАК - ШИРИНАК
  ГрафичнеВікно.ЗаполнитьПрямоугольник(x - 20, y, ШИРИНАК * 5, ШИРИНАК * 6)
  ГрафичнеВікно.НамалюватиПрямокутник(x - 20, y, ШИРИНАК * 5, ШИРИНАК * 6)

  ГрафичнеВікно.ЗаполнитьПрямоугольник(x - 20, y + 190, 310, 170)
  ГрафичнеВікно.НамалюватиПрямокутник(x - 20, y + 190, 310, 170)

  ГрафичнеВікно.ЦветКисти <- Кольори.Black
  ГрафичнеВікно.КурсивностьШрифта <- false
  ГрафичнеВікно.ИмяШрифта <- "Comic Sans MS"
  ГрафичнеВікно.РозмірШрифта <- 16.0
  ГрафичнеВікно.НарисоватьТекст(x, y + 200, "Game control keys:")
  ГрафичнеВікно.НарисоватьТекст(x + 25, y + 220, "Left Arrow = Move piece left")
  ГрафичнеВікно.НарисоватьТекст(x + 25, y + 240, "Right Arrow = Move piece right")
  ГрафичнеВікно.НарисоватьТекст(x + 25, y + 260, "Up Arrow = Rotate piece")
  ГрафичнеВікно.НарисоватьТекст(x + 25, y + 280, "Down Arrow = Drop piece")
  ГрафичнеВікно.НарисоватьТекст(x, y + 320, "Press to stop game")

  Програма.Затримка(200) // without this delay, the above text will use the fontsize of the score 

  ГрафичнеВікно.ЦветКисти <- Кольори.Black
  ГрафичнеВікно.ИмяШрифта <- "Georgia"
  ГрафичнеВікно.КурсивностьШрифта <- true
  ГрафичнеВікно.РозмірШрифта <- 36.0
  ГрафичнеВікно.НарисоватьТекст(x - 20, y + 400, "Small Basic Tetris")
  Програма.Затримка(200) // without this delay, the above text will use the fontsize of the score 
  ГрафичнеВікно.РозмірШрифта <- 16.0
  ГрафичнеВікно.НарисоватьТекст(x - 20, y + 440, "ver.0.1")

  Програма.Затримка(200) // without this delay, the above text will use the fontsize of the score 
  счет <- 0
  НапечататьСчет()

and НапечататьСчет () =
  ГрафичнеВікно.ШиринаПера <- 4.0
  ГрафичнеВікно.ЦветКисти <- Кольори.Pink
  ГрафичнеВікно.ЗаполнитьПрямоугольник(480, 65, 150, 50)
  ГрафичнеВікно.ЦветКисти <- Кольори.Black
  ГрафичнеВікно.НамалюватиПрямокутник(480, 65, 150, 50)
  ГрафичнеВікно.КурсивностьШрифта <- false
  ГрафичнеВікно.РозмірШрифта <- 32.0
  ГрафичнеВікно.ИмяШрифта <- "Impact"
  ГрафичнеВікно.ЦветКисти <- Кольори.Black
  ГрафичнеВікно.НарисоватьТекст(485, 70, Текст.Добавить(Текст.ПолучитьПодТекст( "00000000", 0, 8 - Текст.ПолучитьДлину( string счет ) ), счет))

and НастроитьШаблоны () =
  // each piece has 4 boxes.
  // the index of each entry within a piece represents the box number (1-4)
  // the value of each entry represents to box zero-based box coordinate within the piece: tens place is x, ones place y

  //_X_
  //_X_
  //_XX
  шаблоны.["template1"] <- { Значения=[|10;11;12;22|]; Цвет=Кольори.Yellow; Разм=3; ViewX = -12; ViewY = 12 }

  //_X_
  //_X_
  //XX_
  шаблоны.["template2"] <- { Значения=[|10;11;12;02|]; Цвет=Кольори.Magenta; Разм=3; ViewX=12; ViewY=12 }

  //_X_
  //XXX
  //_
  шаблоны.["template3"] <- { Значения=[|10;01;11;21|]; Цвет=Кольори.Gray; Разм=3; ViewX=0; ViewY=25}

  //XX_
  //XX_
  //_
  шаблоны.["template4"] <- { Значения=[|00;10;01;11|]; Цвет=Кольори.Cyan; Разм=2; ViewX=12; ViewY=25 }

  //XX_
  //_XX
  //_
  шаблоны.["template5"] <- { Значения=[|00;10;11;21|]; Цвет=Кольори.Green; Разм=3; ViewX=0; ViewY=25 }

  //_XX
  //XX_
  //_
  шаблоны.["template6"] <- { Значения=[|10;20;01;11|]; Цвет=Кольори.Blue; Разм=3; ViewX=0; ViewY=25}

  //_X
  //_X
  //_X
  //_X
  шаблоны.["template7"] <- { Значения=[|10;11;12;13|]; Цвет=Кольори.Red; Разм=4; ViewX=0; ViewY=0}

ГрафичнеВікно.Высота <- 580
ГрафичнеВікно.Ширина <- 700
ГрафичнеВікно.КнопкаНатиснута <- Callback(ОбработатьКнопки)
ГрафичнеВікно.КолірФона <- ГрафичнеВікно.ПолучитьЦветИзRGB( 253, 252, 251 )

while true do
  КВАДРАТЫ <- 4      // number of boxes per piece
  ШИРИНАК <- 25    // box width in pixels
  СМЕЩЩЕНИЕX <- 40   // Screen X offset in pixels of where the board starts
  СМЕЩЩЕНИЕY <- 40   // Screen Y offset in pixels of where the board starts
  ШИРИНАХ <- 10    // Ширина Полотноа в количестве квадратов
  ВЫСОТАХ <- 20   // Высота Полотноа в количестве квадратов.
  НАЧАЛЬНАЯЗАДЕРЖКА <- 800
  КОНЕЧНАЯЗАДЕРЖКА <- 175
  ПРЕДПРОСМОТР_xпоз <- 13
  ПРЕДПРОСМОТР_yпоз <- 2

  ГрафичнеВікно.Очистити()
  ГрафичнеВікно.Заголовок <- "Small Basic Tetris"

  ГрафичнеВікно.Показать()

  НастроитьШаблоны()
  НастроитьПолотно()
  ГлавныйЦикл()

  ГрафичнеВікно.ПоказатьСообщение( "Игра завершена", "Small Basic Tetris" )


