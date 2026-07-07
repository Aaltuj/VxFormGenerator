using System.Reflection;
using VxFormGenerator.Core.Scaffolding;

if (args.Length == 0 || HasOption(args, "--help") || HasOption(args, "-h"))
{
    PrintHelp();
    return 0;
}

if (!string.Equals(args[0], "scaffold", StringComparison.OrdinalIgnoreCase))
{
    Console.Error.WriteLine($"Unknown command '{args[0]}'.");
    PrintHelp();
    return 1;
}

var assemblyPath = GetOption(args, "--assembly");
var typeName = GetOption(args, "--type");
var outputPath = GetOption(args, "--output");
var componentName = GetOption(args, "--component");
var noTemplate = HasOption(args, "--no-field-template");

if (string.IsNullOrWhiteSpace(assemblyPath) || string.IsNullOrWhiteSpace(typeName) || string.IsNullOrWhiteSpace(outputPath))
{
    Console.Error.WriteLine("Missing required options: --assembly, --type, and --output are required.");
    PrintHelp();
    return 1;
}

var fullAssemblyPath = Path.GetFullPath(assemblyPath);
if (!File.Exists(fullAssemblyPath))
{
    Console.Error.WriteLine($"Assembly not found: {fullAssemblyPath}");
    return 1;
}

var assembly = Assembly.LoadFrom(fullAssemblyPath);
var modelType = assembly.GetType(typeName, throwOnError: false);

if (modelType == null)
{
    Console.Error.WriteLine($"Type '{typeName}' was not found in '{fullAssemblyPath}'.");
    return 1;
}

var source = VxFormRazorComponentSourceGenerator.Generate(modelType, new VxFormRazorComponentScaffoldOptions
{
    ComponentName = componentName,
    IncludeFieldTemplate = !noTemplate
});

var fullOutputPath = Path.GetFullPath(outputPath);
var outputDirectory = Path.GetDirectoryName(fullOutputPath);

if (!string.IsNullOrWhiteSpace(outputDirectory))
{
    Directory.CreateDirectory(outputDirectory);
}

File.WriteAllText(fullOutputPath, source);
Console.WriteLine($"Generated {fullOutputPath}");
return 0;

static string? GetOption(string[] args, string name)
{
    for (var index = 0; index < args.Length - 1; index++)
    {
        if (string.Equals(args[index], name, StringComparison.OrdinalIgnoreCase))
        {
            return args[index + 1];
        }
    }

    return null;
}

static bool HasOption(string[] args, string name)
{
    return args.Any(arg => string.Equals(arg, name, StringComparison.OrdinalIgnoreCase));
}

static void PrintHelp()
{
    Console.WriteLine("VxFormGenerator.Tools");
    Console.WriteLine();
    Console.WriteLine("Usage:");
    Console.WriteLine("  vxform scaffold --assembly <path> --type <full.type.name> --output <component.razor> [--component <name>] [--no-field-template]");
    Console.WriteLine();
    Console.WriteLine("Example:");
    Console.WriteLine("  vxform scaffold --assembly ./bin/Debug/net10.0/MyApp.dll --type MyApp.CustomerFormModel --output Forms/CustomerForm.razor");
}
