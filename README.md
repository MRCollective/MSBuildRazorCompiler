
# MSBuild Razor Compiler

[![Build status](https://ci.appveyor.com/api/projects/status/yndlxwfnxom60n1s?svg=true)](https://ci.appveyor.com/project/MRCollective/msbuildrazorcompiler)
[![NuGet version](https://img.shields.io/nuget/vpre/MSBuildRazorCompiler.svg)](https://www.nuget.org/packages/MSBuildRazorCompiler)

This library provides a .NET Core 3.1 compatible MSBuild task that will automatically compile all `.cshtml` files into `.cs` files in-place and also provides you code you can call to then invoke those classes with a model and get the resulting output.

This library is inspired by [RazorGenerator](https://github.com/RazorGenerator/RazorGenerator), which was a custom tool for Visual Studio that created a compiled `.cs` file when saving a `.cshtml` file in your IDE. This was handy for situations where you want to have static template files (e.g. email templates, [library templates](https://github.com/MRCollective/ChameleonForms/tree/master/ChameleonForms/Templates/Default), etc.). RazorGenerator doesn't support .NET Core, hence creating this.

This library is implemented by providing a thin wrapper over the [RazorLight](https://github.com/toddams/RazorLight) library and breaking it up into two stages:

1. Compilation of `.cshtml` files to `.cs` files - this happens just before the `CoreCompile` task of the project you install this library into via the included MSBuild task.
2. Rendering / invocation of the resultant `.cs` code

Because this library generates `.cs` files you don't need to worry about compilation performance, runtime compilation errors, caching, etc. It also allows you to have the predictability of having real classes you can reference from your code rather than relying on magic strings to find your `.cshtml` files and needing to either ship your `.cshtml` files with your code or embed them into your dll. Using this solution those files are only used at compile time and can then be discarded.

## Getting started

1. Include `SomeFile.cshtml` in your (netcoreapp3.1) project
2. `Install-Package MSBuildRazorCompiler`
3. Compile - you should now see `SomeFile.cshtml.cs` next to your `SomeFile.cshtml`, this new file will contain a class `SomeFile` that extends `RazorLight.TemplatePage<TModel>` and will be in the namespace of your project at the folder level your file was in
4. Execute the following code to get a rendered result (note: you'll need to add a `using RazorRenderer;`): `new SomeFile().Render(model, viewBag or null)` - there is also an async variant

## Intellisense

For intellisense to work it's recommended you add the following to the top of your file:

```cshtml
@inherits RazorLight.TemplatePage<MyModelType>
```

or:

```cshtml
@inherits RazorRenderer.BasePage<MyModelType>
```

The latter will opt you into functionality added to this library (see below).

## Including a sub-file

The RazorLight `Include` functionality has been disabled since it uses a magic string and this library aims to avoid runtime dynamic functionality, however, there is an include functionality that you can use. If you extend from `RazorRenderer.BasePage<TModel>` from the parent page and the child page:

```cshtml
@inherits RazorRenderer.BasePage<MyModelType>
```

Then in your parent file you can include another page using:

```cshtml
@{ Include<SubPage, MyModelType>(new MyModelType()); }
```

Where `SubPage` is the typename generated for the included page.

## Ignoring generated files from source control

It's recommended that you add `*.cshtml.cs` to your `.gitignore` file so that you don't commit the generated files. That avoids superfluous diff/commit noise.

## File Nesting

By default, your `.cshtml.cs` files will be nested under their `.cshtml` counterparts this using the [File Nesting feature in Visual Studio](https://docs.microsoft.com/en-us/visualstudio/ide/file-nesting-solution-explorer?view=vs-2019) if your project is classified as an ASP.NET Core project.

If you have a console app or class library you can trick it by ensuring your `.csproj` file starts with `<Project Sdk="Microsoft.NET.Sdk.Web">`. There are a couple of automatic behaviours you need to opt-out of though if you do that.

Either way you should add the following to the `.csproj`:

```xml
  <PropertyGroup>
    <PreserveCompilationContext>true</PreserveCompilationContext>
    <RazorLangVersion>3.0</RazorLangVersion>
    <RazorCompileOnBuild>false</RazorCompileOnBuild>
    <RazorCompileOnPublish>false</RazorCompileOnPublish>
  </PropertyGroup>
```

### Console app

Note: if you have a console app that means you'll need to add a `launchsettings.json` file in your `Properties` folder with something like this so that F5 still launches the app rather than IIS Express:

```json
{
  "profiles": {
    "Test": {
      "commandName": "Project"
    }
  }
}
```

### Class library

You should add the following to the `.csproj`:

```xml
  <PropertyGroup>
    <OutputType>Library</OutputType>
  </PropertyGroup>
```

And add a `launchsettings.json` file in your `Properties` folder with something like this so that F5 doesn't launch IIS Express (it will fail, but it's meant to - it's a class library):

```json
{}
```
