module Бiблiотека.Звук

open System.Media

type private ИМаркер = interface end

let private грати імя =
    let сборка = System.Reflection.Assembly.GetAssembly(typeof<ИМаркер>)
    let поток = сборка.GetManifestResourceStream(імя+".wav")
    let програвач = new SoundPlayer(поток)
    програвач.Play()   

let ИгратьЗвонок () =
    грати "Звонок"

let ИгратьПерезвон () =
    грати "Перезвон"

let ИгратьЩелчек () =
    грати "Щелчек"