using Fmbm.DirPaths;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var b = DirPreset.Base.NameIs("bin").Parent();
var c = DirPreset.Base.SearchUp("bin").Sibling("AppRoot").ExistingDebug();
var d = DirPreset.Current;

DirPaths.SetAppRootPath(b, c, d);

Console.WriteLine(DirPaths.AppRoot.CheckedPath);
