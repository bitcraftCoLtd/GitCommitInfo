# Overview

This tool generates a C# source code file containing the information of the latest commit of your local git repository.

The name of the built binary is `dotnet-git-commit-info`, this is for integration with .NET Core CLI tool.

# NuGet package

A NuGet package is available here: https://www.nuget.org/packages/Bitcraft.Tools.GitCommitInfo

# How to use

First, for versions 1.1.0 or higher, be aware that running `dotnet restore` then `dotnet build` at the solution level will not work.

You have to `restore` and `build` both projects separately:

```
dotnet restore Bitcraft.Tools.GitCommitInfo
dotnet build Bitcraft.Tools.GitCommitInfo
```

then

```
dotnet restore TestLibrary
dotnet build TestLibrary
```

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

## With .NET Core CLI tool (msbuild)

This works for version 1.1.0 or greater of the `Bitcraft.Tools.GitCommitInfo` tool.

To integrate this tool in your project, you have to modify your `.csproj` file, as follow:

The name of the target `GitCommitInfoTarget` can probably be whatever you want.

The target have to be run after the `Restore` target in order to ensure the tool is restored, and it has to run before the build happens.

### Simple version

```XML
    ...
    <!-- ========== begining of Bitcraft.Tools.GitCommitInfo section ========== -->
    <ItemGroup>
        <Compile Include="GitCommitInfo.cs" />
        <DotNetCliToolReference Include="Bitcraft.Tools.GitCommitInfo" Version="1.1.0" />
    </ItemGroup>

    <Target Name="GitCommitInfoTarget" AfterTargets="Restore" BeforeTargets="BeforeBuild">
        <Exec Command="dotnet git-commit-info" />
    </Target>
    <!-- ========== end of Bitcraft.Tools.GitCommitInfo section ========== -->
    ...
```

`GitCommitInfo.cs` in the `Compile` directive is the default filename produced by the tool.
If this `Compile` directive is not declared, each build where the said file does not exist yet, will fail miserably.

`1.1.0` is the current version of the tool.


### Advanced version

```XML
    ...
    <!-- ========== begining of Bitcraft.Tools.GitCommitInfo section ========== -->
    <PropertyGroup>
        <!-- feel free to tweak bellow variables -->
        <GitCommitInfoToolVersion>1.1.0</GitCommitInfoToolVersion>
        <GitCommitInfoFilename>GitCommitInfo.cs</GitCommitInfoFilename>
        <GitCommitInfoToolOptions></GitCommitInfoToolOptions>
    </PropertyGroup>

    <ItemGroup>
        <Compile Condition="!Exists('$(GitCommitInfoFilename)')" Include="$(GitCommitInfoFilename)" />
        <DotNetCliToolReference Include="Bitcraft.Tools.GitCommitInfo" Version="$(GitCommitInfoToolVersion)" />
    </ItemGroup>

    <Target Name="GitCommitInfoTarget" AfterTargets="Restore" BeforeTargets="BeforeBuild">
        <Exec Command="dotnet git-commit-info $(GitCommitInfoToolOptions) --output $(GitCommitInfoFilename)" />
    </Target>
    <!-- ========== end of Bitcraft.Tools.GitCommitInfo section ========== -->
    ...
```

The variables:

- `GitCommitInfoToolVersion` is set to the current version of the tool.
- `GitCommitInfoFilename` is set to the default filename produced by the tool.
- `GitCommitInfoToolOptions` are additional options to provide to the tool.

## With .NET Core CLI tool (project.json)

This works only for version bellow 1.1.0 of the `Bitcraft.Tools.GitCommitInfo` tool.

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
        "precompile": "dotnet git-commit-info"
        ...
    }
    ...
```

### Note

Bellow note is for versions lower than 1.1.0 of the tool.

If you are already using the generated class in your project, and for some reasons the file to generate in not present on your local machine, you may get a compile error saying the class you generated does not exist.

This is because files to compile are evaluated before the precompile scripts run, and thus the generated file is not taken into account. To fix this, just run compilation again and it will work.

For more information about this issue, you can refer to the following GitHub links:
- https://github.com/dotnet/cli/issues/1475
- https://github.com/dotnet/cli/issues/3807
