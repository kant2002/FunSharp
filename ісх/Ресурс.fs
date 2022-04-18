module internal Бiблiотека.Ресурс

let private сборка = System.Reflection.Assembly.GetEntryAssembly()
let ПолучитьПоток путь =
    сборка.GetManifestResourceStream(путь)
let ЗагрузитьБайты путь =
    use поток = ПолучитьПоток(путь)
    let длина = int поток.Length
    let байты = Array.zeroCreate длина
    поток.Read(байты, 0, длина) |> ignore
    байты

