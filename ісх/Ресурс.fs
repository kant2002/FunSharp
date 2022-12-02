модуль внутрішній Бібліотека.Ресурс

нехай приватний збірка = System.Reflection.Assembly.GetEntryAssembly()
нехай ОтриматиСтрум шлях =
    збірка.GetManifestResourceStream(шлях)
нехай ЗавантажитиБайти шлях =
    вживати струм = ОтриматиСтрум(шлях)
    нехай довжина = int струм.Length
    нехай байти = Array.zeroCreate довжина
    струм.Read(байти, 0, довжина) |> ignore
    байти

