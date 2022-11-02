namespace Бібліотека

[<Sealed>]
type Таймер private () =
   static member Пауза() = Моя.Апплікація.ПаузаТаймера()
   static member Відновити() = Моя.Апплікація.ВідновитиТаймер()
   static member Тик with set (callback:Callback) = Моя.Апплікація.TimerTick <- callback
   static member Интервал with set (ms:int) = Моя.Апплікація.TimerInterval <- ms