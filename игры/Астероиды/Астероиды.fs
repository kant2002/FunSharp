#if INTERACTIVE
#r "./bin/debug/Xwt.dll"
#r "./bin/debug/FunSharp.dll"
#endif

open Библиотека

// Asteroids Game
// Copyright (C) 2009, Jason T. Jacques 
// License: MIT license http://www.opensource.org/licenses/mit-license.php

// Game area controls
let ширинаИгры  = 640.0
let высотаИгры = 480.0
let цветФона = Цвета.Black

// Window title
let заголовокИгры = "Астероиды, Счет: "

// Target frames per second
let fps = 25

// Управление кнопками
let кнопкаЛево  = "Left"
let кнопкаПраво = "Right"
let КнопкаВперед = "Up"
let кнопкаНазад = "Down"
let кнопкаОгонь = "Space"
let кнопкаПауза = "P"

// Asteroid (rock) settings
let скоростьАстероидов = 1.0
let цветАстероидов = Цвета.White
let минАстероид = 20 // small size rock
let типыАстероидов = 3 // number of rock sizes (multiples of small rock size)
let mutable иницАстероиды = 5

// Ammo settings
let скоростьПули = 5.0
let цветПули = Цвета.White
let жизньПули = 60 // moves before auto destruct
let максПули = 10
let размерПули = 5.0

// Player settings
let цветИгрока = Цвета.White
let высотаИгрока = 30.0
let ширинаИгрока = 20.0
let mutable игрок = ""
let mutable уголИгрока = 0.0
let mutable скоростьИгрока = 0.0
let безопасноеВремя = 100 // time player has to get out of the way on level up

// Point multiplier
let мультипликаторОчков = 10

// Array name initialisation
let астероид = ResizeArray()
let уголАстероида = ResizeArray()
let размерАстероида = ResizeArray()
let пуля = ResizeArray()
let уголПули = ResizeArray<float>()
let возрастПули = ResizeArray()

let mutable количествоАстероидов = 0
let mutable количествоПуль = 0

let путь = "" // "http://smallbasic.com/drop/"
let большойАстероид = СписокИзображений.ЗагрузитьИзображение(путь + "Asteroids_BigRock.png")
let среднийАстероид = СписокИзображений.ЗагрузитьИзображение(путь + "Asteroids_MediumRock.png")
let малАстероид = СписокИзображений.ЗагрузитьИзображение(путь + "Asteroids_SmallRock.png")
let фон = СписокИзображений.ЗагрузитьИзображение(путь + "Asteroids_Sky.png")

let mutable пауза = 0
let mutable игра = 0
let mutable безопасностьИгрока = 0
let mutable счет = 0

let mutable следующийРазмер = 0
let mutable следующаяПозиция = ""
let mutable px1 = 0.0
let mutable py1 = 0.0
let mutable px2 = 0.0
let mutable py2 = 0.0

// Настроить мир
let rec Инициализация () =
   ГрафическоеОкно.Спрятать()
   ГрафическоеОкно.Заголовок <- заголовокИгры + "0"
   //GraphicsWindow.CanResize <- "False"
   ГрафическоеОкно.Ширина <- int ширинаИгры
   ГрафическоеОкно.Высота <-int высотаИгры

   ГрафическоеОкно.ЦветФона <- цветФона
   ГрафическоеОкно.ЦветКисти <- цветФона
   ГрафическоеОкно.НарисоватьИзображение(фон, 0, 0)

   ПроверкаУровня()

   ГрафическоеОкно.ЦветПера <- цветИгрока
   игрок <- Фигуры.ДобавитьИзображение(путь + "Asteroids_Ship.png")
   // player = Фигуры.AddTriangle(playerWidth/2, 0, 0, playerHeight, playerWidth, playerHeight)
   Фигуры.Переместить(игрок, (ширинаИгры - ширинаИгрока) / 2.0, (float высотаИгры - высотаИгрока) / 2.0)
   уголИгрока <- 0.0

// Главная игровая процедура
and Игра () =
   ГрафическоеОкно.Показать()
   ГрафическоеОкно.КнопкаНажата <- Callback(ИзменитьНаправление)

   // Main loop
   игра <- 1
   пауза <- 0
   while (игра = 1) do
     Программа.Задержка(1000/fps)
     if (пауза = 0) then
       Движение()
       ПроверкаСтолкновений()
       УстареваниеПуль()
       ПроверкаУровня()

// Read key event and act
and ИзменитьНаправление () =   
   if (ГрафическоеОкно.ПоследняяКнопка = кнопкаПраво) then
     уголИгрока <- (уголИгрока + 10.0) % 360.0
   elif (ГрафическоеОкно.ПоследняяКнопка = кнопкаЛево) then
     уголИгрока <- (уголИгрока - 10.0) % 360.0
   elif (ГрафическоеОкно.ПоследняяКнопка = КнопкаВперед) then
     скоростьИгрока <- скоростьИгрока + 1.0
   elif (ГрафическоеОкно.ПоследняяКнопка = кнопкаНазад) then
     скоростьИгрока <- скоростьИгрока - 1.0
   elif (ГрафическоеОкно.ПоследняяКнопка = кнопкаОгонь) then
     Выстрел()
   elif (ГрафическоеОкно.ПоследняяКнопка = кнопкаПауза) then
     пауза <- Math.Остаток(пауза + 1, 2)  
   Фигуры.Повернуть(игрок, уголИгрока)

// Move all on screen items
and Движение  () =
   // Move player
   let x = Math.Остаток(Фигуры.ПолучитьЛево(игрок) + (Math.Cos(Math.ВзятьРадианы(уголИгрока - 90.0)) * скоростьИгрока) + ширинаИгры, ширинаИгры)
   let y = Math.Остаток(Фигуры.ПолучитьВерх(игрок) + (Math.Sin(Math.ВзятьРадианы(уголИгрока - 90.0)) * скоростьИгрока) + высотаИгры, высотаИгры)
   Фигуры.Переместить(игрок, x, y)

   // Move rocks
   for i = 0 to количествоАстероидов-1 do
     let x = Math.Остаток(Фигуры.ПолучитьЛево(астероид.[i]) + (Math.Cos(Math.ВзятьРадианы(уголАстероида.[i] - 90.0)) * скоростьАстероидов) + ширинаИгры, ширинаИгры)
     let y = Math.Остаток(Фигуры.ПолучитьВерх(астероид.[i]) + (Math.Sin(Math.ВзятьРадианы(уголАстероида.[i] - 90.0)) * скоростьАстероидов) + высотаИгры, высотаИгры)
     Фигуры.Переместить(астероид.[i], x, y)

   // Move ammo
   for i = 0 to количествоПуль-1 do
     let x = Math.Остаток(Фигуры.ПолучитьЛево(пуля.[i]) + (Math.Cos(Math.ВзятьРадианы(уголПули.[i] - 90.0)) * скоростьПули) + ширинаИгры, ширинаИгры)
     let y = Math.Остаток(Фигуры.ПолучитьВерх(пуля.[i]) + (Math.Sin(Math.ВзятьРадианы(уголПули.[i] - 90.0)) * скоростьПули) + высотаИгры, высотаИгры)
     Фигуры.Переместить(пуля.[i], x, y)
     возрастПули.[i] <- возрастПули.[i] + 1

// Check for collisions between onscreen items
and ПроверкаСтолкновений () =
   // Calculate player bounding box.
   px1 <- Фигуры.ПолучитьЛево(игрок) - ( (Math.Abs(ширинаИгрока * Math.Cos(Math.ВзятьРадианы(уголИгрока)) + высотаИгрока * Math.Sin(Math.ВзятьРадианы(уголИгрока))) - ширинаИгрока) / 2.0)
   py1 <- Фигуры.ПолучитьВерх(игрок) - ( (Math.Abs(ширинаИгрока * Math.Sin(Math.ВзятьРадианы(уголИгрока)) + высотаИгрока * Math.Cos(Math.ВзятьРадианы(уголИгрока))) - высотаИгрока) / 2.0)
   px2 <- px1 + Math.Abs(ширинаИгрока * Math.Cos(Math.ВзятьРадианы(уголИгрока)) + высотаИгрока * Math.Sin(Math.ВзятьРадианы(уголИгрока)))
   py2 <- py1 + Math.Abs(ширинаИгрока * Math.Sin(Math.ВзятьРадианы(уголИгрока)) + высотаИгрока * Math.Cos(Math.ВзятьРадианы(уголИгрока)))

   // Re-order co-oridinates if they are the wrong way arround
   if (px1 > px2) then
     let tmp = px1
     px1 <- px2
     px2 <- tmp  
   if (py1 > py2) then
     let tmp = py1
     py1 <- py2
     py2 <- tmp 

   let астероидыКУдалению = ResizeArray()
   let пулиКУдалению = ResizeArray()
   // Check if each rock has hit something
   for i = 0 to количествоАстероидов-1 do
     let ax1 = Фигуры.ПолучитьЛево(астероид.[i])
     let ay1 = Фигуры.ПолучитьВерх(астероид.[i])
     let ax2 = ax1 + float размерАстероида.[i]
     let ay2 = ay1 + float размерАстероида.[i]

     // Player collison
     if (безопасностьИгрока < 1) then
       if ( (ax1 < px1 && ax2 > px1) || (ax1 < px2 && ax2 > px2) ) then
         if ( (ay1 < py1 && ay2 > py1) || (ay1 < py2 && ay2 > py2) ) then
           КонецИгры()

     // Ammo collison
     for j in 0..количествоПуль-1 do          
         let bx1 = Фигуры.ПолучитьЛево(пуля.[j])
         let by1 = Фигуры.ПолучитьВерх(пуля.[j])
         let bx2 = bx1 + размерПули
         let by2 = by1 + размерПули

         if ( (ax1 < bx1 && ax2 > bx1) || (ax1 < bx2 && ax2 > bx2) ) then
            if ( (ay1 < by1 && ay2 > by1) || (ay1 < by2 && ay2 > by2) ) then           
               астероидыКУдалению.Add(i)
               пулиКУдалению.Add(j)

   for i in астероидыКУдалению |> Seq.distinct |> List.ofSeq |> List.rev do
      УдалитьАстероид i
   for j in пулиКУдалению |> Seq.distinct |> List.ofSeq |> List.rev do
      УдалитьПулю j

   // Decrease the time player is safe
   if (безопасностьИгрока > 0) then
     безопасностьИгрока <- безопасностьИгрока - 1   

// Add a new rock to the world
and ДобавитьАстероид () =
   // Check if the next rock size/position has been specified   
   let размер,x,y =
      if (следующийРазмер <> 0) then
         let размер = минАстероид * следующийРазмер
         let x = Фигуры.ПолучитьЛево(следующаяПозиция)
         let y = Фигуры.ПолучитьВерх(следующаяПозиция)
         следующийРазмер <- 0
         размер,x,y
      else
         // Choose a random size and position
         let размер = минАстероид * Math.ВзятьСлучайноеЧисло(типыАстероидов)
         let x = Math.ВзятьСлучайноеЧисло(int ширинаИгры - размер)
         let y = Math.ВзятьСлучайноеЧисло(int высотаИгры - размер)
         размер,float x,float y
   // Draw the rock
   ГрафическоеОкно.ЦветПера <- цветАстероидов
   let изображение =
      if размер = 60 then большойАстероид
      elif размер = 40 then среднийАстероид
      else малАстероид
   астероид.Add(Фигуры.ДобавитьИзображение(изображение))
   //Фигуры.Zoom(rock.[rockCount],1.0,1.0)
   Фигуры.Переместить(астероид.[количествоАстероидов], x, y)
   уголАстероида.Add(float (Math.ВзятьСлучайноеЧисло(360)))
   размерАстероида.Add(размер)
   количествоАстероидов <- количествоАстероидов + 1

// Remove a rock from the world and update score
and УдалитьАстероид следующийУдаляемый =
   let mutable удаляемыйРазмер = размерАстероида.[следующийУдаляемый] / минАстероид

   // If not a mini rock
   if (удаляемыйРазмер > 1) then
     // ... add new rocks until we have made up for it being broken apart...
     while (удаляемыйРазмер > 0) do
       следующийРазмер <- Math.ВзятьСлучайноеЧисло(удаляемыйРазмер - 1)
       следующаяПозиция <- астероид.[следующийУдаляемый]
       удаляемыйРазмер <- удаляемыйРазмер - следующийРазмер
       ДобавитьАстероид ()
     // And give a point for a 'hit'
     счет <- счет + 1
   else
     // We've destroyed it - give some extra points and 
     счет <- счет + 5   

   // Show updated score
   ГрафическоеОкно.Заголовок <- заголовокИгры + (счет * мультипликаторОчков).ToString()

   // Remove all references from the arrays
   Фигуры.Удалить(астероид.[следующийУдаляемый])
      
   астероид.RemoveAt(следующийУдаляемый)
   уголАстероида.RemoveAt(следующийУдаляемый)
   размерАстероида.RemoveAt(следующийУдаляемый)
   количествоАстероидов <- количествоАстероидов - 1

// Check if the player has completed the level, if so, level up
and ПроверкаУровня () =
   if (количествоАстероидов < 1) then
     следующийРазмер <- 0
     for i = 1 to иницАстероиды do
       ДобавитьАстероид()     
     иницАстероиды <- иницАстероиды + 1
     // Give players some time to move out of the way
     безопасностьИгрока <- безопасноеВремя   

// Add ammo to game
and Выстрел () =
   // Remove additional ammo
   while (количествоПуль > (максПули - 1)) do     
     УдалитьПулю 0
   // Add the ammo
   ГрафическоеОкно.ЦветПера <- цветПули   
   пуля.Add(Фигуры.ДобавитьЭллипс(размерПули, размерПули))
   Фигуры.Переместить(пуля.[количествоПуль], (px1 + px2 - размерПули) / 2.0, (py1 + py2 - размерПули) / 2.0)
   уголПули.Add(уголИгрока)
   возрастПули.Add(0)
   количествоПуль <- количествоПуль + 1

// Check ammo age
and УстареваниеПуль () =
   while возрастПули.Count > 0 && (возрастПули.[0] > жизньПули) do     
      УдалитьПулю 0

// Remove top Ammo
and УдалитьПулю следующийУдаляемый =
   Фигуры.Удалить(пуля.[следующийУдаляемый])
   пуля.RemoveAt(следующийУдаляемый)
   уголПули.RemoveAt(следующийУдаляемый)
   возрастПули.RemoveAt(следующийУдаляемый)
   количествоПуль <- количествоПуль - 1
   
// Display simple end game message box
and КонецИгры () =
   игра <- 0
   Фигуры.Удалить(игрок)
   ГрафическоеОкно.ПоказатьСообщение("Ваш счет " + (счет * мультипликаторОчков).ToString() + " очков. Спасибо за игру.", "Игра окончена!")

// Start game
Инициализация()
Игра()
