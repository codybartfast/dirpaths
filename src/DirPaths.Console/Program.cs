using Fmbm.Dir;

DirPaths.ClearAppRoot();
DirPaths.SetAppRoot(
    RootPresets.Base.SearchUp("src").Sibling("AppRoot")
);
Console.WriteLine(DirPaths.EtcDir.Path);
