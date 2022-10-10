module internal Бiблiотека.Ресурс

let private сборка = System.Reflection.Assembly.GetEntryAssembly()
let ОтриматиСтрум путь =
    сборка.GetManifestResourceStream(путь)
let ЗавантажитиБайти путь =
    use струм = ОтриматиСтрум(путь)
    let длина = int струм.Length
    let байты = Array.zeroCreate длина
    струм.Read(байты, 0, длина) |> ignore
    байты

