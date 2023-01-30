namespace Кітапхана

open System
open System.Threading
open Avalonia
open Avalonia.Controls.ApplicationLifetimes

[<Sealed>]
type Program private () =
   static member Delay(ms:int) = Thread.Sleep(ms)
   static member End() = 
      My.App.Invoke(fun () -> (Application.Current.ApplicationLifetime :?> IClassicDesktopStyleApplicationLifetime).Shutdown(0))
      Environment.Exit(0)     