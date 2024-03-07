using Codeer.LowCode.Blazor.DesignLogic;
using Codeer.LowCode.Blazor.OperatingModel;
using Codeer.LowCode.Blazor.Repository.Data;
using Codeer.LowCode.Blazor.Repository.Design;
using GettingStarted.CustomComponents.Samples.ColorPicker;

namespace Design.Samples.ColorPicker
{
  public class ColorPickerFieldDesign : ValueFieldDesignBase
  {
    public ColorPickerFieldDesign() : base(typeof(ColorPickerFieldDesign).FullName!) { }

    [Designer(Index = 0, CandidateType = CandidateType.DbColumn), DbColumn(nameof(ColorPickerFieldData.Value))]
    public string DbColumn { get; set; } = string.Empty;

    [Designer(Index = 1)]
    public string Default { get; set; } = "#000000";

    public override string GetWebComponentTypeFullName() => typeof(ColorPickerFieldComponent).FullName!;
    public override string GetSearchWebComponentTypeFullName() => String.Empty;
    public override string GetSearchControlTypeFullName() => String.Empty;
    public override FieldBase CreateField(FieldDataBase? data) => new ColorPickerField(this, data.CastOrDefault<ColorPickerFieldData>());
    public override FieldDataBase? CreateData() => new ColorPickerFieldData();
  }
}
