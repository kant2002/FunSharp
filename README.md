# FunSharp
Fun cross-platform graphics library, based on [Small Basic](http://smallbasic.com/)'s library, made specifically for F# and C#.

## Deployment

FunSharp runs on Raspbian, Linux, Windows and MacOS.

## Dependencies

FunSharp uses [Avalonia](https://github.com/AvaloniaUI/Avalonia)

## Other languages

This project has translations to other languages
- [Ukrainian](https://github.com/kant2002/funsharp)
- [Russian](https://github.com/kant2002/funsharp/tree/%D0%B3%D0%BE%D0%BB%D0%BE%D0%B2%D0%BD%D0%B0)

## Building

* Appveyor: [![Build status](https://ci.appveyor.com/api/projects/status/94dkcwcrkwhj06vj?svg=true)](https://ci.appveyor.com/project/ptrelford/funsharp)
* Travis: [![Build Status](https://travis-ci.org/ptrelford/FunSharp.png?branch=master)](https://travis-ci.org/ptrelford/FunSharp/)

## Example

```F#
open Library

GraphicsWindow.PenColor <- Colors.Purple
Turtle.X <- 150.
Turtle.Y <- 150.
for i in 0..5..200 do
   Turtle.Move(i)
   Turtle.Turn(90)
```
![Alt text](http://trelford.com/FunSharp/Turtle_Example.png "Turtle Example")

## Games

![Alt text](http://trelford.com/FunSharp/1942.png "1942")

![Alt text](http://trelford.com/FunSharp/Asteroids.png "Asteroids")

![Alt text](http://trelford.com/FunSharp/Tetris.png "Tetris")

## Contributing

Contributions are welcome, particularly new examples, bug fixes and filling out the API via the up-for-grabs issues.

## Up-for-grabs

- implement Shapes.Animate(x,y,duration)
- implement Controls.AddMultilineTextBox(left,top)
