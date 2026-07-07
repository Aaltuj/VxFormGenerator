using System.Globalization;
using System.Resources;

namespace VxFormGeneratorDemoData.Resources
{
    public class Address
    {
        private static readonly ResourceManager ResourceManager = new ResourceManager("VxFormGeneratorDemoData.Resources.Address", typeof(Address).Assembly);

        public static string FIRSTNAME_LABEL => ResourceManager.GetString(nameof(FIRSTNAME_LABEL), CultureInfo.CurrentUICulture);

        public static string STREET_MIN_LENGTH => ResourceManager.GetString(nameof(STREET_MIN_LENGTH), CultureInfo.CurrentUICulture);
    }
}
