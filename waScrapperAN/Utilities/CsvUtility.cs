using System.Text;
using waScrapperAN.Models;

namespace waScrapperAN.Utilities;

public static class CsvUtility
{
    public static string GenerateCsv(IReadOnlyCollection<DimDuiTrackingEvent> eventos)
    {
        var sb = new StringBuilder();
        sb.AppendLine("DIM;Gestion;Aduana;Serie;Numero;Etapa;Fecha;Hora;FechaHora;Canal;Estado;Observacion;Usuario");

        foreach (var ev in eventos)
        {
            sb.AppendLine(
                $"{EscapeCsvField(ev.Dim)};{EscapeCsvField(ev.Gestion)};{EscapeCsvField(ev.Aduana)};{EscapeCsvField(ev.Serie)};{EscapeCsvField(ev.Numero)};{EscapeCsvField(ev.Etapa)};{EscapeCsvField(ev.Fecha)};{EscapeCsvField(ev.Hora)};{EscapeCsvField(ev.FechaHora)};{EscapeCsvField(ev.Canal)};{EscapeCsvField(ev.Estado)};{EscapeCsvField(ev.Observacion)};{EscapeCsvField(ev.Usuario)}");
        }

        return sb.ToString();
    }

    private static string EscapeCsvField(string value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        if (value.Contains(';') || value.Contains('"') || value.Contains('\n') || value.Contains('\r'))
        {
            return $"\"{value.Replace("\"", "\"\"")}\"";
        }

        return value;
    }
}