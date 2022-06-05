using Fmbm.Dir;

Console.WriteLine("Hello, World!");

// DirPaths.SetAppRootPath(
//     RootPresets.EnvironmentVariable(),
//     RootPresets.Base.NameIs("bin").Parent(),
//     RootPresets.Base.SearchUp("bin").Sibling("AppRoot").ExistingDebug()
// );
// Console.WriteLine(DirPaths.AppRoot.Path);

// Get path for <AppRoot>/etc but don't create it if it doesn't exist
string etcDir = DirPaths.EtcDir.Path;

// Get path for <AppRoot>/temp and create it if needed.
string tempDir = DirPaths.TempDir.CheckedPath;

// Get path for <AppRoot>/cat (and create it if needed)
string catDir = DirPaths.GetDir("cats").CheckedPath;

// Specify custom path for 'data' and 'dog'.
DirPaths.DataDir.Path = @"FMBM_Sample/Data";
DirPaths.GetDir("dog").Path = @"FMBM_Sample/Dogs";

var _ = DirPaths.DataDir.CheckedPath;
_ = DirPaths.GetDir("dog").CheckedPath;

Console.WriteLine(DirPaths.GetDir("dog").Path);