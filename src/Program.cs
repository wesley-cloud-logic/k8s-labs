using System.Text;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

var config = builder.Configuration;
var app = builder.Build();
var bootstrapUrl = "<link href=\"https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css\" rel=\"stylesheet\" integrity=\"sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH\" crossorigin=\"anonymous\">";
var navbar = $"<nav class=\"{config["Settings:navBackgroundClass"]} navbar\">"
+ "<div class=\"container-fluid\">"
+ $"<a class=\"navbar-brand\" href=\"#\">{config["Settings:siteName"]}</a></div></nav>";


app.MapGet("/", () => new CustomHTMLResult($"<html>{bootstrapUrl}{navbar}<body class=\"{config["Settings:backgroundClass"]}\"><h1 class=\"p-3\">Hello World! from <span>{Environment.MachineName}</span></body></html>"));

app.MapGet("/write", (IConfiguration config) =>
{
    var redis = ConnectionMultiplexer.Connect(config["ConnectionStrings:Redis"] ?? string.Empty);
    var database = redis.GetDatabase();

    database.StringSet("timeOfWrite", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

    return new CustomHTMLResult($"<html>{bootstrapUrl}{navbar}<span>View Data <a href='/read'>here</a></span></html>");
});

app.MapGet("/read", (IConfiguration config) =>
{
    var redis = ConnectionMultiplexer.Connect(config["ConnectionStrings:Redis"] ?? string.Empty);
    var database = redis.GetDatabase();
    var value = database.StringGet("timeOfWrite");

    return new CustomHTMLResult($"<html>{bootstrapUrl}{navbar}<h1>Database Read Success</h1><span>Last time data was written was at {value}</span></html>");
});

app.MapGet("/config", (IConfiguration config) =>
{
    var confKeys = config.AsEnumerable().ToList();
    var stringBuilder = new StringBuilder();

    stringBuilder.Append("<html>");
    stringBuilder.Append(bootstrapUrl);
    stringBuilder.Append(navbar);
    stringBuilder.Append("<h1>Config Values</h1>");
    stringBuilder.Append("<table class=\"table table-striped\"><thead><tr><th>Name</th><th>Value</th><tbody>");
    confKeys.ForEach(kv => stringBuilder.Append($"<tr><td>{kv.Key}</td><td>{kv.Value}</td></tr>"));

    stringBuilder.Append("</tbody></table>");
    stringBuilder.Append("</html>");

    return new CustomHTMLResult($"{stringBuilder.ToString()}");
});

app.Run();
