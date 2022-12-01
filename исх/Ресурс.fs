модуль внутренний Библиотека.Ресурс

пусть частный сборка = System.Reflection.Assembly.GetEntryAssembly()
пусть ПолучитьПоток путь =
    сборка.GetManifestResourceStream(путь)
пусть ЗагрузитьБайты путь =
    использовать поток = ПолучитьПоток(путь)
    пусть длина = int поток.Length
    пусть байты = Array.zeroCreate длина
    поток.Read(байты, 0, длина) |> ignore
    байты

