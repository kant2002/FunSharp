простір Бібліотека

відкрити System
відкрити System.Threading
відкрити Avalonia
відкрити Avalonia.Controls.ApplicationLifetimes

[<Sealed>]
тип Програма private () =
   static member Затримка(мс:int) = Thread.Sleep(мс)
   static member Закінчити() = 
      Моя.Апплікація.Викликати(fun () -> (Application.Current.ApplicationLifetime :?> IClassicDesktopStyleApplicationLifetime).Shutdown(0))
      Environment.Exit(0)