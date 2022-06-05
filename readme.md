DirPaths
========

Tools to help with local directories used by an application.  E.g.:

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
  * Tools for setting the AppRoot with code or configuration.
  * Configure alternate paths for top level subdirectories.


For Me, By Me (FMBM)
--------------------
This package is primarily intended for use by the author.  It has only been
tested with Console applications on Windows and MacOS.  It is aimed at getting ad-hoc
applications up and running quickly.  It is not intended for complex,
production or evolving environments.  (The name is inspired by the [Fubu][Fubu]
_For Us, By Us_ project, but there is no other connection.)

---

Contents
--------
  - [Basic Usage](#basic-usage)
  - [Set Before Use](#set-before-use)
  - [Default AppRoot](#default-approot)
  - [Changing The AppRoot](#changing-the-approot)
  - [AppRoot Presets](#approot-presets)
  - [Directory Tools](#dirtools)
  - [Examples](#examples)

---

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
    DirPaths.DataDir.Path = "FMBM_Sample/Data";
    DirPaths.GetDir("dog").Path = "FMBM_Sample\Canine";

---

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

---

Default AppRoot
---------------
By default DirPaths.AppRoot.Path will be set to first successful rule:
  1. Use the value stored in the FMBM_AppRoot environment variable.

  2. If the name of the Base directory (see 4. below) is ``bin`` then use the 
  parent of that directory as the AppRoot.  E.g., if the Base directory is
  ``...\apps\myapp\bin`` then ``...\apps\myapp\`` will be the AppRoot.  This is useful for deploying applications using a tree structure similar to that at the top of this page.

  3. If one of the parent directories of the Base dir is named ``bin`` then
  use a sibling of that ``bin`` directory named ``AppRoot``.  This is useful for
  running the application under Visual Studio.  If the Base directory is
  ``..\Project\bin\Debug\net9.0`` then the AppRoot will be: 
  ``..\Project\AppRoot\``.\
 \
  However, if the application is a Debug build and
  the directory ``AppRoot-Debug`` _already_ exists, then that will be used
  instead.  It will not be used automatically, only if the directory has already
  been manually created beforehand.  (This would allow Release and Debug builds
  to run with different environments.)

  4. If none of the above rules match, then use the 
  [Base directory][MSBaseDir].  This is usually the directory containing the application's executable.

---

Changing The AppRoot Path
-------------------------

The AppRoot.Path can only be set before its value has been read. 

### Runtime
The AppRoot can be changed at runtime by setting the ``FMBM_APPROOT``
environment variable.

### SetAppRootPath

``DirPaths.SetAppRootPath`` is a helper function that takes an arbitary number of strings and sets the AppRoot path to the first non-null value.  If all the values are null then
the AppRoot path is unchanged.

### ClearAppRootPath
``DirPaths.ClearAppRootPath`` clears any AppRoot path.  This is useful if you
don't want to use the default path and to make sure only an explicitly set 
path is used.

### AppRoot.Path
``DirPaths.AppRoot.Path`` can be set directly, like subdirectories can, it
will not accept a null value.

---

AppRoot Presets
---------------

``RootPresets.Base`` The applications [Base directory][MSBaseDir].  (Used by the fourth rule for finding the [Default AppRoot](#default-approot).)

``RootPresets.Current`` The process's current working directory.

``RootPresets.EnvironmentVariable()`` The value stored in the FMBM_APPROOT
environment variable.  (Used by the first rule for finding
the [Default AppRoot](#default-approot).)  - can be null.

``RootPresets.EnvironmentVariable(NAME)`` The value stored in the ``<NAME>`` environment variable - can be null.

``RootPresets.BinParent()`` The value described in the second rule for finding
the [Default AppRoot](#default-approot) - can be null.

``RootPresets.AnyBinAppRootSibling()`` The value described in the third rule
for finding the [Default AppRoot](#default-approot) - can be null.

``RootPresets.FMBM()`` The function used to find the [Default AppRoot](#default-approot) using the helper functions above.

``RootPresets.FMBM(NAME)`` The same as ``FMBM()`` except it checks the ``<NAME>``
environment variable instead of the ``FMBM_APPROOT`` environment variable.

---

DirTools
--------
Helper methods used by the RootPesets.  These are extension methods to ``string``
that can accept (and then return) a null value and return ``null`` to indicate
failure.

Name comparison is case insensitive.

### NameIs
``path.NameIs(name)`` returns ``path`` if the directory's name is ``name`` otherwise ``null``.

### Parent
``path.Parent()`` return ``path``'s parent directory path or ``null`` if ``path``
is ``null`` or is the root directory.

### SearchUp
``path.SearchUp(name)`` if ``path`` or any of its parent directories match the
name ``name`` that directory path is returned, otherwize ``null``.

### SubDir
``path.SubDir(name)`` the subdirectory of ``path`` with name ``name``, or ``null``
if ``path`` is null.

### Sibling
``path.Sibling(name)`` equivalent to ``path.Parent().SubDir(name)``

### ExistingDebug
``path.ExistingDebug()`` if the application is a Debug build and a sibling
to ``path`` already exists which has the name ``<name>-Debug`` then the path
to ``<name>-Debug`` is returned, otherwise ``path`` is returned.

---

Examples
--------

[Fubu]: <https://fubumvc.github.io/>
[MSBaseDir]: <https://docs.microsoft.com/en-us/dotnet/api/system.appcontext.basedirectory>
