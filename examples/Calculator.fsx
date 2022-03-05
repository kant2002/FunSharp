#r "nuget: Xwt"
#r "../src/bin/Debug/net48/FunSharp.Library.dll"

open Библиотека
open System.Threading

let mutable p = 0

ГрафическоеОкно.CanResize <- false
ГрафическоеОкно.Ширина <- 260
ГрафическоеОкно.Высота <- 230
ГрафическоеОкно.Заголовок <- "Calculator v. 1.0 by Alex_2000"

ГрафическоеОкно.ЦветФона = ГрафическоеОкно.GetColorFromRGB(240, 240, 240)
ГрафическоеОкно.FontBold <- false
ГрафическоеОкно.ЦветКисти <- Цвета.Black

let m = Фигуры.ДобавитьТекст("M")
Фигуры.Переместить(m, 22, 62)
Фигуры.СкрытьФигуру(m)

let N = 1

let t = Controls.ДобавитьТекстовоеПоле(10, 10)
Controls.УстановитьРазмер(t, 240*N, 22*N)
Controls.УстановитьТекстТекстовогоПоля(t, "")

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
ГрафическоеОкно.ЦветКисти <- Цвета.Red
let keyMC = Controls.ДобавитьКнопку("MC", C0, R1)
Controls.УстановитьРазмер(keyMC, 35*N, 25*N)
let keyMR = Controls.ДобавитьКнопку("MR", C0, R2)
Controls.УстановитьРазмер(keyMR, 35*N, 25*N)
let keyMS = Controls.ДобавитьКнопку("MS", C0, R3)
Controls.УстановитьРазмер(keyMS, 35*N, 25*N)
let keyMP = Controls.ДобавитьКнопку("M+", C0, R4)
Controls.УстановитьРазмер(keyMP, 35*N, 25*N)
let keyC = Controls.ДобавитьКнопку("C", T2, R0)
Controls.УстановитьРазмер(keyC, 55*N, 25*N )
let keyCE = Controls.ДобавитьКнопку("CE", T1, R0)
Controls.УстановитьРазмер(keyCE, 55*N, 25*N)
let keyBackspase = Controls.ДобавитьКнопку("Backspace", T0, R0)
Controls.УстановитьРазмер(keyBackspase, 70*N, 25*N)

let keydelenie = Controls.ДобавитьКнопку("/", C4, R1)
Controls.УстановитьРазмер(keydelenie, 35*N, 25*N)
let keyumnogenie = Controls.ДобавитьКнопку("*", C4, R2)
Controls.УстановитьРазмер(keyumnogenie, 35*N, 25*N)
let keyminus = Controls.ДобавитьКнопку("-", C4, R3)
Controls.УстановитьРазмер(keyminus, 35*N, 25*N)
let keyplus = Controls.ДобавитьКнопку("+", C4, R4)
Controls.УстановитьРазмер(keyplus, 35*N, 25*N)
let keyravno = Controls.ДобавитьКнопку("=", C5, R4)
Controls.УстановитьРазмер(keyravno, 35*N, 25*N)

ГрафическоеОкно.ЦветКисти <- Цвета.Blue
let key7 = Controls.ДобавитьКнопку("7", C1, R1)
Controls.УстановитьРазмер(key7, 35*N, 25*N)
let key8 = Controls.ДобавитьКнопку("8", C2, R1)
Controls.УстановитьРазмер(key8, 35*N, 25*N)
let key9 = Controls.ДобавитьКнопку("9", C3, R1)
Controls.УстановитьРазмер(key9, 35*N, 25*N)
let key4 = Controls.ДобавитьКнопку("4", C1, R2)
Controls.УстановитьРазмер(key4, 35*N, 25*N)
let key5 = Controls.ДобавитьКнопку("5", C2, R2)
Controls.УстановитьРазмер(key5, 35*N, 25*N)
let key6 = Controls.ДобавитьКнопку("6", C3, R2)
Controls.УстановитьРазмер(key6, 35*N, 25*N)
let key1 = Controls.ДобавитьКнопку("1", C1, R3)
Controls.УстановитьРазмер(key1, 35*N, 25*N)
let key2 = Controls.ДобавитьКнопку("2", C2, R3)
Controls.УстановитьРазмер(key2, 35*N, 25*N)
let key3 = Controls.ДобавитьКнопку("3", C3, R3)
Controls.УстановитьРазмер(key3, 35*N, 25*N)
let key0 = Controls.ДобавитьКнопку("0", C2, R4)
Controls.УстановитьРазмер(key0, 35*N, 25*N)

let keyplusminus = Controls.ДобавитьКнопку("+/-", C3, R4)
Controls.УстановитьРазмер(keyplusminus, 35*N, 25*N)
let keytochka = Controls.ДобавитьКнопку(".", C1, R4)
Controls.УстановитьРазмер(keytochka, 35*N, 25*N)

let keysqrt = Controls.ДобавитьКнопку("sqrt", C5, R1)
Controls.УстановитьРазмер(keysqrt, 35*N, 25*N)
let keyprocent = Controls.ДобавитьКнопку("%", C5, R2)
Controls.УстановитьРазмер(keyprocent, 35*N, 25*N)
let keynax = Controls.ДобавитьКнопку("1/x", C5, R3)
Controls.УстановитьРазмер(keynax, 35*N, 25*N)

let mutable a = ""
let mutable b = ""
let mutable c = ""
let mutable знак = ""

let clicked () =
  //Sound.PlayClick()
  if Controls.ПоследняяКликнутаяКнопка = key0 then
    Controls.УстановитьТекстТекстовогоПоля(t, Text.Append(Controls.ПолучитьТекстТекстовогоПоля(t), "0"))
  elif Controls.ПоследняяКликнутаяКнопка = key1 then
    Controls.УстановитьТекстТекстовогоПоля(t, Text.Append(Controls.ПолучитьТекстТекстовогоПоля(t), "1"))
  elif Controls.ПоследняяКликнутаяКнопка = key2 then
    Controls.УстановитьТекстТекстовогоПоля(t, Text.Append(Controls.ПолучитьТекстТекстовогоПоля(t), "2"))
  elif Controls.ПоследняяКликнутаяКнопка = key3 then
    Controls.УстановитьТекстТекстовогоПоля(t, Text.Append(Controls.ПолучитьТекстТекстовогоПоля(t), "3"))
  elif Controls.ПоследняяКликнутаяКнопка = key4 then
    Controls.УстановитьТекстТекстовогоПоля(t, Text.Append(Controls.ПолучитьТекстТекстовогоПоля(t), "4"))
  elif Controls.ПоследняяКликнутаяКнопка = key5 then
    Controls.УстановитьТекстТекстовогоПоля(t, Text.Append(Controls.ПолучитьТекстТекстовогоПоля(t), "5"))
  elif Controls.ПоследняяКликнутаяКнопка = key6 then
    Controls.УстановитьТекстТекстовогоПоля(t, Text.Append(Controls.ПолучитьТекстТекстовогоПоля(t), "6"))
  elif Controls.ПоследняяКликнутаяКнопка = key7 then
    Controls.УстановитьТекстТекстовогоПоля(t, Text.Append(Controls.ПолучитьТекстТекстовогоПоля(t), "7"))
  elif Controls.ПоследняяКликнутаяКнопка = key8 then
    Controls.УстановитьТекстТекстовогоПоля(t, Text.Append(Controls.ПолучитьТекстТекстовогоПоля(t), "8"))
  elif Controls.ПоследняяКликнутаяКнопка = key9 then
    Controls.УстановитьТекстТекстовогоПоля(t, Text.Append(Controls.ПолучитьТекстТекстовогоПоля(t), "9"))
  elif Controls.ПоследняяКликнутаяКнопка = keytochka then
    Controls.УстановитьТекстТекстовогоПоля(t, Text.Append(Controls.ПолучитьТекстТекстовогоПоля(t), "."))
  elif Controls.ПоследняяКликнутаяКнопка = keyplusminus then
    if p = 0 then
      Controls.УстановитьТекстТекстовогоПоля(t, Text.Append("-", Controls.ПолучитьТекстТекстовогоПоля(t)))
      p <- 1
    else
      Controls.УстановитьТекстТекстовогоПоля(t, string (Math.Abs(float (Controls.ПолучитьТекстТекстовогоПоля(t)))))
      p <- 0   
  elif Controls.ПоследняяКликнутаяКнопка = keyplus then
    a <- Controls.ПолучитьТекстТекстовогоПоля(t)
    Controls.УстановитьТекстТекстовогоПоля(t, "")
    знак <- "+"
  elif Controls.ПоследняяКликнутаяКнопка = keyminus then
    a <- Controls.ПолучитьТекстТекстовогоПоля(t)
    Controls.УстановитьТекстТекстовогоПоля(t, "")
    знак <- "-"
  elif Controls.ПоследняяКликнутаяКнопка = keyumnogenie then
    a <- Controls.ПолучитьТекстТекстовогоПоля(t)
    Controls.УстановитьТекстТекстовогоПоля(t, "")
    знак <- "*"
  elif Controls.ПоследняяКликнутаяКнопка = keydelenie then
    a <- Controls.ПолучитьТекстТекстовогоПоля(t)
    Controls.УстановитьТекстТекстовогоПоля(t, "")
    знак <- "/"
  elif Controls.ПоследняяКликнутаяКнопка = keysqrt then
    Controls.УстановитьТекстТекстовогоПоля(t, string (Math.SquareRoot(float(Controls.ПолучитьТекстТекстовогоПоля(t)))))
  elif Controls.ПоследняяКликнутаяКнопка = keynax then
    Controls.УстановитьТекстТекстовогоПоля(t, string (1.0/float (Controls.ПолучитьТекстТекстовогоПоля(t))))
  elif Controls.ПоследняяКликнутаяКнопка = keyC then
    a <- ""
    b <- ""
    Controls.УстановитьТекстТекстовогоПоля(t, "")
  elif Controls.ПоследняяКликнутаяКнопка = keyCE then
    Controls.УстановитьТекстТекстовогоПоля(t, "")
  elif Controls.ПоследняяКликнутаяКнопка = keyBackspase then
    Controls.УстановитьТекстТекстовогоПоля(t, Text.GetSubText(Controls.ПолучитьТекстТекстовогоПоля(t), 1, Text.GetLength(Controls.ПолучитьТекстТекстовогоПоля(t))-1))
  elif Controls.ПоследняяКликнутаяКнопка = keyMP || Controls.ПоследняяКликнутаяКнопка = keyMS then
    c <- Controls.ПолучитьТекстТекстовогоПоля(t)
    Фигуры.ПоказатьФигуру(m)
  elif Controls.ПоследняяКликнутаяКнопка = keyMC then
    c <- ""
    Фигуры.СкрытьФигуру(m)
  elif Controls.ПоследняяКликнутаяКнопка = keyMR then
    Controls.УстановитьТекстТекстовогоПоля(t, c)
  elif Controls.ПоследняяКликнутаяКнопка = keyprocent then
    Controls.УстановитьТекстТекстовогоПоля(t, string (float a * float (Controls.ПолучитьТекстТекстовогоПоля(t))/100.0))
  elif Controls.ПоследняяКликнутаяКнопка = keyravno then
    b <- Controls.ПолучитьТекстТекстовогоПоля(t)
    if знак = "+" then
      Controls.УстановитьТекстТекстовогоПоля(t, string (float a + float b))
    elif знак = "-" then
      Controls.УстановитьТекстТекстовогоПоля(t, string (float a - float b))
    elif знак = "*" then
      Controls.УстановитьТекстТекстовогоПоля(t, string (float a * float b))
    elif знак = "/" then
      if float b = 0.0 then
        Controls.УстановитьТекстТекстовогоПоля(t, "Divide by zero is impossible!")
      else
        Controls.УстановитьТекстТекстовогоПоля(t, string (float a / float b))

Controls.КнопкаКликнута <- Callback(clicked)

Thread.Sleep 1000