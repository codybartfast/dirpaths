using Fmbm.Dir;

Console.WriteLine("Hello, World!");

// DirPaths.SetAppRootPath(
//     RootPresets.EnvironmentVariable(),
//     RootPresets.Base.NameIs("bin").Parent(),
//     RootPresets.Base.SearchUp("bin").Sibling("AppRoot").ExistingDebug()
// );
// Console.WriteLine(DirPaths.AppRoot.Path);

Console.WriteLine(StringComparer.InvariantCultureIgnoreCase.Compare(null, null));