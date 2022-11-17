простір Бібліотека

відкрити System
відкрити System.Threading
відкрити Avalonia
відкрити Avalonia.Controls.ApplicationLifetimes

[<Sealed>]
тип Програма приватний () =
   статичний член Затримка(мс:int) = Thread.Sleep(мс)
   статичний член Закінчити() = 
      Моя.Апплікація.Викликати(фун () -> (Application.Current.ApplicationLifetime :?> IClassicDesktopStyleApplicationLifetime).Shutdown(0))
      Environment.Exit(0)