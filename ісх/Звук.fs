module Бібліотека.Звук

відкрити System.Media

тип private ИМаркер = interface end

нехай private грати імя =
    нехай сборка = System.Reflection.Assembly.GetAssembly(typeof<ИМаркер>)
    нехай струм = сборка.GetManifestResourceStream(імя+".wav")
    нехай програвач = new SoundPlayer(струм)
    програвач.Play()   

нехай ГратиДзвінок () =
    грати "Дзвінок"

нехай ГратиБійГодинника () =
    грати "БійГодинника"

нехай ГратиКлацання () =
    грати "Клацання"