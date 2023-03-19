#if INTERACTIVE
#r "./bin/debug/Xwt.dll"
#r "./bin/debug/FunSharp.dll"
#endif

open Кітапхана

let mutable BOXES = 4      // number of boxes per piece
let mutable BWIDTH = 25    // box width in pixels
let mutable XOFFSET = 40   // Screen X offset in pixels of where the board starts
let mutable YOFFSET = 40   // Screen Y offset in pixels of where the board starts
let mutable CWIDTH = 10    // Canvas Width, in number of boxes
let mutable CHEIGHT = 20   // Canvas Height, in number of boxes.
let mutable STARTDELAY = 800
let mutable ENDDELAY = 175
let mutable PREVIEW_xpos = 13
let mutable PREVIEW_ypos = 2

let mutable template = ""
let mutable basetemplate = ""
let mutable rotation = ""
let mutable h = ""
let mutable nextPiece = ""
let mutable hcount = 0
let mutable xpos = 0
let mutable ypos = 0
let mutable ``done`` = 0
let mutable moveDirection = 0
let mutable invalidMove = 0
let mutable delay = 0
let mutable score = 0

type Template = {
   Values : int[]
   mutable Color : Түс
   mutable Dim : int
   mutable ViewX : int
   mutable ViewY : int
   }

/// Named templates
let templates = Dictionary<string,Template>()
/// Array of box shape names
type Boxes () as this=
   inherit ResizeArray<string>()
   do for i = 0 to BOXES-1 do this.Add("")
/// Piece name to boxes
let pieces = System.Collections.Generic.Dictionary<string,Boxes>()
/// Piece name to template name
let pieceToTemplate = System.Collections.Generic.Dictionary<string,string>()
/// Spots on the grid as shape names
let spots = Array.create (CWIDTH*(CHEIGHT+1)) ""

let rec MainLoop () =
  template <- Text.Append("template", Математика.АлуКездейсоқСаны(7))

  CreatePiece() // in: template ret: h
  nextPiece <- h

  let mutable ``end`` = 0
  let mutable sessionDelay = STARTDELAY
  while ``end`` = 0 do
    if sessionDelay > ENDDELAY then
      sessionDelay <- sessionDelay - 1    

    delay <- sessionDelay
    let thisPiece = nextPiece
    template <- Text.Append("template", Математика.АлуКездейсоқСаны(7))

    CreatePiece() // in: template ret: h
    nextPiece <- h
    DrawPreviewPiece()

    h <- thisPiece

    ypos <- 0
    ``done`` <- 0
    xpos <- 3 // always drop from column 3
    CheckStop() // in: ypos, xpos, h ret: done
    if ``done`` = 1 then
      ypos <- ypos - 1
      MovePiece()  // in: ypos, xpos, h
      ``end`` <- 1    

    let mutable yposdelta = 0
    while ``done`` = 0 || yposdelta > 0 do
      MovePiece()   // in: ypos, xpos, h

      // Delay, but break if the delay get set to 0 if the piece gets dropped
      let mutable delayIndex = delay
      while delayIndex > 0 && delay > 0 do
        Program.Delay(10)
        delayIndex <- delayIndex - 10      

      if yposdelta > 0 then
        yposdelta <- yposdelta - 1  // used to create freespin, when the piece is rotated
      else
        ypos <- ypos + 1            // otherwise, move the piece down.      

      // Check if the piece should stop.
      CheckStop() // in: ypos, xpos, h ret: done    

and HandleKey () =
  // Stop game
  if ГрафикалықТерезе.LastKey = "Escape" then
    Program.End()  

  // Move piece left
  if ГрафикалықТерезе.LastKey = "Left" then
    moveDirection <- -1
    ValidateMove()  // in: ypos, xpos, h, moveDirection ret: invalidMove = 1 or -1 or 2 if move is invalid, otherwise 0
    if invalidMove = 0 then
      xpos <- xpos + moveDirection   
    MovePiece()   // in: ypos, xpos, h 

  // Move piece right
  if ГрафикалықТерезе.LastKey = "Right" then
    moveDirection <- 1
    ValidateMove()  // in: ypos, xpos, h, moveDirection ret: invalidMove = 1 or -1 or 2 if move is invalid, otherwise 0
    if invalidMove = 0 then
      xpos <- xpos + moveDirection    
    MovePiece()  // in: ypos, xpos, h  

  // Move piece down
  if ГрафикалықТерезе.LastKey = "Down" || ГрафикалықТерезе.LastKey = "Space" then
    delay <- 0
 
  // Rotate piece
  if ГрафикалықТерезе.LastKey = "Up" then
    basetemplate <- pieceToTemplate.[h]
    template <- "temptemplate"
    rotation <- "CW"
    CopyPiece()  // in basetemplate, template, rotation

    pieceToTemplate.[h] <- template
    moveDirection <- 0
    ValidateMove()  // in: ypos, xpos, h, moveDirection ret: invalidMove = 1 or -1 or 2 if move is invalid, otherwise 0

    // See if it can be moved so that it will rotate.
    let xposbk = xpos
    let mutable yposdelta = 0
    while yposdelta = 0 && Математика.Abs(xposbk - xpos) < 3 do // move up to 3 times only
      // if the rotation move worked, copy the temp to "rotatedtemplate" and use that from now on
      if invalidMove = 0 then
        basetemplate <- template
        template <- "rotatedtemplate"
        pieceToTemplate.[h] <- template
        rotation <- "COPY"
        CopyPiece()  // in basetemplate, template, rotation
        yposdelta <- 1 // Don't move down if we rotate
        MovePiece()  // in: ypos, xpos, h
      elif invalidMove = 2 then
        // Don't support shifting piece when hitting another piece to the right or left.
        xpos <- 99 // exit the loop
      else
        // if the rotated piece can't be placed, move it left or right and try again.
        xpos <- xpos - invalidMove
        ValidateMove()  // in: ypos, xpos, h, moveDirection ret: invalidMove = 1 or -1 or 2 if move is invalid, otherwise 0

    if invalidMove <> 0 then
      xpos <- xposbk
      pieceToTemplate.[h] <- basetemplate
      template <- ""

and DrawPreviewPiece () =
  xpos <- PREVIEW_xpos
  ypos <- PREVIEW_ypos
  h <- nextPiece

  let XOFFSETBK = XOFFSET
  let YOFFSETBK = YOFFSET
  XOFFSET <- XOFFSET - 20 + templates.[pieceToTemplate.[h]].ViewX
  YOFFSET <- YOFFSET + templates.[pieceToTemplate.[h]].ViewY
  MovePiece()  // in: ypos, xpos, h

  XOFFSET <- XOFFSETBK
  YOFFSET <- YOFFSETBK

// creates template that's a rotated basetemplate
and CopyPiece () = // in basetemplate, template, rotation 
  let L = templates.[basetemplate].Dim

  if not (templates.ContainsKey template) then
      templates.[template] <-
         { Values=[|0;0;0;0|]; Color=Түстер.Қара; Dim=0; ViewX=0; ViewY=0 }        

  if rotation = "CW" then
    for i = 0 to BOXES - 1 do // x' = y y' = L - 1 - x
      let v = templates.[basetemplate].Values.[i]

      //x = Math.Floor(v/10)
      //y = Math.Remainder(v, 10)

      // new x and y
      let x = (Математика.Remainder(v, 10))
      let y = (L - 1 - Математика.Floor(float v/10.0))
      templates.[template].Values.[i] <- x * 10 + y
    
  // Count-Cockwise is not currently used
  elif rotation = "CCW" then
    for i = 0 to BOXES - 1 do // x' = L - 1 - y y' = x
      let v = templates.[basetemplate].Values.[i]
      //x = Math.Floor(v/10)
      //y = Math.Remainder(v, 10)

      // new x and y
      let x = (L - 1 - Математика.Remainder(v, 10))
      let y = Математика.Floor(float v / 10.0)
      templates.[template].Values.[i] <- x * 10 + y
    
  elif rotation = "COPY" then
    for i = 0 to BOXES - 1 do
      templates.[template].Values.[i] <- templates.[basetemplate].Values.[i]
  else
    ГрафикалықТерезе.ShowMessage("invalid parameter", "Error")
    Program.End() 

  // Copy the remain properties from basetemplate to template.
  templates.[template].Color <- templates.[basetemplate].Color
  templates.[template].Dim <- templates.[basetemplate].Dim
  templates.[template].ViewX <- templates.[basetemplate].ViewX
  templates.[template].ViewY <- templates.[basetemplate].ViewY

and CreatePiece () = // in: template ret: h
  // Create a new handle, representing an arrayName, that will represent the piece
  hcount <- hcount + 1
  h <- Text.Append("piece", hcount)

  pieceToTemplate.[h] <- template

  ГрафикалықТерезе.PenWidth <- 1.0
  ГрафикалықТерезе.ҚаламТүсі <- Түстер.Қара
  ГрафикалықТерезе.ҚылқаламТүсі <- templates.[template].Color

  pieces.[h] <- Boxes()
  for i = 0 to BOXES - 1 do
    let s = Пішіндері.AddRectangle(BWIDTH, BWIDTH)
    Пішіндері.Move(s, -BWIDTH, -BWIDTH) // move off screen
    pieces.[h].[i] <- s    

and MovePiece () = // in: ypos, xpos, h. ypos/xpos is 0-19, representing the top/left box coordinate of the piece on the canvas. h returned by CreatePiece
  for i = 0 to BOXES - 1 do
    let v = templates.[pieceToTemplate.[h]].Values.[i]
    let x = Математика.Floor(float v / 10.0)
    let y = Математика.Remainder(v, 10)

    // Array.GetValue(h, i) = box for piece h.
    // xpos/ypos = are topleft of shape. x/y is the box offset within the shape.
    Пішіндері.Move(pieces.[h].[i], XOFFSET + xpos * BWIDTH + x * BWIDTH, YOFFSET + ypos * BWIDTH + y * BWIDTH)  

and ValidateMove () = // in: ypos, xpos, h, moveDirection ret: invalidMove = 1 or -1 or 2 if move is invalid, otherwise 0
  let mutable i = 0
  invalidMove <- 0
  while i < BOXES do
    let v = templates.[pieceToTemplate.[h]].Values.[i]

    // x/y is the box offset within the shape.
    let x = Математика.Floor(float v / 10.0)
    let y = Математика.Remainder(v, 10)

    if (x + xpos + moveDirection) < 0 then
      invalidMove <- -1
      i <- BOXES // force getting out of the loop    

    if (x + xpos + moveDirection) >= CWIDTH then
      invalidMove <- 1
      i <- BOXES // force getting out of the loop   

    if spots.[(x + xpos + moveDirection) + (y + ypos) * CWIDTH] <> "." then
      invalidMove <- 2
      i <- BOXES // force getting out of the loop    

    i <- i + 1 

and CheckStop () = // in: ypos, xpos, h ret: done
  ``done`` <- 0
  let mutable i = 0
  while i < BOXES do
    let v = templates.[pieceToTemplate.[h]].Values.[i]

    // x/y is the box offset within the shape.
    let x = Математика.Floor(float v / 10.0)
    let y = Математика.Remainder(v, 10)

    if y + ypos > CHEIGHT || spots.[(x + xpos) + (y + ypos) * CWIDTH] <> "." then
      ``done`` <- 1
      i <- BOXES // force getting out of the loop   

    i <- i + 1 

  // If we need to stop the piece, move the box handles to the canvas
  if ``done`` = 1 then
    for i = 0 to BOXES - 1 do
      let v = templates.[pieceToTemplate.[h]].Values.[i]
      //x = Math.Floor(v/10)
      //y = Math.Remainder(v, 10)
      let x = (Математика.Floor(float v / 10.0) + xpos)
      let y = (Математика.Remainder(v, 10) + ypos - 1)
      if y >= 0 then
         spots.[x + y * CWIDTH] <- pieces.[h].[i]

    // 1 points for every piece successfully dropped
    score <- score + 1
    PrintScore()

    // Delete cleared lines
    DeleteLines()

and DeleteLines () =
  let mutable linesCleared = 0

  // Iterate over each row, starting from the bottom
  for y = CHEIGHT - 1 downto 0 do

    // Check to see if the whole row is filled
    let mutable x = CWIDTH
    while x = CWIDTH do
      x <- 0
      while x < CWIDTH do
        let piece = spots.[x + y * CWIDTH]
        if piece = "." then
          x <- CWIDTH        
        x <- x + 1    

      // if non of them were empty (i.e "."), then remove the line.
      if x = CWIDTH then

        // Delete the line
        for x1 = 0 to CWIDTH - 1 do
          Пішіндері.Remove(spots.[x1 + y * CWIDTH])
        linesCleared <- linesCleared + 1

        // Move everything else down one.
        for y1 = y downto 1 do
          for x1 = 0 to CWIDTH - 1 do
            let piece = spots.[x1 + (y1 - 1) * CWIDTH]
            spots.[x1 + y1 * CWIDTH] <- piece
            Пішіндері.Жылжытуға(piece, Пішіндері.GetLeft(piece), Пішіндері.GetTop(piece) + float BWIDTH)

  if linesCleared > 0 then
    score <- score + 100 * int (Математика.Round(float linesCleared * 2.15 - 1.0))
    PrintScore()

and SetupCanvas () =
// GraphicsWindow.DrawResizedImage( Flickr.GetRandomPicture( "bricks" ), 0, 0, GraphicsWindow.Width, GraphicsWindow.Биіктік)

  ГрафикалықТерезе.ҚылқаламТүсі <- ГрафикалықТерезе.ФонныңТүсі
  ГрафикалықТерезе.FillRectangle(XOFFSET, YOFFSET, CWIDTH*BWIDTH, CHEIGHT*BWIDTH)

  Program.Delay(200)
  ГрафикалықТерезе.PenWidth <- 1.0
  ГрафикалықТерезе.ҚаламТүсі <- Түстер.Pink
  for x = 0 to CWIDTH-1 do
    for y = 0 to CHEIGHT-1 do
      spots.[x + y * CWIDTH] <- "." // "." indicates spot is free
      ГрафикалықТерезе.DrawRectangle(XOFFSET + x * BWIDTH, YOFFSET + y * BWIDTH, BWIDTH, BWIDTH)

  ГрафикалықТерезе.PenWidth <- 4.0
  ГрафикалықТерезе.ҚаламТүсі <- Түстер.Қара
  ГрафикалықТерезе.DrawLine(XOFFSET, YOFFSET, XOFFSET, YOFFSET + CHEIGHT*BWIDTH)
  ГрафикалықТерезе.DrawLine(XOFFSET + CWIDTH*BWIDTH, YOFFSET, XOFFSET + CWIDTH*BWIDTH, YOFFSET + CHEIGHT*BWIDTH)
  ГрафикалықТерезе.DrawLine(XOFFSET, YOFFSET + CHEIGHT*BWIDTH, XOFFSET + CWIDTH*BWIDTH, YOFFSET + CHEIGHT*BWIDTH)

  ГрафикалықТерезе.ҚаламТүсі <- Түстер.Lime
  ГрафикалықТерезе.DrawLine(XOFFSET - 4, YOFFSET, XOFFSET - 4, YOFFSET + CHEIGHT*BWIDTH + 6)
  ГрафикалықТерезе.DrawLine(XOFFSET + CWIDTH*BWIDTH + 4, YOFFSET, XOFFSET + CWIDTH*BWIDTH + 4, YOFFSET + CHEIGHT*BWIDTH + 6)
  ГрафикалықТерезе.DrawLine(XOFFSET - 4, YOFFSET + CHEIGHT*BWIDTH + 4, XOFFSET + CWIDTH*BWIDTH + 4, YOFFSET + CHEIGHT*BWIDTH + 4)

  ГрафикалықТерезе.ҚаламТүсі <- Түстер.Қара
  ГрафикалықТерезе.ҚылқаламТүсі <- Түстер.Pink
  let x = XOFFSET + PREVIEW_xpos * BWIDTH - BWIDTH
  let y = YOFFSET + PREVIEW_ypos * BWIDTH - BWIDTH
  ГрафикалықТерезе.FillRectangle(x - 20, y, BWIDTH * 5, BWIDTH * 6)
  ГрафикалықТерезе.DrawRectangle(x - 20, y, BWIDTH * 5, BWIDTH * 6)

  ГрафикалықТерезе.FillRectangle(x - 20, y + 190, 310, 170)
  ГрафикалықТерезе.DrawRectangle(x - 20, y + 190, 310, 170)

  ГрафикалықТерезе.ҚылқаламТүсі <- Түстер.Қара
  ГрафикалықТерезе.FontItalic <- false
  ГрафикалықТерезе.FontName <- "Comic Sans MS"
  ГрафикалықТерезе.FontSize <- 16.0
  ГрафикалықТерезе.DrawText(x, y + 200, "Game control keys:")
  ГрафикалықТерезе.DrawText(x + 25, y + 220, "Left Arrow = Move piece left")
  ГрафикалықТерезе.DrawText(x + 25, y + 240, "Right Arrow = Move piece right")
  ГрафикалықТерезе.DrawText(x + 25, y + 260, "Up Arrow = Rotate piece")
  ГрафикалықТерезе.DrawText(x + 25, y + 280, "Down Arrow = Drop piece")
  ГрафикалықТерезе.DrawText(x, y + 320, "Press to stop game")

  Program.Delay(200) // without this delay, the above text will use the fontsize of the score 

  ГрафикалықТерезе.ҚылқаламТүсі <- Түстер.Қара
  ГрафикалықТерезе.FontName <- "Georgia"
  ГрафикалықТерезе.FontItalic <- true
  ГрафикалықТерезе.FontSize <- 36.0
  ГрафикалықТерезе.DrawText(x - 20, y + 400, "Small Basic Tetris")
  Program.Delay(200) // without this delay, the above text will use the fontsize of the score 
  ГрафикалықТерезе.FontSize <- 16.0
  ГрафикалықТерезе.DrawText(x - 20, y + 440, "ver.0.1")

  Program.Delay(200) // without this delay, the above text will use the fontsize of the score 
  score <- 0
  PrintScore()

and PrintScore () =
  ГрафикалықТерезе.PenWidth <- 4.0
  ГрафикалықТерезе.ҚылқаламТүсі <- Түстер.Pink
  ГрафикалықТерезе.FillRectangle(480, 65, 150, 50)
  ГрафикалықТерезе.ҚылқаламТүсі <- Түстер.Қара
  ГрафикалықТерезе.DrawRectangle(480, 65, 150, 50)
  ГрафикалықТерезе.FontItalic <- false
  ГрафикалықТерезе.FontSize <- 32.0
  ГрафикалықТерезе.FontName <- "Impact"
  ГрафикалықТерезе.ҚылқаламТүсі <- Түстер.Қара
  ГрафикалықТерезе.DrawText(485, 70, Text.Append(Text.GetSubText( "00000000", 0, 8 - Text.GetLength( string score ) ), score))

and SetupTemplates () =
  // each piece has 4 boxes.
  // the index of each entry within a piece represents the box number (1-4)
  // the value of each entry represents to box zero-based box coordinate within the piece: tens place is x, ones place y

  //_X_
  //_X_
  //_XX
  templates.["template1"] <- { Values=[|10;11;12;22|]; Color=Түстер.Yellow; Dim=3; ViewX = -12; ViewY = 12 }

  //_X_
  //_X_
  //XX_
  templates.["template2"] <- { Values=[|10;11;12;02|]; Color=Түстер.Magenta; Dim=3; ViewX=12; ViewY=12 }

  //_X_
  //XXX
  //_
  templates.["template3"] <- { Values=[|10;01;11;21|]; Color=Түстер.Gray; Dim=3; ViewX=0; ViewY=25}

  //XX_
  //XX_
  //_
  templates.["template4"] <- { Values=[|00;10;01;11|]; Color=Түстер.Cyan; Dim=2; ViewX=12; ViewY=25 }

  //XX_
  //_XX
  //_
  templates.["template5"] <- { Values=[|00;10;11;21|]; Color=Түстер.Green; Dim=3; ViewX=0; ViewY=25 }

  //_XX
  //XX_
  //_
  templates.["template6"] <- { Values=[|10;20;01;11|]; Color=Түстер.Blue; Dim=3; ViewX=0; ViewY=25}

  //_X
  //_X
  //_X
  //_X
  templates.["template7"] <- { Values=[|10;11;12;13|]; Color=Түстер.Red; Dim=4; ViewX=0; ViewY=0}

ГрафикалықТерезе.Биіктік <- 580
ГрафикалықТерезе.Ен <- 700
ГрафикалықТерезе.KeyDown <- Callback(HandleKey)
ГрафикалықТерезе.ФонныңТүсі <- ГрафикалықТерезе.GetColorFromRGB( 253, 252, 251 )

while true do
  BOXES <- 4      // number of boxes per piece
  BWIDTH <- 25    // box width in pixels
  XOFFSET <- 40   // Screen X offset in pixels of where the board starts
  YOFFSET <- 40   // Screen Y offset in pixels of where the board starts
  CWIDTH <- 10    // Canvas Width, in number of boxes
  CHEIGHT <- 20   // Canvas Height, in number of boxes.
  STARTDELAY <- 800
  ENDDELAY <- 175
  PREVIEW_xpos <- 13
  PREVIEW_ypos <- 2

  ГрафикалықТерезе.Clear()
  ГрафикалықТерезе.Title <- "Small Basic Tetris"

  ГрафикалықТерезе.Show()

  SetupTemplates()
  SetupCanvas()
  MainLoop()

  ГрафикалықТерезе.ShowMessage( "Game Over", "Small Basic Tetris" )


