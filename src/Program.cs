using System.Text;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

var config = builder.Configuration;
var app = builder.Build();

app.MapGet("/", () => $"Hello World! from {Environment.MachineName}");

app.MapGet("/write", (IConfiguration config) =>
{
    var redis = ConnectionMultiplexer.Connect(config["ConnectionStrings:Redis"] ?? string.Empty);
    var database = redis.GetDatabase();

    database.StringSet("timeOfWrite", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

    return new CustomHTMLResult("<span>View Data <a href='/read'>here</a></span>");
});

app.MapGet("/read", (IConfiguration config) =>
{
    var redis = ConnectionMultiplexer.Connect(config["ConnectionStrings:Redis"] ?? string.Empty);
    var database = redis.GetDatabase();
    var value = database.StringGet("timeOfWrite");

    return new CustomHTMLResult($"<h1>Database Read Success</h1><span>Last time data was written was at {value}</span>");
});

app.MapGet("/config", (IConfiguration config) =>
{
    var confKeys = config.AsEnumerable().ToList();
    var stringBuilder = new StringBuilder();

    confKeys.ForEach(kv => stringBuilder.Append($"{kv.Key}={kv.Value}<br/>"));

    return new CustomHTMLResult($"<h1>Config Values</h1> {stringBuilder.ToString()}");
});

app.Run();
