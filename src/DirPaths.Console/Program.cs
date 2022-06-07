using Fmbm.Paths;

DirPaths.ClearAppRoot();
DirPaths.SetAppRoot(
    RootPresets.Base.SearchUp("src").Sibling("AppRoot")
);
Console.WriteLine(DirPaths.EtcDir.Path);
