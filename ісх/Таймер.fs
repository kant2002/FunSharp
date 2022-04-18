namespace Бiблiотека

[<Sealed>]
type Таймер private () =
   static member Пауза() = Мое.Приложение.ПаузаТаймера()
   static member Возобновить() = Мое.Приложение.ВозобновитьТаймер()
   static member Тик with set (callback:Callback) = Мое.Приложение.TimerTick <- callback
   static member Интервал with set (ms:int) = Мое.Приложение.TimerInterval <- ms