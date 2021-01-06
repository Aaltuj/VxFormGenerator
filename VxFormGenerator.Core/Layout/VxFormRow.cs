using System;
using System.Collections.Generic;
using System.Text;

namespace VxFormGenerator.Core.Layout
{
    public class VxFormRow : ICloneable
    {

        public string Label { get; set; }
        public string Id { get; set; }

        public List<VxFormElementDefinition> Columns { get; set; } = new List<VxFormElementDefinition>();

        internal VxFormRowLayoutAttribute RowLayoutAttribute { get; set; }

        public VxFormRow(int rowId)
        {
            Id = rowId.ToString();
        }
        public VxFormRow(string rowId)
        {
            Id = rowId;
        }

        public object Clone()
        {
            return new VxFormRow(this.Id) { Columns = this.Columns, RowLayoutAttribute = this.RowLayoutAttribute };
        }

        internal static VxFormRow Create(VxFormElementLayoutAttribute layoutAttr, VxFormRowLayoutAttribute vxFormRowLayoutAttribute, VxFormLayoutOptions options)
        {
            // If no rowid is specified use the name instead
            var output = layoutAttr.RowId == 0 ? new VxFormRow(layoutAttr.FieldIdentifier) : new VxFormRow(layoutAttr.RowId);
            output.RowLayoutAttribute = vxFormRowLayoutAttribute;

            if (options.LabelOrientation == LabelOrientation.LEFT && output.RowLayoutAttribute?.Label != null)
                output.Label = vxFormRowLayoutAttribute.Label;

            return output;
        }

        internal static void AddColumn(VxFormRow foundRow, VxFormElementDefinition formColumn, VxFormLayoutOptions options)
        {
            foundRow.Columns.Add(formColumn);

            var layoutAttr = formColumn.RenderOptions;
            // Turn off all Labels for the elements
            switch (options.LabelOrientation)
            {
                case LabelOrientation.LEFT: layoutAttr.ShowLabel = false; break;
                case LabelOrientation.NONE: layoutAttr.ShowLabel = false; break;
                case LabelOrientation.TOP: layoutAttr.ShowLabel = true; break;
            }


            if (options.ShowPlaceholder == PlaceholderPolicy.EXPLICIT_LABEL_FALLBACK)
                layoutAttr.Placeholder = layoutAttr.Placeholder != null ? layoutAttr.Placeholder : layoutAttr.Label;
            else if (
                (options.ShowPlaceholder == PlaceholderPolicy.IMPLICIT || options.ShowPlaceholder == PlaceholderPolicy.IMPLICIT_LABEL_FALLBACK)
                && foundRow.Columns.Count > 1)
            {
                foundRow.Columns.ForEach(x =>
                {
                    x.RenderOptions.Placeholder = x.RenderOptions.Label;
                });
            }
            else if (options.ShowPlaceholder == PlaceholderPolicy.IMPLICIT_LABEL_FALLBACK && foundRow.Columns.Count == 1)
            {
                layoutAttr.Placeholder = layoutAttr.Label;
            }
            else if (options.ShowPlaceholder == PlaceholderPolicy.NONE)
                layoutAttr.Placeholder = null;

        }
    }
}
