using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace VxFormGenerator.Core
{
    public static class VxHelpers
    {
        public static bool IsTypeDerivedFromGenericType(Type typeToCheck, Type genericType)
        {
            if (typeToCheck == typeof(object))
            {
                return false;
            }
            else if (typeToCheck == null)
            {
                return false;
            }
            else if (typeToCheck.IsGenericType && typeToCheck.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }
            else
            {
                return IsTypeDerivedFromGenericType(typeToCheck.BaseType, genericType);
            }
        }

        internal static IEnumerable<PropertyInfo> GetModelProperties(Type modelType)
        {
            return modelType.GetProperties()
                     .Where(w => w.GetCustomAttribute<VxIgnoreAttribute>() == null);
        }

        internal static List<T> GetAllAttributes<T>(Type modelType) where T : Attribute => modelType.GetCustomAttributes<T>().ToList();

        internal static bool TypeImplementsInterface(Type type, Type typeToImplement)
        {
            Type foundInterface = type
                .GetInterfaces()
                .Where(i =>
                {
                    return i.Name == typeToImplement.Name;
                })
                .Select(i => i)
                .FirstOrDefault();

            return foundInterface != null;
        }

    }


    public static class VxEnumExtensions
    {

        // This extension method is broken out so you can use a similar pattern with 
        // other MetaData elements in the future. This is your base method for each.
        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
            return attributes.Length > 0
              ? (T)attributes[0]
              : null;
        }

    }

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

        public static List<VxSelectItem> ToSelectItems(IDictionary<string, string> values)
        {
            List<VxSelectItem> list = new List<VxSelectItem>();

            foreach (var value in values)
            {
                list.Add(new VxSelectItem() { Label = value.Value, Key = value.Key });
            }

            return list;
        }
    }
}
