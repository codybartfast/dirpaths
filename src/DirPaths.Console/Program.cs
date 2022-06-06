using Fmbm.Dir;

DirPaths.SetAppRoot(
    RootPresets.Base.SearchUp("Stuff").Sibling("Basket")
);
Console.WriteLine(DirPaths.EtcDir.Path);

