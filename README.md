DirPaths
========

A tool to help with the local directories used by an application.  E.g.: 

```
<AppRoot>
    │
    ├── bin
    │
    ├── etc
    │
    ├── data
    │
    └── log
```

Features:
  * Default rules for determining the AppRoot without configuration.
  * Custom rules for determining the AppRoot.
  * Configure alternate paths for top level subdirectories.
  * Thread Safe.


For Me, By Me (FMBM)
--------------------
This package is primarily intended for use by the author.  It has only been 
tested with Console applications on Windows and MacOS.  It intended for 
getting ad-hoc applications up and running quickly.  It probably is not 
suitable for complex, production, nor evolving projects.  (The name is 
inspired by the [Fubu][Fubu] _For Us, By Us_ project, but there is no other 
connection.)

----------------------------------------------------------------------------

Contents
--------
  - [Usage](#usage)
  - [Set Before Get](#set-before-get)
  - [Default AppRoot](#default-approot)
  - [Changing The AppRoot](#changing-the-approot)
  - [AppRoot Presets](#approot-presets)
  - [Directory Tools](#dirtools)
  - [Extensibility](#extensibility)
  - [Examples](#examples)

----------------------------------------------------------------------------

Usage
-----
`archive`, `data`, `download`, `etc`, `log`, `temp` and 
`upload` are predefined.  Other top level directories can be accessed 
using `GetDir(name)`.

The `Path` property returns the path for the directory.  `CheckedPath` 
checks if the directory exists and creates it if doesn't exist.

    using Fmbm.Dir;

    // Get path for <AppRoot>/etc but do not create it if it does not exist
    string etcDir = DirPaths.EtcDir.Path;

    // Get path for <AppRoot>/temp and create it if needed
    string tempDir = DirPaths.TempDir.CheckedPath;

    // Get path for <AppRoot>/cats (and create it if needed)
    string catDir = DirPaths.GetDir("cats").CheckedPath;

    // Specify custom path for 'data' and 'dog'.
    DirPaths.DataDir.Path = "FMBM_Sample/Data";
    DirPaths.GetDir("dog").Path = "FMBM_Sample\Canine";

----------------------------------------------------------------------------

Set Before Get
--------------
No `Path` property can be set after it has been got.  So all gets of a given 
`Path` property will always return the same value (or all will throw the 
same exception).

`Path` can be set any number of times before it it is got.

By default AppRoot and subdirectories will have a non-null value.

The AppRoot Path can be cleared.  An InvalidOperationException will be
thrown if `Path`'s value is got while it is clear. (I.e. before a new value
has been set.)

----------------------------------------------------------------------------

Default AppRoot
---------------
The default DirPaths.AppRoot.Path will be set by the first successful rule:
  1. Use the value stored in the FMBM_APPROOT environment variable.

  2. If the name of the Base directory (see 4. below) is `bin` then use the
  parent of that directory as the AppRoot.  E.g., if the Base directory  is
  `...\apps\myapp\bin` then `...\apps\myapp\` will be the AppRoot.  This is
  useful for deploying applications using a tree structure similar to that 
  at the top of this page when the executable is in the `bin` directory.

  3. If one of the parent directories of the Base dir is named `bin` then
  use a sibling of that `bin` directory named `AppRoot`.  This is useful for
  running the application under Visual Studio.  If the Base directory is
  `..\Project\bin\Debug\net9.0` then the AppRoot will be: 
  `..\Project\AppRoot\`.\
 \
  However, if the application is a Debug build and the directory 
  `AppRoot-Debug` _already_ exists, then that will be used instead.  It will
  not be used automatically, only if the directory has already been manually
  created beforehand.  (This would allow Release and Debug builds to run 
  with different environments.)

  4. If none of the above rules match, then use the 
  [Base directory][MSBaseDir].  This is usually the directory containing the
  application's   executable.

----------------------------------------------------------------------------

Changing The AppRoot
--------------------

The AppRoot.Path can only be set before its value has been read. 

### Runtime
The AppRoot can be changed at runtime by setting the `FMBM_APPROOT`
environment variable.

### SetAppRoot

`DirPaths.SetAppRoot(path1, path2, ...)` is a helper function that takes 
an arbitary number of strings and sets the AppRoot to the first non-null 
value.  If all the values are null then the AppRoot path is unchanged.

### ClearAppRoot
`DirPaths.ClearAppRoot()` clears any AppRoot path.  This is useful if you
do not want to use the default path and to make sure only an explicitly set 
path is used.

### AppRoot.Path
`DirPaths.AppRoot.Path` can be set directly, like subdirectories can, it
will not accept a null value.

----------------------------------------------------------------------------

AppRoot Presets
---------------
The `RootPresets` class contains properties and methods for some 
predefined rules for choosing the AppRoot directory.

  - `RootPresets.Base` The applications [Base directory][MSBaseDir].  (Used
  by the fourth rule for finding the [Default AppRoot](#default-approot).)

  - `RootPresets.Current` The process's current working directory.

  - `RootPresets.EnvironmentVariable()` The value stored in the 
  `FMBM_APPROOT` environment variable.  (Used by the first rule to find the 
  [Default AppRoot](#default-approot).)  - can be null.

  - `RootPresets.EnvironmentVariable(NAME)` The value stored in the `<NAME>`
  environment variable - can be null.

  - `RootPresets.BinParent()` The value described in the second rule to 
  find the [Default AppRoot](#default-approot) - can be null.

  - `RootPresets.AnyBinAppRootSibling()` The value described in the third
  rule to find the [Default AppRoot](#default-approot) - can be null.

  - `RootPresets.FMBM()` The function that combines all four rules to find
  the [Default AppRoot](#default-approot).

  - `RootPresets.FMBM(NAME)` The same as `FMBM()` except it checks the
  `<NAME>` environment variable instead of the `FMBM_APPROOT`
  environment variable.

----------------------------------------------------------------------------

DirTools
--------
The `DirTools` class contains helper methods used by the RootPesets.  These
are extension methods of `string`.  They use `null` to indicate failure so
all the methods can accept `null` as input, which will then be the return
value.  Name comparison is case insensitive.

  - `path.NameIs(name)` returns `path` if the directory's name matches 
  `name` otherwise `null`.

  - `path.Parent()` return `path`'s parent path or `null` if `path` is the
  root directory or it is null.

  - `path.SearchUp(name)` if `path` or any of its parent directories match
  the name `name` then that directory's path is returned, otherwize `null`.

  - `path.SubDir(name)` the subdirectory of `path` with name `name`, or 
  `null` if `path` is null.

  - `path.Sibling(name)` is equivalent to `path.Parent().SubDir(name)`

  - `path.ExistingDebug()` if the application is a Debug build and a sibling
  to `path` already exists that has the name `<name>-Debug` then the path to
  `<name>-Debug` is returned, otherwise `path` is returned.  

----------------------------------------------------------------------------

Extensibility
-------------
None.

----------------------------------------------------------------------------

Examples
--------

These are examples of how to set the AppRoot.  See [usage](#usage) above for
examples of how to get and set the path of its sub directories.
  
Set a hard coded path
```
DirPaths.SetAppRoot(@"C:\Apple\Banana\Cherry");

// EtcDir.Path is C:\Apple\Banana\Cherry\etc
```
<br/>

Use the value of a the FRUIT_APPROOT environment variable but fallback to
default AppRoot if the environment variable is not set:
```
Environment.SetEnvironmentVariable("FRUIT_APPROOT", @"D:\Fruity");
DirPaths.SetAppRoot(RootPresets.EnvironmentVariable("FRUIT_APPROOT"));

// EtcDir.Path is D:\Fruity\etc or the default <AppRoot>\etc if the
// environment variable is not set.
```
<br/>

Use the value of a the FRUIT_APPROOT environment variable but throw an
exception if the environment variable is not set:
```
// If the environment variable is set:
DirPaths.ClearAppRoot();
Environment.SetEnvironmentVariable("FRUIT_APPROOT", @"D:\Fruity");
DirPaths.SetAppRoot(RootPresets.EnvironmentVariable("FRUIT_APPROOT"));

// EtcDir.Path is D:\Fruity\etc

// Or if the environment variable is not set:
DirPaths.ClearAppRoot();
DirPaths.SetAppRoot(RootPresets.EnvironmentVariable("FRUIT_APPROOT"));

// InvalidOperationException is thrown when EtcDir.Path is got.
```
<br>

If the Base directory is named `Cherry` then use its parent as the AppRoot
(otherwise fallback to the default):
```
DirPaths.SetAppRoot(RootPresets.Base.NameIs("Cherry").Parent());

// If the Base directory is C:\Apple\Banana\Cherry then EtcDir.Path is 
// C:\Apple\Banana\etc
```
<br>

If any parent of the Base directory is called `Apple` then use a sibling of
that directory named `Basket`, otherwise fail:
```
DirPaths.ClearAppRoot();
DirPaths.SetAppRoot(RootPresets.Base.SearchUp("Apple").Sibling("Basket"));

// If the Base directory is C:\Apple\Banana\Cherry then EtcDir.Path is 
// C:\Basket\etc. If 'Apple' is not found then an InvalidOperationException
// is thrown.
 
```
<br>

Use the process's Current working directory:
```
DirPaths.SetAppRoot(RootPresets.Current);
```
<br>

Try four of the above rules and use the first that works:
```
DirPaths.ClearAppRoot();
DirPaths.SetAppRoot(
    RootPresets.EnvironmentVariable("FRUIT_APPROOT"),
    RootPresets.Base.NameIs("Cherry").Parent(),
    RootPresets.Base.SearchUp("Apple").Sibling("Basket"),
    RootPresets.Current
);
```
The `ClearAppRoot()` in the above shouldn't have any effect in this case
because `RootPresets.Current` should always have a non-null value so it
should naver fallback to the default path.


[Fubu]: <https://fubumvc.github.io/>
[MSBaseDir]: <https://docs.microsoft.com/en-us/dotnet/api/system.appcontext.basedirectory>
