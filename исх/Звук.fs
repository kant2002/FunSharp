﻿модуль Библиотека.Звук

открыть System.Media

тип частный ИМаркер = интерфейс конец

пусть частный играть name =
    пусть сборка = System.Reflection.Assembly.GetAssembly(typeof<ИМаркер>)
    пусть поток = сборка.GetManifestResourceStream(name+".wav")
    пусть проигрыватель = новый SoundPlayer(поток)
    проигрыватель.Play()   

пусть ИгратьЗвонок () =
    играть "Звонок"

пусть ИгратьПерезвон () =
    играть "Перезвон"

пусть ИгратьЩелчек () =
    играть "Щелчек"