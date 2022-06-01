using Fmbm.Dir;

Console.WriteLine("Hello, World!");

// DirPaths.SetAppRootPath(
//     RootPresets.EnvironmentVariable(),
//     RootPresets.Base.NameIs("bin").Parent(),
//     RootPresets.Base.SearchUp("bin").Sibling("AppRoot").ExistingDebug()
// );
// Console.WriteLine(DirPaths.AppRoot.Path);

var child1 = DirPaths.GetDir("CHILD");
var child2 = DirPaths.GetDir("child");
var child3 = DirPaths.GetDir(" Childs ");