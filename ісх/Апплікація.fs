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

тип Callback = delegate of unit -> unit

тип АппАвалонії() =
    inherit Avalonia.Application()
    override сам.Initialize() =
        нехай тема = new FluentTheme(new Uri("avares://ВеселШарп.Бібліотека"), Mode = FluentThemeMode.Light)
        сам.Styles.Add(тема)


тип internal МояАпплікація () =
   нехай змінливий приховане : bool = true
   нехай змінливий головнеВікно : Window = null
   нехай змінливий головнеПолотно : ПолотноДляМалювання = null
   нехай змінливий клавішаВгору = Callback(ignore)
   нехай змінливий клавішаВниз = Callback(ignore)   
   нехай змінливий мишиВниз = Callback(ignore)
   нехай змінливий мишаВгору = Callback(ignore)
   нехай змінливий мишаРухається = Callback(ignore)
   нехай змінливий цокТаймера = Callback(ignore)
   нехай змінливий таймерПризупинено = false
   нехай змінливий останняКлавіша = ""
   нехай змінливий мишаX = 0.0
   нехай змінливий мишаY = 0.0
   нехай змінливий ліваКлавішаВниз = false
   нехай змінливий праваКлавішаВниз = false
   нехай запуститиНаІКПотоці (дія: Func<'a>) =
    нехай змінливий результат : 'a = null
    async {
        let! x = Dispatcher.UIThread.InvokeAsync(дія) |> Async.AwaitTask
        результат <- x
    } |> Async.RunSynchronously
    результат
   нехай ініціюватиПолотно () =
      головнеПолотно <- new ПолотноДляМалювання(Background=new Avalonia.Media.SolidColorBrush(доКольораАвалонії Кольори.White))
      головнеПолотно.KeyUp.Add(fun арги -> 
         останняКлавіша <- арги.Key.ToString()
         if клавішаВниз <> null then клавішаВниз.Invoke()
      )
      головнеПолотно.KeyDown.Add(fun арги ->
         останняКлавіша <- арги.Key.ToString()
         if клавішаВгору <> null then клавішаВгору.Invoke()
      )
      головнеПолотно.PointerPressed.Add(fun арги ->
         нехай точка = арги.GetCurrentPoint(головнеПолотно)
         мишаX <- точка.Position.X
         мишаY <- точка.Position.Y
         if точка.Properties.IsLeftButtonPressed then ліваКлавішаВниз <-true
         if точка.Properties.IsRightButtonPressed then праваКлавішаВниз <-true
         if мишиВниз <> null then мишиВниз.Invoke()
      )
      головнеПолотно.PointerReleased.Add(fun args ->
         нехай точка = args.GetCurrentPoint(головнеПолотно)
         мишаX <- точка.Position.X
         мишаY <- точка.Position.Y
         if точка.Properties.IsLeftButtonPressed then ліваКлавішаВниз <- false
         if мишаВгору <> null then мишаВгору.Invoke()
      )
      головнеПолотно.PointerMoved.Add(fun args ->
         нехай позиція = args.GetPosition(головнеПолотно)
         мишаX <- позиція.X
         мишаY <- позиція.Y
         if мишаРухається <> null then мишаРухається.Invoke()
      )
      головнеВікно.Content <- головнеПолотно
      головнеПолотно.Focusable <- true
      головнеПолотно.Focus()
   нехай показатиВікно () = 
      if приховане then головнеВікно.Show(); приховане <- false
   нехай сховатиВікно () = 
      if not приховане then головнеВікно.Hide(); приховане <- true
   нехай змінливий timerDisposable : IDisposable = null
   нехай установитьИнтервалТаймера (мс:int) =
      if timerDisposable <> null then timerDisposable.Dispose()
      нехай таймер = new System.Timers.Timer(мс)
      таймер.Elapsed.Add(fun (_) -> if not таймерПризупинено then цокТаймера.Invoke())
      таймер.Start()
      timerDisposable <- таймер
   
   нехай запуститиДодаток наІніц =      
      нехай апп = AppBuilder.Configure<АппАвалонії>().UsePlatformDetect();
      апп.AfterSetup(fun (_) ->
        головнеВікно <- new Window(Title="Додаток", (* Padding = WidgetSpacing(), *) Width=640.0, Height=480.0)
        ініціюватиПолотно ()
        показатиВікно ()         
        нехай наИниц = unbox<unit->unit> наІніц
        наИниц ()
      ) |> ignore
      апп.StartWithClassicDesktopLifetime(Environment.GetCommandLineArgs()) |> ignore
   нехай запуститиПотікДодатка () =
      use ініційован = new AutoResetEvent(false)
      нехай потік = Thread(ParameterizedThreadStart запуститиДодаток)
      if RuntimeInformation.IsOSPlatform(OSPlatform.Windows) then потік.SetApartmentState(ApartmentState.STA)
      потік.Start(fun () -> ініційован.Set() |> ignore)
      ініційован.WaitOne() |> ignore
   зробити запуститиПотікДодатка()
   member апп.Вікно = головнеВікно
   member апп.ВстановитиШиринуВікна(ширина) =
      сховатиВікно()     
      головнеВікно.Width <- ширина
      показатиВікно()
   member апп.ВстановитиВисотуВікна(висота) =
      сховатиВікно()     
      головнеВікно.Height <- висота
      показатиВікно()
   member апп.Полотно = головнеПолотно
   member апп.Викликати дія = Dispatcher.UIThread.Post дія 
   member апп.ВикликатиЗРезультатом действие = запуститиНаІКПотоці действие 
   member апп.KeyUp із set callback = клавішаВгору <- callback
   member апп.KeyDown із set callback = клавішаВниз <- callback
   member апп.ОстанняКнопка із get() = останняКлавіша
   member апп.MouseDown із set callback = мишиВниз <- callback
   member апп.MouseUp із set callback = мишаВгору <- callback
   member апп.MouseMove із set callback = мишаРухається <- callback
   member апп.МишаX із get() = мишаX
   member апп.МишаY із get() = мишаY
   member апп.ЛіваКнопкаНатиснута із get() = ліваКлавішаВниз
   member апп.ПраваКнопкаНатиснута із get() = праваКлавішаВниз
   member апп.Показати() = апп.Викликати (fun () -> показатиВікно())
   member апп.Сховати() = апп.Викликати (fun () -> сховатиВікно())
   member апп.TimerTick із set callback = цокТаймера <- callback
   member апп.ПаузаТаймера() = таймерПризупинено <- true
   member апп.ВідновитиТаймер() = таймерПризупинено <- false
   member апп.TimerInterval із set ms = апп.Викликати(fun () -> установитьИнтервалТаймера ms)
   member апп.ПоказатьСообщение(текст:string,заголовок) = 
    нехай в = new Window()
    в.Title <- заголовок
    в.Content <- TextBlock(Text = текст)
    в.Width <- 200
    в.Height <- 100
    using (new CancellationTokenSource()) (fun джерело ->
        в.ShowDialog(головнеВікно).ContinueWith(fun _ -> джерело.Cancel(), TaskScheduler.Default) |> ignore;
        Dispatcher.UIThread.MainLoop(джерело.Token);
    )

[<Sealed>]
тип internal Моя private () = 
   static нехай змінливий аплікація = None
   static нехай сінх = obj ()
   static нехай уFsi () =
      нехай args = System.Environment.GetCommandLineArgs()
      нехай netFxFsi = args.Length > 0 && System.IO.Path.GetFileName(args.[0]) = "fsi.exe"
      нехай netcoreFsi = args.Length > 1 && System.IO.Path.GetFileName(args.[1]) = "fsi"
      netFxFsi || netcoreFsi
   static нехай закритиАплікацію () =
      lock (сінх) (fun () ->
         (Application.Current.ApplicationLifetime :?> IClassicDesktopStyleApplicationLifetime).TryShutdown(0) |> ignore
         if not (уFsi()) then
            Environment.Exit(0)
         аплікація <- None       
      )
   static нехай отриматиАплікацію () =
      lock (сінх) (fun () ->
         match аплікація із
         | Some аплікація -> аплікація
         | None ->
            нехай новаАплікація = МояАпплікація()
            аплікація <- Some (новаАплікація)
            новаАплікація.Вікно.Closed.Add(fun e ->
               закритиАплікацію()
            )
            новаАплікація
      )      
   static member Апплікація = отриматиАплікацію ()

[<AutoOpen>]
module internal ДодатиМалюнок =
   нехай додатиМалюнок drawing =
      Моя.Апплікація.Викликати (fun () -> Моя.Апплікація.Полотно.ДодатиМалюнок(drawing))
   нехай додатиМалюнокУ drawing (x,y) =
      Моя.Апплікація.Викликати (fun () -> Моя.Апплікація.Полотно.ДодатиМалюнокУ(drawing,Point(x,y)))

