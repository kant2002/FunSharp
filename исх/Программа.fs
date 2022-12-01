пространство Библиотека

открыть System
открыть System.Threading
открыть Avalonia
открыть Avalonia.Controls.ApplicationLifetimes

[<Sealed>]
тип Программа частный () =
   статический член Задержка(мс:int) = Thread.Sleep(мс)
   статический член Закончить() = 
      Мое.Приложение.Вызвать(фун () -> (Application.Current.ApplicationLifetime :?> IClassicDesktopStyleApplicationLifetime).Shutdown(0))
      Environment.Exit(0)     