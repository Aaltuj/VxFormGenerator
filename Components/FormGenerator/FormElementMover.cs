namespace FormGeneratorDemo.Components.FormGenerator
{
    public class FormElementMover
    {
        public IRenderChildren Payload { get; private set; }

        public FormElementMover(IRenderChildren payload)
        {
            this.Payload = payload;
        }
    }
}