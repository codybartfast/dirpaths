namespace Fmbm.DirPaths;

public static class DirPreset
{
    public static string Base { get; } = AppContext.BaseDirectory;

    public static string Current => Directory.GetCurrentDirectory();

}