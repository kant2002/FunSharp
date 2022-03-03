namespace Library

[<Sealed>]
type Timer private () =
   static member Pause() = Мое.Приложение.PauseTimer()
   static member Resume() = Мое.Приложение.ResumeTimer()
   static member Tick with set (callback:Callback) = Мое.Приложение.TimerTick <- callback
   static member Interval with set (ms:int) = Мое.Приложение.TimerInterval <- ms