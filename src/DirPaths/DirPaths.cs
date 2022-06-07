namespace Fmbm.Paths;

public static class DirPaths
{
    static readonly object dirsLock = new Object();
    static readonly Dictionary<string, DirPath> namedDirs =
        new Dictionary<string, DirPath>();

    static DirPaths()
    {
        AppRoot = new DirPath("AppRoot");
        SetAppRoot(RootPresets.Fmbm());
    }

    public static DirPath AppRoot { get; }

    public static bool SetAppRoot(params string?[] paths)
    {
        var path = paths.FirstNonNull();
        if (path is not null)
        {
            AppRoot.Path = path;
            return true;
        }
        return false;
    }

    public static void ClearAppRoot(params string?[] paths)
    {
        AppRoot.ClearPath();
    }

    public static DirPath GetDir(string name)
    {
        var key = Key(name);
        lock (dirsLock)
        {
            if (!namedDirs.TryGetValue(key, out var child))
            {
                var childPath = AppRoot.Path.SubDir(name);
                child = new DirPath(name, childPath);
                namedDirs.Add(key, child);
            }
            return child;
        }
    }

    static string Key(string name)
    {
        return name.ToUpperInvariant().Trim();
    }

    public static DirPath ArchiveDir => GetDir("archive");
    public static DirPath DataDir => GetDir("data");
    public static DirPath DownloadDir => GetDir("download");
    public static DirPath EtcDir => GetDir("etc");
    public static DirPath LogDir => GetDir("log");
    public static DirPath TempDir => GetDir("temp");
    public static DirPath UploadDir => GetDir("upload");

    public class DirPath
    {
        string? path = null;
        bool pathHasBeenRead = false;
        readonly object pathLock = new Object();

        internal DirPath(string moniker, string? path = null)
        {
            Moniker = moniker;
            SetPath(path);
        }

        internal string Moniker { get; }

        public string Path
        {
            get
            {
                lock (pathLock)
                {
                    pathHasBeenRead = true;
                }
                if (path is null)
                {
                    var errMsg = $"Path for {Moniker} is not set.";
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
            lock (pathLock)
            {
                if (pathHasBeenRead)
                {
                    var errMsg =
                        $"Cannot set path for {Moniker} to {newPath} "
                        + $"after the existing value, {path}, has already "
                        + "been read.";
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
