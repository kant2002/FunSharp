пространство Библиотека

открыть System
открыть System.Runtime.InteropServices
открыть System.Threading
открыть System.Threading.Tasks
открыть Avalonia
открыть Avalonia.Themes.Fluent
открыть Avalonia.Controls.ApplicationLifetimes
открыть Avalonia.Controls
открыть Avalonia.Threading

тип Callback = делегат из unit -> unit

тип AvaloniaApp() =
    наследует Avalonia.Application()
    переопределить self.Initialize() =
        пусть theme = новый FluentTheme(новый Uri("avares://ВеселШарп.Бiблiотека"), Mode = FluentThemeMode.Light)
        self.Styles.Add(theme)

тип внутренний МоеПриложение () =
   пусть изменяемый скрыто : bool = истина
   пусть изменяемый главноеОкно : Window = нуль
   пусть изменяемый главныйХолст : ХолстДляРисования = нуль
   пусть изменяемый кнопкаВверх = Callback(ignore)
   пусть изменяемый кнопкаВниз = Callback(ignore)   
   пусть изменяемый мышьВниз = Callback(ignore)
   пусть изменяемый мышьВверх = Callback(ignore)
   пусть изменяемый mouseMove = Callback(ignore)
   пусть изменяемый timerTick = Callback(ignore)
   пусть изменяемый timerPaused = ложь
   пусть изменяемый последняяКнопка = ""
   пусть изменяемый мышьX = 0.0
   пусть изменяемый мышьY = 0.0
   пусть изменяемый isLeftButtonDown = ложь
   пусть изменяемый isRightButtonDown = ложь
   пусть runOnUiThread (action: Func<'a>) =
    пусть изменяемый result : 'a = нуль
    async {
        пусть! x = Dispatcher.UIThread.InvokeAsync(action) |> Async.AwaitTask
        result <- x
    } |> Async.RunSynchronously
    result
   пусть инициализироватьХолст () =
      главныйХолст <- новый ХолстДляРисования(Background=новый Avalonia.Media.SolidColorBrush(кXwtЦвету Цвета.White))
      главныйХолст.KeyUp.Add(фун args -> 
         последняяКнопка <- args.Key.ToString()
         если кнопкаВниз <> нуль тогда кнопкаВниз.Invoke()
      )
      главныйХолст.KeyDown.Add(фун args ->
         последняяКнопка <- args.Key.ToString()
         если кнопкаВверх <> нуль тогда кнопкаВверх.Invoke()
      )
      главныйХолст.PointerPressed.Add(фун args ->
         пусть point = args.GetCurrentPoint(главныйХолст)
         мышьX <- point.Position.X
         мышьY <- point.Position.Y
         если point.Properties.IsLeftButtonPressed тогда isLeftButtonDown <-истина
         если point.Properties.IsRightButtonPressed тогда isRightButtonDown <-истина
         если мышьВниз <> нуль тогда мышьВниз.Invoke()
      )
      главныйХолст.PointerReleased.Add(фун args ->
         пусть point = args.GetCurrentPoint(главныйХолст)
         мышьX <- point.Position.X
         мышьY <- point.Position.Y
         если point.Properties.IsLeftButtonPressed тогда isLeftButtonDown <- ложь
         если мышьВверх <> нуль тогда мышьВверх.Invoke()
      )
      главныйХолст.PointerMoved.Add(фун args ->
         пусть position = args.GetPosition(главныйХолст)
         мышьX <- position.X
         мышьY <- position.Y
         если mouseMove <> нуль тогда mouseMove.Invoke()
      )
      главноеОкно.Content <- главныйХолст
      главныйХолст.Focusable <- истина
      главныйХолст.Focus()
   пусть показатьОкно () = 
      если скрыто тогда главноеОкно.Show(); скрыто <- ложь
   пусть спрятатьОкно () = 
      если not скрыто тогда главноеОкно.Hide(); скрыто <- истина
   пусть изменяемый timerDisposable : IDisposable = нуль
   пусть установитьИнтервалТаймера (мс:int) =
      если timerDisposable <> нуль тогда timerDisposable.Dispose()
      пусть timer = новый System.Timers.Timer(мс)
      timer.Elapsed.Add(фун (_) -> если not timerPaused тогда timerTick.Invoke())
      timer.Start()
      timerDisposable <- timer
   пусть запуститьПриложение наИниц =      
      пусть app = AppBuilder.Configure<AvaloniaApp>().UsePlatformDetect();
      app.AfterSetup(фун (_) ->
        главноеОкно <- новый Window(Title="App", (* Padding = WidgetSpacing(), *) Width=640.0, Height=480.0)
        инициализироватьХолст ()
        показатьОкно ()         
        пусть наИниц = unbox<unit->unit> наИниц
        наИниц ()
      ) |> ignore
      //Application.Run()
      app.StartWithClassicDesktopLifetime(Environment.GetCommandLineArgs()) |> ignore
      //наИниц ()
   пусть запуститьПотокПриложения () =
      использовать инициализирован = новый AutoResetEvent(ложь)
      пусть поток = Thread(ParameterizedThreadStart запуститьПриложение)
      если RuntimeInformation.IsOSPlatform(OSPlatform.Windows) тогда поток.SetApartmentState(ApartmentState.STA)
      поток.Start(фун () -> инициализирован.Set() |> ignore)
      инициализирован.WaitOne() |> ignore
   сделать запуститьПотокПриложения()
   член app.Окно = главноеОкно
   член app.SetWindowWidth(width) =
      спрятатьОкно()     
      главноеОкно.Width <- width
      показатьОкно()
   член app.SetWindowHeight(height) =
      спрятатьОкно()     
      главноеОкно.Height <- height
      показатьОкно()
   член app.Холст = главныйХолст
   член app.Вызвать действие = Dispatcher.UIThread.Post действие
   член app.ВызватьСРезультатом действие = runOnUiThread действие 
   член app.KeyUp с set callback = кнопкаВверх <- callback
   член app.KeyDown с set callback = кнопкаВниз <- callback
   член app.ПоследняяКнопка с get() = последняяКнопка
   член app.MouseDown с set callback = мышьВниз <- callback
   член app.MouseUp с set callback = мышьВверх <- callback
   член app.MouseMove с set callback = mouseMove <- callback
   член app.МышьX с get() = мышьX
   член app.МышьY с get() = мышьY
   член app.ЛеваяКнопкаНажата с get() = isLeftButtonDown
   член app.ПраваяКнопкаНажата с get() = isRightButtonDown
   член app.Показать() = app.Вызвать (фун () -> показатьОкно())
   член app.Спрятать() = app.Вызвать (фун () -> спрятатьОкно())
   член app.TimerTick с set callback = timerTick <- callback
   член app.ПаузаТаймера() = timerPaused <- истина
   член app.ВозобновитьТаймер() = timerPaused <- ложь
   член app.TimerInterval с set ms = app.Вызвать(фун () -> установитьИнтервалТаймера ms)
   член app.ПоказатьСообщение(текст:string,заголовок) = 
    пусть w = новый Window()
    w.Title <- заголовок
    w.Content <- TextBlock(Text = текст)
    w.Width <- 200
    w.Height <- 100
    using (новый CancellationTokenSource()) (фун source ->
        w.ShowDialog(главноеОкно).ContinueWith(фун t -> source.Cancel(), TaskScheduler.Default) |> ignore
        Dispatcher.UIThread.MainLoop(source.Token);
    )

[<Sealed>]
тип внутренний Мое частный () = 
   статический пусть изменяемый приложение = None
   статический пусть sync = obj ()
   статический пусть isFsi () =
      пусть args = System.Environment.GetCommandLineArgs()
      пусть netFxFsi = args.Length > 0 && System.IO.Path.GetFileName(args.[0]) = "fsi.exe"
      пусть netcoreFsi = args.Length > 1 && System.IO.Path.GetFileName(args.[1]) = "fsi"
      netFxFsi || netcoreFsi
   статический пусть закрытьПриложение () =
      lock (sync) (фун () ->
         (Application.Current.ApplicationLifetime :?> IClassicDesktopStyleApplicationLifetime).TryShutdown(0) |> ignore
         если not (isFsi()) тогда
            Environment.Exit(0)
         приложение <- None       
      )
   статический пусть получитьПриложение () =
      lock (sync) (фун () ->
         сопоставить приложение с
         | Some приложение -> приложение
         | None ->
            пусть новоеПриложение = МоеПриложение()
            приложение <- Some (новоеПриложение)
            новоеПриложение.Окно.Closed.Add(фун e ->
               закрытьПриложение()
            )
            новоеПриложение
      )      
   статический член Приложение = получитьПриложение ()

[<AutoOpen>]
модуль внутренний AppDrawing =
   пусть addDrawing drawing =
      Мое.Приложение.Вызвать (фун () -> Мое.Приложение.Холст.ДобавитьРисунок(drawing))
   пусть addDrawingAt drawing (x,y) =
      Мое.Приложение.Вызвать (фун () -> Мое.Приложение.Холст.AddDrawingAt(drawing,Point(x,y)))

