DirPaths
========

Tools to help with local directories used by an application.  E.g.:

    AppRoot
       │
       ├── bin
       │
       ├── etc
       │
       ├── data
       │
       └── log

Features:
  * Default rules for determining the AppRoot without configuration.
  * Tools for setting the AppRoot with code or configuration.
  * Configure alternate paths for top level subdirectories.


For Me, By Me (FMBM)
--------------------
This package is intended primarily for use by the author. Currently it has
only been tested on Windows. (The name is inspired by the [Fubu][Fubu]
_For Us, By Us_ project, but there is no other relationship.)


Basic Usage
-----------

    // Get path for <AppRoot>/etc but don't create it if it doesn't exist
    string etcDir = DirPaths.EtcDir.Path;

    // Get path for <AppRoot>/temp and create it if needed.
    string tempDir = DirPaths.TempDir.CheckedPath;

    // Get path for <AppRoot>/cat (and create it if needed)
    string catDir = DirPaths.GetDir("cats").CheckedPath;

    // Specify custom path for 'data' and 'dog'.
    DirPaths.DataDir.Path = @"C:\Users\Public\FMBM\Storage";
    DirPaths.GetDir("dog").Path = @"C:\Users\Public\FMBM\Canine";


Set Before Use
--------------
No Path property can be set after it has been got.  So all gets of a given
Path property will always return the same value (or all will raise the same
exception).

A path can be set any number of times before it it is got.

By default AppRoot and subdirectories will have a (non null) value.

The AppRoot Path can be cleared (set to null).  Any attempt to get the Path
while it is null will raise an InvalidOperationException.  If AppRoot.Path
is got while it is null it then cannot be set.  (I.e. The path must be set
before any gets).


Default AppRoot
---------------
By default DirPaths.AppRoot.Path will be set to first successful rule:
  1. Use the value stored in the FMBM_AppRoot environment variable.
  1. If the [Base][MSBaseDir] directory's name is 'bin' use the parent of
  that directory.


[Fubu]: <https://fubumvc.github.io/>
[MSBaseDir]: <https://docs.microsoft.com/en-us/dotnet/api/system.appcontext.basedirectory>
