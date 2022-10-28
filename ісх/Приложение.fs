namespace Бiблiотека

open System
open System.Runtime.InteropServices
open System.Threading
open System.Threading.Tasks
open Avalonia
open Avalonia.Themes.Fluent
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Controls
open Avalonia.Threading

type Callback = delegate of unit -> unit

type AvaloniaApp() =
    inherit Avalonia.Application()
    override self.Initialize() =
        let theme = new FluentTheme(new Uri("avares://ВеселШарп.Бiблiотека"), Mode = FluentThemeMode.Light)
        self.Styles.Add(theme)

    override self.OnFrameworkInitializationCompleted() =
        let desktop = self.ApplicationLifetime :?> IClassicDesktopStyleApplicationLifetime
        //desktop.ShutdownMode <- ShutdownMode.OnMainWindowClose
        //desktop.MainWindow <- Моя.главноеОкно
        //desktop.MainWindow <- new Window()
        base.OnFrameworkInitializationCompleted()


type internal МоеПриложение () =
   let mutable скрыто : bool = true
   let mutable главноеОкно : Window = null
   let mutable главныйХолст : ХолстДляРисования = null
   let mutable кнопкаВверх = Callback(ignore)
   let mutable кнопкаВниз = Callback(ignore)   
   let mutable мышьВниз = Callback(ignore)
   let mutable мышьВверх = Callback(ignore)
   let mutable mouseMove = Callback(ignore)
   let mutable timerTick = Callback(ignore)
   let mutable timerPaused = false
   let mutable последняяКнопка = ""
   let mutable мышьX = 0.0
   let mutable мышьY = 0.0
   let mutable isLeftButtonDown = false
   let mutable isRightButtonDown = false
   let runOnUiThread (action: Func<'a>) =
    let mutable result : 'a = null
    async {
        let! x = Dispatcher.UIThread.InvokeAsync(action) |> Async.AwaitTask
        result <- x
    } |> Async.RunSynchronously
    result
   let инициализироватьХолст () =
      главныйХолст <- new ХолстДляРисования(Background=new Avalonia.Media.SolidColorBrush(кXwtЦвету Кольори.White))
      главныйХолст.KeyUp.Add(fun args -> 
         последняяКнопка <- args.Key.ToString()
         if кнопкаВниз <> null then кнопкаВниз.Invoke()
      )
      главныйХолст.KeyDown.Add(fun args ->
         последняяКнопка <- args.Key.ToString()
         if кнопкаВверх <> null then кнопкаВверх.Invoke()
      )
      главныйХолст.PointerPressed.Add(fun args ->
         let point = args.GetCurrentPoint(главныйХолст)
         мышьX <- point.Position.X
         мышьY <- point.Position.Y
         if point.Properties.IsLeftButtonPressed then isLeftButtonDown <-true
         if point.Properties.IsRightButtonPressed then isRightButtonDown <-true
         if мышьВниз <> null then мышьВниз.Invoke()
      )
      главныйХолст.PointerReleased.Add(fun args ->
         let point = args.GetCurrentPoint(главныйХолст)
         мышьX <- point.Position.X
         мышьY <- point.Position.Y
         if point.Properties.IsLeftButtonPressed then isLeftButtonDown <- false
         if мышьВверх <> null then мышьВверх.Invoke()
      )
      главныйХолст.PointerMoved.Add(fun args ->
         let position = args.GetPosition(главныйХолст)
         мышьX <- position.X
         мышьY <- position.Y
         if mouseMove <> null then mouseMove.Invoke()
      )
      главноеОкно.Content <- главныйХолст
      главныйХолст.Focusable <- true
      главныйХолст.Focus()
   let показатьОкно () = 
      if скрыто then главноеОкно.Show(); скрыто <- false
   let спрятатьОкно () = 
      if not скрыто then главноеОкно.Hide(); скрыто <- true
   let mutable timerDisposable : IDisposable = null
   let установитьИнтервалТаймера (мс:int) =
      if timerDisposable <> null then timerDisposable.Dispose()
      let timer = new System.Timers.Timer(мс)
      timer.Elapsed.Add(fun (_) -> if not timerPaused then timerTick.Invoke())
      timer.Start()
      timerDisposable <- timer
   
   let запуститьПриложение наИниц =      
      let app = AppBuilder.Configure<AvaloniaApp>().UsePlatformDetect();
      app.AfterSetup(fun (_) ->
        главноеОкно <- new Window(Title="App", (* Padding = WidgetSpacing(), *) Width=640.0, Height=480.0)
        инициализироватьХолст ()
        показатьОкно ()         
        let наИниц = unbox<unit->unit> наИниц
        наИниц ()
      ) |> ignore
      //Application.Run()
      app.StartWithClassicDesktopLifetime(Environment.GetCommandLineArgs()) |> ignore
      //наИниц ()
   let запуститьПотокПриложения () =
      use инициализирован = new AutoResetEvent(false)
      let струм = Thread(ParameterizedThreadStart запуститьПриложение)
      if RuntimeInformation.IsOSPlatform(OSPlatform.Windows) then струм.SetApartmentState(ApartmentState.STA)
      струм.Start(fun () -> инициализирован.Set() |> ignore)
      инициализирован.WaitOne() |> ignore
   do запуститьПотокПриложения()
   member app.Вікно = главноеОкно
   member app.SetWindowWidth(width) =
      спрятатьОкно()     
      главноеОкно.Width <- width
      показатьОкно()
   member app.SetWindowHeight(height) =
      спрятатьОкно()     
      главноеОкно.Height <- height
      показатьОкно()
   member app.Полотно = главныйХолст
   member app.Викликати действие = Dispatcher.UIThread.Post действие 
   member app.ВикликатиЗРезультатом действие = runOnUiThread действие 
   member app.KeyUp with set callback = кнопкаВверх <- callback
   member app.KeyDown with set callback = кнопкаВниз <- callback
   member app.ОстанняКнопка with get() = последняяКнопка
   member app.MouseDown with set callback = мышьВниз <- callback
   member app.MouseUp with set callback = мышьВверх <- callback
   member app.MouseMove with set callback = mouseMove <- callback
   member app.МишаX with get() = мышьX
   member app.МишаY with get() = мышьY
   member app.ЛіваКнопкаНатиснута with get() = isLeftButtonDown
   member app.ПраваКнопкаНатиснута with get() = isRightButtonDown
   member app.Показать() = app.Викликати (fun () -> показатьОкно())
   member app.Сховати() = app.Викликати (fun () -> спрятатьОкно())
   member app.TimerTick with set callback = timerTick <- callback
   member app.ПаузаТаймера() = timerPaused <- true
   member app.ВозобновитьТаймер() = timerPaused <- false
   member app.TimerInterval with set ms = app.Викликати(fun () -> установитьИнтервалТаймера ms)
   member app.ПоказатьСообщение(текст:string,заголовок) = 
    let w = new Window()
    w.Title <- заголовок
    w.Content <- TextBlock(Text = текст)
    w.Width <- 200
    w.Height <- 100
    using (new CancellationTokenSource()) (fun source ->
        w.ShowDialog(главноеОкно).ContinueWith(fun t -> source.Cancel(), TaskScheduler.Default) |> ignore;
        Dispatcher.UIThread.MainLoop(source.Token);
    )

[<Sealed>]
type internal Моя private () = 
   static let mutable приложение = None
   static let sync = obj ()
   static let isFsi () =
      let args = System.Environment.GetCommandLineArgs()
      let netFxFsi = args.Length > 0 && System.IO.Path.GetFileName(args.[0]) = "fsi.exe"
      let netcoreFsi = args.Length > 1 && System.IO.Path.GetFileName(args.[1]) = "fsi"
      netFxFsi || netcoreFsi
   static let закрытьПриложение () =
      lock (sync) (fun () ->
         (Application.Current.ApplicationLifetime :?> IClassicDesktopStyleApplicationLifetime).TryShutdown(0) |> ignore
         if not (isFsi()) then
            Environment.Exit(0)
         приложение <- None       
      )
   static let получитьПриложение () =
      lock (sync) (fun () ->
         match приложение with
         | Some приложение -> приложение
         | None ->
            let новоеПриложение = МоеПриложение()
            приложение <- Some (новоеПриложение)
            новоеПриложение.Вікно.Closed.Add(fun e ->
               закрытьПриложение()
            )
            новоеПриложение
      )      
   static member Апплікація = получитьПриложение ()

[<AutoOpen>]
module internal AppDrawing =
   let додатиМалюнок drawing =
      Моя.Апплікація.Викликати (fun () -> Моя.Апплікація.Полотно.ДодатиМалюнок(drawing))
   let додатиМалюнокУ drawing (x,y) =
      Моя.Апплікація.Викликати (fun () -> Моя.Апплікація.Полотно.AddDrawingAt(drawing,Point(x,y)))

