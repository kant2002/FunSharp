namespace Бібліотека

module Текст =
   let Додати(а:string,б:obj) = а + б.ToString()
   let ОтриматиДовжину (текст:string) = текст.Length
   let ОтриматиЧастинуТекста (текст:string,стартовийІндекс,довжина) = текст.Substring(стартовийІндекс,довжина)

