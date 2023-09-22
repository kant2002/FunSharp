﻿namespace Библиотека

open System
open System.Runtime.InteropServices
open System.Threading
open System.Threading.Tasks
open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Controls
open Avalonia.Threading
open Avalonia.Markup.Xaml

type Callback = delegate of unit -> unit

type ПриложениеАвалонии() as self =
    inherit Avalonia.Application()
    do
        AvaloniaXamlLoader.Load(self)

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
    result <- Dispatcher.UIThread.InvokeAsync(action).Result
    result
   let инициализироватьХолст () =
      главныйХолст <- new ХолстДляРисования(Background=new Avalonia.Media.SolidColorBrush(кXwtЦвету Цвета.Белый))
      главныйХолст.KeyUp.Add(fun args -> 
         последняяКнопка <- args.Key.ToString()
         if кнопкаВверх <> null then кнопкаВверх.Invoke()
      )
      главныйХолст.KeyDown.Add(fun args ->
         последняяКнопка <- args.Key.ToString()
         if кнопкаВниз <> null then кнопкаВниз.Invoke()
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
      let app = AppBuilder.Configure<ПриложениеАвалонии>().UsePlatformDetect();
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
      let поток = Thread(ParameterizedThreadStart запуститьПриложение)
      if RuntimeInformation.IsOSPlatform(OSPlatform.Windows) then поток.SetApartmentState(ApartmentState.STA)
      поток.Start(fun () -> инициализирован.Set() |> ignore)
      инициализирован.WaitOne() |> ignore
   do запуститьПотокПриложения()
   member app.Окно = главноеОкно
   member app.SetWindowWidth(width) =
      спрятатьОкно()     
      главноеОкно.Width <- width
      показатьОкно()
   member app.SetWindowHeight(height) =
      спрятатьОкно()     
      главноеОкно.Height <- height
      показатьОкно()
   member app.Холст = главныйХолст
   member app.Вызвать действие = Dispatcher.UIThread.Post действие
   member app.ВызватьСРезультатом действие = runOnUiThread действие 
   member app.KeyUp with set callback = кнопкаВверх <- callback
   member app.KeyDown with set callback = кнопкаВниз <- callback
   member app.ПоследняяКнопка with get() = последняяКнопка
   member app.MouseDown with set callback = мышьВниз <- callback
   member app.MouseUp with set callback = мышьВверх <- callback
   member app.MouseMove with set callback = mouseMove <- callback
   member app.МышьX with get() = мышьX
   member app.МышьY with get() = мышьY
   member app.ЛеваяКнопкаНажата with get() = isLeftButtonDown
   member app.ПраваяКнопкаНажата with get() = isRightButtonDown
   member app.Показать() = app.Вызвать (fun () -> показатьОкно())
   member app.Спрятать() = app.Вызвать (fun () -> спрятатьОкно())
   member app.TimerTick with set callback = timerTick <- callback
   member app.ПаузаТаймера() = timerPaused <- true
   member app.ВозобновитьТаймер() = timerPaused <- false
   member app.TimerInterval with set ms = app.Вызвать(fun () -> установитьИнтервалТаймера ms)
   member app.ПоказатьСообщение(текст:string,заголовок) = 
    let w = new Window()
    w.Title <- заголовок
    w.Content <- TextBlock(Text = текст)
    w.Width <- 200
    w.Height <- 100
    using (new CancellationTokenSource()) (fun source ->
        w.ShowDialog(главноеОкно).ContinueWith(fun t -> source.Cancel(), TaskScheduler.Default) |> ignore
        Dispatcher.UIThread.MainLoop(source.Token);
    )

[<Sealed>]
type internal Мое private () = 
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
            новоеПриложение.Окно.Closed.Add(fun e ->
               закрытьПриложение()
            )
            новоеПриложение
      )      
   static member Приложение = получитьПриложение ()

[<AutoOpen>]
module internal AppDrawing =
   let addDrawing drawing =
      Мое.Приложение.Вызвать (fun () -> Мое.Приложение.Холст.ДобавитьРисунок(drawing))
   let addDrawingAt drawing (x,y) =
      Мое.Приложение.Вызвать (fun () -> Мое.Приложение.Холст.AddDrawingAt(drawing,Point(x,y)))

