namespace Htax

module PathResolver =
  let inline resolve (path: string) =
    System.IO.Path.Combine(__SOURCE_DIRECTORY__, path)

