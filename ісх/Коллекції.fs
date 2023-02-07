простір Бібліотека

тип Словник<'ТКлюч,'ТЗначення> = System.Collections.Generic.Dictionary<'ТКлюч,'ТЗначення>
тип МасивЗмінногоРозміру<'ТЗначення> = ResizeArray<'ТЗначення>

[<AutoOpen>]
module ExtraTopLevelOperators1 =
    
  ///<summary>Builds an asynchronous workflow using computation expression syntax.</summary>
  ///<example id="async-1"><code lang="fsharp">
  /// let sleepExample() =
  ///     async {
  ///         printfn "sleeping"
  ///         do! Async.Sleep 10
  ///         printfn "waking up"
  ///         return 6
  ///      }
  ///
  /// sleepExample() |&gt; Async.RunSynchronously
  /// </code></example>
  нехай асинх = async

  ///<summary>Builds a read-only lookup table from a sequence of key/value pairs. The key objects are indexed using generic hashing and equality.</summary>
  ///<example id="dict-1"><code lang="fsharp">
  /// let table = dict [ (1, 100); (2, 200) ]
  ///
  /// table[1]
  /// </code>
  /// Evaluates to <c>100</c>.
  /// </example>
  ///<example id="dict-2"><code lang="fsharp">
  /// let table = dict [ (1, 100); (2, 200) ]
  ///
  /// table[3]
  /// </code>
  /// Throws <c>System.Collections.Generic.KeyNotFoundException</c>.
  /// </example>
  нехай слов = dict
  тип плаваюча = float
  нехай inline плаваюча значення = float значення
  нехай зменьш = decr
  тип цел = int  
  нехай inline цел значення = int значення
  тип строка = string
  нехай inline строка значення = string значення
  тип об' = System.Object
  нехай inline ігнорувати значення = ignore значення
  нехай inline не значення = not значення

  тип Асинх = 
      static member ДочекатисяЗадачу(задача) = Async.AwaitTask задача
      static member Запустити(обчислення: Async<unit>, ?маркерСкасування: System.Threading.CancellationToken) = 
        let маркерСкасування =
            defaultArg маркерСкасування Async.DefaultCancellationToken
        Async.Start( обчислення, маркерСкасування)


///<summary>Contains operations for working with values of type <see cref="T:Microsoft.FSharp.Collections.seq`1" />.</summary>
[<RequireQualifiedAccess; CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Посл =
    нехай уникальні = Seq.distinct
    ;
///<summary>Contains operations for working with values of type <see cref="T:Microsoft.FSharp.Collections.list`1" />.</summary>
///<namespacedoc><summary>Operations for collections such as lists, arrays, sets, maps and sequences. See also 
///    <a href="https://docs.microsoft.com/dotnet/fsharp/language-reference/fsharp-collection-types">F# Collection Types</a> in the F# Language Guide.
/// </summary></namespacedoc>
[<RequireQualifiedAccess; CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Список =
    нехай уникальні = List.distinct
    нехай ізПосл = List.ofSeq
    нехай обр = List.rev
