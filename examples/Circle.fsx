#r "nuget: Avalonia.Desktop, 11.0.0-preview3"
#r "nuget: Avalonia.Themes.Fluent, 11.0.0-preview3"
#r "../src/bin/Debug/net7.0/FunSharp.Library.dll"

open Library

GraphicsWindow.ФонныңТүсі <- Түстер.Қара
for i = 1 to 1200 do
   GraphicsWindow.ҚылқаламТүсі <- GraphicsWindow.АлуКездейсоқТүсі()
   GraphicsWindow.ТолтыруЭллипс(Math.АлуКездейсоқСаны(800), Math.АлуКездейсоқСаны(600), 30, 30)
