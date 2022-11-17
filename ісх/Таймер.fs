простір Бібліотека

[<Sealed>]
тип Таймер private () =
   static member Пауза() = Моя.Апплікація.ПаузаТаймера()
   static member Відновити() = Моя.Апплікація.ВідновитиТаймер()
   static member Тик із set (callback:Callback) = Моя.Апплікація.TimerTick <- callback
   static member Интервал із set (ms:int) = Моя.Апплікація.TimerInterval <- ms