module Бібліотека.Звук

open System.Media

type private ИМаркер = interface end

let private грати імя =
    let сборка = System.Reflection.Assembly.GetAssembly(typeof<ИМаркер>)
    let струм = сборка.GetManifestResourceStream(імя+".wav")
    let програвач = new SoundPlayer(струм)
    програвач.Play()   

let ГратиДзвінок () =
    грати "Дзвінок"

let ГратиБійГодинника () =
    грати "БійГодинника"

let ГратиКлацання () =
    грати "Клацання"