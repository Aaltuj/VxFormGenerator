using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VxFormGenerator.Core;

namespace VxFormGenerator.Components.Plain.Models
{
    public class VxSelectItem
    {
        public VxSelectItem()
        {

        }
        public VxSelectItem(DisplayAttribute displayAttribute, Enum value)
        {
            this.Order = displayAttribute.GetOrder() ?? 0;
            this.Label = displayAttribute.GetName();
            this.Key = value.ToString();
            this.Description = displayAttribute.GetDescription();
        }


        public int Order { get; set; }

        public string Label { get; set; }

        public string Key { get; set; }

        public string Description { get; set; }

        public bool Selected { get; set; }

        // This method creates a specific call to the above method, requesting the
        // Description MetaData attribute.
        public static VxSelectItem ToSelectItem(Enum value)
        {
            var foundAttr = value.GetAttribute<DisplayAttribute>();
            if (foundAttr != null)
            {
                return new VxSelectItem(foundAttr, value);
            }
            else
            {
                return new VxSelectItem() { Label = value.ToString() };
            }
        }
    }

}
