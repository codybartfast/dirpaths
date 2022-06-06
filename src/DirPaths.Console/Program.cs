using Fmbm.Dir;

DirPaths.ClearAppRoot();
DirPaths.SetAppRoot(
    RootPresets.EnvironmentVariable("FRUIT_APPROOT"),
    RootPresets.Base.NameIs("Cherry").Parent(),
    RootPresets.Base.SearchUp("Apple").Sibling("Basket"),
    RootPresets.Current
);
Console.WriteLine(DirPaths.EtcDir.Path);

