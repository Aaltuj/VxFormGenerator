namespace VxFormGenerator.Core.Scaffolding
{
    public sealed class VxFormRazorComponentScaffoldOptions
    {
        public string ComponentName { get; set; }

        public string ModelNamespace { get; set; }

        public string ModelTypeName { get; set; }

        public string ModelPropertyName { get; set; } = "Model";

        public string SubmitCallbackName { get; set; } = "ValidSubmit";

        public bool UseObjectGraphValidator { get; set; } = true;

        public bool IncludeFieldTemplate { get; set; } = true;
    }
}
