﻿#r "nuget: Xwt"
#r "../src/bin/Debug/net48/FunSharp.Library.dll"

open Library
open System.Threading

let mutable p = 0

ГрафическоеОкно.CanResize <- false
ГрафическоеОкно.Ширина <- 260
ГрафическоеОкно.Высота <- 230
ГрафическоеОкно.Заголовок <- "Calculator v. 1.0 by Alex_2000"

ГрафическоеОкно.ФоновыйЦвет = ГрафическоеОкно.GetColorFromRGB(240, 240, 240)
ГрафическоеОкно.FontBold <- false
ГрафическоеОкно.ЦветКисти <- Colors.Black

let m = Shapes.AddText("M")
Shapes.Move(m, 22, 62)
Shapes.HideShape(m)

let N = 1

let t = Controls.AddTextBox(10, 10)
Controls.SetSize(t, 240*N, 22*N)
Controls.SetTextBoxText(t, "")

let R0 = 58
let R1 = R0 + (95-58)*N
let R2 = R1 + (128-95)*N 
let R3 = R2 + (161-128)*N
let R4 = R3 + (194-161)*N
let C0 = 10
let C1 = C0  + (58-10)*N
let C2 = C1  + (99-58)*N
let C3 = C2 + (140-99)*N
let C4 = C3 + (181-140)*N
let C5 = C4 + (222-181)*N
let T0 = C0
let T1 = T0 + (139-58)*N
let T2 = T1 + (202-139)*N 
ГрафическоеОкно.ЦветКисти <- Colors.Red
let keyMC = Controls.ДобавитьКнопку("MC", C0, R1)
Controls.SetSize(keyMC, 35*N, 25*N)
let keyMR = Controls.ДобавитьКнопку("MR", C0, R2)
Controls.SetSize(keyMR, 35*N, 25*N)
let keyMS = Controls.ДобавитьКнопку("MS", C0, R3)
Controls.SetSize(keyMS, 35*N, 25*N)
let keyMP = Controls.ДобавитьКнопку("M+", C0, R4)
Controls.SetSize(keyMP, 35*N, 25*N)
let keyC = Controls.ДобавитьКнопку("C", T2, R0)
Controls.SetSize(keyC, 55*N, 25*N )
let keyCE = Controls.ДобавитьКнопку("CE", T1, R0)
Controls.SetSize(keyCE, 55*N, 25*N)
let keyBackspase = Controls.ДобавитьКнопку("Backspace", T0, R0)
Controls.SetSize(keyBackspase, 70*N, 25*N)

let keydelenie = Controls.ДобавитьКнопку("/", C4, R1)
Controls.SetSize(keydelenie, 35*N, 25*N)
let keyumnogenie = Controls.ДобавитьКнопку("*", C4, R2)
Controls.SetSize(keyumnogenie, 35*N, 25*N)
let keyminus = Controls.ДобавитьКнопку("-", C4, R3)
Controls.SetSize(keyminus, 35*N, 25*N)
let keyplus = Controls.ДобавитьКнопку("+", C4, R4)
Controls.SetSize(keyplus, 35*N, 25*N)
let keyravno = Controls.ДобавитьКнопку("=", C5, R4)
Controls.SetSize(keyravno, 35*N, 25*N)

ГрафическоеОкно.ЦветКисти <- Colors.Blue
let key7 = Controls.ДобавитьКнопку("7", C1, R1)
Controls.SetSize(key7, 35*N, 25*N)
let key8 = Controls.ДобавитьКнопку("8", C2, R1)
Controls.SetSize(key8, 35*N, 25*N)
let key9 = Controls.ДобавитьКнопку("9", C3, R1)
Controls.SetSize(key9, 35*N, 25*N)
let key4 = Controls.ДобавитьКнопку("4", C1, R2)
Controls.SetSize(key4, 35*N, 25*N)
let key5 = Controls.ДобавитьКнопку("5", C2, R2)
Controls.SetSize(key5, 35*N, 25*N)
let key6 = Controls.ДобавитьКнопку("6", C3, R2)
Controls.SetSize(key6, 35*N, 25*N)
let key1 = Controls.ДобавитьКнопку("1", C1, R3)
Controls.SetSize(key1, 35*N, 25*N)
let key2 = Controls.ДобавитьКнопку("2", C2, R3)
Controls.SetSize(key2, 35*N, 25*N)
let key3 = Controls.ДобавитьКнопку("3", C3, R3)
Controls.SetSize(key3, 35*N, 25*N)
let key0 = Controls.ДобавитьКнопку("0", C2, R4)
Controls.SetSize(key0, 35*N, 25*N)

let keyplusminus = Controls.ДобавитьКнопку("+/-", C3, R4)
Controls.SetSize(keyplusminus, 35*N, 25*N)
let keytochka = Controls.ДобавитьКнопку(".", C1, R4)
Controls.SetSize(keytochka, 35*N, 25*N)

let keysqrt = Controls.ДобавитьКнопку("sqrt", C5, R1)
Controls.SetSize(keysqrt, 35*N, 25*N)
let keyprocent = Controls.ДобавитьКнопку("%", C5, R2)
Controls.SetSize(keyprocent, 35*N, 25*N)
let keynax = Controls.ДобавитьКнопку("1/x", C5, R3)
Controls.SetSize(keynax, 35*N, 25*N)

let mutable a = ""
let mutable b = ""
let mutable c = ""
let mutable знак = ""

let clicked () =
  //Sound.PlayClick()
  if Controls.LastClickedButton = key0 then
    Controls.SetTextBoxText(t, Text.Append(Controls.GetTextBoxText(t), "0"))
  elif Controls.LastClickedButton = key1 then
    Controls.SetTextBoxText(t, Text.Append(Controls.GetTextBoxText(t), "1"))
  elif Controls.LastClickedButton = key2 then
    Controls.SetTextBoxText(t, Text.Append(Controls.GetTextBoxText(t), "2"))
  elif Controls.LastClickedButton = key3 then
    Controls.SetTextBoxText(t, Text.Append(Controls.GetTextBoxText(t), "3"))
  elif Controls.LastClickedButton = key4 then
    Controls.SetTextBoxText(t, Text.Append(Controls.GetTextBoxText(t), "4"))
  elif Controls.LastClickedButton = key5 then
    Controls.SetTextBoxText(t, Text.Append(Controls.GetTextBoxText(t), "5"))
  elif Controls.LastClickedButton = key6 then
    Controls.SetTextBoxText(t, Text.Append(Controls.GetTextBoxText(t), "6"))
  elif Controls.LastClickedButton = key7 then
    Controls.SetTextBoxText(t, Text.Append(Controls.GetTextBoxText(t), "7"))
  elif Controls.LastClickedButton = key8 then
    Controls.SetTextBoxText(t, Text.Append(Controls.GetTextBoxText(t), "8"))
  elif Controls.LastClickedButton = key9 then
    Controls.SetTextBoxText(t, Text.Append(Controls.GetTextBoxText(t), "9"))
  elif Controls.LastClickedButton = keytochka then
    Controls.SetTextBoxText(t, Text.Append(Controls.GetTextBoxText(t), "."))
  elif Controls.LastClickedButton = keyplusminus then
    if p = 0 then
      Controls.SetTextBoxText(t, Text.Append("-", Controls.GetTextBoxText(t)))
      p <- 1
    else
      Controls.SetTextBoxText(t, string (Math.Abs(float (Controls.GetTextBoxText(t)))))
      p <- 0   
  elif Controls.LastClickedButton = keyplus then
    a <- Controls.GetTextBoxText(t)
    Controls.SetTextBoxText(t, "")
    знак <- "+"
  elif Controls.LastClickedButton = keyminus then
    a <- Controls.GetTextBoxText(t)
    Controls.SetTextBoxText(t, "")
    знак <- "-"
  elif Controls.LastClickedButton = keyumnogenie then
    a <- Controls.GetTextBoxText(t)
    Controls.SetTextBoxText(t, "")
    знак <- "*"
  elif Controls.LastClickedButton = keydelenie then
    a <- Controls.GetTextBoxText(t)
    Controls.SetTextBoxText(t, "")
    знак <- "/"
  elif Controls.LastClickedButton = keysqrt then
    Controls.SetTextBoxText(t, string (Math.SquareRoot(float(Controls.GetTextBoxText(t)))))
  elif Controls.LastClickedButton = keynax then
    Controls.SetTextBoxText(t, string (1.0/float (Controls.GetTextBoxText(t))))
  elif Controls.LastClickedButton = keyC then
    a <- ""
    b <- ""
    Controls.SetTextBoxText(t, "")
  elif Controls.LastClickedButton = keyCE then
    Controls.SetTextBoxText(t, "")
  elif Controls.LastClickedButton = keyBackspase then
    Controls.SetTextBoxText(t, Text.GetSubText(Controls.GetTextBoxText(t), 1, Text.GetLength(Controls.GetTextBoxText(t))-1))
  elif Controls.LastClickedButton = keyMP || Controls.LastClickedButton = keyMS then
    c <- Controls.GetTextBoxText(t)
    Shapes.ShowShape(m)
  elif Controls.LastClickedButton = keyMC then
    c <- ""
    Shapes.HideShape(m)
  elif Controls.LastClickedButton = keyMR then
    Controls.SetTextBoxText(t, c)
  elif Controls.LastClickedButton = keyprocent then
    Controls.SetTextBoxText(t, string (float a * float (Controls.GetTextBoxText(t))/100.0))
  elif Controls.LastClickedButton = keyravno then
    b <- Controls.GetTextBoxText(t)
    if знак = "+" then
      Controls.SetTextBoxText(t, string (float a + float b))
    elif знак = "-" then
      Controls.SetTextBoxText(t, string (float a - float b))
    elif знак = "*" then
      Controls.SetTextBoxText(t, string (float a * float b))
    elif знак = "/" then
      if float b = 0.0 then
        Controls.SetTextBoxText(t, "Divide by zero is impossible!")
      else
        Controls.SetTextBoxText(t, string (float a / float b))

Controls.ButtonClicked <- Callback(clicked)

Thread.Sleep 1000