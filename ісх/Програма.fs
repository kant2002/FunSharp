namespace Бiблiотека

open System
open System.Threading

[<Sealed>]
type Програма private () =
   static member Затримка(мс:int) = Thread.Sleep(мс)
   static member Закончить() = 
      Xwt.Application.Exit()
      Environment.Exit(0)     