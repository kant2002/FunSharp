namespace Бiблiотека

module Текст =
   let Добавить(a:string,b:obj) = a + b.ToString()
   let ПолучитьДлину (текст:string) = текст.Length
   let ПолучитьПодТекст (текст:string,стартовыйИндекс,длина) = текст.Substring(стартовыйИндекс,длина)

