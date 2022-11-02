namespace Бібліотека

open System
open System.Threading
open Avalonia
open Avalonia.Controls.ApplicationLifetimes

[<Sealed>]
type Програма private () =
   static member Затримка(мс:int) = Thread.Sleep(мс)
   static member Закончить() = 
      Моя.Апплікація.Викликати(fun () -> (Application.Current.ApplicationLifetime :?> IClassicDesktopStyleApplicationLifetime).Shutdown(0))
      Environment.Exit(0)