namespace Бібліотека

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

type АппАвалонії() =
    inherit Avalonia.Application()
    override сам.Initialize() =
        let тема = new FluentTheme()
        сам.Styles.Add(тема)


type internal МояАпплікація () =
   let mutable приховане : bool = true
   let mutable головнеВікно : Window = null
   let mutable головнеПолотно : ПолотноДляМалювання = null
   let mutable клавішаВгору = Callback(ignore)
   let mutable клавішаВниз = Callback(ignore)   
   let mutable мишиВниз = Callback(ignore)
   let mutable мишаВгору = Callback(ignore)
   let mutable мишаРухається = Callback(ignore)
   let mutable цокТаймера = Callback(ignore)
   let mutable таймерПризупинено = false
   let mutable останняКлавіша = ""
   let mutable мишаX = 0.0
   let mutable мишаY = 0.0
   let mutable ліваКлавішаВниз = false
   let mutable праваКлавішаВниз = false
   let запуститиНаІКПотоці (дія: Func<'a>) =
    let mutable результат : 'a = null
    async {
        let! x = Dispatcher.UIThread.InvokeAsync(дія) |> Async.AwaitTask
        результат <- x
    } |> Async.RunSynchronously
    результат
   let ініціюватиПолотно () =
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
         let точка = арги.GetCurrentPoint(головнеПолотно)
         мишаX <- точка.Position.X
         мишаY <- точка.Position.Y
         if точка.Properties.IsLeftButtonPressed then ліваКлавішаВниз <-true
         if точка.Properties.IsRightButtonPressed then праваКлавішаВниз <-true
         if мишиВниз <> null then мишиВниз.Invoke()
      )
      головнеПолотно.PointerReleased.Add(fun args ->
         let точка = args.GetCurrentPoint(головнеПолотно)
         мишаX <- точка.Position.X
         мишаY <- точка.Position.Y
         if точка.Properties.IsLeftButtonPressed then ліваКлавішаВниз <- false
         if мишаВгору <> null then мишаВгору.Invoke()
      )
      головнеПолотно.PointerMoved.Add(fun args ->
         let позиція = args.GetPosition(головнеПолотно)
         мишаX <- позиція.X
         мишаY <- позиція.Y
         if мишаРухається <> null then мишаРухається.Invoke()
      )
      головнеВікно.Content <- головнеПолотно
      головнеПолотно.Focusable <- true
      головнеПолотно.Focus()
   let показатиВікно () = 
      if приховане then головнеВікно.Show(); приховане <- false
   let сховатиВікно () = 
      if not приховане then головнеВікно.Hide(); приховане <- true
   let mutable timerDisposable : IDisposable = null
   let установитьИнтервалТаймера (мс:int) =
      if timerDisposable <> null then timerDisposable.Dispose()
      let таймер = new System.Timers.Timer(мс)
      таймер.Elapsed.Add(fun (_) -> if not таймерПризупинено then цокТаймера.Invoke())
      таймер.Start()
      timerDisposable <- таймер
   
   let запуститиДодаток наІніц =      
      let апп = AppBuilder.Configure<АппАвалонії>().UsePlatformDetect();
      апп.AfterSetup(fun (_) ->
        головнеВікно <- new Window(Title="Додаток", (* Padding = WidgetSpacing(), *) Width=640.0, Height=480.0)
        ініціюватиПолотно ()
        показатиВікно ()         
        let наИниц = unbox<unit->unit> наІніц
        наИниц ()
      ) |> ignore
      апп.StartWithClassicDesktopLifetime(Environment.GetCommandLineArgs()) |> ignore
   let запуститиПотікДодатка () =
      use ініційован = new AutoResetEvent(false)
      let потік = Thread(ParameterizedThreadStart запуститиДодаток)
      if RuntimeInformation.IsOSPlatform(OSPlatform.Windows) then потік.SetApartmentState(ApartmentState.STA)
      потік.Start(fun () -> ініційован.Set() |> ignore)
      ініційован.WaitOne() |> ignore
   do запуститиПотікДодатка()
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
   member апп.KeyUp with set callback = клавішаВгору <- callback
   member апп.KeyDown with set callback = клавішаВниз <- callback
   member апп.ОстанняКнопка with get() = останняКлавіша
   member апп.MouseDown with set callback = мишиВниз <- callback
   member апп.MouseUp with set callback = мишаВгору <- callback
   member апп.MouseMove with set callback = мишаРухається <- callback
   member апп.МишаX with get() = мишаX
   member апп.МишаY with get() = мишаY
   member апп.ЛіваКнопкаНатиснута with get() = ліваКлавішаВниз
   member апп.ПраваКнопкаНатиснута with get() = праваКлавішаВниз
   member апп.Показати() = апп.Викликати (fun () -> показатиВікно())
   member апп.Сховати() = апп.Викликати (fun () -> сховатиВікно())
   member апп.TimerTick with set callback = цокТаймера <- callback
   member апп.ПаузаТаймера() = таймерПризупинено <- true
   member апп.ВідновитиТаймер() = таймерПризупинено <- false
   member апп.TimerInterval with set ms = апп.Викликати(fun () -> установитьИнтервалТаймера ms)
   member апп.ПоказатьСообщение(текст:string,заголовок) = 
    let в = new Window()
    в.Title <- заголовок
    в.Content <- TextBlock(Text = текст)
    в.Width <- 200
    в.Height <- 100
    using (new CancellationTokenSource()) (fun джерело ->
        в.ShowDialog(головнеВікно).ContinueWith(fun _ -> джерело.Cancel(), TaskScheduler.Default) |> ignore;
        Dispatcher.UIThread.MainLoop(джерело.Token);
    )

[<Sealed>]
type internal Моя private () = 
   static let mutable аплікація = None
   static let сінх = obj ()
   static let уFsi () =
      let args = System.Environment.GetCommandLineArgs()
      let netFxFsi = args.Length > 0 && System.IO.Path.GetFileName(args.[0]) = "fsi.exe"
      let netcoreFsi = args.Length > 1 && System.IO.Path.GetFileName(args.[1]) = "fsi"
      netFxFsi || netcoreFsi
   static let закритиАплікацію () =
      lock (сінх) (fun () ->
         (Application.Current.ApplicationLifetime :?> IClassicDesktopStyleApplicationLifetime).TryShutdown(0) |> ignore
         if not (уFsi()) then
            Environment.Exit(0)
         аплікація <- None       
      )
   static let отриматиАплікацію () =
      lock (сінх) (fun () ->
         match аплікація with
         | Some аплікація -> аплікація
         | None ->
            let новаАплікація = МояАпплікація()
            аплікація <- Some (новаАплікація)
            новаАплікація.Вікно.Closed.Add(fun e ->
               закритиАплікацію()
            )
            новаАплікація
      )      
   static member Апплікація = отриматиАплікацію ()

[<AutoOpen>]
module internal ДодатиМалюнок =
   let додатиМалюнок drawing =
      Моя.Апплікація.Викликати (fun () -> Моя.Апплікація.Полотно.ДодатиМалюнок(drawing))
   let додатиМалюнокУ drawing (x,y) =
      Моя.Апплікація.Викликати (fun () -> Моя.Апплікація.Полотно.ДодатиМалюнокУ(drawing,Point(x,y)))

