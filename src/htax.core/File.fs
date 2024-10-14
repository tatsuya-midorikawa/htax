namespace Htax

module File =
  let inline addAttributes (attributes: System.IO.FileAttributes) (path: string) =
    let attr = System.IO.File.GetAttributes path
    System.IO.File.SetAttributes(path, attr ||| attributes)
    path

  let inline copy src dst =
    System.IO.File.Copy(src, dst, true)
    dst

  let inline remove (path) = 
    if System.IO.File.Exists path then System.IO.File.Delete path
    path

  let inline exists path =
    System.IO.File.Exists path

  let inline isHtax (path: string) =
    System.IO.Path.GetExtension path = ".htax"

  let inline isHtml (path: string) =
    System.IO.Path.GetExtension path = ".html"
    

module Html =
  type Html = private Html of path: string
  let create (path: string) =
    if File.exists path 
      then
        Html path 
      else 
        use sw = System.IO.File.CreateText path
        Html path

  let path (Html path) = path

module Htax =
  type Htax = private Htax of path: string

  let create (path: string) =
    if File.exists path 
      then
        Htax path 
      else 
        use sw = System.IO.File.CreateText path
        Htax path

  let copyToHiddenHtml (Htax src) =
    System.IO.Path.ChangeExtension(src, ".html")
      |> File.remove
      |> File.copy src
      |> File.addAttributes System.IO.FileAttributes.Hidden
      |> Html.create
