module internal Бібліотека.Ресурс

let private збірка = System.Reflection.Assembly.GetEntryAssembly()
let ОтриматиСтрум шлях =
    збірка.GetManifestResourceStream(шлях)
let ЗавантажитиБайти шлях =
    use струм = ОтриматиСтрум(шлях)
    let довжина = int струм.Length
    let байти = Array.zeroCreate довжина
    струм.Read(байти, 0, довжина) |> ignore
    байти

