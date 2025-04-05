using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Http;

public class NullHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public NullHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.EnableBuffering();
        var bodyAsText = await new StreamReader(context.Request.Body).ReadToEndAsync();
        context.Request.Body.Position = 0;

        var json = JsonDocument.Parse(bodyAsText);
        Console.WriteLine(bodyAsText); // Log the JSON content
        var rootElement = json.RootElement;

        var modifiedJson = new JsonObject();
        foreach (var property in rootElement.EnumerateObject())
        {
            modifiedJson[property.Name] = property.Value.ValueKind == JsonValueKind.Null
                ? "default_value"
                : property.Value.GetString();
        }

        var modifiedBody = JsonSerializer.Serialize(modifiedJson);
        var bytes = Encoding.UTF8.GetBytes(modifiedBody);

        context.Request.Body = new MemoryStream(bytes);
        context.Request.ContentLength = bytes.Length;

        await _next(context);
    }
}
