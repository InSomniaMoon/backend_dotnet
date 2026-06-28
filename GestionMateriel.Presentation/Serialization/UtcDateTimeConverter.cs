using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GestionMateriel.Presentation.Serialization;

/// <summary>
/// Normalise toute date entrante en UTC quel que soit le format reçu (Z, offset, ou naïf -
/// dans ce dernier cas la valeur est considérée comme déjà en UTC). En sortie, la date est
/// toujours écrite en UTC avec le suffixe "Z", même si la colonne en base ne porte pas
/// l'information de fuseau (les valeurs stockées sont par convention toujours de l'UTC).
/// </summary>
public class UtcDateTimeConverter : JsonConverter<DateTime>
{
    private const string OutputFormat = "yyyy-MM-ddTHH:mm:ss.fffffffZ";

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        var dt = DateTime.Parse(value!, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);

        return dt.Kind switch
        {
            DateTimeKind.Utc => dt,
            DateTimeKind.Local => dt.ToUniversalTime(),
            _ => DateTime.SpecifyKind(dt, DateTimeKind.Utc)
        };
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        => writer.WriteStringValue(DateTime.SpecifyKind(value, DateTimeKind.Utc)
            .ToString(OutputFormat, CultureInfo.InvariantCulture));
}

public class NullableUtcDateTimeConverter : JsonConverter<DateTime?>
{
    private static readonly UtcDateTimeConverter Inner = new();

    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.TokenType == JsonTokenType.Null ? null : Inner.Read(ref reader, typeToConvert, options);

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
        }
        else
        {
            Inner.Write(writer, value.Value, options);
        }
    }
}
