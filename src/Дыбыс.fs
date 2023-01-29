module Library.Дыбыс

open System.Media

type private IMarker = interface end

let private тарту есім =
    let қүрастыру = System.Reflection.Assembly.GetAssembly(typeof<IMarker>)
    let ағын = қүрастыру.GetManifestResourceStream(есім+".wav")
    let ойнатқыш = new SoundPlayer(ағын)
    ойнатқыш.Play()   

let ТартуҚоңырауды () =
    тарту "BellRing"

let ТартуЦимбалдар () =
    тарту "Chime"

let ТартуБасыңыз () =
    тарту "Click"