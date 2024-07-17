using System.Text;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

var config = builder.Configuration;
var app = builder.Build();
var bootstrapUrl = "<link href=\"https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css\" rel=\"stylesheet\" integrity=\"sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH\" crossorigin=\"anonymous\">";

app.MapGet("/", () => $"Hello World! from {Environment.MachineName}");

app.MapGet("/write", (IConfiguration config) =>
{
    var redis = ConnectionMultiplexer.Connect(config["ConnectionStrings:Redis"] ?? string.Empty);
    var database = redis.GetDatabase();

    database.StringSet("timeOfWrite", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

    return new CustomHTMLResult($"<html>{bootstrapUrl}<span>View Data <a href='/read'>here</a></span></html>");
});

app.MapGet("/read", (IConfiguration config) =>
{
    var redis = ConnectionMultiplexer.Connect(config["ConnectionStrings:Redis"] ?? string.Empty);
    var database = redis.GetDatabase();
    var value = database.StringGet("timeOfWrite");

    return new CustomHTMLResult($"<html>{bootstrapUrl}<h1>Database Read Success</h1><span>Last time data was written was at {value}</span></html>");
});

app.MapGet("/config", (IConfiguration config) =>
{
    var confKeys = config.AsEnumerable().ToList();
    var stringBuilder = new StringBuilder();
    stringBuilder.Append(bootstrapUrl);

    stringBuilder.Append("<table class=\"table table-striped\"><thead><tr><th>Name</th><th>Value</th><tbody>");
    confKeys.ForEach(kv => stringBuilder.Append($"<tr><td>{kv.Key}</td><td>{kv.Value}</td></tr>"));

    stringBuilder.Append("</tbody></table>");

    return new CustomHTMLResult($"<h1>Config Values</h1> {stringBuilder.ToString()}");
});

app.Run();
