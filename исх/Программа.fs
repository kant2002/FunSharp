namespace Библиотека

open System
open System.Threading
open Avalonia
open Avalonia.Controls.ApplicationLifetimes

[<Sealed>]
type Программа private () =
   static member Задержка(мс:int) = Thread.Sleep(мс)
   static member Закончить() = 
      Мое.Приложение.Вызвать(fun () -> (Application.Current.ApplicationLifetime :?> IClassicDesktopStyleApplicationLifetime).Shutdown(0))
      Environment.Exit(0)     