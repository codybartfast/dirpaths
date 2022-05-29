namespace Bmfm;

public static partial class DirPaths
{
    static DirPaths()
    {
        AppRoot = new DirPath("AppRoot");
    }

    public static DirPath AppRoot { get; }

    public static void SetAppRootPath(params Func<string?>[] finders){
        foreach(var find in finders){
            var path = find();
            if(path is not null){
                AppRoot.Path = path;
                return;
            }
        }
    }

    public static void XXX()
    {
        var p = new DirPath("Banana", "Tom");
        // p.Path = "Fred";
        System.Console.WriteLine(p.Path);
        System.Console.WriteLine(Directory.GetCurrentDirectory());
        System.Console.WriteLine(AppContext.BaseDirectory);
    }

}
