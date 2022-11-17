простір Бібліотека

відкрити System

тип Годинник private () =
   static member Рік = DateTime.Now.Year
   static member Місяць = DateTime.Now.Month
   static member День = DateTime.Now.Day
   static member Година = DateTime.Now.Hour
   static member Хвилина = DateTime.Now.Minute
   static member Секунда = DateTime.Now.Second
   static member МинуліМілісекунди = Environment.TickCount