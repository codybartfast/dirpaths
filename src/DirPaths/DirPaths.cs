namespace Fmbm.DirPaths;

public static partial class DirPaths
{
    static DirPaths()
    {
        AppRoot = new DirPath("AppRoot");
        SetAppRootPath(DirPreset.Current);
    }

    public static DirPath AppRoot { get; }

    public static bool SetAppRootPath(params string?[] paths)
    {
        var path = paths.FirstNonNull();
        if (path is not null)
        {
            AppRoot.Path = path;
            return true;
        }
        return false;
    }

    public static void ClearAppRootPath(params string?[] paths)
    {
        AppRoot.ClearPath();
    }


    public class DirPath
    {
        string? path = null;
        bool pathHasBeenRead = false;
        readonly object lockObj = new Object();

        internal DirPath(string moniker) { Moniker = moniker; }

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
                SetPath(value);
            }
        }

        void SetPath(string? newPath)
        {
            lock (lockObj)
            {
                if (pathHasBeenRead)
                {
                    var errMsg =
                        $"Cannot set path for {Moniker} to {newPath} after the existing value, {path}, has already been read.";
                    throw new InvalidOperationException(errMsg);
                }
                this.path = newPath;
            }
        }

        internal void ClearPath()
        {
            SetPath(null);
        }

        public string CheckedPath
        {
            get
            {
                var path = Path;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }
    }
}
