using Codeer.LowCode.Blazor.Repository.Data;

namespace GettingStarted.CustomComponents.Samples.ColorPicker
{
  public class ColorPickerFieldData : ValueFieldDataBase<string>
  {
    public ColorPickerFieldData() : base(typeof(ColorPickerFieldData).FullName!) { }
  }
}
