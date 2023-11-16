using System.IO.Ports;
using System.Text.Json.Serialization;
using WebApp.Device;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();


app.MapGet("/", () => SerialPort.GetPortNames());
app.MapGet("/t", () => {
    var com2 = A2.COM2;
    var buf = com2.ReadHoldingRegister(1, 0, 2);
    var h = Math.Round(buf.ToUInt16(0, false) * 0.1, 1);
    var t = Math.Round(buf.ToUInt16(2, false) * 0.1 - 40, 1);
    return new string[] { h.ToString(), t.ToString() };
});

app.Run();

public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);

[JsonSerializable(typeof(string[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
