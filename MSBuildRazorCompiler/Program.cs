using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using RazorLight;
using RazorLight.Generation;

namespace MSBuildRazorCompiler
{
    class Program
    {
        private static readonly Regex LeadingSlash = new Regex(@"^[\\/]", RegexOptions.Compiled);

        static int Main(string[] args)
        {
            var directoryToApply = args.Length > 0 ? args[0] : null;
            var rootNamespace = args.Length > 1 ? args[1] : null;
            var classModifier = args.Length > 2 ? args[2] : "internal";

            if (string.IsNullOrEmpty(directoryToApply) || !Directory.Exists(directoryToApply) || !Path.IsPathFullyQualified(directoryToApply))
            {
                Console.Error.WriteLine($"Invalid directory passed in as commandline argument: {directoryToApply}");
                return 1;
            }

            if (string.IsNullOrEmpty(rootNamespace))
            {
                Console.Error.WriteLine($"Invalid namespace passed in as commandline argument: {rootNamespace}");
            }

            var engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(directoryToApply, ".cshtml")
                .Build();

            // todo: submit PR to RazorGenerator to remove the need for reflection
            var sourceGenerator = (RazorSourceGenerator)(engine.Handler.Compiler.GetType()
                    .GetField("_razorSourceGenerator", BindingFlags.Instance | BindingFlags.NonPublic)
                    .GetValue(engine.Handler.Compiler));

            foreach(var file in Directory.EnumerateFiles(directoryToApply, "*.cshtml", SearchOption.AllDirectories))
            {
                Console.WriteLine($"Processing {file}...");

                var itemKey = LeadingSlash.Replace(file.Replace(directoryToApply, ""), "");
                var source = sourceGenerator.GenerateCodeAsync(itemKey).GetAwaiter().GetResult();

                var (namespacePath, className) = GetNamespaceAndClassName(itemKey, rootNamespace);
                Console.WriteLine($"  Classname will be `{className}` and namespace will be `{namespacePath}`");

                var code = source.GeneratedCode
                    .Replace("public class GeneratedTemplate", $"{classModifier} class {className}")
                    .Replace("namespace RazorLight.CompiledTemplates", $"namespace {namespacePath}")
                    .Replace("typeof(RazorLight.CompiledTemplates.GeneratedTemplate)", $"typeof({namespacePath}.{className})");

                Console.WriteLine($"  Writing {file}.cs");
                File.WriteAllText(file + ".cs", code, Encoding.UTF8);
            }

            return 0;
        }

        // todo: implement this as a Razor Feature (as pe https://github.com/toddams/RazorLight/blob/master/src/RazorLight/DefaultRazorEngine.cs)
        private static (string, string) GetNamespaceAndClassName(string itemKey, string rootNamespace)
        {
            var finalSlash = itemKey.LastIndexOf(Path.DirectorySeparatorChar);
            var className = "";
            var namespacePath = "";
            if (finalSlash > -1)
            {
                className = itemKey.Substring(finalSlash + 1).Replace(".cshtml", "");
                namespacePath = rootNamespace + "." + itemKey.Substring(0, finalSlash).Replace(Path.DirectorySeparatorChar, '.');
            }
            else
            {
                className = itemKey.Replace(".cshtml", "");
                namespacePath = rootNamespace;
            }
            className = className[0].ToString().ToUpper() + className.Substring(1);

            return (namespacePath, className);
        }
    }
}
