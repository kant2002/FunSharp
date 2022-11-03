module internal Library.Http

open System.IO
open System.Net

let LoadBytes (url:string) =
   let request = HttpWebRequest.Create(url)
   let response = request.GetResponse()  
   use responseStream = response.GetResponseStream()     
   use memoryStream = new MemoryStream()
   responseStream.CopyTo(memoryStream)
   memoryStream.GetBuffer()

open Avalonia.Media.Imaging
open Avalonia.Media

let LoadImageAsync (url:string) = async {
   let request = HttpWebRequest.Create(url)
   use! response = request.AsyncGetResponse()      
   use stream = response.GetResponseStream()
   return new Bitmap(stream) :> IImage
   }

