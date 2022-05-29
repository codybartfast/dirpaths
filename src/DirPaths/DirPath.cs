using System;

namespace Bmfm;

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
                    $"Cannot get path for {Moniker} before it has not been set.";
                throw new InvalidOperationException(errMsg);
            }
            return path;
        }
        set{
            lock(lockObj){
                if(pathHasBeenRead){
                    var errMsg = 
                        $"Cannot set path for {Moniker} to {value} after the existing value, {path}, has already been read.";
                }
                path = value;
            }
        }
    }
}
