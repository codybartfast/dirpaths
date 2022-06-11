using Fmbm.IO;
using Fmbm.IO.StringExtensions;

DirPaths.ClearAppRoot();
DirPaths.SetAppRoot(
    RootPresets.Base.SearchUp("src").Sibling("AppRoot")
);
Console.WriteLine(DirPaths.EtcDir.Path);
