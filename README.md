# htax.exe

## Prerequisites

- [.NET 8.0](https://dotnet.microsoft.com/ja-jp/download/dotnet/8.0) or [later](https://dotnet.microsoft.com/ja-jp/download/dotnet)

## Associate htax.exe with .htax file

note: Administrative privileges are required.

```cmd
assoc .htax=htaxfile
ftype htaxfile="C:\path\to\htax.exe" "%1"
```
