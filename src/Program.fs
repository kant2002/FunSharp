namespace Библиотека

open System
open System.Threading

[<Sealed>]
type Программа private () =
   static member Задержка(мс:int) = Thread.Sleep(мс)
   static member Закончить() = 
      Xwt.Application.Exit()
      Environment.Exit(0)     