#r "nuget: Xwt"
#r "../src/bin/Debug/net48/FunSharp.Library.dll"

open Library
open System.Threading

ГрафическоеОкно.ФоновыйЦвет <- Colors.Black
for i = 1 to 1200 do
   ГрафическоеОкно.ЦветКисти <- ГрафическоеОкно.ПолучитьСлучайныйЦвет()
   ГрафическоеОкно.ЗаполнитьЭллипс(Math.GetRandomNumber(800), Math.GetRandomNumber(600), 30, 30)
   Thread.Sleep 50
