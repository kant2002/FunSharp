простір Бібліотека

відкрити System
відкрити System.Runtime.InteropServices
відкрити System.Threading
відкрити System.Threading.Tasks
відкрити Avalonia
відкрити Avalonia.Themes.Fluent
відкрити Avalonia.Controls.ApplicationLifetimes
відкрити Avalonia.Controls
відкрити Avalonia.Threading

тип Callback = делегат з unit -> unit

тип АппАвалонії() =
    успадкує Avalonia.Application()
    перевизначити сам.Initialize() =
        нехай тема = новий FluentTheme(новий Uri("avares://ВеселШарп.Бібліотека"), Mode = FluentThemeMode.Light)
        сам.Styles.Add(тема)


тип внутрішній МояАпплікація () =
   нехай змінливий приховане : bool = істина
   нехай змінливий головнеВікно : Window = нуль
   нехай змінливий головнеПолотно : ПолотноДляМалювання = нуль
   нехай змінливий клавішаВгору = Callback(ignore)
   нехай змінливий клавішаВниз = Callback(ignore)   
   нехай змінливий мишиВниз = Callback(ignore)
   нехай змінливий мишаВгору = Callback(ignore)
   нехай змінливий мишаРухається = Callback(ignore)
   нехай змінливий цокТаймера = Callback(ignore)
   нехай змінливий таймерПризупинено = ложь
   нехай змінливий останняКлавіша = ""
   нехай змінливий мишаX = 0.0
   нехай змінливий мишаY = 0.0
   нехай змінливий ліваКлавішаВниз = ложь
   нехай змінливий праваКлавішаВниз = ложь
   нехай запуститиНаІКПотоці (дія: Func<'a>) =
    нехай змінливий результат : 'a = нуль
    async {
        нехай! x = Dispatcher.UIThread.InvokeAsync(дія) |> Async.AwaitTask
        результат <- x
    } |> Async.RunSynchronously
    результат
   нехай ініціюватиПолотно () =
      головнеПолотно <- новий ПолотноДляМалювання(Background=новий Avalonia.Media.SolidColorBrush(доКольораАвалонії Кольори.White))
      головнеПолотно.KeyUp.Add(фун арги -> 
         останняКлавіша <- арги.Key.ToString()
         якщо клавішаВниз <> нуль тоді клавішаВниз.Invoke()
      )
      головнеПолотно.KeyDown.Add(фун арги ->
         останняКлавіша <- арги.Key.ToString()
         якщо клавішаВгору <> нуль тоді клавішаВгору.Invoke()
      )
      головнеПолотно.PointerPressed.Add(фун арги ->
         нехай точка = арги.GetCurrentPoint(головнеПолотно)
         мишаX <- точка.Position.X
         мишаY <- точка.Position.Y
         якщо точка.Properties.IsLeftButtonPressed тоді ліваКлавішаВниз <-істина
         якщо точка.Properties.IsRightButtonPressed тоді праваКлавішаВниз <-істина
         якщо мишиВниз <> нуль тоді мишиВниз.Invoke()
      )
      головнеПолотно.PointerReleased.Add(фун args ->
         нехай точка = args.GetCurrentPoint(головнеПолотно)
         мишаX <- точка.Position.X
         мишаY <- точка.Position.Y
         якщо точка.Properties.IsLeftButtonPressed тоді ліваКлавішаВниз <- ложь
         якщо мишаВгору <> нуль тоді мишаВгору.Invoke()
      )
      головнеПолотно.PointerMoved.Add(фун args ->
         нехай позиція = args.GetPosition(головнеПолотно)
         мишаX <- позиція.X
         мишаY <- позиція.Y
         якщо мишаРухається <> нуль тоді мишаРухається.Invoke()
      )
      головнеВікно.Content <- головнеПолотно
      головнеПолотно.Focusable <- істина
      головнеПолотно.Focus()
   нехай показатиВікно () = 
      якщо приховане тоді головнеВікно.Show(); приховане <- ложь
   нехай сховатиВікно () = 
      якщо not приховане тоді головнеВікно.Hide(); приховане <- істина
   нехай змінливий timerDisposable : IDisposable = нуль
   нехай установитьИнтервалТаймера (мс:int) =
      якщо timerDisposable <> нуль тоді timerDisposable.Dispose()
      нехай таймер = новий System.Timers.Timer(мс)
      таймер.Elapsed.Add(фун (_) -> якщо not таймерПризупинено тоді цокТаймера.Invoke())
      таймер.Start()
      timerDisposable <- таймер
   
   нехай запуститиДодаток наІніц =      
      нехай апп = AppBuilder.Configure<АппАвалонії>().UsePlatformDetect();
      апп.AfterSetup(фун (_) ->
        головнеВікно <- новий Window(Title="Додаток", (* Padding = WidgetSpacing(), *) Width=640.0, Height=480.0)
        ініціюватиПолотно ()
        показатиВікно ()         
        нехай наИниц = unbox<unit->unit> наІніц
        наИниц ()
      ) |> ignore
      апп.StartWithClassicDesktopLifetime(Environment.GetCommandLineArgs()) |> ignore
   нехай запуститиПотікДодатка () =
      вживати ініційован = новий AutoResetEvent(ложь)
      нехай потік = Thread(ParameterizedThreadStart запуститиДодаток)
      якщо RuntimeInformation.IsOSPlatform(OSPlatform.Windows) тоді потік.SetApartmentState(ApartmentState.STA)
      потік.Start(фун () -> ініційован.Set() |> ignore)
      ініційован.WaitOne() |> ignore
   зробити запуститиПотікДодатка()
   член апп.Вікно = головнеВікно
   член апп.ВстановитиШиринуВікна(ширина) =
      сховатиВікно()     
      головнеВікно.Width <- ширина
      показатиВікно()
   член апп.ВстановитиВисотуВікна(висота) =
      сховатиВікно()     
      головнеВікно.Height <- висота
      показатиВікно()
   член апп.Полотно = головнеПолотно
   член апп.Викликати дія = Dispatcher.UIThread.Post дія 
   член апп.ВикликатиЗРезультатом действие = запуститиНаІКПотоці действие 
   член апп.KeyUp із set callback = клавішаВгору <- callback
   член апп.KeyDown із set callback = клавішаВниз <- callback
   член апп.ОстанняКнопка із get() = останняКлавіша
   член апп.MouseDown із set callback = мишиВниз <- callback
   член апп.MouseUp із set callback = мишаВгору <- callback
   член апп.MouseMove із set callback = мишаРухається <- callback
   член апп.МишаX із get() = мишаX
   член апп.МишаY із get() = мишаY
   член апп.ЛіваКнопкаНатиснута із get() = ліваКлавішаВниз
   член апп.ПраваКнопкаНатиснута із get() = праваКлавішаВниз
   член апп.Показати() = апп.Викликати (фун () -> показатиВікно())
   член апп.Сховати() = апп.Викликати (фун () -> сховатиВікно())
   член апп.TimerTick із set callback = цокТаймера <- callback
   член апп.ПаузаТаймера() = таймерПризупинено <- істина
   член апп.ВідновитиТаймер() = таймерПризупинено <- ложь
   член апп.TimerInterval із set ms = апп.Викликати(фун () -> установитьИнтервалТаймера ms)
   член апп.ПоказатьСообщение(текст:string,заголовок) = 
    нехай в = новий Window()
    в.Title <- заголовок
    в.Content <- TextBlock(Text = текст)
    в.Width <- 200
    в.Height <- 100
    using (новий CancellationTokenSource()) (фун джерело ->
        в.ShowDialog(головнеВікно).ContinueWith(фун _ -> джерело.Cancel(), TaskScheduler.Default) |> ignore;
        Dispatcher.UIThread.MainLoop(джерело.Token);
    )

[<Sealed>]
тип внутрішній Моя приватний () = 
   статичний нехай змінливий аплікація = None
   статичний нехай сінх = obj ()
   статичний нехай уFsi () =
      нехай args = System.Environment.GetCommandLineArgs()
      нехай netFxFsi = args.Length > 0 && System.IO.Path.GetFileName(args.[0]) = "fsi.exe"
      нехай netcoreFsi = args.Length > 1 && System.IO.Path.GetFileName(args.[1]) = "fsi"
      netFxFsi || netcoreFsi
   статичний нехай закритиАплікацію () =
      lock (сінх) (фун () ->
         (Application.Current.ApplicationLifetime :?> IClassicDesktopStyleApplicationLifetime).TryShutdown(0) |> ignore
         якщо not (уFsi()) тоді
            Environment.Exit(0)
         аплікація <- None       
      )
   статичний нехай отриматиАплікацію () =
      lock (сінх) (фун () ->
         відповідає аплікація із
         | Some аплікація -> аплікація
         | None ->
            нехай новаАплікація = МояАпплікація()
            аплікація <- Some (новаАплікація)
            новаАплікація.Вікно.Closed.Add(фун e ->
               закритиАплікацію()
            )
            новаАплікація
      )      
   статичний член Апплікація = отриматиАплікацію ()

[<AutoOpen>]
модуль внутрішній ДодатиМалюнок =
   нехай додатиМалюнок drawing =
      Моя.Апплікація.Викликати (фун () -> Моя.Апплікація.Полотно.ДодатиМалюнок(drawing))
   нехай додатиМалюнокУ drawing (x,y) =
      Моя.Апплікація.Викликати (фун () -> Моя.Апплікація.Полотно.ДодатиМалюнокУ(drawing,Point(x,y)))

