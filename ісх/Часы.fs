namespace Бiблiотека

open System

type Часы private () =
   static member Год = DateTime.Now.Year
   static member Месяц = DateTime.Now.Month
   static member День = DateTime.Now.Day
   static member Час = DateTime.Now.Hour
   static member Минута = DateTime.Now.Minute
   static member Секунда = DateTime.Now.Second
   static member ПрошедшиеМиллисекунды = Environment.TickCount