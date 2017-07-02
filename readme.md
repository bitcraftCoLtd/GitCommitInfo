# Overview

This tool generates a C# source code file containing the information of the latest commit of your local git repository.

The name of the built binary is `dotnet-git-commit-info`, this is for integration with .NET Core CLI tool.

This documentation is valid for version 1.1.0 or higher and may not match with lower version of `Bitcraft.Tools.GitCommitInfo`.

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
/// <summary>
/// Stores the git information of the current HEAD of your local repository.
/// </summary>
public class GitCommitInfo
{
    /// <summary>
    /// Gets an instance of the git commit information.
    /// </summary>
    public static GitCommitInfo Instance { get; }

    static GitCommitInfo()
    {
        Instance = new GitCommitInfo();
    }

    /// <summary>
    /// Gets the branch name.
    /// </summary>
    public string BranchName
    {
        get { return "master"; }
    }

    /// <summary>
    /// Gets the short commit hash.
    /// </summary>
    public string ShortCommitHash
    {
        get { return "cd1316d"; }
    }

    /// <summary>
    /// Gets the long commit hash.
    /// </summary>
    public string LongCommitHash
    {
        get { return "cd1316dc68fbf8d2e2e74754ef97101e96d8d687"; }
    }

    /// <summary>
    /// Gets the committer date.
    /// </summary>
    public string CommitterDate
    {
        get { return "2017-07-02 16:59:52 +0900"; }
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
namespace Alice.Bob
{
  /// <summary>
  /// Stores the git information of the current HEAD of your local repository.
  /// </summary>
  internal class Charly
  {
    /// <summary>
    /// Gets an instance of the git commit information.
    /// </summary>
    public static Charly Instance { get; }

    static Charly()
    {
      Instance = new Charly();
    }

    /// <summary>
    /// Gets the branch name.
    /// </summary>
    public string BranchName
    {
      get { return "master"; }
    }

    /// <summary>
    /// Gets the short commit hash.
    /// </summary>
    public string ShortCommitHash
    {
      get { return "91f73cf"; }
    }

    /// <summary>
    /// Gets the long commit hash.
    /// </summary>
    public string LongCommitHash
    {
      get { return "91f73cff7fce2a6525994be3884170ab9e07dc0e"; }
    }

    /// <summary>
    /// Gets the committer date.
    /// </summary>
    public string CommitterDate
    {
      get { return "2017-07-02 17:14:48 +0900"; }
    }
  }
}
```

## Integrate with .NET Core CLI tool (msbuild)

This works for version 1.1.0 or greater of the `Bitcraft.Tools.GitCommitInfo` tool.

To integrate this tool in your project, you have to modify your `.csproj` file, as follow:

The name of the target `GitCommitInfoTarget` can probably be whatever you want.

The target have to be run after the `Restore` target in order to ensure the tool is restored, and it has to run before the build happens.

```XML
    ...
    <!-- ========== begining of Bitcraft.Tools.GitCommitInfo section ========== -->
    <ItemGroup>
        <DotNetCliToolReference Include="Bitcraft.Tools.GitCommitInfo" Version="1.1.0" />
    </ItemGroup>

    <Target Name="GitCommitInfoTarget" AfterTargets="Restore" BeforeTargets="BeforeBuild">
        <Exec Command="dotnet git-commit-info" />
        <ItemGroup>
            <Compile Include="**/*$(DefaultLanguageSourceExtension)"
                     Exclude="$(DefaultItemExcludes);$(DefaultExcludesInProjectFolder);$(BaseIntermediateOutputPath)**;$(BaseOutputPath)**;@(Compile)" />
        </ItemGroup>
    </Target>
    <!-- ========== end of Bitcraft.Tools.GitCommitInfo section ========== -->
    ...
```

### Note

The `Include` and `Exclude` rules trick of the `Compile` node of the project described in the previous section has been given by [Martin Ullrich](https://stackoverflow.com/users/784387/martin-ullrich) on [this post](https://stackoverflow.com/questions/44818730/is-there-a-net-core-cli-pre-before-build-task).
