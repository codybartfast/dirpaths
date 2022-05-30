using Fmbm.Dir;

Console.WriteLine("Hello, World!");

DirPaths.SetAppRootPath(
    RootPresets.EnvironmentVariable(),
    RootPresets.Base.NameIs("bin").Parent(),
    RootPresets.Base.SearchUp("bin").Sibling("AppRoot").ExistingDebug()
);
Console.WriteLine(DirPaths.AppRoot.Path);
var logDir = DirPaths.GetChildDir("Log");
// Console.WriteLine(logDir.Path);
logDir.Path = "serial-logs";
Console.WriteLine(logDir.Path);
Console.WriteLine(DirPaths.GetChildDir("lOG ").Path);
