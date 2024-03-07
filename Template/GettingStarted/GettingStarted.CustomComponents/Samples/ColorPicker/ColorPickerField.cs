using Codeer.LowCode.Blazor.OperatingModel;
using Codeer.LowCode.Blazor.Script;
using Design.Samples.ColorPicker;

namespace GettingStarted.CustomComponents.Samples.ColorPicker
{
  public class ColorPickerField : ValueField<ColorPickerFieldData, string>
  {
    ColorPickerFieldDesign _design;
    public ColorPickerField(ColorPickerFieldDesign design, ColorPickerFieldData data) : base(design, data) => _design = design;

    [ScriptHide]
    public override bool ValidateInput() => true;
  }
}
