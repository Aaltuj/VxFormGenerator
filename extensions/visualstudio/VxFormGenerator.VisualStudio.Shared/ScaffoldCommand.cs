namespace VxFormGenerator.VisualStudio.Shared
{
    public sealed class ScaffoldCommand
    {
        public ScaffoldCommand(string fileName, string arguments)
        {
            FileName = fileName;
            Arguments = arguments;
        }

        public string FileName { get; }

        public string Arguments { get; }
    }
}
