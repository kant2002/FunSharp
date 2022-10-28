namespace Бiблiотека

[<Sealed>]
type Таймер private () =
   static member Пауза() = Моя.Апплікація.ПаузаТаймера()
   static member Возобновить() = Моя.Апплікація.ВозобновитьТаймер()
   static member Тик with set (callback:Callback) = Моя.Апплікація.TimerTick <- callback
   static member Интервал with set (ms:int) = Моя.Апплікація.TimerInterval <- ms