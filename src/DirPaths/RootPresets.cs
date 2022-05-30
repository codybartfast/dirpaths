namespace Fmbm.Dir;

public static class RootPresets
{
    public const string DefaultEnvironmentVariableName = "FMBM_APPROOT";

    public static string Base { get; } = AppContext.BaseDirectory;

    public static string Current => Directory.GetCurrentDirectory();

    public static string? EnvironmentVariable(string name = DefaultEnvironmentVariableName)
    {
        return Environment.GetEnvironmentVariable(name);
    }

    public static string? BinParent()
    {
        return RootPresets.Base.NameIs("bin").Parent();
    }

    public static string? AnyBinAppRootSibling()
    {
        return RootPresets.Base.SearchUp("bin").Sibling("AppRoot");
    }

    public static string? Fmbm(string environmentVariableName = DefaultEnvironmentVariableName)
    {
        return new string?[]{
            RootPresets.EnvironmentVariable(environmentVariableName),
            RootPresets.BinParent(),
            RootPresets.AnyBinAppRootSibling().ExistingDebug(),
            RootPresets.Current
        }.FirstNonNull();
    }
}
