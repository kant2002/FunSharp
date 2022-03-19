module Библиотека.Звук

open System.Media

type private ИМаркер = interface end

let private играть name =
    let сборка = System.Reflection.Assembly.GetAssembly(typeof<ИМаркер>)
    let поток = сборка.GetManifestResourceStream(name+".wav")
    let проигрыватель = new SoundPlayer(поток)
    проигрыватель.Play()   

let ИгратьЗвонок () =
    играть "BellRing"

let ИгратьПерезвон () =
    играть "Chime"

let ИгратьЩелчек () =
    играть "Click"