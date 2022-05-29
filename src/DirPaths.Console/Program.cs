using Fmbm.DirPaths;

Console.WriteLine("Hello, World!");

DirPaths.SetAppRootPath(
    DirPreset.EnvironmentVariable(),
    DirPreset.Base.NameIs("bin").Parent(),
    DirPreset.Base.SearchUp("bin").Sibling("AppRoot").ExistingDebug()
);
Console.WriteLine(DirPaths.AppRoot.Path);
