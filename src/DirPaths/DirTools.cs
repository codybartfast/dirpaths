using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("DirPaths.Test")]

namespace Fmbm.Dir;

public static class DirTools
{
    internal static string? FirstNonNull(this string?[] strings)
    {
        foreach (string? str in strings)
        {
            if (str is not null)
            {
                return str;
            }
        }
        return null;
    }

    internal static bool NamesAreEqual(this string name1, string name2)
    {
        return
            (name1 != null) &&
                (StringComparer
                    .InvariantCultureIgnoreCase.Compare(name1, name2) == 0);
    }

    public static string? NameIs(this string? path, string name)
    {
        if (path is null)
        {
            return null;
        }
        if (new DirectoryInfo(path).Name.NamesAreEqual(name))
        {
            return path;
        }
        return null;
    }

    public static string? Parent(this string? path)
    {
        if (path is null)
        {
            return null;
        }
        var parent = new DirectoryInfo(path).Parent;
        if (parent is null)
        {
            return null;
        }
        return parent.FullName;
    }

    public static string? SearchUp(this string? path, string name)
    {
        if (path is null)
        {
            return null;
        }
        for (var dir = new DirectoryInfo(path); dir is not null; dir = dir.Parent)
        {
            if (dir.Name.NamesAreEqual(name))
            {
                return dir.FullName;
            }
        }
        return null;
    }

    public static string? SubDir(this string? path, string name)
    {
        if (path is null)
        {
            return null;
        }
        return Path.Combine(path, name);
    }

    public static string? Sibling(this string? path, string name)
    {
        return path.Parent().SubDir(name);
    }

    public static string? ExistingDebug(this string? path)
    {
#if DEBUG
        if (path is null)
        {
            return null;
        }
        var debugPath = path.Sibling(new DirectoryInfo(path).Name + "-Debug");
        return Directory.Exists(debugPath) ? debugPath : path;
#else
        return path;
#endif    
    }
}
