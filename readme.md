# Overview

This tool generates a C# source code file containing the hash of the latest commit of your local git repository.

The name of the built binary is `dotnet-git-commit-info`, this is for integration with .NET Core CLI tool.

# NuGet package

A NuGet package is available here: https://www.nuget.org/packages/Bitcraft.Tools.GitCommitInfo

# How to use

## In command line

Hereafter is the list of supported arguments:

`--namespace`, `--ns`

Valid values | Default | Description
---|---|---
Valid C# namespace name | (nothing) | If namespace argument is not provided, the static class is generated without namespace.

---

`--class`

Valid values | Default | Description
---|---|---
Valid C# class name | GitCommitInfo | The name of the static class that contains the git commit hash.

---

`--output`

Valid values | Default | Description
---|---|---
Valid filename | GitCommitInfo.cs | The output source code file. If a relative filename is provided, it is relative to the current directory.<br>This uses `Directory.GetCurrentDirectory()`.

---

`--access-modifier`

Valid values | Default | Description
---|---|---
**public** or **internal** | public | The access modifier of the generated static class.

---

`--indent`, `--indenting`

Valid values | Default | Description
---|---|---
**space**, **spaces**, **tab** or **tabs** | space | The characters used to indent the generated code.

---

`--indent-size`, `--indenting-size`

Valid values | Default | Description
---|---|---
Integer greater than or equal to zero | 4 | The number of indent character per level of indentation.

---

`--line-ending`

Valid values | Default | Description
---|---|---
**crlf** or **lf** | lf | The character(s) used for line ending of the generated code.

### Example

Run the command `dotnet dotnet-git-commit-info.dll`, this will generate a file named `GitCommitInfo.cs` in the current directory, containing the following code.

```CSharp
using System;

/// <summary>
/// Stores the git information of the current HEAD of your local repository.
/// </summary>
public static class GitCommitInfo
{
    /// <summary>
    /// Gets the branch name.
    /// </summary>
    public static string BranchName
    {
        get { return "master"; }
    }

    /// <summary>
    /// Gets the short commit hash.
    /// </summary>
    public static string ShortCommitHash
    {
        get { return "d173597"; }
    }

    /// <summary>
    /// Gets the long commit hash.
    /// </summary>
    public static string LongCommitHash
    {
        get { return "d17359787eef1e5047e6c5542b16608b4782083b"; }
    }

    /// <summary>
    /// Gets the committer date.
    /// </summary>
    public static string CommitterDate
    {
        get { return "2017-02-03 12:10:04 +0900"; }
    }
}
```

### Another example

```
dotnet dotnet-git-commit-info.dll \
   --namespace Alice.Bob \
   --class Charly \
   --output Misc/MyFile.cs \
   --access-modifier internal \
   --indent spaces \
   --indent-size 2 \
   --line-ending lf
```

Then in the file `<output>/Misc/MyFile.cs`:

```CSharp
using System;

namespace Alice.Bob
{
  /// <summary>
  /// Stores the git information of the current HEAD of your local repository.
  /// </summary>
  internal static class Charly
  {
    /// <summary>
    /// Gets the branch name.
    /// </summary>
    public static string BranchName
    {
      get { return "master"; }
    }

    /// <summary>
    /// Gets the short commit hash.
    /// </summary>
    public static string ShortCommitHash
    {
      get { return "d173597"; }
    }

    /// <summary>
    /// Gets the long commit hash.
    /// </summary>
    public static string LongCommitHash
    {
      get { return "d17359787eef1e5047e6c5542b16608b4782083b"; }
    }

    /// <summary>
    /// Gets the committer date.
    /// </summary>
    public static string CommitterDate
    {
      get { return "2017-02-03 12:10:04 +0900"; }
    }
  }
}
```

## With .NET Core CLI tool

To integrate this tool in your project, you have to modify your `project.json` file, as follow:

```
    ...
    "tools": {
        ...
        "Bitcraft.Tools.GitCommitInfo": "<version>"
        ...
    }
    ...
    "scripts": {
        ...
        "precompile": "dotnet git-commit-info --access-modifier internal"
        ...
    }
    ...
```

### Note

If you are already using the generated class in your project, and for some reasons the file to generate in not present on your local machine, you may get a compile error saying the class you generated does not exist.

This is because files to compile are evaluated before the precompile scripts run, and thus the generated file is not taken into account. To fix this, just run compilation again and it will work.

For more information about this issue, you can refer to the following GitHub links:
- https://github.com/dotnet/cli/issues/1475
- https://github.com/dotnet/cli/issues/3807
