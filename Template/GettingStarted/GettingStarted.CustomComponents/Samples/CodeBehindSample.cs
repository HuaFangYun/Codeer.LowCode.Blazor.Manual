using Codeer.LowCode.Blazor.OperatingModel;
using Codeer.LowCode.Blazor.ProCode;

namespace GettingStarted.CustomComponents.Samples
{
  public class CodeBehindSample : ProCodeBehindBase
  {
    public TextField? Name { get; set; }
    public NumberField? Age { get; set; }
    public ButtonField? OK { get; set; }
    public TextField? Result { get; set; }

    public override async Task InitializeDetailAsync()
    {
      await Task.CompletedTask;
      OK!.Click += OK_Click;
    }

    private async void OK_Click(object? sender, EventArgs e)
    {
      var value = @$"This is a sample code-behind.
This string is created using C# code.
Implemented in {GetType().FullName}.
Your name is {Name!.Value}.
And I'm {Age!.Value} years old.";

      await Result!.SetValueAsync(value);
    }

  }
}
