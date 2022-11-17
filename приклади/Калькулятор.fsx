﻿#r "nuget: Avalonia.Desktop, 11.0.0-preview3"
#r "nuget: Avalonia.Themes.Fluent, 11.0.0-preview3"
#r "../ісх/bin/Debug/net7.0/ВеселШарп.Бібліотека.dll"

відкрити Бібліотека

нехай змінливий p = 0

ГрафичнеВікно.МожнаЗмінитиРозмір <- ложь
ГрафичнеВікно.Ширина <- 520
ГрафичнеВікно.Висота <- 460
ГрафичнеВікно.Заголовок <- "Калькулятор v. 1.0 від Alex_2000"

ГрафичнеВікно.КолірФона = ГрафичнеВікно.ОтриматиКолірЗRGB(240, 240, 240)
ГрафичнеВікно.ЖирністьШрифта <- ложь
ГрафичнеВікно.КолірПензлика <- Кольори.Black

нехай m = Фігури.ДодатиТекст("M")
Фігури.Перемістити(m, 22, 62)
Фігури.СкрытьФигуру(m)

нехай N = 2

нехай т = ЭлементиКерування.ДодатиТекстовеВікно(10, 10)
ЭлементиКерування.ВстановитиРозмір(т, 240*N, 22*N)
ЭлементиКерування.ВстановитиТекстТекстовогоВікна(т, "")

нехай R0 = 58
нехай R1 = R0 + (95-58)*N
нехай R2 = R1 + (128-95)*N 
нехай R3 = R2 + (161-128)*N
нехай R4 = R3 + (194-161)*N
нехай C0 = 10
нехай C1 = C0  + (58-10)*N
нехай C2 = C1  + (99-58)*N
нехай C3 = C2 + (140-99)*N
нехай C4 = C3 + (181-140)*N
нехай C5 = C4 + (222-181)*N
нехай T0 = C0
нехай T1 = T0 + (139-58)*N
нехай T2 = T1 + (202-139)*N 
ГрафичнеВікно.КолірПензлика <- Кольори.Red
нехай кнопкаMC = ЭлементиКерування.ДодатиКнопку("MC", C0, R1)
ЭлементиКерування.ВстановитиРозмір(кнопкаMC, 35*N, 25*N)
нехай кнопкаMR = ЭлементиКерування.ДодатиКнопку("MR", C0, R2)
ЭлементиКерування.ВстановитиРозмір(кнопкаMR, 35*N, 25*N)
нехай кнопкаMS = ЭлементиКерування.ДодатиКнопку("MS", C0, R3)
ЭлементиКерування.ВстановитиРозмір(кнопкаMS, 35*N, 25*N)
нехай кнопкаMплюс = ЭлементиКерування.ДодатиКнопку("M+", C0, R4)
ЭлементиКерування.ВстановитиРозмір(кнопкаMплюс, 35*N, 25*N)
нехай кнопкаC = ЭлементиКерування.ДодатиКнопку("C", T2, R0)
ЭлементиКерування.ВстановитиРозмір(кнопкаC, 55*N, 25*N )
нехай кнопкаCE = ЭлементиКерування.ДодатиКнопку("CE", T1, R0)
ЭлементиКерування.ВстановитиРозмір(кнопкаCE, 55*N, 25*N)
нехай кнопкаСтереть = ЭлементиКерування.ДодатиКнопку("Backspace", T0, R0)
ЭлементиКерування.ВстановитиРозмір(кнопкаСтереть, 70*N, 25*N)

нехай кнопкаделение = ЭлементиКерування.ДодатиКнопку("/", C4, R1)
ЭлементиКерування.ВстановитиРозмір(кнопкаделение, 35*N, 25*N)
нехай кнопкаумножение = ЭлементиКерування.ДодатиКнопку("*", C4, R2)
ЭлементиКерування.ВстановитиРозмір(кнопкаумножение, 35*N, 25*N)
нехай кнопкаминус = ЭлементиКерування.ДодатиКнопку("-", C4, R3)
ЭлементиКерування.ВстановитиРозмір(кнопкаминус, 35*N, 25*N)
нехай кнопкаплюс = ЭлементиКерування.ДодатиКнопку("+", C4, R4)
ЭлементиКерування.ВстановитиРозмір(кнопкаплюс, 35*N, 25*N)
нехай кнопкаравно = ЭлементиКерування.ДодатиКнопку("=", C5, R4)
ЭлементиКерування.ВстановитиРозмір(кнопкаравно, 35*N, 25*N)

ГрафичнеВікно.КолірПензлика <- Кольори.Blue
нехай кнопка7 = ЭлементиКерування.ДодатиКнопку("7", C1, R1)
ЭлементиКерування.ВстановитиРозмір(кнопка7, 35*N, 25*N)
нехай кнопка8 = ЭлементиКерування.ДодатиКнопку("8", C2, R1)
ЭлементиКерування.ВстановитиРозмір(кнопка8, 35*N, 25*N)
нехай кнопка9 = ЭлементиКерування.ДодатиКнопку("9", C3, R1)
ЭлементиКерування.ВстановитиРозмір(кнопка9, 35*N, 25*N)
нехай кнопка4 = ЭлементиКерування.ДодатиКнопку("4", C1, R2)
ЭлементиКерування.ВстановитиРозмір(кнопка4, 35*N, 25*N)
нехай кнопка5 = ЭлементиКерування.ДодатиКнопку("5", C2, R2)
ЭлементиКерування.ВстановитиРозмір(кнопка5, 35*N, 25*N)
нехай кнопка6 = ЭлементиКерування.ДодатиКнопку("6", C3, R2)
ЭлементиКерування.ВстановитиРозмір(кнопка6, 35*N, 25*N)
нехай кнопка1 = ЭлементиКерування.ДодатиКнопку("1", C1, R3)
ЭлементиКерування.ВстановитиРозмір(кнопка1, 35*N, 25*N)
нехай кнопка2 = ЭлементиКерування.ДодатиКнопку("2", C2, R3)
ЭлементиКерування.ВстановитиРозмір(кнопка2, 35*N, 25*N)
нехай кнопка3 = ЭлементиКерування.ДодатиКнопку("3", C3, R3)
ЭлементиКерування.ВстановитиРозмір(кнопка3, 35*N, 25*N)
нехай кнопка0 = ЭлементиКерування.ДодатиКнопку("0", C2, R4)
ЭлементиКерування.ВстановитиРозмір(кнопка0, 35*N, 25*N)

нехай кнопкаплюсминус = ЭлементиКерування.ДодатиКнопку("+/-", C3, R4)
ЭлементиКерування.ВстановитиРозмір(кнопкаплюсминус, 35*N, 25*N)
нехай кнопкаточка = ЭлементиКерування.ДодатиКнопку(".", C1, R4)
ЭлементиКерування.ВстановитиРозмір(кнопкаточка, 35*N, 25*N)

нехай кнопкакорень = ЭлементиКерування.ДодатиКнопку("sqrt", C5, R1)
ЭлементиКерування.ВстановитиРозмір(кнопкакорень, 35*N, 25*N)
нехай keyprocent = ЭлементиКерування.ДодатиКнопку("%", C5, R2)
ЭлементиКерування.ВстановитиРозмір(keyprocent, 35*N, 25*N)
нехай кнопканаХ = ЭлементиКерування.ДодатиКнопку("1/x", C5, R3)
ЭлементиКерування.ВстановитиРозмір(кнопканаХ, 35*N, 25*N)

нехай змінливий a = ""
нехай змінливий b = ""
нехай змінливий c = ""
нехай змінливий знак = ""

нехай нажато () =
  //Sound.PlayClick()
  якщо ЭлементиКерування.ОстанняНажатаКнопка = кнопка0 тоді
    ЭлементиКерування.ВстановитиТекстТекстовогоВікна(т, Текст.Додати(ЭлементиКерування.ОтриматиТекстТекстовогоВікна(т), "0"))
  інякщо ЭлементиКерування.ОстанняНажатаКнопка = кнопка1 тоді
    ЭлементиКерування.ВстановитиТекстТекстовогоВікна(т, Текст.Додати(ЭлементиКерування.ОтриматиТекстТекстовогоВікна(т), "1"))
  інякщо ЭлементиКерування.ОстанняНажатаКнопка = кнопка2 тоді
    ЭлементиКерування.ВстановитиТекстТекстовогоВікна(т, Текст.Додати(ЭлементиКерування.ОтриматиТекстТекстовогоВікна(т), "2"))
  інякщо ЭлементиКерування.ОстанняНажатаКнопка = кнопка3 тоді
    ЭлементиКерування.ВстановитиТекстТекстовогоВікна(т, Текст.Додати(ЭлементиКерування.ОтриматиТекстТекстовогоВікна(т), "3"))
  інякщо ЭлементиКерування.ОстанняНажатаКнопка = кнопка4 тоді
    ЭлементиКерування.ВстановитиТекстТекстовогоВікна(т, Текст.Додати(ЭлементиКерування.ОтриматиТекстТекстовогоВікна(т), "4"))
  інякщо ЭлементиКерування.ОстанняНажатаКнопка = кнопка5 тоді
    ЭлементиКерування.ВстановитиТекстТекстовогоВікна(т, Текст.Додати(ЭлементиКерування.ОтриматиТекстТекстовогоВікна(т), "5"))
  інякщо ЭлементиКерування.ОстанняНажатаКнопка = кнопка6 тоді
    ЭлементиКерування.ВстановитиТекстТекстовогоВікна(т, Текст.Додати(ЭлементиКерування.ОтриматиТекстТекстовогоВікна(т), "6"))
  інякщо ЭлементиКерування.ОстанняНажатаКнопка = кнопка7 тоді
    ЭлементиКерування.ВстановитиТекстТекстовогоВікна(т, Текст.Додати(ЭлементиКерування.ОтриматиТекстТекстовогоВікна(т), "7"))
  інякщо ЭлементиКерування.ОстанняНажатаКнопка = кнопка8 тоді
    ЭлементиКерування.ВстановитиТекстТекстовогоВікна(т, Текст.Додати(ЭлементиКерування.ОтриматиТекстТекстовогоВікна(т), "8"))
  інякщо ЭлементиКерування.ОстанняНажатаКнопка = кнопка9 тоді
    ЭлементиКерування.ВстановитиТекстТекстовогоВікна(т, Текст.Додати(ЭлементиКерування.ОтриматиТекстТекстовогоВікна(т), "9"))
  інякщо ЭлементиКерування.ОстанняНажатаКнопка = кнопкаточка тоді
    ЭлементиКерування.ВстановитиТекстТекстовогоВікна(т, Текст.Додати(ЭлементиКерування.ОтриматиТекстТекстовогоВікна(т), "."))
  інякщо ЭлементиКерування.ОстанняНажатаКнопка = кнопкаплюсминус тоді
    якщо p = 0 тоді
      ЭлементиКерування.ВстановитиТекстТекстовогоВікна(т, Текст.Додати("-", ЭлементиКерування.ОтриматиТекстТекстовогоВікна(т)))
      p <- 1
    інакше
      ЭлементиКерування.ВстановитиТекстТекстовогоВікна(т, string (Математика.Модуль(float (ЭлементиКерування.ОтриматиТекстТекстовогоВікна(т)))))
      p <- 0   
  інякщо ЭлементиКерування.ОстанняНажатаКнопка = кнопкаплюс тоді
    a <- ЭлементиКерування.ОтриматиТекстТекстовогоВікна(т)
    ЭлементиКерування.ВстановитиТекстТекстовогоВікна(т, "")
    знак <- "+"
  інякщо ЭлементиКерування.ОстанняНажатаКнопка = кнопкаминус тоді
    a <- ЭлементиКерування.ОтриматиТекстТекстовогоВікна(т)
    ЭлементиКерування.ВстановитиТекстТекстовогоВікна(т, "")
    знак <- "-"
  інякщо ЭлементиКерування.ОстанняНажатаКнопка = кнопкаумножение тоді
    a <- ЭлементиКерування.ОтриматиТекстТекстовогоВікна(т)
    ЭлементиКерування.ВстановитиТекстТекстовогоВікна(т, "")
    знак <- "*"
  інякщо ЭлементиКерування.ОстанняНажатаКнопка = кнопкаделение тоді
    a <- ЭлементиКерування.ОтриматиТекстТекстовогоВікна(т)
    ЭлементиКерування.ВстановитиТекстТекстовогоВікна(т, "")
    знак <- "/"
  інякщо ЭлементиКерування.ОстанняНажатаКнопка = кнопкакорень тоді
    ЭлементиКерування.ВстановитиТекстТекстовогоВікна(т, string (Математика.КвадратнийКорінь(float(ЭлементиКерування.ОтриматиТекстТекстовогоВікна(т)))))
  інякщо ЭлементиКерування.ОстанняНажатаКнопка = кнопканаХ тоді
    ЭлементиКерування.ВстановитиТекстТекстовогоВікна(т, string (1.0/float (ЭлементиКерування.ОтриматиТекстТекстовогоВікна(т))))
  інякщо ЭлементиКерування.ОстанняНажатаКнопка = кнопкаC тоді
    a <- ""
    b <- ""
    ЭлементиКерування.ВстановитиТекстТекстовогоВікна(т, "")
  інякщо ЭлементиКерування.ОстанняНажатаКнопка = кнопкаCE тоді
    ЭлементиКерування.ВстановитиТекстТекстовогоВікна(т, "")
  інякщо ЭлементиКерування.ОстанняНажатаКнопка = кнопкаСтереть тоді
    ЭлементиКерування.ВстановитиТекстТекстовогоВікна(т, Текст.ОтриматиЧастинуТекста(ЭлементиКерування.ОтриматиТекстТекстовогоВікна(т), 1, Текст.ОтриматиДовжину(ЭлементиКерування.ОтриматиТекстТекстовогоВікна(т))-1))
  інякщо ЭлементиКерування.ОстанняНажатаКнопка = кнопкаMплюс || ЭлементиКерування.ОстанняНажатаКнопка = кнопкаMS тоді
    c <- ЭлементиКерування.ОтриматиТекстТекстовогоВікна(т)
    Фігури.ПоказатьФигуру(m)
  інякщо ЭлементиКерування.ОстанняНажатаКнопка = кнопкаMC тоді
    c <- ""
    Фігури.СкрытьФигуру(m)
  інякщо ЭлементиКерування.ОстанняНажатаКнопка = кнопкаMR тоді
    ЭлементиКерування.ВстановитиТекстТекстовогоВікна(т, c)
  інякщо ЭлементиКерування.ОстанняНажатаКнопка = keyprocent тоді
    ЭлементиКерування.ВстановитиТекстТекстовогоВікна(т, string (float a * float (ЭлементиКерування.ОтриматиТекстТекстовогоВікна(т))/100.0))
  інякщо ЭлементиКерування.ОстанняНажатаКнопка = кнопкаравно тоді
    b <- ЭлементиКерування.ОтриматиТекстТекстовогоВікна(т)
    якщо знак = "+" тоді
      ЭлементиКерування.ВстановитиТекстТекстовогоВікна(т, string (float a + float b))
    інякщо знак = "-" тоді
      ЭлементиКерування.ВстановитиТекстТекстовогоВікна(т, string (float a - float b))
    інякщо знак = "*" тоді
      ЭлементиКерування.ВстановитиТекстТекстовогоВікна(т, string (float a * float b))
    інякщо знак = "/" тоді
      якщо float b = 0.0 тоді
        ЭлементиКерування.ВстановитиТекстТекстовогоВікна(т, "Divide by zero is impossible!")
      інакше
        ЭлементиКерування.ВстановитиТекстТекстовогоВікна(т, string (float a / float b))

ЭлементиКерування.КнопкаКликнута <- Callback(нажато)

Програма.Затримка(20_000)