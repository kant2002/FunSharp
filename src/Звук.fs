module Библиотека.Звук

open System.Media

type private ИМаркер = interface end

let private играть name =
    let сборка = System.Reflection.Assembly.GetAssembly(typeof<ИМаркер>)
    let поток = сборка.GetManifestResourceStream(name+".wav")
    let музыкант = new SoundPlayer(поток)
    музыкант.Play()   

let ИгратьЗвонок () =
    играть "BellRing"

let ИгратьПерезвон () =
    играть "Chime"

let ИгратьЩелчек () =
    играть "Click"