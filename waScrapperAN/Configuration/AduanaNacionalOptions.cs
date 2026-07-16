namespace waScrapperAN.Configuration;

public sealed class AduanaNacionalOptions
{
    public const string SectionName = "AduanaNacional";

    public string Endpoint { get; set; } = string.Empty;
    public string Namespace { get; set; } = string.Empty;
    public string MetodoSeguimiento { get; set; } = string.Empty;
    public string Usuario { get; set; } = "ANDROID";
    public int TimeoutSeconds { get; set; } = 60;
    public string OutputDirectory { get; set; } = string.Empty;

    public bool IsValid()
    {
        if (string.IsNullOrWhiteSpace(Endpoint))
            return false;
        if (string.IsNullOrWhiteSpace(Namespace))
            return false;
        if (string.IsNullOrWhiteSpace(MetodoSeguimiento))
            return false;
        if (string.IsNullOrWhiteSpace(Usuario))
            return false;
        if (TimeoutSeconds <= 0)
            return false;
        if (string.IsNullOrWhiteSpace(OutputDirectory))
            return false;
        return true;
    }
}