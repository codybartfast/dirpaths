namespace Fmbm.DirPaths;

public static class DirPreset
{
    public static string? AnyBinAppRootSibling()
    {
        return DirPreset.Base.SearchUp("bin").Sibling("AppRoot");
    }

    public static string Base { get; } = AppContext.BaseDirectory;

    public static string? BinParent()
    {
        return DirPreset.Base.NameIs("bin").Parent();
    }

    public static string Current => Directory.GetCurrentDirectory();

    public static string? EnvironmentVariable(string name = "FMBM_APPROOT")
    {
        return Environment.GetEnvironmentVariable(name);
    }

    public static string? FMBM(string environmentVariableName = "FMBM_APPROOT")
    {
        return new string?[]{
            DirPreset.EnvironmentVariable(environmentVariableName),
            DirPreset.BinParent(),
            DirPreset.AnyBinAppRootSibling().ExistingDebug(),
            DirPreset.Current
        }.FirstNonNull();
    }
}