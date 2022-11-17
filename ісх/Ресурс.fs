module internal Бібліотека.Ресурс

нехай private збірка = System.Reflection.Assembly.GetEntryAssembly()
нехай ОтриматиСтрум шлях =
    збірка.GetManifestResourceStream(шлях)
нехай ЗавантажитиБайти шлях =
    use струм = ОтриматиСтрум(шлях)
    нехай довжина = int струм.Length
    нехай байти = Array.zeroCreate довжина
    струм.Read(байти, 0, довжина) |> ignore
    байти

