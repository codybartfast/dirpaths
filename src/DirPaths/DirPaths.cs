namespace Fmbm.DirPaths;

public static partial class DirPaths
{
    static DirPaths()
    {
        AppRoot = new DirPath("AppRoot");
        SetAppRootPath(DirPreset.Base);
    }

    public static DirPath AppRoot { get; }

    public static void SetAppRootPath(params string?[] paths)
    {
        foreach (var path in paths)
        {
            if (path is not null)
            {
                AppRoot.Path = path;
                return;
            }
        }
    }

    public class DirPath
    {
        string? path = null;
        bool pathHasBeenRead = false;
        readonly object lockObj = new Object();

        internal DirPath(string moniker)
        {
            Moniker = moniker;
        }

        internal string Moniker { get; }

        public string Path
        {
            get
            {
                lock (lockObj)
                {
                    pathHasBeenRead = true;
                }
                if (path is null)
                {
                    var errMsg =
                        $"Cannot get path for {Moniker} before it has been set.";
                    throw new InvalidOperationException(errMsg);
                }
                return path;
            }
            set
            {
                lock (lockObj)
                {
                    if (pathHasBeenRead)
                    {
                        var errMsg =
                            $"Cannot set path for {Moniker} to {value} after the existing value, {path}, has already been read.";
                        throw new InvalidOperationException(errMsg);
                    }
                    path = value;
                }
            }
        }
        public string CheckedPath
        {
            get { 
                var path = Path;
                if(!Directory.Exists(path)){
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }

    }

    public static void XXX()
    {

    }
}
