using Spectre.Console;
using Spectre.Console.Json;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Calabonga.OopExercises.ConsoleApp.Helpers;
public static class Dumper
{
    public static T Dump<T>(this T value)
    {
        var json = value.ToPrettyString();
        if (string.IsNullOrEmpty(json))
        {
            AnsiConsole.WriteLine("Empty");
            return default!;
        }

        AnsiConsole.Write(
            new Panel(new JsonText(json))
                .Header(value!.GetType().Name)
                .Collapse()
                .RoundedBorder()
                .BorderColor(Color.Yellow));
        return value;
    }

    private static string ToPrettyString(this object? value)
    {
        if (value is null)
        {
            return string.Empty;
        }

        return JsonSerializer.Serialize(value, new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
    }
}
