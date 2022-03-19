﻿module internal Библиотека.Http

open System.IO
open System.Net

let ЗагрузитьБайты (url:string) =
   let запрос = HttpWebRequest.Create(url)
   let ответ = запрос.GetResponse()  
   use потокОтвета = ответ.GetResponseStream()     
   use потокПамяти = new MemoryStream()
   потокОтвета.CopyTo(потокПамяти)
   потокПамяти.GetBuffer()

open Xwt.Drawing

let ЗагрузитьИзображениеАсинх (url:string) = async {
   let запрос = HttpWebRequest.Create(url)
   use! ответ = запрос.AsyncGetResponse()      
   use поток = ответ.GetResponseStream()
   return Image.FromStream(поток)
   }

