namespace Бiблiотека

open Xwt
open System
open System.Runtime.InteropServices
open System.Threading

type Callback = delegate of unit -> unit

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
   let инициализироватьХолст () =
      главныйХолст <- new ХолстДляРисования(BackgroundColor=кXwtЦвету Кольори.White)
      главныйХолст.KeyPressed.Add(fun args -> 
         последняяКнопка <- args.Key.ToString()
         if кнопкаВниз <> null then кнопкаВниз.Invoke()
      )
      главныйХолст.KeyReleased.Add(fun args ->
         последняяКнопка <- args.Key.ToString()
         if кнопкаВверх <> null then кнопкаВверх.Invoke()
      )
      главныйХолст.ButtonPressed.Add(fun args ->
         мышьX <- args.X
         мышьY <- args.Y
         if args.Button = PointerButton.Left then isLeftButtonDown <-true
         if args.Button = PointerButton.Right then isRightButtonDown <-true
         if мышьВниз <> null then мышьВниз.Invoke()
      )
      главныйХолст.ButtonReleased.Add(fun args ->
         мышьX <- args.X
         мышьY <- args.Y
         if args.Button = PointerButton.Left then isLeftButtonDown <- false
         if мышьВверх <> null then мышьВверх.Invoke()
      )
      главныйХолст.MouseMoved.Add(fun args ->
         мышьX <- args.X
         мышьY <- args.Y
         if mouseMove <> null then mouseMove.Invoke()
      )
      главноеОкно.Content <- главныйХолст
      главныйХолст.CanGetFocus <- true
      главныйХолст.SetFocus()
   let показатьОкно () = 
      if скрыто then главноеОкно.Show(); скрыто <- false
   let спрятатьОкно () = 
      if not скрыто then главноеОкно.Hide(); скрыто <- true
   let mutable timerDisposable : IDisposable = null
   let установитьИнтервалТаймера (мс:int) =
      if timerDisposable <> null then timerDisposable.Dispose()
      timerDisposable <-
         Application.TimeoutInvoke(мс, fun () -> 
            if not timerPaused then timerTick.Invoke()
            true
         )
   let запуститьПриложение наИниц =      
      Application.Initialize (ToolkitType.Gtk3)      
      главноеОкно <- new Window(Title="App", Padding = WidgetSpacing(), Width=640.0, Height=480.0)
      инициализироватьХолст ()
      показатьОкно ()         
      let наИниц = unbox<unit->unit> наИниц
      наИниц ()
      Application.Run()
      //наИниц ()
   let запуститьПотокПриложения () =
      use инициализирован = new AutoResetEvent(false)
      let струм = Thread(ParameterizedThreadStart запуститьПриложение)
      if RuntimeInformation.IsOSPlatform(OSPlatform.Windows) then струм.SetApartmentState(ApartmentState.STA)
      струм.Start(fun () -> инициализирован.Set() |> ignore)
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
   member app.Полотно = главныйХолст
   member app.Вызвать действие = Application.Invoke действие   
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
   member app.Показать() = app.Вызвать (fun () -> показатьОкно())
   member app.Сховати() = app.Вызвать (fun () -> спрятатьОкно())
   member app.TimerTick with set callback = timerTick <- callback
   member app.ПаузаТаймера() = timerPaused <- true
   member app.ВозобновитьТаймер() = timerPaused <- false
   member app.TimerInterval with set ms = app.Вызвать(fun () -> установитьИнтервалТаймера ms)
   member app.ПоказатьСообщение(текст:string,заголовок) = MessageDialog.ShowMessage(главноеОкно,текст,заголовок)

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
         Application.Exit()
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
            новоеПриложение.Окно.CloseRequested.Add(fun e ->
               закрытьПриложение()
            )
            новоеПриложение
      )      
   static member Приложение = получитьПриложение ()

[<AutoOpen>]
module internal AppDrawing =
   let addDrawing drawing =
      Мое.Приложение.Вызвать (fun () -> Мое.Приложение.Полотно.ДодатиМалюнок(drawing))
   let addDrawingAt drawing (x,y) =
      Мое.Приложение.Вызвать (fun () -> Мое.Приложение.Полотно.AddDrawingAt(drawing,Point(x,y)))

