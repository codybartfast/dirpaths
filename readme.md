DirPaths
========

Tools to help with local directories used by an application.  E.g.:

    <AppRoot>
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
This package is primarily intended for use by the author.  It has only been
tested with Console applications on Windows.  It is aimed at getting ad-hoc
applications up and running quickly.  It is not intended for complex,
production or evolving environments.  (The name is inspired by the [Fubu][Fubu]
_For Us, By Us_ project, but there is no other connection.)


Basic Usage
-----------
    using Fmbm.Dir;

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

The AppRoot Path can be cleared.  An InvalidOperationException will be
if the Path's value is got while in a cleared state (I.e. before a new
value has been set).


Default AppRoot
---------------
By default DirPaths.AppRoot.Path will be set to first successful rule:
  1. Use the value stored in the FMBM_AppRoot environment variable.

  2. If the name of the Base directory (see 4. below) is ``bin`` then use the 
  parent of that directory as the AppRoot.  E.g., if the Base directory is
  ``...\apps\myapp\bin`` then ``...\apps\myapp\`` will be the AppRoot.  This
  case is for when we want to deploy applications using a tree structure
  similar to that at the top of this page.

  3. If one of the parent directories of the Base dir is named ``bin`` then
  use a sibling of that ``bin`` directory named ``AppRoot``.  This case is for
  running the application under Visual Studio.  If the Base directory is
  ``..\Project\bin\Debug\net9.0`` then the AppRoot will be: 
  ``..\Project\AppRoot\``.\
 \
  However, if the application is a Debug build and
  the directory ``AppRoot-Debug`` _already_ exists, then that will be used
  instead.  It will not be used automatically, only if the directory has already
  been manually created beforehand.  (This allows Release and Debug builds
  to run with different environments.)

  4. If none of the above rules match, then use the 
  [Base directory][MSBaseDir].  This is usually directory containing the application's executable.


[Fubu]: <https://fubumvc.github.io/>
[MSBaseDir]: <https://docs.microsoft.com/en-us/dotnet/api/system.appcontext.basedirectory>
